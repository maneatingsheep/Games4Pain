using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;


public class TestSingleWithDistraction : PatternRecognitionTest {

	override public void Reset(){
		base.Reset ();


        Instructions[0] = "Pay attention to your";
        Instructions[1] = bodyPart1;
        Instructions[2] = "ignore your";
        Instructions[3] = bodyPart2;
        
	}

	override public float DeliverPattern(){

        BluetoothProxy.Instance.DeliverTest4(channelByte1, channelByte2, amp1, correctPattern, numOfPulses);
        
		return 3f;
	}


}
