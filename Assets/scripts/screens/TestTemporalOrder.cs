using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TestTemporalOrder : PatternTest {

	public Button ChannelAButt;
	public Button ChannelBButt;

	private byte pulseLen;

	override public void Reset(){
		base.Reset ();
		
		Instruction = "Pay attention to your " + bodyPart1 + " and " + bodyPart2;


		ChannelAButt.gameObject.SetActive (true);
		ChannelBButt.gameObject.SetActive (true);
	}

	override public float DeliverPattern(){
		
		BluetoothSequence = new byte[]{0xc3, 0xd0, 
			0x02, //test num
			channelByte1, 
			amp1,
			Settings.Instance.GetProperty(Settings.SettingsTypes.TemporalJudjmentPulseLength), 
			Settings.Instance.GetProperty(Settings.SettingsTypes.TemporalJudjmentDelay), 
			channelByte2, 
			amp2,
			Settings.Instance.GetProperty(Settings.SettingsTypes.TemporalJudjmentPulseLength), 
			0, //delay - not used
			0, //time between pulses in 15mSec intervals - not used
			0xd2};
		
		BluetoothProxy.Instance.DeliverPattern(BluetoothSequence);
		return 4f;
	}

	public void ChanAClicked(){
		if (!Responsive) return;
		
		TestManager.Instance.AnswerReceived (channel == BluetoothProxy.Channels.ChannelA);

	}
	
	public void ChanBClicked(){
		if (!Responsive) return;
		
		TestManager.Instance.AnswerReceived (channel == BluetoothProxy.Channels.ChannelB);

	}


	override public void FreezeControls(){
		ChannelAButt.gameObject.SetActive (false);
		ChannelBButt.gameObject.SetActive (false);
	}
}
