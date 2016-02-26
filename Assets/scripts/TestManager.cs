using UnityEngine;
using UnityEngine.UI;
using Holoville.HOTween;
using System.Collections.Generic;
using System.Linq;

public class TestManager : MonoBehaviour {

	//public enum BackModes {TestBegin, TestWrong, TestCorrect, Neutral, NeutralForce};

	public static TestManager Instance;

    //public TestTransition TestTransitionInst;
    public TestIntroduction TestIntroductionInst;

    private int currentTestIndex;
	private BasicTest currentTest;
	public Text Instruction;
	public Image InstructionCont;

    public Text RewindCount;
	public Button RewindButt;

    private int _currentVisibleTest;
    private int _totalVisibleTests;

    public Color[] RoundInstruction;
	public Color[] AnswerCorrect;
	public Color[] AnswerWrong;


	public Image RoundBack;
	

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
        TestIntroductionInst.gameObject.SetActive(false);
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

        /*TestTRansitionInst.SetMap(_totalVisibleTests);

        TestTRansitionInst.gameObject.SetActive(false);*/

        TestIntroductionInst.gameObject.SetActive(false); 

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
        
        _currentRewinds = Settings.Instance.GetProperty(Settings.SettingsTypes.NumOfRewinds, currentTestIndex);
		RewindCount.text = "X" + _currentRewinds;
		RewindButt.interactable = _currentRewinds > 0;

		RewindCount.gameObject.SetActive (false);
		RewindButt.gameObject.SetActive (false);

        //SetBackMode (BackModes.TestBegin);
        InstructionCont.gameObject.SetActive(false);
        SetImageOpacity(true);
        Instruction.text = "";


        /*if (_lastTestCorrect) {
            Instruction[2].text = "Destination found!";
            Instruction[3].text = "Full speed ahead";
        } else {
            Instruction[2].text = "Destination unknown";
            Instruction[3].text = "Lets see...";
        }*/

        SetImage(null);
        TestIntroductionInst.gameObject.SetActive(true);
        TestIntroductionInst.SetRoundData((_currentVisibleTest + 1), _totalVisibleTests, currentTest);

        //TestTRansitionInst.ProgressToNextStar(_lastTestCorrect);

        if (SkipTransitions) {
            StartQuaestion();
        } else {
            DoInvoke("StartQuaestion", 3f);
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

        InstructionCont.gameObject.SetActive(false);
        Instruction.text = "";

        //Instruction[2].text = "Receiving transmission...";

        SetImage (ImageSprites[Random.Range(0, ImageSprites.Length)]);
        //SetImageOpacity(true);
		//SetBackMode (TestManager.BackModes.Neutral);

        TestIntroductionInst.gameObject.SetActive(false);

        if (SkipTransitions) {
            ShowInstruction();
        } else {
            DoInvoke("ShowInstruction", 2f);
        }
	}

	private void ShowInstruction(){

        InstructionCont.gameObject.SetActive(true);
        Instruction.text = currentTest.Instruction;


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

        InstructionCont.gameObject.SetActive(false);
        SetImageOpacity(false);
        Instruction.text = currentTest.ActionInstruction;
        

        //SetImageOpacity(false);
        //SetImage(null);
        //SetBackMode(TestManager.BackModes.Neutral);

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

        //SetBackMode ((_lastAnswerCorrect)?TestManager.BackModes.TestCorrect:TestManager.BackModes.TestWrong);

        InstructionCont.gameObject.SetActive(false);
        Instruction.text =  "<b>" +  ((_lastAnswerCorrect) ? "Correct" : "Wrong")  + "</b>";
        

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

	/*public void SetBackMode(BackModes mode){
		
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
	}*/
	
	public void SetImage(Sprite sprite){
		RoundBack.sprite = sprite;
	}

    public void SetImageOpacity(bool opaque) {
        RoundBack.color = (opaque) ? new Color(1, 1, 1, 1) : new Color(1, 1, 1, 0.2f);
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
            //TestTRansitionInst.Pause();
        }
    }

    public void Resume() {
        if (_lastRemaningInvoke > 0) {
            DoInvoke(_lastInvokeMethod, _lastRemaningInvoke);
            //TestTRansitionInst.Resume();
        }
    }
    
   

}


