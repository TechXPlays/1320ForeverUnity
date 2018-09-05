using UnityEngine;
using System.Collections;

public class RS_RaceStartPanel : MonoBehaviour {


	//--------------------------------------
	// Initialization
	//--------------------------------------



	//--------------------------------------
	// Public Methods
	//--------------------------------------

	public void StartRaceWithRandomPlayer() {
		TBM.Matchmaker.FindMatch(2, 2);
	}

	public void TrainWithBot() {
		RS_LevelLoader.LoadLevel(RS_Scene.RS_DesertTrack);
	}


	//--------------------------------------
	// Handlers
	//--------------------------------------
	
}
