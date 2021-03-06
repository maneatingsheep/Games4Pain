using UnityEngine;
using System.Collections;

public class PatternTest : BasicTest {
		
	protected byte numOfPulses = 0;
	protected byte correctPattern = 0;
	protected byte wrongPattern = 0;

    protected float correctPatternTime;
    protected float wrongPatternTime;

    protected int mistake;

	protected string bodyPart1;
	protected string bodyPart2;
	protected byte channelByte1;
	protected byte channelByte2;
	protected byte amp1;
	protected byte amp2;


	override public void Reset(){

		base.Reset ();

		int totalPulseOptions = 0;
		totalPulseOptions += Settings.Instance.GetProperty (Settings.SettingsTypes.QuestionsWith2Beats);
		totalPulseOptions += Settings.Instance.GetProperty (Settings.SettingsTypes.QuestionsWith3Beats);
		totalPulseOptions += Settings.Instance.GetProperty (Settings.SettingsTypes.QuestionsWith4Beats);
		totalPulseOptions += Settings.Instance.GetProperty (Settings.SettingsTypes.QuestionsWith5Beats);
		totalPulseOptions += Settings.Instance.GetProperty (Settings.SettingsTypes.QuestionsWith6Beats);

		int pulseSelection = Random.Range (0, totalPulseOptions);

		numOfPulses = 6;

		if (pulseSelection < totalPulseOptions - Settings.Instance.GetProperty (Settings.SettingsTypes.QuestionsWith6Beats)){
			numOfPulses--;
			totalPulseOptions -= Settings.Instance.GetProperty (Settings.SettingsTypes.QuestionsWith6Beats);
		}
		if (pulseSelection < totalPulseOptions - Settings.Instance.GetProperty (Settings.SettingsTypes.QuestionsWith5Beats)){
			numOfPulses--;
			totalPulseOptions -= Settings.Instance.GetProperty (Settings.SettingsTypes.QuestionsWith5Beats);
		}
		if (pulseSelection < totalPulseOptions - Settings.Instance.GetProperty (Settings.SettingsTypes.QuestionsWith4Beats)){
			numOfPulses--;
			totalPulseOptions -= Settings.Instance.GetProperty (Settings.SettingsTypes.QuestionsWith4Beats);
		}
		if (pulseSelection < totalPulseOptions - Settings.Instance.GetProperty (Settings.SettingsTypes.QuestionsWith3Beats)){
			numOfPulses--;
			totalPulseOptions -= Settings.Instance.GetProperty (Settings.SettingsTypes.QuestionsWith3Beats);
		}
		

		if (channel == BluetoothProxy.Channels.ChannelA) {
			bodyPart1 = Settings.Instance.BodyPartTreated.Name.ToUpper();
			bodyPart2 = Settings.Instance.BodyPartHealthy.Name.ToUpper();
			channelByte1 = 0x01;
			channelByte2 = 0x02;
			amp1 = Settings.Instance.AmplitudeA.CurrentGlobalVal;
			amp2 = Settings.Instance.AmplitudeB.CurrentGlobalVal;
		} else {
			bodyPart1 = Settings.Instance.BodyPartHealthy.Name.ToUpper();
			bodyPart2 = Settings.Instance.BodyPartTreated.Name.ToUpper();
			channelByte1 = 0x02;
			channelByte2 = 0x01;
			amp1 = Settings.Instance.AmplitudeB.CurrentGlobalVal;
			amp2 = Settings.Instance.AmplitudeA.CurrentGlobalVal;
		}
		
		
		correctPattern = 0;
		
		for (int i = 0; i < numOfPulses; i++) {
			byte pulse = (byte)Random.Range(0, 2);
			correctPattern |= (byte)(pulse << i);
		}

		mistake = Random.Range (0, numOfPulses);
		wrongPattern = (byte)(correctPattern ^ (1 << mistake));

        correctPatternTime = CalculatePatternTime(correctPattern, numOfPulses);
        wrongPatternTime = CalculatePatternTime(wrongPattern, numOfPulses);
    }


    private float CalculatePatternTime(byte pattern, byte numOfPulses) {

        float res = 0;

        for (int i = 0; i < numOfPulses; i++) {

            if ((pattern & (1 << i)) == 0) {
                res += Settings.Instance.GetProperty(Settings.SettingsTypes.ShortPulse) * 0.015f;
            } else {
                res += Settings.Instance.GetProperty(Settings.SettingsTypes.LongPulse) * 0.015f;
            }

        }

        res += Settings.Instance.GetProperty(Settings.SettingsTypes.InterbeatPause) * 0.015f * (numOfPulses - 1);

        return res;
    }
}

