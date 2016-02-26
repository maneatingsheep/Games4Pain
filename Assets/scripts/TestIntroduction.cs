using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;

public class TestIntroduction : MonoBehaviour {

    public Text TopHeader;
    public Text Description;
    public Image RoundImage;

   

    internal void SetRoundData(int currenttest, int TotalTests, BasicTest test) {
        TopHeader.text = "Round " + (currenttest) + " / " + TotalTests;
        Description.text = "<b>" + test.NumOfQuestions + "</b> " +  test.Description;
        RoundImage.sprite = test.IntroImage;
    }
}
