using UnityEngine;
using System.Collections;

public class RaceTimer : MonoBehaviour  {
		
	private UnityEngine.UI.Text TimerLabel;

	private bool TimerTurnedOn;

	private float globaltime = 0f;

	private float lastCheckpointTime = 0f;

	private static RaceTimer _Instance;


	//--------------------------------------
	// Initialization
	//--------------------------------------


	void Start() {
		Debug.Log ("creating RaceTimer");

		_Instance = this;

		if (TimerLabel == null) {
			TimerLabel = GetComponent<UnityEngine.UI.Text>();
		}
		TimerLabel.text = GetTimeFormated ();
		//StartTimer();
		RS_GamePlayController.Instance.OnLevelStartedAction += HandleOnLevelStartedAction;
		RS_GamePlayController.Instance.OnLevelFinishedAction += HandleOnLevelFinishedAction;

	}

	public static RaceTimer Instance {
		get {
			return _Instance;
		}
	}

	//--------------------------------------
	// Unity Events
	//--------------------------------------


	void FixedUpdate () {
		if (TimerTurnedOn) {
			globaltime += Time.deltaTime;
			
			TimerLabel.text = GetTimeFormated ();
		}
	}

	//--------------------------------------
	// Public Methods
	//--------------------------------------

	public float GetTimeInTicks() {
		return globaltime;
	}

	public void SaveCheckpointTime() {
		lastCheckpointTime = globaltime;
	}

	public void BackToLastCheckpointTime(int penalty = 0) {
		lastCheckpointTime += penalty;
		globaltime = lastCheckpointTime;
	}

	public int GetCurrnetTimeInSeconds() {
		return (int)globaltime;
	}

	public string GetCurrentTime() {
		return GetTimeFormated ();
	}

	public float GetCurrentGlobalTime() {
		return globaltime;
	}

	public long GetCurrentGlobalTimeInMilliSec() {
		return (long)(globaltime * 1000);
	}

	public string GetCurrentTimeForAnailitics() {
		return GetTimeFormatedForAnalitics ();
	}

	public string GetTimeFormatedForWinScreen (float gt) {
		int minutes = (int) gt / 60;
		int seconds = (int) gt % 60;
		int miliseconds = (int)(gt * 100 % 100);
		
		return string.Format ("{0:d2}:{1:d2}:{2:d2}", minutes, seconds, miliseconds);
	}

	public string GetTimeFormatedForWinScreenWithoutMinutes (float gt) {
		int seconds = (int) gt;
		int miliseconds = (int)(gt * 100 % 100);
		
		return string.Format ("{0:d2}:{1:d2}", seconds, miliseconds);
	}

	public string GetTimeFormatedForWinScreen (int gt) {
		int minutes = gt / 60;
		int seconds = gt % 60;
		int miliseconds = (gt * 100 % 100);
		
		return string.Format ("{0:d2}:{1:d2}:{2:d2}", minutes, seconds, miliseconds);
	}

	public string GetTimeFormatedForWinScreen () {
		int minutes = (int) globaltime / 60;
		int seconds = (int) globaltime % 60;
		int miliseconds = (int)(globaltime * 100 % 100);
		
		return string.Format ("{0:d2}:{1:d2}:{2:d2}", minutes, seconds, miliseconds);
	}

	public string GetTimeFormatedForWinScreenWithouMinutes () {
		int seconds = (int) globaltime;
		int miliseconds = (int)(globaltime * 100 % 100);
		
		return string.Format ("{0:d2}:{1:d2}",  seconds, miliseconds);
	}

	//--------------------------------------
	// Action Handlers Methods
	//--------------------------------------
	
	void HandleOnLevelStartedAction () {
		StartTimer();
	}

	void HandleOnLevelFinishedAction () {
		StopTimer();
	}

	//--------------------------------------
	// Private Methods
	//--------------------------------------

	private string GetTimeFormated () {
		int minutes = (int) globaltime / 60;
		int seconds = (int) globaltime % 60;
		int miliseconds = (int)(globaltime * 100 % 100);

		return string.Format ("{0:d2}:{1:d2}:{2:d2}", minutes, seconds, miliseconds);
	}

	private void StartTimer() {
		TimerTurnedOn = true;
	}

	private void StopTimer() {
		TimerTurnedOn = false;
	}

	private string GetTimeFormatedForAnalitics () {
		int minutes = (int) globaltime / 60;
		int seconds = (int) globaltime % 60;
		
		return string.Format ("{0:d2}:{1:d2}", minutes, seconds);
	}


}
