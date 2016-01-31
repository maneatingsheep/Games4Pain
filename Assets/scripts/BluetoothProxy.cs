using UnityEngine;
using System.Collections;

public class BluetoothProxy : MonoBehaviour {

	public enum Channels{ChannelA, ChannelB};

	public static BluetoothProxy Instance;

	private byte[] data;
	//private BluetoothDeviceScript _bluetoothDeviceScript;
	
	private string _serviceUUID;
	private string _characteristicUUID;
	private string _deviceUID;

	internal bool DeviceFound = false;

	internal bool UseRealDevice = true;

	public void Init(){

		Instance = this;

		/*_bluetoothDeviceScript = */BluetoothLEHardwareInterface.Initialize (
			true, false, 
			Scan, 
		(e) => {ScreenManager.Instance.ShowError("Error: " + e);});

	}

	public void Scan(){
		MainMenuScreen.Instance.ShowScanning(); 
		BluetoothLEHardwareInterface.ScanForPeripheralsWithServices (null,(devUID1, devName)=>{
			if (devName == "SRP-1"){
				BluetoothLEHardwareInterface.StopScan();
				_deviceUID = devUID1;
				MainMenuScreen.Instance.ShowDeviceFound(devName); 
				BluetoothLEHardwareInterface.ConnectToPeripheral(_deviceUID, null, null, 
				(devUID2, serviceUID, charUID) => {
					if (charUID.Contains("2a2f")){
						_serviceUUID = serviceUID;
						_characteristicUUID = charUID/*"2A2F"*/;
						DeviceFound = true;
					}
					
				}, 
				null);
			}
		});
	}

	public void TestIntensity(Channels channel){

		data = new byte[]{0xC3, 0xD0, 0x05, 
			(byte)((channel == Channels.ChannelA)?0x01:0x02), 
			0x0, 0x1, 0x02,
			(byte)((channel == Channels.ChannelA)?0x01:0x02), 
			(channel == Channels.ChannelA)?Settings.Instance.AmplitudeA.CurrentGlobalVal:Settings.Instance.AmplitudeB.CurrentGlobalVal, 
			0x42, 0x03, 0x02, 0xD2};

		TransmitData ();
	}

	public void DeliverPattern(byte[] pattern){
		data = pattern;
		TransmitData ();
	}

	private void TransmitData(){
		string str = "";
		for (int i = 0; i < data.Length; i++) {
			str += " " + data[i].ToString();
		}
		ScreenManager.Instance.ShowError(str);
		if (UseRealDevice) {
			BluetoothLEHardwareInterface.WriteCharacteristic (_deviceUID, _serviceUUID, _characteristicUUID, data, data.Length, false, null);
		}

	}
}
