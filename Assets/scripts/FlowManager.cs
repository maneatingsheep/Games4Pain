using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class FlowManager : MonoBehaviour {

	public enum ConfirmedActions{MainMenu, Quit, Settings};

	public static FlowManager Instance;

	public Settings SettingsInst;
	public SettingsScreen SettingsScreenInst;
	public ScreenManager ScreenManagerInst;
	public BluetoothProxy BluetoothProxyInst;

	private ConfirmedActions _pendingAction;
    private bool _passwordCleared = false;
    private bool _playAfterSetup = false;

    void Awake () {
		Instance = this;
	}

	void Start () {

		SettingsInst.Init ();

		SettingsScreenInst.Init ();
		ScreenManagerInst.Init();
		BluetoothProxyInst.Init();

		ScreenManager.Instance.SetScreen(ScreenManager.Screens.MainMenu);

		//BluetoothProxy.Instance.Scan ();
        
	}

	public void Play(){

		if (Settings.Instance.CalibrationNeeded) {
            _playAfterSetup = true;
            StartUserSetup();
		} else {
			StartGame();
		}
	}

	public void ResumeGame(){
		MainMenuScreen.Instance.EnableResume(false);
		ScreenManager.Instance.SetScreen(ScreenManager.Screens.Last);
        TestManager.Instance.Resume();
    }

	public void StartUserSetup(){
        if (ScreenManager.Instance.CurrentScreen == ScreenManager.Screens.Game) {
            TestManager.Instance.Pause();
        }
		ScreenManager.Instance.ShowPopoup(ScreenManager.Popups.UserSetup, true);
		ScreenUserSetup.Instance.StartUserSetup();
	}

	public void UserSetupEnded(){
		Settings.Instance.CalibrationDone ();
        ScreenManager.Instance.ClosePopup();

        if (ScreenManager.Instance.CurrentScreen == ScreenManager.Screens.Game) {
            TestManager.Instance.Resume();
        }
        if (_playAfterSetup) {
            StartGame();
        }
        
        _playAfterSetup = false;
    }

	public void StartGame(){
		ScreenManager.Instance.SetScreen(ScreenManager.Screens.Game);
        TestManager.Instance.Reset();
        TestManager.Instance.NextTest ();
		TestManager.Instance.StartTest ();

	}
    

	public void TestFinished(){
		if (TestManager.Instance.NextTest ()) {
			TestManager.Instance.StartTest ();
		} else {
			ScreenManager.Instance.SetScreen(ScreenManager.Screens.End);
            Conclusion.Instance.ShowConclusion(TestManager.Instance.CompletedTests);
		}
	}

	public void SettingsClicked(){
        if (_passwordCleared) {
		    ScreenManager.Instance.ShowPopoup (ScreenManager.Popups.Settings, true);
        } else {
            ScreenManager.Instance.ShowPopoup(ScreenManager.Popups.Password);
        }
	}

	public void BackClicked(){
		_pendingAction = ConfirmedActions.MainMenu;
		ScreenManager.Instance.ShowPopoup (ScreenManager.Popups.Confirmation);
	}

	public void BackConfirmed(bool confirmed){
		ScreenManager.Instance.ClosePopup ();

		switch (_pendingAction) {
		    case ConfirmedActions.MainMenu:

			    if (confirmed) {
				    MainMenuScreen.Instance.EnableResume(true);
				    ScreenManager.Instance.SetScreen(ScreenManager.Screens.MainMenu);
                    TestManager.Instance.Pause();
                }
			    break;
		    case ConfirmedActions.Quit:
			    if (confirmed) {
				    Application.Quit ();
			    }
			    break;
		}

	}

    public void PasswordConfirmed(InputField passField) {
        ScreenManager.Instance.ClosePopup();
        if (passField.text.ToLower() == "admin") {
            _passwordCleared = true;
            ScreenManager.Instance.ShowPopoup(ScreenManager.Popups.Settings, true);
        }
    }

    public void ConclusionOkclicked() {
        ScreenManager.Instance.SetScreen(ScreenManager.Screens.MainMenu);
    }

    public void QuitClicked(){
		_pendingAction = ConfirmedActions.Quit;
		ScreenManager.Instance.ShowPopoup (ScreenManager.Popups.Confirmation);
	}
}
