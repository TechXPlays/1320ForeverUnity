using UnityEngine;
using System.Collections;

public class MultiplayerPreloaderTimeoutTask : MonoBehaviour {

	public static MultiplayerPreloaderTimeoutTask Create() {
		GameObject go = new GameObject("MultiplayerPreloaderTimeoutTask");
		return go.AddComponent<MultiplayerPreloaderTimeoutTask>();
	}

	void Awake() {
		DontDestroyOnLoad(gameObject);

		Invoke("Timeout", 10f);
	}


	public void Cancel() {
		Remove();
	}

	private void Timeout() {
		MultiplayerGameController.PrelaoderTimeOut();
	}

	private void Remove() {
		CancelInvoke("Timeout");
		DestroyImmediate(gameObject);
	}


}
