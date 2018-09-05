using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class RS_TBM_MatchUI : MonoBehaviour {
	
	public const string AssetName = "CubMatchUI";


	//UI elements
	public Image OponentIcon;
	public Text OponentName;
	public Text MatchStatus;


	public Text WaitTag;
	public GameObject PlayButton;



	public GameObject AcceptInviteButton;
	public GameObject DeclineInviteButton;

	public GameObject RemoveButton;

	public Texture2D DefaultOponentIcon;
	public Texture2D DefaultPlayerIcon;


	private UM_TBM_Match _Match;
	private UM_TBM_Invite _Invite;
	private UM_TBM_Participant Competitor;


	void Awake() {
		TBM.Matchmaker.MatchLoadedEvent += HandleMatchLoadedEvent;


		WaitTag.gameObject.SetActive(false);
		PlayButton.SetActive(false);
		RemoveButton.SetActive(false);

		AcceptInviteButton.SetActive(false);
		DeclineInviteButton.SetActive(false);
	}



	void OnDestroy() {
		if(Competitor != null) {
			Competitor.SmallPhotoLoaded -= HandleSmallPhotoLoaded;
		}
		
		TBM.Matchmaker.MatchLoadedEvent -= HandleMatchLoadedEvent;
		
	}

	public void SetInvite(UM_TBM_Invite invite) {
		_Invite = invite;
		Competitor = invite.Inviter;

		OponentName.text = Competitor.DisplayName;
		Competitor.SmallPhotoLoaded += HandleSmallPhotoLoaded;
		Competitor.LoadSmallPhoto();

		MatchStatus.text = invite.CreationTimestamp.ToString("MMMM dd, yyyy hh:mm:ss");

		AcceptInviteButton.SetActive(true);
		DeclineInviteButton.SetActive(true);

	}


	public void SetMatch(UM_TBM_Match match) {
		_Match = match;

		RemoveButton.SetActive(true);

		OponentIcon.sprite = Sprite.Create (DefaultOponentIcon, new Rect (0, 0, DefaultOponentIcon.width, DefaultOponentIcon.height), new Vector2 (0.5f, 0.5f));
		Competitor = _Match.Competitor;

		if(Competitor != null && Competitor.IsPlayerDefined) {
			OponentName.text = Competitor.DisplayName;
			Competitor.SmallPhotoLoaded += HandleSmallPhotoLoaded;
			Competitor.LoadSmallPhoto();
		} else {
			OponentName.text = "Mtaching...";
		}

		if(match.Status == UM_TBM_MatchStatus.Ended) {
			WaitTag.gameObject.SetActive(true);
			WaitTag.text = match.LocalParticipant.Outcome.ToString();

		} else {
			if(match.IsLocalPlayerTurn) {
				PlayButton.SetActive(true);
			} else {
				WaitTag.gameObject.SetActive(true);
			}
		}

		string statusString = string.Empty;

		if(match.IsLocalPlayerTurn) {
			statusString = statusString + "Your Turn \n";
		} else {
			statusString = statusString + "Waiting for Opponent  Move \n";
		}

		statusString += "Opponent Status: ";
		if(match.Competitor != null) {
			statusString += match.Competitor.Status + "\n";
		} else {
			statusString += "Matching \n";
		}

		statusString += "Match: " + match.Status + "  ";

		statusString += match.LocalParticipant.Outcome.ToString() + " / ";
		if(match.Competitor != null) {
			statusString += match.Competitor.Outcome.ToString();
		} else {
			statusString += UM_TBM_Outcome.None.ToString();
		}


		MatchStatus.text = statusString;
	}

	void HandleSmallPhotoLoaded (Texture2D photo) {
		Competitor.SmallPhotoLoaded -= HandleSmallPhotoLoaded;
		if(photo ==  null) {
			photo = DefaultPlayerIcon;
		}
		OponentIcon.sprite = Sprite.Create (photo, new Rect (0, 0, photo.width, photo.height), new Vector2 (0.5f, 0.5f));

	}



	public void Play() {
		TBM.Matchmaker.LoadMatch(_Match.Id);
	}

	public void Accept() {
		TBM.Matchmaker.AcceptInvite(_Invite);
	}

	public void Decline() {
		TBM.Matchmaker.DeclineInvite(_Invite);
	}

	public void Remove() {
		if(_Match.IsEnded) {
			TBM.Matchmaker.RemoveMatch(_Match.Id);
		} else {
			if(_Match.IsLocalPlayerTurn) {
				_Match.QuitInTurn(_Match.Competitor);
			} else {
				_Match.QuitOutOfTurn();
			}
		}

	}


	void HandleMatchLoadedEvent (UM_TBM_MatchResult res){
		if(res.IsSucceeded) {
			RS_TBM_RaceController.CurrentMatch = res.Match;
			RS_LevelLoader.LoadLevel(RS_Scene.RS_DesertTrack);
		}
	}




}
