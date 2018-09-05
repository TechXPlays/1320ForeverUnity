using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MatchPanelUI: MonoBehaviour {

	public Image EnemyAvatar;
	public Text EnemyName;
	public Text Track;
	public Text Car;

	public Text Timer;
	public Text TimeDelay;

	public Color WinningColor;
	public Color LosingColor;

	public Button PlayButton;
	public Button RematchButton;
	public Button CloseButton;
	public Image WaitPanel;

	public Image Winner;
	public Image Loser;

	private int PlayerNameMaxLenght = 16;

	private UM_TBM_Match CurrentMatch = null;

	private UM_TBM_Participant Oponent = null;

	private MultiplayerUI _parent;

	public void AttachToParent (MultiplayerUI parent) {
		_parent = parent;
	}

	public void SetMatchInfo(UM_TBM_Match match) {
		Debug.Log ("[SetMatchInfo] match Id: " + match.Id);

		CurrentMatch = match;
		SetCompetitor (CurrentMatch.Competitor);

		ST_TBM_MatchData data = new ST_TBM_MatchData (match);

		if (match.Status == UM_TBM_MatchStatus.Ended) {
			bool iAmTheWinner = data.LocalPlayerTime <= data.CompetitorTime;
            if(iAmTheWinner) {
                RS_PlayerData.Instance.MultiplayerWins++;
                UM_GameServiceManager.Instance.SubmitScore("Most Wanted", RS_PlayerData.Instance.MultiplayerWins);
                _parent.TopPanelUI.UpdateWins();
            }
                
			Winner.gameObject.SetActive (iAmTheWinner);
			Loser.gameObject.SetActive (!iAmTheWinner);
			Track.text = data.TrackId;
			Car.text = RS_PlayerData.Instance.GetCarTemplate(data.CompetitorReplay.CarId).Title;

			string prefix = iAmTheWinner ? "-" : "+";
			Timer.text = GetTimeFormated (iAmTheWinner ? data.LocalPlayerTime : data.CompetitorTime);
			TimeDelay.text = prefix + GetTimeFormated (Mathf.Abs(data.LocalPlayerTime - data.CompetitorTime));
			TimeDelay.color = iAmTheWinner ? WinningColor : LosingColor;
			TimeDelay.gameObject.SetActive (true);

			RematchButton.gameObject.SetActive (true);
		} else {
			Winner.gameObject.SetActive (false);
			Loser.gameObject.SetActive (false);

			TimeDelay.gameObject.SetActive (false);
			PlayButton.gameObject.SetActive (match.IsLocalPlayerTurn);

			if (data.CompetitorReplay != null) {
				Timer.text = GetTimeFormated (data.CompetitorTime);
				Track.text = data.TrackId;
				Car.text = RS_PlayerData.Instance.GetCarTemplate(data.CompetitorReplay.CarId).Title;
			} else {
				Timer.text = GetTimeFormated (data.LocalPlayerTime);
				Track.text = data.TrackId;
				Car.text = RS_PlayerData.Instance.GetCarTemplate(data.MyCarId).Title;
			}
		}
	}

	private string GetTimeFormated (float time) {
		int minutes = (int) time / 60;
		int seconds = (int) time % 60;
		int miliseconds = (int)(time * 100 % 100);

		return string.Format ("{0:d2}:{1:d2}:{2:d3}", minutes, seconds, miliseconds);
	}

	private void SetCompetitor(UM_TBM_Participant particiopant) {
		Oponent = particiopant;

		if (Oponent != null) {
			EnemyName.text = Oponent.DisplayName.Length <= PlayerNameMaxLenght ? Oponent.DisplayName : Oponent.DisplayName.Substring(0, PlayerNameMaxLenght) + "...";

			if(EnemyName.text.Length == 0) {
				EnemyName.text = "Matching...";
			}

			Oponent.SmallPhotoLoaded += HandleSmallPhotoLoaded;
			Oponent.LoadSmallPhoto ();

		} else {
			EnemyName.text = "Matching...";
		}
	}

	private void HandleSmallPhotoLoaded (Texture2D photo) {
		Oponent.SmallPhotoLoaded -= HandleSmallPhotoLoaded;

		if(photo != null) {
			EnemyAvatar.sprite = Sprite.Create (photo, new Rect (0, 0, photo.width, photo.height), new Vector2 (0.5f, 0.5f));
		}
	}

	public void OnButtonPlayClick() {
		Debug.Log ("[OnButtonPlayClick]");

		if (CurrentMatch != null) {
			//Redirect to the play scene and make a move
			RS_GamePlayController.Mode = RS_GameMode.Multiplayer;
			RS_GamePlayController.ActiveMatch = CurrentMatch;
			RS_LevelLoader.LoadLevel (RS_Scene.RS_DesertTrack);
		}
	}

	public void OnButtonRematchClick() {
		Debug.Log ("[OnButtonRematchClick]");
		TBM.Matchmaker.Rematch (CurrentMatch.Id);
	}

	public void OnButtonRemoveClick() {
		Debug.Log ("[OnButtonRemoveClick]");
		if (CurrentMatch != null) {
			TBM.Matchmaker.RemoveMatch (CurrentMatch.Id);
			_parent.RemoveMatchUI (CurrentMatch.Id);
		}
	}
}
