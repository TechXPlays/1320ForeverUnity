using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TopPanelUI : MonoBehaviour {

	[SerializeField]
	private Text Tickets;

	[SerializeField]
	private Text Wins;

	[SerializeField]
	private Text PlayerName;

	[SerializeField]
	private Image PlayerAvatar;

	[SerializeField]
	private Text CarName;

	[SerializeField]
	private GameObject TopPanel;

	public static event System.Action ButtonSettingsClicked = delegate {};

	void Awake() {
		RS_GameData.MenuCarIdChanged += HandleMenuCarIdChanged;
	}

	void Start() {
		Tickets.text = RS_PlayerData.Instance.MultiplayerTickets.ToString () + "/" + RS_PlayerData.MAX_TICKETS.ToString ();
		Wins.text = RS_PlayerData.Instance.MultiplayerWins.ToString();

		if (UM_GameServiceManager.Instance.ConnectionSate == UM_ConnectionState.CONNECTED) {
			PlayerName.text = UM_GameServiceManager.Instance.Player.Name;
			LoadPlayerAvatar ();
		} else {
			UM_GameServiceManager.OnPlayerConnected += HandleOnPlayerConnected;
		}

        if(!GoogleMobileAd.IsInited) {
            GoogleMobileAd.Init();
            GoogleMobileAd.LoadInterstitialAd();
        }
	}

	private void HandleOnPlayerConnected() {
		UM_GameServiceManager.OnPlayerConnected -= HandleOnPlayerConnected;
		PlayerName.text = UM_GameServiceManager.Instance.Player.Name;
        UM_GameServiceManager.ActionScoresListLoaded += UM_GameServiceManager_ActionScoresListLoaded;
        UM_GameServiceManager.Instance.LoadPlayerCenteredScores("Most Wanted", 1, UM_TimeSpan.ALL_TIME, UM_CollectionType.GLOBAL);
		LoadPlayerAvatar ();
	}

	private void LoadPlayerAvatar() {
		if (UM_GameServiceManager.Instance.Player.SmallPhoto != null) {
			AssignAvatar (UM_GameServiceManager.Instance.Player.SmallPhoto);
		} else {
			UM_GameServiceManager.Instance.Player.SmallPhotoLoaded += UM_GameServiceManager_Instance_Player_SmallPhotoLoaded;
			UM_GameServiceManager.Instance.Player.LoadSmallPhoto ();
		}
	}

	private void AssignAvatar(Texture2D avatar) {
		PlayerAvatar.sprite = Sprite.Create (avatar, new Rect (0, 0, avatar.width, avatar.height), new Vector2 (0.5f, 0.5f));
	}

	void UM_GameServiceManager_Instance_Player_SmallPhotoLoaded (Texture2D avatar)
	{
		UM_GameServiceManager.Instance.Player.SmallPhotoLoaded -= UM_GameServiceManager_Instance_Player_SmallPhotoLoaded;
		AssignAvatar (avatar);
	}

	void OnDestroy() {
		RS_GameData.MenuCarIdChanged -= HandleMenuCarIdChanged;
	}

	public void ButtonSettingsHandler() {
		ButtonSettingsClicked ();
	}

	private void ShowTopPanel() {
		TopPanel.SetActive (true);
	}

	private void HideTopPanel() {
		TopPanel.SetActive (false);
	}

	private void HandleMenuCarIdChanged(RS_CarTemplate car) {
		CarName.text = car.Title;
	}

    public void UpdateWins() {
        Wins.text = RS_PlayerData.Instance.MultiplayerWins.ToString();
    }

    public void ShowLeaderBoard() {
        UM_GameServiceManager.Instance.ShowLeaderBoardUI("Most Wanted");
    }

    void UM_GameServiceManager_ActionScoresListLoaded(UM_LeaderboardResult obj)
    {
        UM_Score score = obj.Leaderboard.GetCurrentPlayerScore(UM_TimeSpan.ALL_TIME, UM_CollectionType.GLOBAL);
        RS_PlayerData.Instance.MultiplayerWins = (int)score.LongScore;
        UpdateWins();
    }
}
