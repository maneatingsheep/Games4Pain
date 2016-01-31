using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class TestTwoSites : PatternRecognitionTest {

	override public void Reset(){
		base.Reset ();
		
		Instruction = "Pay attention to your " + bodyPart1 + " and " + bodyPart2;

	}

	override public float DeliverPattern(){

		BluetoothSequence = new byte[]{0xc3, 0xd0, 
			0x05, //test num
			channelByte1, 
			0, //amp low
			Settings.Instance.GetProperty(Settings.SettingsTypes.ShortPulse), 
			(byte)numOfPulses, 
			channelByte2, 
			amp1, 
			Settings.Instance.GetProperty(Settings.SettingsTypes.LongPulse), 
			correctPattern, 
			Settings.Instance.GetProperty(Settings.SettingsTypes.InterbeatPause), 
			0xd2};

		BluetoothProxy.Instance.DeliverPattern(BluetoothSequence);
		return 3f;
	}
	
}
