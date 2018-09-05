using UnityEngine;
using System.Text;
using System.Collections;
using System.IO;
using System.Collections.Generic;

public class GhostReplayData : MNT_NetworkPackage {

	protected string _CarId = "";
	protected int _CarLevel = 0;
	protected string _Track = "";
	protected float _FinishTime = 0.0f;

	protected List<GhostDataBundle> _ReplayData = new List<GhostDataBundle>();

	//--------------------------------------
	// Initialization
	//--------------------------------------
	public GhostReplayData(): base(1) {

	}

	public GhostReplayData(byte[] data): base(data) {
		DecomposePackage ();
	}

	public static GhostReplayData GetGhostDataOnTrack (string track) {
		if(!RS_PlayerData.Instance.HasGhostData(track)) { 
			return null; 
		}

		string base64Data = RS_PlayerData.Instance.GetGhostData (track);
		byte[] bytes = System.Convert.FromBase64String (base64Data);

		return new GhostReplayData (bytes);
	}

	//--------------------------------------
	// Public Methods
	//--------------------------------------

	public void SetCarId(string id) {
		_CarId = id;
	}

	public void SetCarLevel(int level) {
		_CarLevel = level;
	}

	public void SetTrack(string track) {
		_Track = track;
	}

	public void SetFinishTime(float time) {
		_FinishTime = time;
	}

	public void AddDataBundle(List<GhostDataBundle> tempList) {
		_ReplayData.InsertRange (_ReplayData.Count, tempList);
	}

	public virtual void DecomposePackage() {
		_CarId = ReadValue<string>();
		_CarLevel = ReadValue<int>();
		_Track = ReadValue<string>();
		_FinishTime = ReadValue<float> ();

		List<MNT_NetworkPackage> PackageList = ReadList<MNT_NetworkPackage>();

		foreach(MNT_NetworkPackage bundleString in PackageList) {
			_ReplayData.Add(new GhostDataBundle(bundleString.GetBytes()));
		}
	}

	public virtual void BuildPackage() {
		WriteValue<string> (_CarId);
		WriteValue<int> (_CarLevel);
		WriteValue<string> (_Track);
		WriteValue<float> (_FinishTime);

		WriteList<GhostDataBundle>(_ReplayData);
	}

	public void SaveLocalRecord() {
		BuildPackage ();

		RS_PlayerData.Instance.SaveGhostData ("Desert", GetBase64DataFormat());
	}

	//--------------------------------------
	// Get / Set
	//--------------------------------------

	public string CarId {
		get {
			return _CarId;
		}
	}

	public int CarLevel {
		get {
			return _CarLevel;
		}
	}

	public string Track {
		get {
			return _Track;
		}
	}

	public float FinishTime {
		get {
			return _FinishTime;
		}
		
	}

	public List<GhostDataBundle> RawData {
		get {
			return _ReplayData;
		}
	}

	public bool HasReplay {
		get {
			return _ReplayData != null;
		}
	}
}
