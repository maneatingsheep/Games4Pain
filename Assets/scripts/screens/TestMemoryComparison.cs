using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class TestMemoryComparison : PatternTest {

	public Image SameButton;
	public Image DiffButton;

    public Sprite[] ButtonSprites;

	override public void Reset(){

		base.Reset ();

        SameButton.sprite = ButtonSprites[0];
        DiffButton.sprite = ButtonSprites[0];
        SameButton.GetComponentInChildren<Text>().color = Color.black;
        DiffButton.GetComponentInChildren<Text>().color = Color.black;

        Instruction = "<b>" + bodyPart1 + " </b>  →  <b>" + bodyPart2 + "</b>";
   
        ActionInstruction = "Did the two patterns feel the <b>same</b> or <b>different</b>?";


		if (Random.value > 0.5f) {
			wrongPattern = correctPattern;
		}

		SameButton.gameObject.SetActive (true);
		DiffButton.gameObject.SetActive (true);
	}
	
	public void SameClicked(){
		if (!Responsive) return;
        SameButton.sprite = ButtonSprites[1];
        SameButton.GetComponentInChildren<Text>().color = Color.white;

        TestManager.Instance.AnswerReceived (wrongPattern == correctPattern);

	}
	
	public void DiffClicked(){
		if (!Responsive) return;
        
        DiffButton.sprite = ButtonSprites[1];
        DiffButton.GetComponentInChildren<Text>().color = Color.white;

        TestManager.Instance.AnswerReceived (wrongPattern != correctPattern);

	}

	override public float DeliverPattern(){

        BluetoothProxy.Instance.DeliverTest5(channelByte1, channelByte1, amp1, correctPattern, numOfPulses);
        
		Invoke ("DeliverSecondPattern", correctPatternTime + 1f);
		return correctPatternTime + wrongPatternTime + 2f;
	}

	private void DeliverSecondPattern(){

        BluetoothProxy.Instance.DeliverTest5(channelByte2, channelByte2, amp2, wrongPattern, numOfPulses);

	}

	override public void FreezeControls(){
		/*SameButton.gameObject.SetActive (false);
		DiffButton.gameObject.SetActive (false);*/
	}
}

