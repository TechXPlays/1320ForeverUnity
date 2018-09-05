using UnityEngine;
using System.Collections;


public class RS_Bot : MonoBehaviour {

	private RS_DragCarController _Car;

	void Awake() {
		_Car = GetComponent<RS_DragCarController>();
	}


	void FixedUpdate() {
		if(_Car.IsLaunched) {
			if(_Car.CurrnetRPM >= _Car.MaxRPM - 10) {
				_Car.ShiftUp();
			}
		} else {
			_Car.StepOnGas();
		}
	}
}
