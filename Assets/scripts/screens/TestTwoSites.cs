using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class TestTwoSites : PatternRecognitionTest {

	override public void Reset(){
		base.Reset ();

        Instruction = "<b>" + bodyPart1 + "</b>  ⇆  <b>" + bodyPart2 + "</b>";

    }

	override public float DeliverPattern(){

        BluetoothProxy.Instance.DeliverTest5(channelByte1, channelByte2, amp1, correctPattern, numOfPulses);

        return correctPatternTime + 1f;
    }
	
}
