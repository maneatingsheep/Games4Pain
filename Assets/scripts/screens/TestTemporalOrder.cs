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

        bodyPart1 = Settings.Instance.BodyPartTreated.Name.ToUpper();
        bodyPart2 = Settings.Instance.BodyPartHealthy.Name.ToUpper();

        Instruction = "<b>" + bodyPart1 + " </b>  +  <b>" + bodyPart2 + "</b>";

        ActionInstruction = "<b>Where</b> did you feel it <b>first</b>?";
 

        ChannelAButt.sprite = ButtonSprites[0];
        ChannelBButt.sprite = ButtonSprites[0];
        ChannelAButt.GetComponentInChildren<Text>().color = Color.black;
        ChannelBButt.GetComponentInChildren<Text>().color = Color.black;

        ChannelAButt.GetComponentInChildren<Text>().text = Settings.Instance.BodyPartTreated.Name;
        ChannelBButt.GetComponentInChildren<Text>().text = Settings.Instance.BodyPartHealthy.Name;


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
