using UnityEngine;
using System.Collections;


public class RS_GamePlayCamera : MonoBehaviour {

	public Transform LookTarget;
	public Vector3 positionOffset;

    private  Vector3  vel  = Vector3.one * 0.2f;
    //private Vector3 vel = Vector3.one;
	public Transform FollowTarget;



	public void FixedUpdate() {

		if(FollowTarget == null) {
			return;
		}

        float speed = 1;
        Vector3 pos =  FollowTarget.position + Vector3.back * (Mathf.Max (0, speed * 0.05f));
        //Vector3 pos = FollowTarget.position;

        pos += positionOffset;

        transform.position = Vector3.SmoothDamp(transform.position, pos, ref vel, 0.08f);
        //transform.position = Vector3.back;
        //transform.position = pos;
		//transform.LookAt(LookTarget.position);

		transform.LookAt(FollowTarget.position);
	}

}