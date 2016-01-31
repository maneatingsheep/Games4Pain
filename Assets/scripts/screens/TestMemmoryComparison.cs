using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class TestMemmoryComparison : PatternTest {

	public Button SameButton;
	public Button DiffButton;
	
	override public void Reset(){

		base.Reset ();

		Instruction = "Pay attention to your " + bodyPart1 + " and then to your " + bodyPart2;


		if (Random.value > 0.5f) {
			wrongPattern = correctPattern;
		}

		SameButton.gameObject.SetActive (true);
		DiffButton.gameObject.SetActive (true);
	}
	
	public void SameClicked(){
		if (!Responsive) return;

		TestManager.Instance.AnswerReceived (wrongPattern == correctPattern);

	}
	
	public void DiffClicked(){
		if (!Responsive) return;
		
		TestManager.Instance.AnswerReceived (wrongPattern != correctPattern);

	}

	override public float DeliverPattern(){

		BluetoothSequence = new byte[]{0xc3, 0xd0, 
			0x05, //test num
			channelByte1, 
			0, //amp low
			Settings.Instance.GetProperty(Settings.SettingsTypes.ShortPulse), 
			(byte)numOfPulses, 
			channelByte1, 
			amp1, 
			Settings.Instance.GetProperty(Settings.SettingsTypes.LongPulse), 
			correctPattern, 
			Settings.Instance.GetProperty(Settings.SettingsTypes.InterbeatPause), 
			0xd2};

		BluetoothProxy.Instance.DeliverPattern(BluetoothSequence);
		Invoke ("DeliverSecondPattern", 3f);
		return 6f;
	}

	private void DeliverSecondPattern(){

		BluetoothSequence = new byte[]{0xc3, 0xd0, 
			0x05, //test num
			channelByte2, 
			0, //amp low
			Settings.Instance.GetProperty(Settings.SettingsTypes.ShortPulse), 
			(byte)numOfPulses, 
			channelByte2, 
			amp2, 
			Settings.Instance.GetProperty(Settings.SettingsTypes.LongPulse), 
			wrongPattern, 
			Settings.Instance.GetProperty(Settings.SettingsTypes.InterbeatPause), 
			0xd2};

		BluetoothProxy.Instance.DeliverPattern(BluetoothSequence);
	}

	override public void FreezeControls(){
		SameButton.gameObject.SetActive (false);
		DiffButton.gameObject.SetActive (false);
	}
}

