using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class SettingsScreen : MonoBehaviour {

	public static SettingsScreen Instance;

	public Dropdown Drop;
	public Text Descrition;

	public List<SettingSlider> Sliders;
	

	private List<Settings.Setting> _settingsList = new List<Settings.Setting>();

	public void Init(){

		Instance = this;

        foreach (SettingSlider slider in Sliders) {
            slider.Init();
        }

        Drop.options.Clear ();

		foreach(KeyValuePair<Settings.SettingsTypes, Settings.Setting> setting in Settings.Instance.SettingsDict){
			Dropdown.OptionData item = new Dropdown.OptionData();
			item.text = setting.Value.Title;
			Drop.options.Add(item);
			_settingsList.Add(setting.Value);
		}

		Drop.value = 1;
		Drop.value = 0;
		
        
	}

	public void DropChanged(){
		FillData (_settingsList [Drop.value]);
	}

	private void FillData(Settings.Setting setting){
		
		Descrition.text = setting.Description;
		for (int i = 0; i < Sliders.Count; i++) {
            Sliders[i].SetMinMax(setting.MinVal, setting.MaxVal);
            
			switch (setting.scope){
			    case Settings.Setting.SettingScope.Global:
				    if (i == 0){
                        Sliders[i].SetValue(setting.CurrentGlobalVal, setting.Testable);
                    } else{
                        Sliders[i].Disabple();
					
				    }
				    break;
			    case Settings.Setting.SettingScope.Difficulty:
                    Sliders[i].SetValue(setting.Difficulties[i].CurrentVal, setting.Testable);
				    break;
			    case Settings.Setting.SettingScope.Array:
				    if (i < setting.Difficulties.Length){
					    Sliders[i].SetValue(setting.Difficulties[i].CurrentVal, setting.Testable);
				    }else{
                        Sliders[i].Disabple();
				    }
				    break;
			}
            
		}
        
	}

	public void SettingChanged(SettingSlider slider){
		
		UpfateRelevantSetting (Sliders.IndexOf(slider), slider.value);
	}

    public void SettingTested(SettingSlider slider) {
        SettingTested(_settingsList[Drop.value], Sliders.IndexOf(slider));
    }

    public void SettingTested(Settings.Setting setting, int testedDiff) {
        int diff = Settings.Instance.CurrentDifficulty;
        Settings.Instance.CurrentDifficulty = testedDiff;

        switch (setting.type) {
            case Settings.SettingsTypes.ShortPulse:
                BluetoothProxy.Instance.DeliverTest5(1, 1, Settings.Instance.GetProperty(Settings.SettingsTypes.AmplitudeA), 0, 1);
                break;
            case Settings.SettingsTypes.LongPulse:
                BluetoothProxy.Instance.DeliverTest5(1, 1, Settings.Instance.GetProperty(Settings.SettingsTypes.AmplitudeA), 1, 1);
                break;
            case Settings.SettingsTypes.InterbeatPause:
                BluetoothProxy.Instance.DeliverTest5(1, 1, Settings.Instance.GetProperty(Settings.SettingsTypes.AmplitudeA), 3, 2);
                break;
            case Settings.SettingsTypes.TemporalJudgementPulseLength:
                BluetoothProxy.Instance.DeliverTest2(1, 1, Settings.Instance.GetProperty(Settings.SettingsTypes.AmplitudeA), Settings.Instance.GetProperty(Settings.SettingsTypes.AmplitudeB));
                break;
            case Settings.SettingsTypes.TemporalJudgementDelay:
                BluetoothProxy.Instance.DeliverTest2(1, 1, Settings.Instance.GetProperty(Settings.SettingsTypes.AmplitudeA), Settings.Instance.GetProperty(Settings.SettingsTypes.AmplitudeB));
                break;
            case Settings.SettingsTypes.DelayBetweenQueAndStimulation:
                break;
            case Settings.SettingsTypes.AmplitudeA:
                BluetoothProxy.Instance.DeliverTest5(1, 1, Settings.Instance.GetProperty(Settings.SettingsTypes.AmplitudeA), 1, 1);
                break;
            case Settings.SettingsTypes.AmplitudeB:
                BluetoothProxy.Instance.DeliverTest5(2, 2, Settings.Instance.GetProperty(Settings.SettingsTypes.AmplitudeB), 1, 1);
                break;
        }

        Settings.Instance.CurrentDifficulty = diff;

    }


    private void UpfateRelevantSetting(int index, byte value){
		if (_settingsList [Drop.value].scope == Settings.Setting.SettingScope.Global && index ==  0) {
			_settingsList [Drop.value].CurrentGlobalVal = value;
		} else {
			_settingsList [Drop.value].Difficulties[index].CurrentVal = value;
		}
	}

	public void SubmitClicked(){
		Settings.Instance.SaveSettings();
		ScreenManager.Instance.ClosePopup ();
	}
}
