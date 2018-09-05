//#define ADMOB_ENABLED

////////////////////////////////////////////////////////////////////////////////
//  
// @module Ultimate Ads
// @author Alexey Yaremenko (Stan's Assets) 
// @support support@stansassets.com
// @website https://stansassets.com
//
////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if ADMOB_ENABLED
using GoogleMobileAds;
using GoogleMobileAds.Api;
#endif

namespace SA.UltimateAds {
	internal class AdMobProvider : IBannerAd, IInterstitialAd, IRewardedVideoAd {

		#if ADMOB_ENABLED
		private AdMobNetwork _network;

		private BannerView banner;
		private InterstitialAd interstitial;
		private RewardBasedVideoAd rewardedVideo;
		#endif

		private bool _inited = false;
		private bool _isInterstitialReady = false;
		private bool _isRewardedReady = false;

		private Action _OnClosed = delegate {};
		private Action<bool, string, int> _OnRewarded = delegate {};

		public AdMobProvider(AdMobNetwork network) {
			#if ADMOB_ENABLED
			_network = network;
			#endif
		}

		public void Init(){
			if (_inited)
				return;
			
			#if ADMOB_ENABLED
			rewardedVideo = RewardBasedVideoAd.Instance;
			rewardedVideo.OnAdClosed 				+= Rewarded_OnAdClosed;
			rewardedVideo.OnAdFailedToLoad 			+= Rewarded_OnAdFailedToLoad;
			rewardedVideo.OnAdLeavingApplication 	+= Rewarded_OnAdLeavingApplication;
			rewardedVideo.OnAdLoaded 				+= Rewarded_OnAdLoaded;
			rewardedVideo.OnAdOpening 				+= Rewarded_OnAdOpening;
			rewardedVideo.OnAdStarted 				+= Rewarded_OnAdStarted;
			rewardedVideo.OnAdRewarded 				+= Rewarded_OnAdRewarded;
			_inited = true;
			#endif
		}

		void SA.UltimateAds.IInterstitialAd.Load() {
			#if ADMOB_ENABLED
			_isInterstitialReady = false;
			if (interstitial != null) {
				CleanUpInterstitialSub(interstitial);
			}

			interstitial = new InterstitialAd (InterstitialUnitId);
			interstitial.OnAdClosed 				+= Interstitial_OnAdClosed;
			interstitial.OnAdFailedToLoad 			+= Interstitial_OnAdFailedToLoad;
			interstitial.OnAdLeavingApplication 	+= Interstitial_OnAdLeavingApplication;
			interstitial.OnAdLoaded 				+= Interstitial_OnAdLoaded;
			interstitial.OnAdOpening 				+= Interstitial_OnAdOpening;
			interstitial.LoadAd (this.CreateAdRequest());
			#endif
		}

		void SA.UltimateAds.IRewardedVideoAd.Load() {
			#if ADMOB_ENABLED
			_isRewardedReady = false;
			rewardedVideo.LoadAd(this.CreateAdRequest(), RewardedUnitId);
			#endif
		}

		bool SA.UltimateAds.IInterstitialAd.IsReady() {
			return _isInterstitialReady;
		}

		bool SA.UltimateAds.IRewardedVideoAd.IsReady() {
			return _isRewardedReady;
		}

		bool SA.UltimateAds.IBannerAd.Show() {
			#if ADMOB_ENABLED
			if (banner != null) {
				CleanUpBannerSub(banner);
			}

			banner = new BannerView (BannerUnitId, AdSize.SmartBanner, AdPosition.Top);
			banner.OnAdClosed 				+= Banner_OnAdClosed;
			banner.OnAdFailedToLoad 		+= Banner_OnAdFailedToLoad;
			banner.OnAdLeavingApplication 	+= Banner_OnAdLeavingApplication;
			banner.OnAdLoaded 				+= Banner_OnAdLoaded;
			banner.OnAdOpening 				+= Banner_OnAdOpening;
			banner.LoadAd (this.CreateAdRequest());
			#endif

			return true;
		}

		bool SA.UltimateAds.IInterstitialAd.Show() {
			#if ADMOB_ENABLED
			if ((this as IInterstitialAd).IsReady()) {
				interstitial.Show ();
				return true;
			}
			#endif
			return false;
		}

		bool SA.UltimateAds.IRewardedVideoAd.Show() {
			#if ADMOB_ENABLED
			if ((this as IRewardedVideoAd).IsReady()) {
				rewardedVideo.Show();
			}
			#endif
			return false;
		}

		#if ADMOB_ENABLED
		private void CleanUpBannerSub (BannerView ad) {
			ad.OnAdClosed 				-= Banner_OnAdClosed;
			ad.OnAdFailedToLoad 		-= Banner_OnAdFailedToLoad;
			ad.OnAdLeavingApplication 	-= Banner_OnAdLeavingApplication;
			ad.OnAdLoaded 				-= Banner_OnAdLoaded;
			ad.OnAdOpening 				-= Banner_OnAdOpening;
			ad = null;
		}

