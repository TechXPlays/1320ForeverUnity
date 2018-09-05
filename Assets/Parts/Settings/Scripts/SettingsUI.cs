using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SettingsUI : MonoBehaviour {

	[SerializeField]
	private Image SoundImage;

	[SerializeField]
	private Sprite SoundOnSprite;

	[SerializeField]
	private Sprite SoundOffSprite;

	[SerializeField]
	private GameObject SettingsPanel;

	void Start() {
		TopPanelUI.ButtonSettingsClicked += ShowSettings;
	}

	void OnDestroy() {
		TopPanelUI.ButtonSettingsClicked -= ShowSettings;
	}

	public void ButtonSoundClickHandler() {
		RS_GameData.IsGameSoundActive = !RS_GameData.IsGameSoundActive;

		if (RS_GameData.IsGameSoundActive) {
			SoundImage.overrideSprite = SoundOnSprite;
			AudioListener.volume = 1f;
		} else {
			SoundImage.overrideSprite = SoundOffSprite;
			AudioListener.volume = 0f;
		}
	}

	public void ButtonBackClickHandler() {
		HideSettings ();
	}

	private void ShowSettings() {
		SettingsPanel.SetActive (true);
	}

	private void HideSettings() {
		SettingsPanel.SetActive (false);
	}
}
