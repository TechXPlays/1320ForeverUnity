using UnityEngine;
using System.Collections;

public class RS_TBM_RaceController : MonoBehaviour {

	
	public static UM_TBM_Match CurrentMatch;

	private RS_RaceDataPackage pack;
	void Awake() {

		pack =  new RS_RaceDataPackage();
		pack.Time = 1;
		
		pack.BuildPackage();

		TBM.Matchmaker.TurnEndedEvent += HandleTurnEndedEvent;
		TBM.Matchmaker.MatchUpdatedEvent += HandleMatchUpdatedEvent;
	}

	void OnDestroy() {
		TBM.Matchmaker.TurnEndedEvent -= HandleTurnEndedEvent;
		TBM.Matchmaker.MatchUpdatedEvent -= HandleMatchUpdatedEvent;
	}

	void HandleTurnEndedEvent (UM_TBM_MatchResult res)	{
		if(res.IsSucceeded) {
			RS_LevelLoader.LoadLevel(RS_Scene.RS_Garage);
		} 
	}

	void HandleMatchUpdatedEvent(UM_TBM_MatchResult res) {
		Debug.Log("HandleMatchUpdatedEvent " + res.IsSucceeded);
		if(res.IsSucceeded) {
			RS_LevelLoader.LoadLevel(RS_Scene.RS_Garage);
		} 

	}


	public void TakeTurn() {

		Debug.Log("Taking the turn");
		CurrentMatch.TakeTrun(pack.GetBytes());


	}


	public void Win() {
		CurrentMatch.Win(pack.GetBytes());
	}

	public void Lose() {
		CurrentMatch.Lose(pack.GetBytes());
	}

	public void Tie() {
		CurrentMatch.Tie(pack.GetBytes());
	}

}
