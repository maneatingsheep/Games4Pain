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

	public void ShowScanning(){
		DeviceFoundText.text = "Scanning...";
	}
	
	public void ShowDeviceFound(string deviceName){
		DeviceFoundText.text = "Device Found - " + deviceName;
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
