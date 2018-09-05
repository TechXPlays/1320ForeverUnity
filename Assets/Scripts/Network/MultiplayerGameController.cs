using UnityEngine;
using System.Collections;

public class MultiplayerGameController : MonoBehaviour {

	private static ST_TBM_MatchData matchData;
	public static int Pot = 0;
	
	//private MultiplayerReplayData _CurrentReplayData = null;
	private static  MultiplayerPreloaderTimeoutTask TimeOutTask = null;

	private bool WaitingForUpdate = false;
	//private bool WasFakeWin = false;

	//--------------------------------------
	// Initialization
	//--------------------------------------


	void Awake () {
		TBM.Matchmaker.MatchesListUpdated += HandleMatchesListUpdated;
	}



	void OnDestroy () {
		TBM.Matchmaker.MatchesListUpdated -= HandleMatchesListUpdated;
	}
	

	//--------------------------------------
	// Static Public Methods
	//--------------------------------------


	public static void FindMatch(string[] recipients = null) {

		if(recipients == null) {
			//BikeTemplate CurrentBike =  BikesConfig.Instance.CurrentBike;
			//TBM.Matchmaker.SetGroup(CurrentBike.CurrentStats.MultiplayerGroup);
		}

		TBM.Matchmaker.FindMatch (2, 2, recipients);
	}


	public void InitMatchData(UM_TBM_Match match) {
		matchData = new ST_TBM_MatchData (match);
		Debug.Log("Match data intialized");
	}




	public static void ShowPreloader() {
		if(TimeOutTask != null) {
			return;
		}

		TimeOutTask = MultiplayerPreloaderTimeoutTask.Create();


		if(Application.platform == RuntimePlatform.Android) {
			AndroidNativeUtility.ShowPreloader("", "");
		}

		if(Application.platform == RuntimePlatform.IPhonePlayer) {
			IOSNativeUtility.ShowPreloader();
		}
	}

	public static void HidePreloader() {
		if(TimeOutTask != null) {
			TimeOutTask.Cancel();
			TimeOutTask = null;
		}

		if(Application.platform == RuntimePlatform.Android) {
			AndroidNativeUtility.HidePreloader();
		}

		if(Application.platform == RuntimePlatform.IPhonePlayer) {
			IOSNativeUtility.HidePreloader();
		}
	}

	public static void PrelaoderTimeOut() {
		HidePreloader ();

		//PlayerData.Instance.UserReturnedToMultiplayer = true;
		//LevelLoader.Instance.LoadLevel (Scene.Garage);
		
	}
	

	//--------------------------------------
	// Public Methods
	//--------------------------------------


	public void ShowFinishScreen() {
		//GameUIController.Instance.FinishRace ();
	}

	/*public void FillUpWinScreen(MultiplayerWinScreen multik) {
		multik.SetReward (Pot);
		multik.CheckAdvertising ();

		if (MultiplayerTutorialController.IsTutorInProgress ()) {
			multik.SetFakeLeftParticipant();
			multik.SetFakeRightParticipant();
			multik.CheckFakeMatchState ();
			WasFakeWin = multik.IsFakeWin;

		} else {
			multik.SetLeftParticipant (CurrentMatch.LocalParticipant, MatchData.LocalPlayerTime);
			multik.CheckMatchState (MatchData);

			if (CurrentMatch.Competitor != null) {
				multik.SetRightParticipant (CurrentMatch.Competitor, MatchData.CompetitorTime);
			}
		}
	}*/

	public void Quit() {
		ShowPreloader ();
		WaitingForUpdate = true;

		if(CurrentMatch.IsLocalPlayerTurn) {
			CurrentMatch.QuitInTurn(matchData.Match.NextParticipant);
		} else {
			CurrentMatch.QuitOutOfTurn();
		}
	}

	public void FinishMatch() {
		WaitingForUpdate = true;
		ShowPreloader();

		if(matchData.ParticipantsData.Count == 2) {

			if(matchData.LocalPlayerTime < matchData.CompetitorTime) {
				matchData.Win();
			} else {
				matchData.Loose();
			}
		} else {
			matchData.TakeTrun();
		}
	}

	//--------------------------------------
	// Get / Set
	//--------------------------------------
	
	public UM_TBM_Match CurrentMatch {
		get {
			return matchData.Match;
		}
	}

	public MultiplayerReplayData CompetitorReplay {
		get {
			if (matchData != null)
				return matchData.CompetitorReplay;
			else
				return null;
		}
	}

	public ST_TBM_MatchData MatchData {
		get {
			return matchData;
		}
	}

	//--------------------------------------
	// Action Hadnlers
	//--------------------------------------


	private void HandleGhostInfoThoroughlyFull(GhostReplayData obj) {
		ShowFinishScreen ();
	}


	void HandleMatchesListUpdated () {

		if(!WaitingForUpdate) {
			return;
		}

		HidePreloader();


		//LevelLoader.Instance.LoadLevel (Scene.Garage);
	}

	
	//--------------------------------------
	// Private Methods
	//--------------------------------------


	


}
