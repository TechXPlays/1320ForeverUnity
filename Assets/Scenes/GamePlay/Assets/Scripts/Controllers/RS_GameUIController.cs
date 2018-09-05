using UnityEngine;
using System.Collections;
using System;

public class RS_GameUIController : MonoBehaviour {
	public event Action ButtonGearUpEvent = delegate {};
	public event Action ButtonGearDownEvent = delegate {};
	public event Action ButtonGasPressedEvent = delegate {};
	public event Action ButtonGasReleasedEvent = delegate {};
	public event Action ButtonRestartEvent = delegate {};
	public event Action ButtonBoostEvent = delegate {};
	public event Action ButtonGoToMainMenuEvent = delegate {};

	[SerializeField]
	private DashboardController Dashboard;

	[SerializeField]
	private WinScreen WinnerScreen;

	[SerializeField]
	private GameTopPanelController TopPanel;

	[SerializeField]
	public CountDownTimer StartTimer;


	//--------------------------------------
	// Initialization
	//--------------------------------------


	void Start() {
		Dashboard.gameObject.SetActive(false);
		WinnerScreen.gameObject.SetActive(false);
		TopPanel.gameObject.SetActive(false);

        GoogleMobileAd.OnInterstitialClosed += GoogleMobileAd_OnInterstitialClosed;
	}

    private void OnDestroy()
    {
        
    }

    //--------------------------------------
    // Public Methods
    //--------------------------------------

    public void SetupUI(RS_DragCarController car) {
		Dashboard.gameObject.SetActive(true);
		TopPanel.gameObject.SetActive(true);
		Dashboard.AttachCar(car);
	}

	public void ShowForSingleplayer(UM_Player player, bool isNewRecord, float time, bool displayDelta, float delta) {
		Dashboard.gameObject.SetActive (false);
		TopPanel.gameObject.SetActive (false);

		WinnerScreen.SetSingleplayer (player, isNewRecord, time, displayDelta, delta);
	}

	public void ShowForMultiplayer (UM_TBM_Match match, float localPlayerTime) {
		WinnerScreen.SetMultiplayer (match, localPlayerTime);
	}

	//--------------------------------------
	// Button Handlers
	//--------------------------------------


	public void ButtonGearUpHandler() {
		ButtonGearUpEvent ();
	}

	public void ButtonGearDownHandler() {
		ButtonGearDownEvent ();
	}

	public void ButtonGasPressed() {
		ButtonGasPressedEvent ();
	}

	public void ButtonGasReleased() {
		ButtonGasReleasedEvent ();
	}

	public void ButtonRestartHandler() {
		ButtonRestartEvent ();
	}

	public void ButtonBoostHandler() {
		ButtonBoostEvent ();
	}

	public void ButtonGoToMainMenuHandler() {
        RS_PlayerData.Instance.AdsCD--;
        if(RS_PlayerData.Instance.AdsCD == 0) {
            if (GoogleMobileAd.IsInterstitialReady)
                GoogleMobileAd.ShowInterstitialAd();
            else
                RS_PlayerData.Instance.AdsCD = 1;
        } else {
            ButtonGoToMainMenuEvent();
        }
	}

    void GoogleMobileAd_OnInterstitialClosed()
    {
        GoogleMobileAd.LoadInterstitialAd();
        ButtonGoToMainMenuEvent();
    }
}
