using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class DashboardController : MonoBehaviour {

	[SerializeField]
	private Text GearValue;
	[SerializeField]
	private Text SpeedValue;
	[SerializeField]
	private Text RMPValue;
	[SerializeField]
	private Image RPMArrow;

	public GameObject warningLight;
	public GameObject[] signalLigts;


	private RS_DragCarController _AttachedCar = null;


	public void AttachCar(RS_DragCarController car) {
		_AttachedCar = car;
	}

	void FixedUpdate() {
		if(_AttachedCar != null) {
			SetValues(_AttachedCar);
		}
	}


	private void SetValues(RS_DragCarController car) {
		
		if (car != null) {

			SpeedValue.text = ((int)car.Speed).ToString();

			if(car.CurrentGear == 0) {
				GearValue.text = "N";
			} else {
				GearValue.text = ((int)car.CurrentGear).ToString();
			}


			SetRMP((int)car.CurrnetRPM);
		}
	}

	private float _rotationValue = 0f;
	private void SetRMP(int rpm) {
		float k = 185f / 7000f;

		float zRotation =  430f - (rpm * k);

		float lerpValue =  Mathf.Lerp(_rotationValue, zRotation, 0.2f);
		_rotationValue = lerpValue;


		RPMArrow.transform.localRotation = Quaternion.Euler(0f, 0f, _rotationValue);




		RMPValue.text = (rpm).ToString();


		foreach(GameObject go in signalLigts) {
			go.SetActive(false);
		}

		warningLight.SetActive(false);


		if(rpm > _AttachedCar.MaxRPM - 2500f) {

			signalLigts[0].SetActive(true);

			if(rpm > _AttachedCar.MaxRPM - 1000f) {
				signalLigts[1].SetActive(true);
			}

			if(rpm > _AttachedCar.MaxRPM - 500f) {
				signalLigts[2].SetActive(true);
				warningLight.SetActive(true);
			}

		} 
	}

}
