using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BotPanelUI : MonoBehaviour {
	[SerializeField]
	private Text EngineValue;

	[SerializeField]
	private Text GearValue;

	[SerializeField]
	private Text GripValue;

	[SerializeField]
	private GameObject BotPanel;

	void Awake() {
		RS_GameData.MenuCarIdChanged += HandleMenuCarIdChanged;
		TBM.Matchmaker.MatchFoundEvent += HandleActionMatchFound;
	}

	void OnDestroy() {
		RS_GameData.MenuCarIdChanged -= HandleMenuCarIdChanged;
		TBM.Matchmaker.MatchFoundEvent -= HandleActionMatchFound;
	}
	public void ButtonTrainingHandler() {
		RS_GamePlayController.Mode = RS_GameMode.SinglePlayer;
		RS_LevelLoader.LoadLevel(RS_Scene.RS_DesertTrack);
	}

	public void ButtonPlayFriendsHandler() {
        if (RS_PlayerData.Instance.MultiplayerTickets > 0)
        {
            Debug.Log("ButtonPlayFriendsHandler");

            TBM.Matchmaker.SetGroup(RS_PlayerData.Instance.GetCarTemplate(RS_GameData.MenuCarId).Index + 1);
            TBM.Matchmaker.ShowNativeFindMatchUI(2, 2);
        }
	}

	public void ButtonStartHandler() {
        if (RS_PlayerData.Instance.MultiplayerTickets > 0)
        {
#if !UNITY_EDITOR
		    TBM.Matchmaker.SetGroup(RS_PlayerData.Instance.GetCarTemplate(RS_GameData.MenuCarId).Index + 1);
		    TBM.Matchmaker.FindMatch(2, 2);
		    RS_GamePlayController.Mode = RS_GameMode.Multiplayer;
#else
            RS_GamePlayController.Mode = RS_GameMode.SinglePlayer;
            RS_PlayerData.Instance.MultiplayerTickets--;
            RS_LevelLoader.LoadLevel(RS_Scene.RS_DesertTrack);
#endif
        }
	}

	private void HandleActionMatchFound (UM_TBM_MatchResult res) {
		Debug.Log("HandleActionMatchFound " + res.IsSucceeded);
		if (res.IsSucceeded) {
			//Redirect to the play scene and make a move
			RS_GamePlayController.Mode = RS_GameMode.Multiplayer;
			RS_GamePlayController.ActiveMatch = res.Match;
            RS_PlayerData.Instance.MultiplayerTickets--;

			RS_LevelLoader.LoadLevel (RS_Scene.RS_DesertTrack);
		} else {
			Debug.Log ("[HandleActionMatchFound] error " + res.Error.Description);
		}
	}

	private void ShowTopPanel() {
		BotPanel.SetActive (true);
	}

	private void HideTopPanel() {
		BotPanel.SetActive (false);
	}

	private void HandleMenuCarIdChanged(RS_CarTemplate car) {
		EngineValue.text = car.EngineValue.ToString();
		GearValue.text = car.GearBoxValue.ToString();
		GripValue.text = car.GripValue.ToString();
	}
}
