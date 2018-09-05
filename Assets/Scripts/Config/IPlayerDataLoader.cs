using System;

public interface IPlayerDataLoader {
	string GetString (string key);
	int GetInt (string key);
	float GetFloat(string key);
	int[] GetIntArray(string key);
	
	void SetString (string key, string value);
	void SetInt (string key, int value);
	void SetFloat(string key, float value);
	void SetIntArray(string key, int[] data);

	bool HasKey(string key);
}

