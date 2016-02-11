using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MainMenuScreen : MonoBehaviour {

	public static MainMenuScreen Instance;

	public Text DeviceFoundText;
	public Button ResumeButt;

	public void Init () {
		Instance = this;
		EnableResume (false);
	}

    public void ClearSettings() {
        PlayerPrefs.DeleteAll();
    }
    
	public void ShowDeviceStatus(string status){
        DeviceFoundText.text = status;
	}

	public void EnableResume(bool value){
		ResumeButt.gameObject.SetActive (value);
	}

	void OnEnable(){
		DeviceFoundText.gameObject.SetActive (true);
	}

	void OnDisable(){
		DeviceFoundText.gameObject.SetActive (false);
	}
}
