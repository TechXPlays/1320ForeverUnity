using UnityEngine;
using System;
using System.Text;
using System.Collections.Generic;

public class RS_PrefsDataLoader : IPlayerDataLoader {

	public Dictionary<string, int> IntValues 		= new Dictionary<string, int>();
	public Dictionary<string, float> FloatValues 	= new Dictionary<string, float>();
	public Dictionary<string, string> StringValues 	= new Dictionary<string, string>();


	
	public int GetInt (string key) {
		if(IntValues.ContainsKey(key)) {
			return IntValues[key];
		} else {

			int val = 0;

			try {
				string str = GetPrefString(key);
				if(str != string.Empty) {
					val = Convert.ToInt32(str);
				}
			} catch(Exception ex) {
				Debug.LogError(ex.Message);
			}

			IntValues.Add(key, val);
			return val;
		}

	}


	public void SetInt (string key, int value) {
		if(IntValues.ContainsKey(key)) {
			IntValues[key] = value;
		} else {
			IntValues.Add(key, value);
		}

		SavePrefString(key, value.ToString());
	}





	public string GetString (string key) {
		if(StringValues.ContainsKey(key)) {
			return StringValues[key];
		} else {
			string val = string.Empty;

			try {
				val = GetPrefString(key);
			} catch(Exception ex) {
				Debug.LogWarning(key);
				Debug.LogError(ex.Message);
			}

			StringValues.Add(key, val);
			return val;
		}
	}

	public void SetString (string key, string value) {
		if(StringValues.ContainsKey(key)) {
			StringValues[key] = value;
		} else {
			StringValues.Add(key, value);
		}
		
		SavePrefString(key, value);
	}

	

	public float GetFloat(string key) {
		if(FloatValues.ContainsKey(key)) {
			return FloatValues[key];
		} else {
			
			float val = 0;
			
			try {
				string str = GetPrefString(key);
				if(str != string.Empty) {
					val = Convert.ToSingle(str);
				}
			} catch(Exception ex) {
				Debug.LogError(ex.Message);
			}
			
			FloatValues.Add(key, val);
			return val;
		}
	}

	public void SetFloat(string key, float value) {
		if(FloatValues.ContainsKey(key)) {
			FloatValues[key] = value;
		} else {
			FloatValues.Add(key, value);
		}
		
		SavePrefString(key, value.ToString());
	}




	public bool HasKey(string value) {
		if(PlayerPrefs.HasKey(value)) {
			return true;
		} else {
			return false;
		}
	}




	

	public int[] GetIntArray(string key) {
		if (PlayerPrefs.HasKey(key)) {
			string[] stringArray = PlayerPrefs.GetString(key).Split("|"[0]);
			int[] intArray = new int[stringArray.Length];
			for (int i = 0; i < stringArray.Length; i++)
				intArray[i] = Convert.ToInt32(stringArray[i]);
			return intArray;
		}
		return new int[0];
	}
	
	public void SetIntArray(string key, int[] data) {
		if (data.Length == 0) return;
		
		System.Text.StringBuilder sb = new System.Text.StringBuilder();
		for (int i = 0; i < data.Length - 1; i++)
			sb.Append(data[i]).Append("|");
		sb.Append(data[data.Length - 1]);
		
		try {
			PlayerPrefs.SetString(key, sb.ToString());
		} catch (Exception e) {
			Debug.Log(e.Message);
		}
	}


	
	private void SavePrefString(string key, string value) {
		string encoded =  Convert.ToBase64String(Encoding.UTF8.GetBytes(key + value));
		PlayerPrefs.SetString(key, encoded);

	}

	
	public string GetPrefString(string key) {
		if(PlayerPrefs.HasKey(key)) {

			string str = PlayerPrefs.GetString(key);
			if(str == string.Empty || str == "") {
				return string.Empty;
			}

			byte[] dec =  Convert.FromBase64String(str);
			string decoded = Encoding.UTF8.GetString(dec);
			decoded = decoded.Substring(key.Length, decoded.Length - key.Length);

			return decoded;
		} else {
			return string.Empty;
		}
	}
}
	


