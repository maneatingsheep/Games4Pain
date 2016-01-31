using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScreenManager : MonoBehaviour {

	public enum Screens {MainMenu, Instruction, Game, End, Last};
	public enum Popups {Settings, Confirmation};


	public Text ErrorText;

	public MainMenuScreen MainMenuScreenInst;
	public ScreenUserSetup ScreenUserSetupInst;
	public TestManager TestManagerInst;

	public SettingsScreen SettingsScreenInst;
	public Image ConfirmationPopoupInst;




	public Button SettingsButt;
	public Button BackButt;

	public static ScreenManager Instance;

	private Screens _currentScreen;
	private Screens _lastScreen;



	public void Init(){

		Instance = this;


		MainMenuScreenInst.Init ();
		ScreenUserSetupInst.Init ();
		TestManagerInst.Init ();

		SettingsScreenInst.gameObject.SetActive (false);
		ConfirmationPopoupInst.gameObject.SetActive (false);
	}

	public void ShowError(string error){
		ErrorText.text = error;
	}
	
	public void SetScreen(Screens screen){
		if (screen == Screens.Last) {
			SetScreen(_lastScreen);
		} else {

			_lastScreen = _currentScreen;
			_currentScreen = screen;
			MainMenuScreenInst.gameObject.SetActive (screen == Screens.MainMenu);
			ScreenUserSetupInst.gameObject.SetActive (screen == Screens.Instruction);
			TestManagerInst.gameObject.SetActive (screen == Screens.Game);

			BackButt.gameObject.SetActive (screen != Screens.MainMenu);
		}
	}

	public void ShowPopoup(Popups popup){
		SettingsButt.gameObject.SetActive (false);
		BackButt.gameObject.SetActive (false);

		ScreenUserSetupInst.gameObject.SetActive (false);
		TestManagerInst.gameObject.SetActive (false);



		switch (popup) {
		case Popups.Settings:
			SettingsScreenInst.gameObject.SetActive (true);
			break;
		case Popups.Confirmation:
			ConfirmationPopoupInst.gameObject.SetActive (true);
			break;
		}

	}

	public void ClosePopup(){
		SettingsButt.gameObject.SetActive (true);
		BackButt.gameObject.SetActive (_currentScreen != Screens.MainMenu);

		SettingsScreenInst.gameObject.SetActive (false);
		ConfirmationPopoupInst.gameObject.SetActive (false);

		SetScreen (_currentScreen);
	}
}
