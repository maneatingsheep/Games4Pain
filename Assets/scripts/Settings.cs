﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Settings : MonoBehaviour {

	public static Settings Instance;

	public enum SettingsTypes{ShortPulse, LongPulse, InterbeatPause, TemporalJudgementPulseLength,
		TemporalJudgementDelay, DelayBetweenQueAndStimulation, QuestionsWith2Beats, QuestionsWith3Beats,
		QuestionsWith4Beats, QuestionsWith5Beats, QuestionsWith6Beats,
		AmplitudeA, AmplitudeB, QuestionsPerRound, NumOfRewinds};


	public Setting ShortPulse;
	public Setting LongPulse;
	public Setting InterbeatPause;
	public Setting TemporalJudgementPulseLength;
	public Setting TemporalJudgementDelay;
	public Setting DelayBetweenQueAndStimulation;
	public Setting QuestionsWith2Beats;
	public Setting QuestionsWith3Beats;
	public Setting QuestionsWith4Beats;
	public Setting QuestionsWith5Beats;
	public Setting QuestionsWith6Beats;
	public Setting QuestionsPerRound;

	internal bool CalibrationNeeded = true;
	internal BodyPart BodyPartTreated;
	internal BodyPart BodyPartHealthy;

	public Setting AmplitudeA;
	public Setting AmplitudeB;

    public Setting NumOfRewinds;

    public BodyPart[] PossibleBodyParts;

    public int CurrentDifficulty;

    public readonly int TotalDifficulties = 6;

	public Dictionary<SettingsTypes, Setting> SettingsDict = new Dictionary<SettingsTypes, Setting>();

	public void Init(){
		Instance = this;

		SettingsDict [SettingsTypes.ShortPulse] = ShortPulse;
		SettingsDict [SettingsTypes.LongPulse] = LongPulse;
		SettingsDict [SettingsTypes.InterbeatPause] = InterbeatPause;
		SettingsDict [SettingsTypes.TemporalJudgementPulseLength] = TemporalJudgementPulseLength;
		SettingsDict [SettingsTypes.TemporalJudgementDelay] = TemporalJudgementDelay;
		SettingsDict [SettingsTypes.DelayBetweenQueAndStimulation] = DelayBetweenQueAndStimulation;
		SettingsDict [SettingsTypes.QuestionsWith2Beats] = QuestionsWith2Beats;
		SettingsDict [SettingsTypes.QuestionsWith3Beats] = QuestionsWith3Beats;
		SettingsDict [SettingsTypes.QuestionsWith4Beats] = QuestionsWith4Beats;
		SettingsDict [SettingsTypes.QuestionsWith5Beats] = QuestionsWith5Beats;
		SettingsDict [SettingsTypes.QuestionsWith6Beats] = QuestionsWith6Beats;
		
		SettingsDict [SettingsTypes.QuestionsPerRound] = QuestionsPerRound;

		SettingsDict [SettingsTypes.AmplitudeA] = AmplitudeA;
		SettingsDict [SettingsTypes.AmplitudeB] = AmplitudeB;
		SettingsDict [SettingsTypes.NumOfRewinds] = NumOfRewinds;

        //PlayerPrefs.DeleteAll();

        if (PlayerPrefs.HasKey ("CurrentDifficulty")) {
			for (int i = 0; i < TotalDifficulties; i++) {
				ShortPulse.Difficulties[i].CurrentVal = (byte)(PlayerPrefs.GetInt ("ShortPulse" + (i + 1)));
				LongPulse.Difficulties[i].CurrentVal = (byte)(PlayerPrefs.GetInt ("LongPulse" + (i + 1)));
				InterbeatPause.Difficulties[i].CurrentVal = (byte)(PlayerPrefs.GetInt ("InterbeatPause" + (i + 1)));
				TemporalJudgementPulseLength.Difficulties[i].CurrentVal = (byte)(PlayerPrefs.GetInt ("TemporalJudgementPulseLength" + (i + 1)));
				TemporalJudgementDelay.Difficulties[i].CurrentVal = (byte)(PlayerPrefs.GetInt ("TemporalJudgementDelay" + (i + 1)));
				DelayBetweenQueAndStimulation.Difficulties[i].CurrentVal = (byte)(PlayerPrefs.GetInt ("DelayBetweenQueAndStimulation" + (i + 1)));
				QuestionsWith2Beats.Difficulties[i].CurrentVal = (byte)(PlayerPrefs.GetInt ("QuestionsWith2Beats" + (i + 1)));
				QuestionsWith3Beats.Difficulties[i].CurrentVal = (byte)(PlayerPrefs.GetInt ("QuestionsWith3Beats" + (i + 1)));
				QuestionsWith4Beats.Difficulties[i].CurrentVal = (byte)(PlayerPrefs.GetInt ("QuestionsWith4Beats" + (i + 1)));
				QuestionsWith5Beats.Difficulties[i].CurrentVal = (byte)(PlayerPrefs.GetInt ("QuestionsWith5Beats" + (i + 1)));
				QuestionsWith6Beats.Difficulties[i].CurrentVal = (byte)(PlayerPrefs.GetInt ("QuestionsWith6Beats" + (i + 1)));
			}

			for (int i = 0; i < QuestionsPerRound.Difficulties.Length; i++) {
				QuestionsPerRound.Difficulties[i] .CurrentVal = (byte)PlayerPrefs.GetInt ("QuestionsPerRound" + (i + 1));
			}

            for (int i = 0; i < NumOfRewinds.Difficulties.Length; i++) {
                NumOfRewinds.Difficulties[i].CurrentVal = (byte)PlayerPrefs.GetInt("NumOfRewinds" + (i + 1));
            }

            AmplitudeA.CurrentGlobalVal = (byte)PlayerPrefs.GetInt ("AmplitudeA");
			AmplitudeB.CurrentGlobalVal = (byte)PlayerPrefs.GetInt ("AmplitudeB");
            

            CurrentDifficulty = (byte)PlayerPrefs.GetInt ("CurrentDifficulty");


		} else {
			for (int i = 0; i < TotalDifficulties; i++) {
				ShortPulse.Difficulties[i].CurrentVal = ShortPulse.Difficulties[i].DefaultVal;
				LongPulse.Difficulties[i].CurrentVal = LongPulse.Difficulties[i].DefaultVal;
				InterbeatPause.Difficulties[i].CurrentVal = InterbeatPause.Difficulties[i].DefaultVal;
				TemporalJudgementPulseLength.Difficulties[i].CurrentVal = TemporalJudgementPulseLength.Difficulties[i].DefaultVal;
				TemporalJudgementDelay.Difficulties[i].CurrentVal = TemporalJudgementDelay.Difficulties[i].DefaultVal;
				DelayBetweenQueAndStimulation.Difficulties[i].CurrentVal = DelayBetweenQueAndStimulation.Difficulties[i].DefaultVal;
				QuestionsWith2Beats.Difficulties[i].CurrentVal = QuestionsWith2Beats.Difficulties[i].DefaultVal;
				QuestionsWith3Beats.Difficulties[i].CurrentVal = QuestionsWith3Beats.Difficulties[i].DefaultVal;
				QuestionsWith4Beats.Difficulties[i].CurrentVal = QuestionsWith4Beats.Difficulties[i].DefaultVal;
				QuestionsWith5Beats.Difficulties[i].CurrentVal = QuestionsWith5Beats.Difficulties[i].DefaultVal;
				QuestionsWith6Beats.Difficulties[i].CurrentVal = QuestionsWith6Beats.Difficulties[i].DefaultVal;
			}


			for (int i = 0; i < QuestionsPerRound.Difficulties.Length; i++) {
				QuestionsPerRound.Difficulties[i].CurrentVal = QuestionsPerRound.Difficulties[i].DefaultVal;
			}
            for (int i = 0; i < NumOfRewinds.Difficulties.Length; i++) {
                NumOfRewinds.Difficulties[i].CurrentVal = NumOfRewinds.Difficulties[i].DefaultVal;
            }

            AmplitudeA.CurrentGlobalVal = AmplitudeA.DefaultGlobalVal;
			AmplitudeB.CurrentGlobalVal = AmplitudeB.DefaultGlobalVal;
            

            CurrentDifficulty = 0;

		}

        if (PlayerPrefs.HasKey("BodypartTreated")) {
            BodyPartTreated = GetBodypartByName(PlayerPrefs.GetString("BodypartTreated"));
            BodyPartHealthy = GetBodypartByName(PlayerPrefs.GetString("BodypartHealthy"));

        } else {
            BodyPartTreated = PossibleBodyParts[0];
            BodyPartHealthy = GetAllowedBodyParts(false)[0];
            SaveSettings();
        }
    }


	public void SaveSettings(){
		for (int i = 0; i < TotalDifficulties; i++) {
			PlayerPrefs.SetInt ("ShortPulse" + (i + 1), ShortPulse.Difficulties[i].CurrentVal);
			PlayerPrefs.SetInt ("LongPulse" + (i + 1), LongPulse.Difficulties[i].CurrentVal);
			PlayerPrefs.SetInt ("InterbeatPause" + (i + 1), InterbeatPause.Difficulties[i].CurrentVal);
			PlayerPrefs.SetInt ("TemporalJudgementPulseLength" + (i + 1), TemporalJudgementPulseLength.Difficulties[i].CurrentVal);
			PlayerPrefs.SetInt ("TemporalJudgementDelay" + (i + 1), TemporalJudgementDelay.Difficulties[i].CurrentVal);
			PlayerPrefs.SetInt ("DelayBetweenQueAndStimulation" + (i + 1), DelayBetweenQueAndStimulation.Difficulties[i].CurrentVal);
			PlayerPrefs.SetInt ("QuestionsWith2Beats" + (i + 1), QuestionsWith2Beats.Difficulties[i].CurrentVal);
			PlayerPrefs.SetInt ("QuestionsWith3Beats" + (i + 1), QuestionsWith3Beats.Difficulties[i].CurrentVal);
			PlayerPrefs.SetInt ("QuestionsWith4Beats" + (i + 1), QuestionsWith4Beats.Difficulties[i].CurrentVal);
			PlayerPrefs.SetInt ("QuestionsWith5Beats" + (i + 1), QuestionsWith5Beats.Difficulties[i].CurrentVal);
			PlayerPrefs.SetInt ("QuestionsWith6Beats" + (i + 1), QuestionsWith6Beats.Difficulties[i].CurrentVal);
		}

		for (int i = 0; i < QuestionsPerRound.Difficulties.Length; i++) {
			PlayerPrefs.SetInt ("QuestionsPerRound" + (i + 1), QuestionsPerRound.Difficulties[i].CurrentVal);
		}

        for (int i = 0; i < NumOfRewinds.Difficulties.Length; i++) {
            PlayerPrefs.SetInt("NumOfRewinds" + (i + 1), NumOfRewinds.Difficulties[i].CurrentVal);
        }

        PlayerPrefs.SetInt ("AmplitudeA", AmplitudeA.CurrentGlobalVal);
		PlayerPrefs.SetInt ("AmplitudeB", AmplitudeB.CurrentGlobalVal);
        

        PlayerPrefs.SetInt ("CurrentDifficulty", CurrentDifficulty);

        PlayerPrefs.SetString("BodypartTreated", BodyPartTreated.Name);
        PlayerPrefs.SetString("BodypartHealthy", BodyPartHealthy.Name);
        

        PlayerPrefs.Save ();
	}

	public void CalibrationDone(){
		CalibrationNeeded = false;
	}

	public byte GetProperty(SettingsTypes type, int index = 0){
		switch (SettingsDict[type].scope){
		case  Setting.SettingScope.Global:
			return SettingsDict[type].CurrentGlobalVal;
		case Setting.SettingScope.Difficulty:
			return SettingsDict[type].Difficulties[CurrentDifficulty].CurrentVal;
		case Setting.SettingScope.Array:
			return SettingsDict[type].Difficulties[index].CurrentVal;
		default:
			return SettingsDict[type].CurrentGlobalVal;

		}
	}

    public BodyPart GetBodypartByName(string partName) {
        return PossibleBodyParts.FirstOrDefault<BodyPart>(p => p.Name == partName);   
    }

    public List<BodyPart> GetAllowedBodyParts(bool forTreadedPart) {

        List<BodyPart> retval = new List<BodyPart>();

        BodyPart bannedPart = null;
        if (forTreadedPart) {
            bannedPart = BodyPartHealthy;
        } else {
            bannedPart = BodyPartTreated;
        }

        for (int i = 0; i < PossibleBodyParts.Length; i++) {
            if (PossibleBodyParts[i] != bannedPart) {
                bool allowPart = true;

                if (bannedPart != null){
                    for (int j = 0; j < PossibleBodyParts[i].ExcludedType.Length; j++) {
                        if (PossibleBodyParts[i].ExcludedType[j] == bannedPart.Type) {
                            allowPart = false;
                        }
                    }
                }
                

                if (allowPart) {
                    retval.Add(PossibleBodyParts[i]);
                }

            }
        }

        return retval;
    }

	[System.Serializable]
	public class Difficulty{
		public byte DefaultVal;
		public byte CurrentVal;

	}

	[System.Serializable]
	public class Setting{
		public enum SettingScope{Global, Difficulty, Array}
		public byte MinVal;
		public byte MaxVal;
		public Difficulty[] Difficulties;
		public byte DefaultGlobalVal;
		public byte CurrentGlobalVal;
		public string Description;
		public string Title;
		public SettingsTypes type;
		public SettingScope scope;
        public bool Testable;
    }
}
