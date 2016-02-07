using UnityEngine;
using System.Collections;

public class FlowManager : MonoBehaviour {

	public enum ConfirmedActions{MainMenu, Quit};

	public static FlowManager Instance;

	public Settings SettingsInst;
	public SettingsScreen SettingsScreenInst;
	public ScreenManager ScreenManagerInst;
	public BluetoothProxy BluetoothProxyInst;

	private ConfirmedActions _pendingAction;

	void Awake () {
		Instance = this;
	}

	void Start () {

		SettingsInst.Init ();

		SettingsScreenInst.Init ();
		ScreenManagerInst.Init();
		BluetoothProxyInst.Init();

		ScreenManager.Instance.SetScreen(ScreenManager.Screens.MainMenu);

		BluetoothProxy.Instance.Scan ();


	}

	public void Play(){
		BluetoothProxy.Instance.UseRealDevice = BluetoothProxy.Instance.DeviceFound;
		if (Settings.Instance.CalibrationNeeded) {
			ShowInstruction ();
		} else {
			StartGame();
		}
	}

	public void ResumeGame(){
		MainMenuScreen.Instance.EnableResume(false);
		ScreenManager.Instance.SetScreen(ScreenManager.Screens.Last);
	}

	public void ShowInstruction(){
		ScreenManager.Instance.SetScreen(ScreenManager.Screens.Instruction);
		ScreenUserSetup.Instance.StartCalibration();
	}

	public void CalibrationEnded(){
		Settings.Instance.CalibrationDone ();
		StartGame ();
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
		ScreenManager.Instance.ShowPopoup (ScreenManager.Popups.Settings);
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
			}
			break;
		case ConfirmedActions.Quit:
			if (confirmed) {
				Application.Quit ();
			}
			break;
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
