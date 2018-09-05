using UnityEngine;
using System.Collections;

public class RS_DesktopController : MonoBehaviour {


	public WheelCollider[] wheels;
	public float Torque = 100;

	void Update()  {
		if(Input.GetKeyDown(KeyCode.W)) {
			

			foreach(WheelCollider w in wheels) {
				w.motorTorque = Torque;
				w.brakeTorque = 0;
			}
		}

		if(Input.GetKeyDown(KeyCode.S)) {
			

			foreach(WheelCollider w in wheels) {
				w.motorTorque = 0;
				w.brakeTorque = Torque;
			}
		}
	}
}
