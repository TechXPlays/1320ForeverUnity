using UnityEngine;
using System;
using System.Collections;

public class TrackFinishPoint : MonoBehaviour {

	public event Action OnPointTriggered = delegate{};

	void OnTriggerEnter(Collider other) {
		OnPointTriggered ();
	}
}
