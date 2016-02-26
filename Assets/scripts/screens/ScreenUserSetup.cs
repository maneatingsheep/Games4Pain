using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class ScreenUserSetup : MonoBehaviour {

	private enum Steps{None, Amplitudes, Diff};

	public static ScreenUserSetup Instance;

	private Steps step;

	public Text InstructionText;
	public Button OkButt;
	public SettingSlider[] IntensitySliders;
	public Dropdown[] BodypartSelectors;
    public Image AmplitudesCont;
	public Image DifficultyGroup;

    public List<Toggle> DiffToggles;
	public ToggleGroup DiffToggleGroup;

	public void Init () {
		Instance = this;
        IntensitySliders[0].Init();
        IntensitySliders[1].Init();
    }

	public void StartUserSetup(){
		step = Steps.None;
		NextStep ();
	}

	public void NextStep(){
		switch (step) {
		    case Steps.None:
			    step = Steps.Amplitudes;
			    InstructionText.text = "Assign amplitudes";
                AmplitudesCont.gameObject.SetActive(true);
                
                FillDropdown(BodypartSelectors[0]);
                BodypartSelectors[0].value = Settings.Instance.GetAllowedBodyParts(true).IndexOf(Settings.Instance.BodyPartTreated);
                BodypartSelectors[0].RefreshShownValue();
                FillDropdown(BodypartSelectors[1]);
                BodypartSelectors[1].value = Settings.Instance.GetAllowedBodyParts(false).IndexOf(Settings.Instance.BodyPartHealthy);
                BodypartSelectors[1].RefreshShownValue();
                DifficultyGroup.gameObject.SetActive(false);
                IntensitySliders[0].SetValue(Settings.Instance.GetProperty(Settings.SettingsTypes.AmplitudeA), true);
                IntensitySliders[1].SetValue(Settings.Instance.GetProperty(Settings.SettingsTypes.AmplitudeB), true);
                break;
		    
		    case Steps.Amplitudes:
                
                step = Steps.Diff;
			    InstructionText.text = "Set Difficulty";
                AmplitudesCont.gameObject.SetActive(false);
			    DifficultyGroup.gameObject.SetActive(true);

			    DiffToggles[Settings.Instance.CurrentDifficulty].isOn = true;
			    DiffToggleGroup.NotifyToggleOn(DiffToggles[Settings.Instance.CurrentDifficulty]);

			    break;
		    case Steps.Diff:
			    FlowManager.Instance.UserSetupEnded();
			    break;
		}
	}

    private void FillDropdown(Dropdown partDrop) {
        partDrop.options.Clear();

        foreach (BodyPart bp in Settings.Instance.GetAllowedBodyParts(partDrop == BodypartSelectors[0])) {
            partDrop.options.Add(new Dropdown.OptionData() { text = bp.Name });
        }
        
    }

    public void PartDropChanged(Dropdown partDrop) {
        if (partDrop == BodypartSelectors[0]) {
            Settings.Instance.BodyPartTreated = Settings.Instance.GetBodypartByName(partDrop.captionText.text);
            FillDropdown(BodypartSelectors[1]);
        } else {
            Settings.Instance.BodyPartHealthy = Settings.Instance.GetBodypartByName(partDrop.captionText.text);
            FillDropdown(BodypartSelectors[0]);
        }

        Settings.Instance.SaveSettings();
    }

	public void DiffClicked(Toggle tog){
		Settings.Instance.CurrentDifficulty = (byte)DiffToggles.IndexOf(tog);
		Settings.Instance.SaveSettings ();
	}

	public void ChangeIntensity(SettingSlider slider){
		if (slider == IntensitySliders[0]) {
			Settings.Instance.AmplitudeA.CurrentGlobalVal = slider.value;
		} else {
			Settings.Instance.AmplitudeB.CurrentGlobalVal = slider.value;
		}

		
	}

	public void TestIntensity(SettingSlider slider) {
        if (slider == IntensitySliders[0]) {
            BluetoothProxy.Instance.TestIntensity(BluetoothProxy.Channels.ChannelA);
        } else {
            BluetoothProxy.Instance.TestIntensity(BluetoothProxy.Channels.ChannelB);
        }
            
	}
}
