using UnityEngine;
using System.Collections;

public class RS_CarEditorTestController : MonoBehaviour {


	public float torgue = 100;
	public WheelCollider[] wheels;

	public Transform COM;

	void Awake() {
		GetComponent<Rigidbody>().centerOfMass = COM.localPosition;
	}


	public void Update() {
		if(Input.GetKeyDown(KeyCode.W)) {

			foreach(WheelCollider w in wheels) {
				w.brakeTorque = 0;
				w.motorTorque = torgue;
			}

		}



		if(Input.GetKeyDown(KeyCode.S)) {
			foreach(WheelCollider w in wheels) {
				w.brakeTorque = torgue;
				w.motorTorque = 0;
			}

		}

	}
}
