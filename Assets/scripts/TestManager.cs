using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Holoville.HOTween;

public class TestManager : MonoBehaviour {

	public enum BackModes {TestBegin, TestWrong, TestCorrect, Neutral, NeutralForce};

	public static TestManager Instance;

	private int currentTestIndex;
	private BasicTest currentTest;
	public Text Instruction;
	public Text RewindCount;
	public Button RewindButt;

	public Color[] RoundInstruction;
	public Color[] AnswerCorrect;
	public Color[] AnswerWrong;


	public Image RoundBack;
	public Image RoundTimer;
	public Image RoundTimerFill;

	public int RoundRewinds;
	private int _currentRewinds;

	public BasicTest.TestTypes[] TestSeq;

	public Sprite[] ImageSprites;

	private bool _lastAnswerCorrect;

	private int currentQuaestion;

	void Awake () {
		Instance = this;
	}
	
	public void Init(){
		currentTestIndex = -1;
		SetImage (null);
	}

	public bool NextTest(){
		do {
			currentTestIndex++;
		} while(currentTestIndex < TestSeq.Length && Settings.Instance.GetProperty(Settings.SettingsTypes.QuestionsPerRound, currentTestIndex) == 0);

		if (currentTestIndex == TestSeq.Length) {
			return false;
		} else {

			foreach (Transform child in transform) {
				if (child.GetComponent<BasicTest> () && child.GetComponent<BasicTest> ().TestType == TestSeq [currentTestIndex]) {
					currentTest = child.GetComponent<BasicTest> ();
					currentTest.NumOfQuestions = Settings.Instance.GetProperty(Settings.SettingsTypes.QuestionsPerRound, currentTestIndex);
				}
			}

			return true;
		}


	}

	public void StartTest(){
		currentQuaestion = 0;
		foreach (Transform child in transform) {
			if (child.GetComponent<BasicTest>()){
				child.gameObject.SetActive(false);
			}
		}

		Instruction.gameObject.SetActive (true);

		_currentRewinds = RoundRewinds;
		RewindCount.text = "X" + _currentRewinds;
		RewindButt.interactable = _currentRewinds > 0;

		RewindCount.gameObject.SetActive (false);
		RewindButt.gameObject.SetActive (false);

		Instruction.text = "Round " + (currentTestIndex + 1) + " / " + TestSeq.Length;
		SetBackMode (TestManager.BackModes.TestBegin);

		Invoke ("StartQuaestion", 2f);
	}

	private void StartQuaestion(){
		foreach (Transform child in transform)
		{
			if (child.GetComponent<BasicTest>()){
				child.gameObject.SetActive(false);
			}
		}
		currentTest.Reset ();
		Instruction.gameObject.SetActive (false);

		SetImage (ImageSprites[Random.Range(0, ImageSprites.Length)]);
		SetBackMode (TestManager.BackModes.Neutral);

		Invoke ("ShowInstruction", 2f);
	}

	private void ShowInstruction(){
		Instruction.gameObject.SetActive (true);
		Instruction.text = currentTest.Instruction;

		SetImage (null);
		SetBackMode (TestManager.BackModes.Neutral);

		Invoke ("DeliverPattern", 2f);
	}

	private void DeliverPattern(){

		Invoke ("ShowTestUI", currentTest.DeliverPattern());
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

		Instruction.gameObject.SetActive (false);

		RewindCount.gameObject.SetActive (true);
		RewindButt.gameObject.SetActive (true);

		SetTimer (6);
	}

	public void AnswerReceived(bool correct){
		currentTest.Responsive = false;
		currentTest.FreezeControls ();
		RewindCount.gameObject.SetActive (false);
		RewindButt.gameObject.SetActive (false);

		_lastAnswerCorrect = correct;

		Invoke ("ShowResult", 1f);

	}

	private void ShowResult(){
		Instruction.gameObject.SetActive (true);
		currentTest.gameObject.SetActive (false);

		Instruction.text = (_lastAnswerCorrect)?"CORRECT":"WRONG";
		SetBackMode ((_lastAnswerCorrect)?TestManager.BackModes.TestCorrect:TestManager.BackModes.TestWrong);
		Invoke ("EndQuestion", 2f);
	}

	public void EndQuestion(){
		currentQuaestion++;

		if (currentQuaestion < currentTest.NumOfQuestions) {
			StartQuaestion ();
		} else {
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
			Instruction.color = instructionCol;
		} else {
			HOTween.To (RoundBack, 0.6f, "color", targetCol);
			HOTween.To (Instruction, 0.6f, "color", instructionCol);
		}
	}
	
	public void SetImage(Sprite sprite){
		RoundBack.sprite = sprite;
	}
	
	public void SetTimer(float time){
		RoundTimer.gameObject.SetActive (true);
		RoundTimerFill.fillAmount = 0;
		HOTween.To (RoundTimerFill, time, new TweenParms ().Prop ("fillAmount", 1).Ease (EaseType.Linear).OnComplete (EndTimer));
	}
	
	public void EndTimer(){
		RoundTimer.gameObject.SetActive (false);
	}
}
