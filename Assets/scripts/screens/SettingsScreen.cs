using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class SettingsScreen : MonoBehaviour {

	public static SettingsScreen Instance;

	public Dropdown Drop;
	public Text Descrition;

	public List<Slider> Sliders;
	public List<InputField> Texts;

	private bool _isAutofilling = false;

	private List<Settings.Setting> _settingsList = new List<Settings.Setting>();

	public void Init(){

		Instance = this;

		Drop.options.Clear ();

		foreach(KeyValuePair<Settings.SettingsTypes, Settings.Setting> setting in Settings.Instance.SettingsDict){
			Dropdown.OptionData item = new Dropdown.OptionData();
			item.text = setting.Value.Title;
			Drop.options.Add(item);
			_settingsList.Add(setting.Value);
		}

		Drop.value = 1;
		Drop.value = 0;
		FillData (_settingsList [Drop.value]);
	}

	public void DropChanged(){
		FillData (_settingsList [Drop.value]);
	}

	private void FillData(Settings.Setting setting){
		_isAutofilling = true;
		Descrition.text = setting.Description;
		for (int i = 0; i < Sliders.Count; i++) {
			Sliders[i].minValue = setting.MinVal;
			Sliders[i].maxValue = setting.MaxVal;
			switch (setting.scope){
			case Settings.Setting.SettingScope.Global:
				if (i == 0){
					Sliders[i].value = setting.CurrentGlobalVal;
					Sliders[i].gameObject.SetActive(true);
					Texts[i].gameObject.SetActive(true);
				}else{
					Sliders[i].gameObject.SetActive(false);
					Texts[i].gameObject.SetActive(false);
				}
				break;
			case Settings.Setting.SettingScope.Difficulty:

				Sliders[i].value = setting.Difficulties[i].CurrentVal;

				Sliders[i].gameObject.SetActive(true);
				Texts[i].gameObject.SetActive(true);

				break;
			case Settings.Setting.SettingScope.Array:
				if (i < setting.Difficulties.Length){
					Sliders[i].value = setting.Difficulties[i].CurrentVal;
					
					Sliders[i].gameObject.SetActive(true);
					Texts[i].gameObject.SetActive(true);
				}else{
					Sliders[i].gameObject.SetActive(false);
					Texts[i].gameObject.SetActive(false);
				}
				break;
			}

			Texts[i].text = Sliders[i].value.ToString();
		}

		_isAutofilling = false;
	}

	public void SliderMoved(Slider slider){
		if (_isAutofilling) return;

		int sliderIndex = Sliders.IndexOf (slider);
		Texts[sliderIndex].text = slider.value.ToString();

		UpfateRelevantSetting (sliderIndex, (byte)slider.value);
	}

	public void ValueInserted(InputField field){
		_isAutofilling = true;

		int sliderIndex = Texts.IndexOf (field);

		byte val = byte.Parse (field.text);

		val = (byte)Mathf.Max (val, (byte)Sliders [Texts.IndexOf (field)].minValue);
		val = (byte)Mathf.Min (val, (byte)Sliders [Texts.IndexOf (field)].maxValue);

		field.text = val.ToString ();
		Sliders [sliderIndex].value = val;

		UpfateRelevantSetting (sliderIndex, val);

		_isAutofilling = false;
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
