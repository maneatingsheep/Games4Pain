using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TestTemporalOrder : PatternTest {

	public Image ChannelAButt;
	public Image ChannelBButt;

    public Sprite[] ButtonSprites;

    private byte pulseLen;

	override public void Reset(){
		base.Reset ();

        Instructions[0] = "Pay attention to your";
        Instructions[1] = bodyPart1;
        Instructions[2] = "and your";
        Instructions[3] = bodyPart2;

        ActionInstructions[0] = "";
        ActionInstructions[1] = "";
        ActionInstructions[2] = "The transmittions weren't in sync";
        ActionInstructions[3] = "Whitch one came first?";

        ChannelAButt.sprite = ButtonSprites[0];
        ChannelBButt.sprite = ButtonSprites[0];
        ChannelAButt.GetComponentInChildren<Text>().color = Color.black;
        ChannelBButt.GetComponentInChildren<Text>().color = Color.black;

        ChannelAButt.GetComponentInChildren<Text>().text = Settings.Instance.BodyPartA;
        ChannelBButt.GetComponentInChildren<Text>().text = Settings.Instance.BodyPartB;


        ChannelAButt.gameObject.SetActive (true);
		ChannelBButt.gameObject.SetActive (true);
	}

	override public float DeliverPattern(){

        BluetoothProxy.Instance.DeliverTest2(channelByte1, channelByte2, amp1, amp2);
        
		return 3f;
	}

	public void ChanAClicked(){
		if (!Responsive) return;
        ChannelAButt.sprite = ButtonSprites[1];
        ChannelAButt.GetComponentInChildren<Text>().color = Color.white;

        TestManager.Instance.AnswerReceived (channel == BluetoothProxy.Channels.ChannelA);

	}
	
	public void ChanBClicked(){
		if (!Responsive) return;
        ChannelBButt.sprite = ButtonSprites[1];
        ChannelBButt.GetComponentInChildren<Text>().color = Color.white;

        TestManager.Instance.AnswerReceived (channel == BluetoothProxy.Channels.ChannelB);

	}


	override public void FreezeControls(){
		/*ChannelAButt.gameObject.SetActive (false);
		ChannelBButt.gameObject.SetActive (false);*/
	}
}
