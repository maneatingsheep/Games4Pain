using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class TestMemmoryComparison : PatternTest {

	public Image SameButton;
	public Image DiffButton;

    public Sprite[] ButtonSprites;

	override public void Reset(){

		base.Reset ();

        SameButton.sprite = ButtonSprites[0];
        DiffButton.sprite = ButtonSprites[0];
        SameButton.GetComponentInChildren<Text>().color = Color.black;
        DiffButton.GetComponentInChildren<Text>().color = Color.black;

        Instructions[0] = "Pay attention to your";
        Instructions[1] = bodyPart1;
        Instructions[2] = "and then your";
        Instructions[3] = bodyPart2;

        ActionInstructions[0] = "";
        ActionInstructions[1] = "";
        ActionInstructions[2] = "The tranmittions got mixed";
        ActionInstructions[3] = "Were they identical?";


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
        
		Invoke ("DeliverSecondPattern", 3f);
		return 6f;
	}

	private void DeliverSecondPattern(){

        BluetoothProxy.Instance.DeliverTest5(channelByte2, channelByte2, amp2, wrongPattern, numOfPulses);

	}

	override public void FreezeControls(){
		/*SameButton.gameObject.SetActive (false);
		DiffButton.gameObject.SetActive (false);*/
	}
}

