using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class ScreenUserSetup : MonoBehaviour {

	private enum Steps{None, AssignA, AssignB, TestA, TestB, Diff};

	public static ScreenUserSetup Instance;

	private Steps step;

	public Text InstructionText;
	public InputField BodyPartField;
	public Button OkButt;
	public Slider IntensitySlider;
	public Button IntensityTest;
	public Image DifficultyGroup;

	public Text SliderValText;

	public List<Toggle> DiffToggles;
	public ToggleGroup DiffToggleGroup;

	public void Init () {
		Instance = this;
	}

	public void StartUserSetup(){
		step = Steps.None;
		NextStep ();
	}

	public void NextStep(){
		switch (step) {
		    case Steps.None:
			    step = Steps.AssignA;
			    InstructionText.text = "Assign stimulator A to";
			    BodyPartField.gameObject.SetActive(true);
			    BodyPartField.text = "";
			    IntensitySlider.gameObject.SetActive(false);
			    IntensityTest.gameObject.SetActive(false);
			    SliderValText.gameObject.SetActive(false);
			    DifficultyGroup.gameObject.SetActive(false);
			    break;
		    case Steps.AssignA:

                if (BodyPartField.text == "") BodyPartField.text = "Body part A";

                Settings.Instance.BodyPartA = BodyPartField.text;
				step = Steps.AssignB;
				InstructionText.text = "Assign timulator B to";
				BodyPartField.text = "";
				IntensitySlider.gameObject.SetActive(false);
				IntensityTest.gameObject.SetActive(false);
			    

			    break;
		    case Steps.AssignB:

                if (BodyPartField.text == "") BodyPartField.text = "Body part B";

                Settings.Instance.BodyPartB = BodyPartField.text;
			    step = Steps.TestA;
			    BodyPartField.gameObject.SetActive(false);
			    InstructionText.text = "Adjust intensity level of stimulator A"; 
			    IntensitySlider.gameObject.SetActive(true);
			    IntensityTest.gameObject.SetActive(true);
			    IntensitySlider.value = Settings.Instance.AmplitudeA.CurrentGlobalVal;
			    SliderValText.gameObject.SetActive(true);
			    SliderValText.text = IntensitySlider.value.ToString();
			
			    break;
		    case Steps.TestA:
			    step = Steps.TestB;
			    InstructionText.text = "Adjust intensity level of stimulator B"; 
			    IntensitySlider.gameObject.SetActive(true);
			    IntensityTest.gameObject.SetActive(true);
			    IntensitySlider.value = Settings.Instance.AmplitudeB.CurrentGlobalVal;
			    SliderValText.text = IntensitySlider.value.ToString();
			    break;
		    case Steps.TestB:
			    step = Steps.Diff;
			    InstructionText.text = "Set Difficulty"; 
			    IntensitySlider.gameObject.SetActive(false);
			    IntensityTest.gameObject.SetActive(false);
			    SliderValText.gameObject.SetActive(false);
			    DifficultyGroup.gameObject.SetActive(true);

			    DiffToggles[Settings.Instance.CurrentDifficulty].isOn = true;
			    DiffToggleGroup.NotifyToggleOn(DiffToggles[Settings.Instance.CurrentDifficulty]);

			    break;
		    case Steps.Diff:
			    FlowManager.Instance.UserSetupEnded();
			    break;
		}
	}

	public void DiffClicked(Toggle tog){
		Settings.Instance.CurrentDifficulty = (byte)DiffToggles.IndexOf(tog);
		Settings.Instance.SaveSettings ();
	}

	public void ChangeIntensity(){
		SliderValText.text = IntensitySlider.value.ToString();
		if (step == Steps.TestA) {
			Settings.Instance.AmplitudeA.CurrentGlobalVal = (byte)IntensitySlider.value;
		} else {
			Settings.Instance.AmplitudeB.CurrentGlobalVal = (byte)IntensitySlider.value;
		}

		Settings.Instance.SaveSettings();
	}

	public void TestIntensity(){
		BluetoothProxy.Instance.TestIntensity ((step == Steps.TestA) ? BluetoothProxy.Channels.ChannelA : BluetoothProxy.Channels.ChannelB);
	}
}
