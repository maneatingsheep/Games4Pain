using UnityEngine;
using UnityEngine.UI;
using Holoville.HOTween;
using System.Collections.Generic;
using System.Linq;

public class TestManager : MonoBehaviour {

	public enum BackModes {TestBegin, TestWrong, TestCorrect, Neutral, NeutralForce};

	public static TestManager Instance;

    public TestTRansition TestTRansitionInst;

    private int currentTestIndex;
	private BasicTest currentTest;
	public Text[] Instruction;
	public Text RewindCount;
	public Button RewindButt;

    private int _currentVisibleTest;
    private int _totalVisibleTests;

    public Color[] RoundInstruction;
	public Color[] AnswerCorrect;
	public Color[] AnswerWrong;


	public Image RoundBack;
	

	public int RoundRewinds;
	private int _currentRewinds;

	public BasicTest.TestTypes[] TestSeq;

	public Sprite[] ImageSprites;

	private bool _lastAnswerCorrect;
	private bool _lastTestCorrect;

    private int currentQuaestion;

    public bool SkipTransitions;

    public List<CompletedTest> CompletedTests = new List<CompletedTest>();

    private float _lasttimeOfInvoke;
    private float _lastInvoketime;
    private string _lastInvokeMethod;
    private float _lastRemaningInvoke;

    void Awake () {
		Instance = this;
	}
	
	public void Init(){
        TestTRansitionInst.gameObject.SetActive(false);
    }

    public void Reset() {
        currentTestIndex = -1;
        _currentVisibleTest = -1;
        _totalVisibleTests = 0;

        SetImage(null);

        for (int i = 0; i < TestSeq.Length; i++) {
            if ( Settings.Instance.GetProperty(Settings.SettingsTypes.QuestionsPerRound, i) > 0){
                _totalVisibleTests++;
            }
        }

        _lastTestCorrect = true;

        TestTRansitionInst.SetMap(_totalVisibleTests);

        TestTRansitionInst.gameObject.SetActive(false);

        CompletedTests.Clear();

    }

    public bool NextTest(){
		do {
			currentTestIndex++;
		} while(currentTestIndex < TestSeq.Length && Settings.Instance.GetProperty(Settings.SettingsTypes.QuestionsPerRound, currentTestIndex) == 0);

        _currentVisibleTest++;

        

        if (currentTestIndex == TestSeq.Length) {
			return false;
		} else {
            CompletedTests.Add(new CompletedTest() { TestType = TestSeq[currentTestIndex] });
            foreach (Transform child in transform) {
				if (child.GetComponent < BasicTest> () && CompareTestTypes(child.GetComponent<BasicTest> ().TestType, TestSeq [currentTestIndex])) {
					currentTest = child.GetComponents<BasicTest> ().First((b => b.TestType == TestSeq[currentTestIndex]));
                    currentTest.NumOfQuestions = Settings.Instance.GetProperty(Settings.SettingsTypes.QuestionsPerRound, currentTestIndex);
                    CompletedTests[CompletedTests.Count - 1].CorrectQuestions = 0;
                    CompletedTests[CompletedTests.Count - 1].TotalQuestions = currentTest.NumOfQuestions;
                }
			}

            TestProgress.Instance.SetNumOfQuestions(currentTest.NumOfQuestions);

            return true;
		}

	}

    private bool CompareTestTypes(BasicTest.TestTypes TestA, BasicTest.TestTypes TestB) {
        if (TestA == TestB) return true;
        if (TestA == BasicTest.TestTypes.SingleSite && TestB == BasicTest.TestTypes.TwoSites) return true;
        if (TestA == BasicTest.TestTypes.SingleSite && TestB == BasicTest.TestTypes.SingleWithDistraction) return true;
        return false;
        
    }

	public void StartTest(){
		currentQuaestion = 0;
		foreach (Transform child in transform) {
			if (child.GetComponent<BasicTest>()){
				child.gameObject.SetActive(false);
			}
		}

        for (int i = 0; i < Instruction.Length; i++) {
            Instruction[i].text = "";
        }

		_currentRewinds = RoundRewinds;
		RewindCount.text = "X" + _currentRewinds;
		RewindButt.interactable = _currentRewinds > 0;

		RewindCount.gameObject.SetActive (false);
		RewindButt.gameObject.SetActive (false);

		SetBackMode (BackModes.TestBegin);

        Instruction[0].text = "Sector " + (_currentVisibleTest + 1) + " / " + _totalVisibleTests;
        Instruction[1].text = "";
        if (_lastTestCorrect) {
            Instruction[2].text = "Destination found!";
            Instruction[3].text = "Full speed ahead";
        } else {
            Instruction[2].text = "Destination unknown";
            Instruction[3].text = "Lets see...";
        }


        TestTRansitionInst.gameObject.SetActive(true);

        TestTRansitionInst.ProgressToNextStar(_lastTestCorrect);

        if (SkipTransitions) {
            StartQuaestion();
        } else {
            DoInvoke("StartQuaestion", 6f);
        }
		
	}

