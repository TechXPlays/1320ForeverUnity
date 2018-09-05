using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ST_TBM_MatchData : MNT_NetworkPackage {

	private List<MultiplayerReplayData> _ParticipantsData = new List<MultiplayerReplayData> ();

	private UM_TBM_Match _Match;


	//--------------------------------------
	// Initialization
	//--------------------------------------



	public ST_TBM_MatchData(byte[] bytes): base(bytes) {
		DecomposePackage ();
	}

	public ST_TBM_MatchData(UM_TBM_Match match): base (2) {
		_Match =  match;

		if (_Match.Data.Length != 0) {
			try {
				Debug.Log("ST_TBM_MatchData initialized with  data: "+ _Match.Data.Length); 
				ST_TBM_MatchData ReceviedData = new ST_TBM_MatchData (_Match.Data);

				foreach (MultiplayerReplayData mrd in ReceviedData.ParticipantsData) {
					SetParticipantData (mrd);
				}
			} catch (System.Exception ex) {
				Debug.Log ("MatchData reading fail - " + ex.Message);
			}
		} else {
			Debug.Log("ST_TBM_MatchData initialized with empty data"); 
		}
	}
	

	//--------------------------------------
	// Public Methods
	//--------------------------------------

	public MultiplayerReplayData GetParticipantData (string participantID) {
		MultiplayerReplayData particip = null;

		foreach (var mrd in _ParticipantsData) {
			if (mrd.ParticipantID.Equals(participantID)) {
				particip = mrd;
			}
		}

		return particip;
	}

	public void SetParticipantData (MultiplayerReplayData newMrd) {

		int index = 0;
		foreach (var mrd in _ParticipantsData) {
			if (mrd.ParticipantID.Equals(newMrd.ParticipantID)) {
				_ParticipantsData.Remove(mrd);
				break;
			}
			index++;
		}

		_ParticipantsData.Add(newMrd);

	}

	public void Win () {
		Debug.Log ("ST_TBM_MatchData::Win");
		BuildPackage();
		Match.Win(GetBytes());
	}

	public void Loose () {
		Debug.Log ("ST_TBM_MatchData::Loose");
		BuildPackage();
		Match.Lose(GetBytes());
	}

	public void Tie() {
		Debug.Log ("ST_TBM_MatchData::Tie");
		BuildPackage();
		Match.Tie(GetBytes());
	}

	public void TakeTrun() {
		Debug.Log ("ST_TBM_MatchData::TakeTrun");
		BuildPackage();
		Match.TakeTrun(GetBytes());
	}

	//--------------------------------------
	// Get / Set
	//--------------------------------------


	public string TrackId {
		get {
			if(ParticipantsData.Count > 0) {
				return ParticipantsData[0].Track;
			} else {
				return string.Empty;
			}
		}
	}

	public string MyCarId {
		get { 
			MultiplayerReplayData data = GetParticipantData(Match.LocalParticipant.Id);

			if (data == null) {
				return string.Empty;
			}

			return data.CarId;
		}
	}	

	public float CompetitorTime {
		get {
			MultiplayerReplayData data = GetParticipantData(Match.Competitor.Id);
			
			if (data == null) {
				return 0f;
			}
			
			return data.FinishTime;
		}
	}

	public MultiplayerReplayData CompetitorReplay {
		get {
			Debug.Log (Match);
			if (Match.Competitor != null) {
				Debug.Log (Match.Competitor);
				Debug.Log (Match.Competitor.Id);
				MultiplayerReplayData data = GetParticipantData (Match.Competitor.Id);
				return data;
			} else {
				return null;
			}
		}
	}

	public float LocalPlayerTime {
		get {
			MultiplayerReplayData data = GetParticipantData(Match.LocalParticipant.Id);
			
			if (data == null) {
				return 0f;
			}
			
			return data.FinishTime;
		}
	}

	public bool IsPlayingwithFirend {
		get {
			if(Match.Competitor != null) {
				return UM_GameServiceManager.Instance.FriendsList.Contains(Match.Competitor.Playerid);
			}

			return false;
		}
	}


	public UM_TBM_Match Match {
		get {
			return _Match;
		}
	}

	public List<MultiplayerReplayData> ParticipantsData {
		get {
			return _ParticipantsData;
		}
	}

	//--------------------------------------
	// Private Methods
	//--------------------------------------

	private void DecomposePackage() {
		Debug.Log ("DecomposePackage");

		List<MNT_NetworkPackage> PackageList = ReadList<MNT_NetworkPackage>();
		
		foreach(MNT_NetworkPackage bundleString in PackageList) {
			_ParticipantsData.Add(new MultiplayerReplayData(bundleString.GetBytes()));
		}

	}
	
	private void BuildPackage() {
		Debug.Log ("BuildPackage");
		Debug.Log (_ParticipantsData.Count);
		WriteList<MultiplayerReplayData>(_ParticipantsData);
	}

}
