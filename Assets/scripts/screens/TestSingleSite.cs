using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;


public class TestSingleSite : PatternRecognitionTest {

	override public void Reset(){
		base.Reset ();

		Instruction = "Pay attention to your " + bodyPart1;

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
		return 3f;
	}

}
