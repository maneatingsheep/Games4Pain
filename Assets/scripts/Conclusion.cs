using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class Conclusion : MonoBehaviour {
     
    static public Conclusion Instance;

    public Text Recomendation;
    public Text[] table; 
    public Text[] tableNumbers;

    void Awake() {
        Instance = this;
    }

    public void ShowConclusion(List<CompletedTest> tests) {


        int totalQuestions = 0;
        int correctQuestions = 0;

        for (int i = 0; i < TestManager.Instance.TestSeq.Length; i++) {

            bool found = false;


            foreach (CompletedTest test in tests) {
                if (test.TestType == TestManager.Instance.TestSeq[i]) {
                    found = true;
                    totalQuestions += test.TotalQuestions;
                    correctQuestions += test.CorrectQuestions;

                    tableNumbers[i].text = test.CorrectQuestions + "/" + test.TotalQuestions;
                }
            }

            table[i].gameObject.SetActive(found);
        }

        float success = (float)correctQuestions / totalQuestions;

        if (success < 0.5f && Settings.Instance.CurrentDifficulty > 0) {
            Recomendation.text = "Mission failed\nConsider selecting an easier difficulty";
        } else if (success > 0.75f && Settings.Instance.CurrentDifficulty < Settings.Instance.TotalDifficulties - 1) {
            Recomendation.text = "Great job!\nConsider selecting a harder difficulty";
        } else if (success < 0.75f) {
            Recomendation.text = "Good work. You got home safe\nKeep working";
        }
    }

    public void OkClicked() {
        FlowManager.Instance.ConclusionOkclicked();
    }
}
