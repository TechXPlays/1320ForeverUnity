using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ExampleScene2Controller : MonoBehaviour {

	public void GoBack () {
		SceneManager.LoadScene ("StartExampleScene");
	}

	public void GoNext() {
		SceneManager.LoadScene ("ExampleScene3");
	}

}