	private void StartQuaestion(){
        //show image

        TestProgress.Instance.SetQuestionState(currentQuaestion, 1);

        foreach (Transform child in transform)
		{
			if (child.GetComponent<BasicTest>()){
				child.gameObject.SetActive(false);
			}
		}
		currentTest.Reset ();

        for (int i = 0; i < Instruction.Length; i++) {
            Instruction[i].text = "";
        }

        Instruction[2].text = "Receiving transmittion...";


        SetImage (ImageSprites[Random.Range(0, ImageSprites.Length)]);
		SetBackMode (TestManager.BackModes.Neutral);

        TestTRansitionInst.gameObject.SetActive(false);

        if (SkipTransitions) {
            ShowInstruction();
        } else {
            DoInvoke("ShowInstruction", 2f);
        }
	}

	private void ShowInstruction(){

        for (int i = 0; i < Instruction.Length; i++) {
            Instruction[i].text = currentTest.Instructions[i];
        }

        if (SkipTransitions) {
            DeliverPattern();
        } else {
            DoInvoke("DeliverPattern", 2f);
        }

	}

	private void DeliverPattern(){

        if (SkipTransitions) {
            ShowTestUI();
        } else {
            DoInvoke("ShowTestUI", currentTest.DeliverPattern());

        }
	}

	private void ShowTestUI(){
		foreach (Transform child in transform)
		{
			if (child.GetComponent<BasicTest>()){
				child.gameObject.SetActive(false);
			}
		}

		currentTest.gameObject.SetActive (true);

		currentTest.Responsive = true;

        for (int i = 0; i < Instruction.Length; i++) {
            Instruction[i].text = currentTest.ActionInstructions[i];
        }
        

        SetImage(null);
        SetBackMode(TestManager.BackModes.Neutral);

        RewindCount.gameObject.SetActive (true);
		RewindButt.gameObject.SetActive (true);
        
	}

	public void AnswerReceived(bool correct){
		currentTest.Responsive = false;
		currentTest.FreezeControls ();
        RewindCount.gameObject.SetActive (false);
		RewindButt.gameObject.SetActive (false);

		_lastAnswerCorrect = correct;

        if (correct) {
            CompletedTests[_currentVisibleTest].CorrectQuestions++;
        }

        if (SkipTransitions) {
            ShowResult();
        } else {
            DoInvoke("ShowResult", 1f);

        }

	}

	private void ShowResult(){
		currentTest.gameObject.SetActive (false);
        
		SetBackMode ((_lastAnswerCorrect)?TestManager.BackModes.TestCorrect:TestManager.BackModes.TestWrong);

        Instruction[0].text = "";
        Instruction[1].text = "";
        Instruction[2].text = "";
        Instruction[3].text = (_lastAnswerCorrect) ? "Correct":"Wrong";

        TestProgress.Instance.SetQuestionState(currentQuaestion, (_lastAnswerCorrect) ? 2 : 3);

        if (SkipTransitions) {
            EndQuestion();
        } else {
            DoInvoke("EndQuestion", 2f);

        }
	}

	public void EndQuestion(){
		currentQuaestion++;

		if (currentQuaestion < currentTest.NumOfQuestions) {
			StartQuaestion ();
		} else {
            TestProgress.Instance.SetNumOfQuestions(0);
            _lastTestCorrect = CompletedTests[_currentVisibleTest].CorrectQuestions >= currentTest.NumOfQuestions / 2;

            FlowManager.Instance.TestFinished();
		}

        
    }

	public void RewindClicked(){
		_currentRewinds--;
		RewindCount.text = "X" + _currentRewinds;
		RewindButt.interactable = _currentRewinds > 0;

		currentTest.DeliverPattern ();
	}

	public void SetBackMode(BackModes mode){
		
		Color targetCol = Color.white;
		Color instructionCol = Color.black;
		bool doJump = false;
		
		switch (mode) {
		case BackModes.TestBegin: 
			targetCol = RoundInstruction[0];
			instructionCol = RoundInstruction[1];
			break;
		case BackModes.TestWrong: 
			targetCol = AnswerWrong[0];
			instructionCol = AnswerWrong[1];
			break;
		case BackModes.TestCorrect: 
			targetCol = AnswerCorrect[0];
			instructionCol = AnswerCorrect[1];
			break;
		case BackModes.NeutralForce:
			targetCol = Color.white;
			instructionCol = Color.black;
			doJump = true;
			break;
		case BackModes.Neutral: 
			targetCol = Color.white;
			instructionCol = Color.black;
			break;
		}
		
		if (doJump) {
			RoundBack.color = targetCol;
		} else {
			HOTween.To (RoundBack, 0.6f, "color", targetCol);
		}
	}
	
	public void SetImage(Sprite sprite){
		RoundBack.sprite = sprite;
	}

    private void DoInvoke(string methodName, float time) {
        _lastInvokeMethod = methodName;
        _lastInvoketime = time;
        _lasttimeOfInvoke = Time.time;
        Invoke(methodName, time);
    }

    public void Pause() {
        _lastRemaningInvoke = _lastInvoketime - (Time.time - _lasttimeOfInvoke);
        if (_lastRemaningInvoke > 0) {
            CancelInvoke(_lastInvokeMethod);
            TestTRansitionInst.Pause();
        }
    }

    public void Resume() {
        if (_lastRemaningInvoke > 0) {
            DoInvoke(_lastInvokeMethod, _lastRemaningInvoke);
            TestTRansitionInst.Resume();
        }
    }

}


