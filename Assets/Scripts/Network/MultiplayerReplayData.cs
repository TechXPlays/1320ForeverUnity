using UnityEngine;
using System.Collections;

public class MultiplayerReplayData : GhostReplayData {

	private string _ParticipantID = "";

	public string ParticipantID {
		get {
			return _ParticipantID;
		}
	}

	public MultiplayerReplayData(string pID, GhostReplayData ghostData) : base() {
		_ParticipantID = pID;

		_CarId = ghostData.CarId;
		_CarLevel = ghostData.CarLevel;
		_Track = ghostData.Track;
		_FinishTime = ghostData.FinishTime;
		
		_ReplayData = ghostData.RawData;

		BuildPackage ();

	}

	public MultiplayerReplayData(byte[] data): base(data) {
	
	}

	public override void DecomposePackage() {
		_ParticipantID = ReadValue<string>();

		base.DecomposePackage ();
	}

	public override void BuildPackage() {
		WriteValue<string> (_ParticipantID);

		base.BuildPackage ();
	}
}
