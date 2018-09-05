using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StartExampleSceneController : MonoBehaviour {

	public Button showVideoBtn;

	void Awake() {
		PlayerPrefs.DeleteAll ();

		/*SA.UltimateAds.Banners.Init ();
		SA.UltimateAds.Banners.Show ();*/
		
		/*SA.UltimateAds.Interstitial.OnClosed += InterstitialClosed;
		SA.UltimateAds.Interstitial.Init ();*/

		SA.UltimateAds.Video.OnFinished += VideoFinished;
		SA.UltimateAds.Video.Init ();

		/*SA.UltimateAds.RewardedVideo.OnRewarded += Rewarded;
		SA.UltimateAds.RewardedVideo.Init ();*/
	}

	private void InterstitialClosed () {
		Debug.Log ("Interstitial Closed");
	}

	private void Rewarded (bool success, string item, int count) {
		Debug.Log ("User Rewarded " + success + " " + item + " " + count);
	}

	private void VideoFinished (bool success) {
		Debug.Log ("Video Finished " + success);
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void LoadVideo () {
		SA.UltimateAds.Video.Load ();
		//SA.UltimateAds.Interstitial.Load();
		//SA.UltimateAds.RewardedVideo.Load();
	}

	public void ShowVideo() {
		SA.UltimateAds.Video.Show ();
		//SA.UltimateAds.Interstitial.Show();
		//SA.UltimateAds.RewardedVideo.Show();
	}

	public void GoToNextScene() {
		SceneManager.LoadScene ("ExampleScene2");
	}
}
