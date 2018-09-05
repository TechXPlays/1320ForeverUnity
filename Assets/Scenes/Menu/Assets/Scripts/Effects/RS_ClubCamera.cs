using UnityEngine;
using System.Collections;

public class RS_ClubCamera : MonoBehaviour {
	public Transform target;

	
	public float xSpeed = 120.0f;
	public float ySpeed = 120.0f;

	public float yMinLimit = -20f;
	public float yMaxLimit = 80f;

	public float distance = 8f;

	private float x = 0.0f;
	private float y = 0.0f;

	protected bool IsInRotationMode = false;


	//--------------------------------------
	// INITIALIZE
	//--------------------------------------


	// Use this for initialization
	void Start () {
		Vector3 angles = transform.eulerAngles;
		x = angles.y;
		y = angles.x;

	}


	public virtual void Update() {
		if(Input.GetMouseButtonDown(0)) {
			IsInRotationMode = true;
		} 

		if(Input.GetMouseButtonUp(0)) {
			IsInRotationMode = false;
		} 

	}

	void LateUpdate () {
		if (target != null && IsInRotationMode) {
			x += Input.GetAxis ("Mouse X") * xSpeed * distance * 0.02f;
			y -= Input.GetAxis ("Mouse Y") * ySpeed * 0.02f;

		}


		y = ClampAngle(y, yMinLimit, yMaxLimit);

		Quaternion rotation = Quaternion.Euler(y, x, 0);

	
		/*RaycastHit hit;
		if (Physics.Linecast (target.position, transform.position, out hit)) {
			distance -=  hit.distance;
		}*/


		Vector3 negDistance = new Vector3(0.0f, 0.0f, -distance);
		Vector3 position = rotation * negDistance + target.position;

		transform.rotation = rotation;
		transform.position = position;

	}

	public static float ClampAngle(float angle, float min, float max) {
		if (angle < -360F)
			angle += 360F;
		if (angle > 360F)
			angle -= 360F;
		return Mathf.Clamp(angle, min, max);
	}


	//--------------------------------------
	// PUBLIC METHODS
	//--------------------------------------
	
	//--------------------------------------
	// GET / SET
	//--------------------------------------
	
	//--------------------------------------
	// EVENTS
	//--------------------------------------
	
	//--------------------------------------
	// PRIVATE METHODS
	//--------------------------------------
	
	//--------------------------------------
	// DESTROY
	//--------------------------------------
}
