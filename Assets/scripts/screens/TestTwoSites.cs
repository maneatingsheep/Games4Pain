using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class TestTwoSites : PatternRecognitionTest {

	override public void Reset(){
		base.Reset ();

        Instructions[0] = "Pay attention to your";
        Instructions[1] = bodyPart1;
        Instructions[2] = "and your";
        Instructions[3] = bodyPart2;

    }

	override public float DeliverPattern(){

        BluetoothProxy.Instance.DeliverTest5(channelByte1, channelByte2, amp1, correctPattern, numOfPulses);

        return correctPatternTime + 1f;
    }
	
}
