using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScreenManager : MonoBehaviour {

	public enum Screens {MainMenu, Game, End, Last};
	public enum Popups {Settings, Confirmation, Password, UserSetup };


	public Text ErrorText;

    //screens
	public MainMenuScreen MainMenuScreenInst;
	public TestManager TestManagerInst;
	public Conclusion ConclusionInst;

    //popups
	public SettingsScreen SettingsScreenInst;
	public Image ConfirmationPopoupInst;
	public Image PasswordPopoupInst;
    public ScreenUserSetup ScreenUserSetupInst;



    public Button SettingsButt;
	public Button BackButt;

	public static ScreenManager Instance;

    public Screens CurrentScreen;
	private Screens _lastScreen;



	public void Init(){

		Instance = this;


		MainMenuScreenInst.Init ();
		ScreenUserSetupInst.Init ();
		TestManagerInst.Init ();

		SettingsScreenInst.gameObject.SetActive (false);
		ConfirmationPopoupInst.gameObject.SetActive (false);
		PasswordPopoupInst.gameObject.SetActive (false);
    }

	public void ShowError(string error){
		ErrorText.text = error;
	}
	
	public void SetScreen(Screens screen){
		if (screen == Screens.Last) {
			SetScreen(_lastScreen);
		} else {

			_lastScreen = CurrentScreen;
            CurrentScreen = screen;

			MainMenuScreenInst.gameObject.SetActive (screen == Screens.MainMenu);
			TestManagerInst.gameObject.SetActive (screen == Screens.Game);
            TestProgress.Instance.gameObject.SetActive(screen == Screens.Game);
            ConclusionInst.gameObject.SetActive(screen == Screens.End);
            
            BackButt.gameObject.SetActive (screen != Screens.MainMenu);
		}
	}

	public void ShowPopoup(Popups popup, bool hideWindows = false) {
		SettingsButt.gameObject.SetActive (false);
		BackButt.gameObject.SetActive (false);

		ScreenUserSetupInst.gameObject.SetActive (false);
		TestManagerInst.gameObject.SetActive (false);

        if (hideWindows) {
            MainMenuScreenInst.gameObject.SetActive(false);
            TestManagerInst.gameObject.SetActive(false);
            TestProgress.Instance.gameObject.SetActive(false);
            ConclusionInst.gameObject.SetActive(false);
        }

		switch (popup) {
		    case Popups.Settings:
			    SettingsScreenInst.gameObject.SetActive (true);
			    break;
		    case Popups.Confirmation:
			    ConfirmationPopoupInst.gameObject.SetActive (true);
			    break;
            case Popups.Password:
                PasswordPopoupInst.gameObject.SetActive(true);
                PasswordPopoupInst.GetComponentInChildren<InputField>().text = "";
                break;
            case Popups.UserSetup:
                ScreenUserSetupInst.gameObject.SetActive(true);
                break;
                
        }

	}

	public void ClosePopup(){
		SettingsButt.gameObject.SetActive (true);
		BackButt.gameObject.SetActive (CurrentScreen != Screens.MainMenu);

		SettingsScreenInst.gameObject.SetActive (false);
		ConfirmationPopoupInst.gameObject.SetActive (false);
        PasswordPopoupInst.gameObject.SetActive (false);
        ScreenUserSetupInst.gameObject.SetActive(false);

        SetScreen (CurrentScreen);
	}
}
