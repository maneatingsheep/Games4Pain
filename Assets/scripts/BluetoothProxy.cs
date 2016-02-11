using UnityEngine;
using System.Collections;
using System;

public class BluetoothProxy : MonoBehaviour {

	public enum Channels{ChannelA, ChannelB};

	public static BluetoothProxy Instance;

	private byte[] data;
	private BluetoothDeviceScript _bluetoothDeviceScript;
	
	private string _serviceUUID;
	private string _characteristicUUID;
	private string _deviceUID;

	internal bool DeviceFound = false;
    private bool _disconnected = false;
    private bool _freeToSend = true;

    public void Init(){

		Instance = this;

		_bluetoothDeviceScript = BluetoothLEHardwareInterface.Initialize (
			true, false, 
			Scan, 
		(e) => {ScreenManager.Instance.ShowError("Error: " + e);});
        
    }

	public void Scan(){
		MainMenuScreen.Instance.ShowDeviceStatus("Scanning...");

		BluetoothLEHardwareInterface.ScanForPeripheralsWithServices (null,(devUID1, devName)=>{
			if (devName == "SRP-1"){
				BluetoothLEHardwareInterface.StopScan();
				_deviceUID = devUID1;

				MainMenuScreen.Instance.ShowDeviceStatus("Device Found - " + devName); 

                BluetoothLEHardwareInterface.ConnectToPeripheral(_deviceUID, null, null, 
				(devUID2, serviceUID, charUID) => {
					if (charUID.ToLower().Contains("2a2f")){
						_serviceUUID = serviceUID;
						_characteristicUUID = charUID;

                        _freeToSend = true;
                        DeviceFound = true;
                        
                    }

                    if (charUID.ToLower().Contains("2a2e")) {
                        BluetoothLEHardwareInterface.SubscribeCharacteristic(devUID2, serviceUID, charUID, null, 
                            (name, data) => {
                               if (data.Length == 2 && data[0] == 0x50 && data[1] == 0) {
                                    _freeToSend = true;
                                }
                            });
                    }


                },
                (device) => {
                    _disconnected = true;
                });
			}
		});
	}

    void Update() {
        if (_disconnected) {
            _disconnected = false;
            MainMenuScreen.Instance.ShowDeviceStatus("Disconnected");
            DeviceFound = false;
            Scan();
        }
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



    public void DeliverTest5(byte Chennel1, byte Chennel2, byte amp, byte pattern, byte numOfPulses) {
        data = new byte[]{0xc3, 0xd0,
            0x05, //test num
			Chennel1,
            0, //amp low
			Settings.Instance.GetProperty(Settings.SettingsTypes.ShortPulse),
            numOfPulses,
            Chennel2,
            amp,
            Settings.Instance.GetProperty(Settings.SettingsTypes.LongPulse),
            pattern,
            Settings.Instance.GetProperty(Settings.SettingsTypes.InterbeatPause),
            0xd2};

        TransmitData();
    }

    public void DeliverTest4(byte Chennel1, byte Chennel2, byte amp, byte pattern, byte numOfPulses) {
        data = new byte[]{0xc3, 0xd0,
            0x04, //test num
			Chennel1,
            0, //amp low
			Settings.Instance.GetProperty(Settings.SettingsTypes.ShortPulse),
            numOfPulses,
            Chennel2,
            amp,
            Settings.Instance.GetProperty(Settings.SettingsTypes.LongPulse),
            pattern,
            Settings.Instance.GetProperty(Settings.SettingsTypes.InterbeatPause),
            0xd2};

        TransmitData();
    }

    public void DeliverTest2(byte Chennel1, byte Chennel2, byte amp1, byte amp2) {
        data = new byte[]{0xc3, 0xd0,
            0x02, //test num
			Chennel1,
            amp1,
            Settings.Instance.GetProperty(Settings.SettingsTypes.TemporalJudjmentPulseLength),
            Settings.Instance.GetProperty(Settings.SettingsTypes.TemporalJudjmentDelay),
            Chennel2,
            amp2,
            Settings.Instance.GetProperty(Settings.SettingsTypes.TemporalJudjmentPulseLength),
            0, //delay - not used
			0, //time between pulses in 15mSec intervals - not used
			0xd2};

        TransmitData();
    }


    private void TransmitData(){
        if (!_freeToSend) return;
        _freeToSend = false;

        string str = "";
		for (int i = 0; i < data.Length; i++) {
			str += " " + data[i].ToString();
		}

        print(str);
        

		if (DeviceFound) {
			BluetoothLEHardwareInterface.WriteCharacteristic (_deviceUID, _serviceUUID, _characteristicUUID, data, data.Length, false, null);
		}

	}
}
