using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class PatternRecognitionTest : PatternTest {

	
	public List<Image> pulseImages;
	public Sprite[] pulseModes;
	
	
	protected int selectedIndex;


	override public void Reset(){
		base.Reset ();

		for (int i = 0; i < pulseImages.Count; i++) {
			pulseImages [i].gameObject.SetActive ((i < numOfPulses));

			bool isLong = (wrongPattern & (1 << i)) != 0;
			pulseImages[i].sprite = (isLong)?pulseModes[1]:pulseModes[0];
		}


        ActionInstructions[0] = "";
        ActionInstructions[1] = "";
        ActionInstructions[2] = "The transmittion contains an error";
        ActionInstructions[3] = "Find it";

    }

	public void ImageClicked(Image image){
		if (!Responsive) return;
		selectedIndex = pulseImages.IndexOf (image);
		for (int i = 0; i < pulseImages.Count; i++) {
			
			bool isLong = (wrongPattern & (1 << i)) != 0;
			
			if (i == selectedIndex){
				pulseImages[i].sprite = (isLong)?pulseModes[2]:pulseModes[3];
			}else{
				pulseImages[i].sprite = (isLong)?pulseModes[1]:pulseModes[0];
			}
			
		} 

		TestManager.Instance.AnswerReceived (selectedIndex == mistake);
	}

}
