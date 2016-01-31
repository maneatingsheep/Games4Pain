using UnityEngine;
using System.Collections;

public class BasicTest : MonoBehaviour {

	public enum TestTypes{SingleSite, TwoSites, SingleWithDistraction, MemoryComparison, TemporalOrder};

	public TestTypes TestType; 

	protected BluetoothProxy.Channels channel;

	public string Instruction;
	protected byte[] BluetoothSequence;
	internal byte NumOfQuestions;
	public bool Responsive;

	virtual public void Reset(){
		channel = (Random.value > 0.5f) ? BluetoothProxy.Channels.ChannelA : BluetoothProxy.Channels.ChannelB;
	}

	virtual public float DeliverPattern(){
		return 0;
	}

	virtual public void FreezeControls(){
	}

}
