using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class RS_GhostCarManager : MonoBehaviour {
	private RS_DragCarController ghostRacer;

	private GhostReplayData NewGhostData;
	private GhostReplayData LoadedGhostData;

	private List<GhostDataBundle> TemporaryBundleList = new List<GhostDataBundle>();

	private bool IsGhostExist = false;

	void Start () {
		RS_GamePlayController.Instance.OnLevelStartedAction += HandleOnLevelStartedAction;
		RS_GamePlayController.Instance.OnLevelFinishedAction += HandleOnLevelFinishedAction;
		RS_GamePlayController.Instance.OnPlayerSpawned += HandleOnSpawnPlayer;
	}

	public bool SavedDataExists() {
		return LoadedGhostData != null;
	}

	public bool IsNewRecord() {
		if (LoadedGhostData != null) {
			return LoadedGhostData.FinishTime > NewGhostData.FinishTime;
		}
		return true;
	}

	public float GetCurrentTime() {
		return NewGhostData.FinishTime;
	}

	public float GetRecordTime () {
		if (LoadedGhostData != null) {
			return LoadedGhostData.FinishTime;
		}
		return NewGhostData.FinishTime;
	}

	private void HandleActionGhostCarLoaded (GameObject obj) {
		Debug.Log ("[GhostController] HandleActionGhostCarLoaded");
		try{
			ghostRacer = obj.GetComponent<RS_DragCarController>();
			ghostRacer.gameObject.layer = LayerMask.NameToLayer("Ghost");

			PlayerSpawnPoint spawnPoint = GameObject.FindObjectOfType<PlayerSpawnPoint>();
			ghostRacer.transform.position = new Vector3(spawnPoint.transform.position.x - 4.8f, spawnPoint.transform.position.y, spawnPoint.transform.position.z);

			if (RS_GamePlayController.Mode == RS_GameMode.Multiplayer) {
				UM_TBM_Participant compet = RS_GamePlayController.Instance.Multiplayer.CurrentMatch.Competitor;					
				if (compet != null) {
					Debug.Log("[Compete With Other Palayer] Name:" + compet.DisplayName);
				} else {
					Debug.Log("[I'm The Creator!!!]");
				}
			} else {
				Debug.Log("[Singleplayer Mode activated! Your data will be saved locally]");
			}
		} catch (Exception ex) {
			Debug.LogError("HandleActionGhostBikeLoaded - " + ex.Message);
		}
	}

	private void HandleOnLevelStartedAction () {
		RS_GamePlayController.Instance.OnLevelStartedAction -= HandleOnLevelStartedAction;

		if (ghostRacer != null) {
			ghostRacer.Launch ();
		}
	}
	
	private void HandleOnLevelFinishedAction () {
		Debug.Log ("[GhostController] HandleOnLevelFinishedAction");
		RS_GamePlayController.Instance.OnLevelFinishedAction -= HandleOnLevelFinishedAction;
		RS_GamePlayController.Instance.OnPlayerSpawned -= HandleOnSpawnPlayer;
		RS_CarFactory.CarLoadedAction -= HandleActionGhostCarLoaded;

		NewGhostData.SetFinishTime (RaceTimer.Instance.GetCurrentGlobalTime());
		NewGhostData.AddDataBundle (TemporaryBundleList);
		TemporaryBundleList.Clear ();

		if (RS_GamePlayController.Mode == RS_GameMode.Multiplayer) {
			//Take Turn of finish the Match
			Debug.Log(RS_GamePlayController.Mode.ToString() + " Time to make a Turn...");
			#if !UNITY_EDITOR
			MultiplayerReplayData replayData = new MultiplayerReplayData (RS_GamePlayController.Instance.Multiplayer.CurrentMatch.LocalParticipant.Id, NewGhostData);
			RS_GamePlayController.Instance.Multiplayer.MatchData.SetParticipantData (replayData);

			ST_TBM_MatchData match = RS_GamePlayController.Instance.Multiplayer.MatchData;

			Debug.Log ("MATCH STATUS: " + match.Match.Status.ToString());
			Debug.Log ("MATCH IsLocalPlayerTurn: " + match.Match.IsLocalPlayerTurn.ToString());

			foreach (UM_TBM_Participant player in match.Match.Participants) {
				Debug.Log ("[PARTICIPANT] " + player.Id + 
					"\t\n" + player.DisplayName + 
					"\t\n" + player.Status.ToString());
			}

			if (RS_GamePlayController.Instance.Multiplayer.CurrentMatch.Competitor != null
				&& RS_GamePlayController.Instance.Multiplayer.CurrentMatch.Competitor.Status != UM_TBM_ParticipantStatus.Unknown) {
				if (match.LocalPlayerTime > match.CompetitorTime) {
					match.Loose ();
				} else if (match.LocalPlayerTime < match.CompetitorTime) {
					match.Win ();
				} else {
					match.Tie ();
				}
			} else {
				match.TakeTrun ();
			}
			#else
			Debug.Log("But, wait! We are in Editor Mode.");
			#endif
		} else {
			if (LoadedGhostData != null) {
				if (LoadedGhostData.FinishTime >= NewGhostData.FinishTime) {
					Debug.Log ("[Saving ghost data locally]");
					Debug.Log (NewGhostData.CarId);
					Debug.Log (NewGhostData.CarLevel);
					Debug.Log (NewGhostData.RawData.ToString () + " Count:" + NewGhostData.RawData.Count);

					NewGhostData.SaveLocalRecord ();
				}
			} else {
				Debug.Log ("[Saving ghost data locally]");
				Debug.Log (NewGhostData.CarId);
				Debug.Log (NewGhostData.CarLevel);
				Debug.Log (NewGhostData.RawData.ToString () + " Count:" + NewGhostData.RawData.Count);

				NewGhostData.SaveLocalRecord ();
			}
		}
	}
	
	private void HandleOnSpawnPlayer () {

		RS_GamePlayController.Instance.Player.OnThrottlePress += HandleOnThrottlePress;
		RS_GamePlayController.Instance.Player.OnThrottleRelease += HandleOnThrottleRelease;
		RS_GamePlayController.Instance.Player.OnGearUp += HandleOnGearUp;
		RS_GamePlayController.Instance.Player.OnGearDown += HandleOnGearDown;

		NewGhostData = new GhostReplayData ();
		NewGhostData.SetCarId (RS_GameData.MenuCarId);
		NewGhostData.SetCarLevel (2);
		NewGhostData.SetTrack ("Desert");

		if (!IsGhostExist) {
			IsGhostExist = true;
			CreateGhostPlayer ();
		}
	}

	private void HandleOnThrottlePress (float time) {
		Debug.Log ("[HandleOnThrottlePress] time: " + time);

		TemporaryBundleList.Add (new GhostDataBundle(time, GhostDataBundle.EventType.ThrottlePress));
	}

	private void HandleOnThrottleRelease (float time) {
		Debug.Log ("[HandleOnThrottleRelease] time: " + time);

		TemporaryBundleList.Add (new GhostDataBundle(time, GhostDataBundle.EventType.ThrottleRelease));
	}

	private void HandleOnGearUp(int gear, float time) {
		Debug.Log (string.Format("[HandleOnGearUp] gear: {0} time: {1:F4}", gear, time));

		TemporaryBundleList.Add (new GhostDataBundle(time, GhostDataBundle.EventType.GearUp));
	}

	private void HandleOnGearDown(int gear, float time) {
		Debug.Log ("[HandleOnGearDown] gear: " + gear + " time: " + time);

		TemporaryBundleList.Add (new GhostDataBundle(time, GhostDataBundle.EventType.GearDown));
	}

	private void CreateGhostPlayer() {
		Debug.Log ("[GhostController] CreateGhostPlayer");

		if (RS_GamePlayController.Mode == RS_GameMode.Multiplayer) {
			#if !UNITY_EDITOR
			LoadedGhostData = RS_GamePlayController.Instance.Multiplayer.CompetitorReplay;
			#endif
		} else {
			if (RS_PlayerData.Instance.HasGhostData ("Desert")) {
				LoadedGhostData = GhostReplayData.GetGhostDataOnTrack ("Desert");
			}
		}

		if(LoadedGhostData != null) {
			Debug.Log (LoadedGhostData.CarId);
			Debug.Log (LoadedGhostData.CarLevel);
			Debug.Log (LoadedGhostData.RawData.ToString() + " Count:" + LoadedGhostData.RawData.Count);

			foreach (GhostDataBundle action in LoadedGhostData.RawData) {
				StartCoroutine(GhostRacerAction(action.Event, action.Time));
			}

			RS_CarFactory.CarLoadedAction += HandleActionGhostCarLoaded;
			RS_CarFactory.CreateCar(LoadedGhostData.CarId, RS_CarCreationMode.Garage);
		}
	}

	private IEnumerator GhostRacerAction(int action, float time) {

		yield return new WaitForSeconds (time);

		if (ghostRacer == null)
			yield return null;

		switch (action) {
		case GhostDataBundle.EventType.ThrottlePress:
			ghostRacer.StepOnGas ();
			break;
		case GhostDataBundle.EventType.ThrottleRelease:
			ghostRacer.ReleaseGas ();
			break;
		case GhostDataBundle.EventType.GearUp:
			ghostRacer.ShiftUp ();
			break;
		case GhostDataBundle.EventType.GearDown:
			ghostRacer.ShiftDown ();
			break;
		default:
			break;
		}

		yield return null;
	}
}
