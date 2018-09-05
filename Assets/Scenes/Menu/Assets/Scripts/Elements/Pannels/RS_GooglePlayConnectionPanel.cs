using UnityEngine;
using System.Collections;

public class RS_GooglePlayConnectionPanel : MonoBehaviour {

	public GameObject ConnectionPanel;

	public GameObject Connecting;
	public GameObject Disconnected;


	void Awake() {

		if(Application.isEditor) {
			return;
		}

		if(UM_GameServiceManager.Instance.ConnectionSate == UM_ConnectionState.UNDEFINED) {
			UM_GameServiceManager.Instance.Connect();
		}

		StartCoroutine(DoConnectionCheck());
	}

	public void Connect() {
		UM_GameServiceManager.Instance.Connect();
	}


	IEnumerator DoConnectionCheck() {
		for(;;) {
			CheckConnection();
			yield return new WaitForSeconds(2f);
		}
	}


	public void CheckConnection() {


		switch(UM_GameServiceManager.Instance.ConnectionSate) {
		case UM_ConnectionState.CONNECTED:
			ConnectionPanel.SetActive(false);
			break;

		case UM_ConnectionState.CONNECTING:
			ConnectionPanel.SetActive(true);
			Connecting.SetActive(true);
			Disconnected.SetActive(false);
			break;
		case UM_ConnectionState.DISCONNECTED:
			ConnectionPanel.SetActive(true);
			Connecting.SetActive(false);
			Disconnected.SetActive(true);
			break;

		}
	}
}
