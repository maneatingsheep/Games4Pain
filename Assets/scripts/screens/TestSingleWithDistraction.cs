using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;


public class TestSingleWithDistraction : PatternRecognitionTest {

	override public void Reset(){
		base.Reset ();

        Instruction = "<b>" + bodyPart1 + "</b> (only)" ;
      
        
	}

	override public float DeliverPattern(){

        BluetoothProxy.Instance.DeliverTest4(channelByte1, channelByte2, amp1, correctPattern, numOfPulses);

        return correctPatternTime + 1f;
    }


}
