using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;


public class TestSingleSite : PatternRecognitionTest {

	override public void Reset(){
		base.Reset ();


        Instruction = "<b>" + bodyPart1 + "</b>";
        
        
	}

	override public float DeliverPattern(){

        BluetoothProxy.Instance.DeliverTest5(channelByte1, channelByte1, amp1, correctPattern, numOfPulses);
        
		return correctPatternTime + 1f;
	}

}