		private void CleanUpInterstitialSub(InterstitialAd ad) {
			ad.OnAdClosed 				-= Interstitial_OnAdClosed;
			ad.OnAdFailedToLoad 		-= Interstitial_OnAdFailedToLoad;
			ad.OnAdLeavingApplication 	-= Interstitial_OnAdLeavingApplication;
			ad.OnAdLoaded 				-= Interstitial_OnAdLoaded;
			ad.OnAdOpening 				-= Interstitial_OnAdOpening;
			ad = null;
		}

		private void Rewarded_OnAdRewarded(object sender, Reward reward) {
			Debug.Log ("Rewarded_OnAdRewarded");
			_OnRewarded (true, reward.Type, System.Convert.ToInt32(reward.Amount));
			_isRewardedReady = false;
		}

		private void Rewarded_OnAdStarted(object sender, EventArgs args) {
			Debug.Log ("Rewarded_OnAdStarted");
		}

		private void Rewarded_OnAdOpening(object sender, EventArgs args) {
			Debug.Log ("Rewarded_OnAdOpening");
		}

		private void Rewarded_OnAdLoaded(object sender, EventArgs args) {
			Debug.Log ("Rewarded_OnAdLoaded");
			_isRewardedReady = true;
		}

		private void Rewarded_OnAdLeavingApplication (object sender, EventArgs args) {
			Debug.Log ("Rewarded_OnAdLeavingApplication");
		}

		private void Rewarded_OnAdFailedToLoad (object sender, AdFailedToLoadEventArgs args) {
			Debug.Log ("Rewarded_OnAdFailedToLoad");
			_isRewardedReady = false;
		}

		private void Rewarded_OnAdClosed(object sender, EventArgs args) {
			Debug.Log ("Rewarded_OnAdClosed");
			_isRewardedReady = false;
		}

		private void Interstitial_OnAdOpening (object sender, System.EventArgs e)
		{
			Debug.Log ("Interstitial_OnAdOpening");
		}

		private void Interstitial_OnAdLoaded (object sender, System.EventArgs e)
		{
			Debug.Log ("Interstitial_OnAdLoaded");
			_isInterstitialReady = true;
		}

		private void Interstitial_OnAdLeavingApplication (object sender, System.EventArgs e)
		{
			Debug.Log ("Interstitial_OnAdLeavingApplication");
		}

		private void Interstitial_OnAdFailedToLoad (object sender, AdFailedToLoadEventArgs e)
		{
			Debug.Log ("Interstitial_OnAdFailedToLoad");
			_isInterstitialReady = false;
		}

		private void Interstitial_OnAdClosed (object sender, System.EventArgs e)
		{
			Debug.Log ("Interstitial_OnAdClosed");
			_isInterstitialReady = false;
			_OnClosed ();
		}

		private void Banner_OnAdOpening (object sender, System.EventArgs e)
		{
			Debug.Log ("Banner_OnAdOpening");
		}

		private void Banner_OnAdLoaded (object sender, System.EventArgs e)
		{
			Debug.Log ("Banner_OnAdLoaded");
			banner.Show ();
		}

		private void Banner_OnAdLeavingApplication (object sender, System.EventArgs e)
		{
			Debug.Log ("Banner_OnAdLeavingApplication");
		}

		private void Banner_OnAdFailedToLoad (object sender, AdFailedToLoadEventArgs e)
		{
			Debug.Log ("Banner_OnAdFailedToLoad");
		}

		private void Banner_OnAdClosed (object sender, System.EventArgs e)
		{
			Debug.Log ("Banner_OnAdClosed");
		}
		#endif

		public void Destroy() {
			#if ADMOB_ENABLED
			banner.Destroy ();
			#endif
		}

		#if ADMOB_ENABLED
		// Returns an ad request with custom ad targeting.
		private AdRequest CreateAdRequest()
		{
			return new AdRequest.Builder()
				.Build();
		}
		#endif

		public Action OnClosed {
			get {
				return _OnClosed;
			}
			set { 
				_OnClosed = value;
			}
		}

		public Action<bool, string, int> OnRewarded {
			get {
				return _OnRewarded;
			}
			set { 
				_OnRewarded = value;
			}
		}

		#if ADMOB_ENABLED
		private string BannerUnitId {
			get {
				#if UNITY_IOS
				return _network.iOSBannerAdUnitId;
				#elif UNITY_ANDROID
				return _network.AndroidBannerAdUnitId;
				#else
				return string.Empty;
				#endif
			}
		}

		private string InterstitialUnitId {
			get {
				#if UNITY_IOS
				return _network.iOSInterstitialAdUnitId;
				#elif UNITY_ANDROID
				return _network.AndroidInterstitialAdUnitId;
				#else
				return string.Empty;
				#endif
			}
		}

		private string RewardedUnitId {
			get  {
				#if UNITY_IOS
				return _network.iOSRewardedVideoAdUnitId;
				#elif UNITY_ANDROID
				return _network.AndroidRewardedVideoAdUnitId;
				#else
				return string.Empty;
				#endif
			}
		}
		#endif
	}
}
