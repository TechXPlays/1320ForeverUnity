using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CountDownTimer : MonoBehaviour {


	public event System.Action OnCountDownFinished = delegate {};

	[SerializeField]
	private Text TimerFiled;
	private int CurrentTime;




	public void StartCountDown() {
		CurrentTime = 4;
		gameObject.SetActive(true);
		NextValue();
	}


	private void NextValue() {

		if(CurrentTime == 0) {
			OnCountDownFinished();
			gameObject.SetActive(false);
			return;
		}

		CurrentTime--;

		if(CurrentTime == 0) {
			TimerFiled.text = "Go";
		} else {
			TimerFiled.text = CurrentTime.ToString();
		}

		Invoke("NextValue", 1f);
	}
}
