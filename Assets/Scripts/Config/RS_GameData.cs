using UnityEngine;
using System;
using System.Collections;

public class RS_GameData {

	public static Action<RS_CarTemplate> MenuCarIdChanged = delegate {};
	public static Action<RS_CarTemplate> PlayerCarIdChanged = delegate {};


	private const string SOUND_STATE = "SOUND_STATE";
	public static bool IsGameSoundActive {
		get {
			if (PlayerPrefs.HasKey (SOUND_STATE) && (PlayerPrefs.GetInt (SOUND_STATE) == 0) ) {
				return false;
			}

			return true;
		}
		set {
			if (value) {
				PlayerPrefs.SetInt (SOUND_STATE, 1);
			} else {
				PlayerPrefs.SetInt (SOUND_STATE, 0);
			}
		}
	}


	private const string MENU_CAR = "MENU_CAR";
	public static string MenuCarId {
		get {
			if (PlayerPrefs.HasKey (MENU_CAR)) {
				return PlayerPrefs.GetString(MENU_CAR);
			} else {
				return RS_GameConfig.DEFAULT_CAR_ID;
			}
		}

		set {
			PlayerPrefs.SetString(MENU_CAR, value);
			MenuCarIdChanged(RS_PlayerData.Instance.GetCarTemplate(value));
		}
	}


	private const string PLAYER_CAR = "PLAYER_CAR";
	public static string PlayerCarId {
		get {
			if (PlayerPrefs.HasKey (PLAYER_CAR)) {
				return PlayerPrefs.GetString(PLAYER_CAR);
			} else {
				return RS_GameConfig.DEFAULT_CAR_ID;
			}
		}

		set {
			PlayerPrefs.SetString(PLAYER_CAR, value);
			PlayerCarIdChanged(RS_PlayerData.Instance.GetCarTemplate(value));
		}
	}



}
