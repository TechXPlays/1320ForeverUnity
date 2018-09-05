//#define ADCOLONY_ENABLED

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

namespace SA.UltimateAds {
	internal class AdColonyProvider : IVideoAd, IRewardedVideoAd {

		#if ADCOLONY_ENABLED
		private AdColonyNetwork _network;
		private AdColony.InterstitialAd _ad;
		#endif

		private bool _inited = false;
		private bool _isVideoReady = false;
		private bool _isRewardedVideoReady = false;

		private Action<bool> _OnFinished = delegate {};
		private Action<bool, string, int> _OnRewarded = delegate {};

		public AdColonyProvider(AdColonyNetwork network) {
			_isRewardedVideoReady = false;
			#if ADCOLONY_ENABLED
			_network = network;
			#endif
		}

		public void Init() {
			if (_inited) return;

			#if ADCOLONY_ENABLED
			AdColony.Ads.OnConfigurationCompleted += AdColony_Ads_OnConfigurationCompleted;
			AdColony.Ads.OnRequestInterstitial += AdColony_Ads_OnRequestInterstitial;
			AdColony.Ads.OnRequestInterstitialFailed += AdColony_Ads_OnRequestInterstitialFailed;
			AdColony.Ads.OnOpened += AdColony_Ads_OnOpened;
			AdColony.Ads.OnClosed += AdColony_Ads_OnClosed;
			AdColony.Ads.OnExpiring += AdColony_Ads_OnExpiring;
			AdColony.Ads.OnIAPOpportunity += AdColony_Ads_OnIAPOpportunity;
			AdColony.Ads.OnRewardGranted += AdColony_Ads_OnRewardGranted;
			AdColony.Ads.OnCustomMessageReceived += AdColony_Ads_OnCustomMessageReceived;

			// Set some test app options with metadata.
			AdColony.AppOptions appOptions = new AdColony.AppOptions();
			appOptions.AdOrientation = AdColony.AdOrientationType.AdColonyOrientationAll;
			AdColony.UserMetadata metadata = new AdColony.UserMetadata();
			appOptions.Metadata = metadata;

			List<string> _androidAdZones = new List<string>() { InterstitialZoneId, RewardedVideoZoneId };
			AdColony.Ads.Configure (AppId, appOptions, _androidAdZones.ToArray());

			_inited = true;
			#endif
		}

		#if ADCOLONY_ENABLED
		private void AdColony_Ads_OnCustomMessageReceived (string type, string message)
		{
			Debug.Log (string.Format("AdColony OnCustomMessageReceived Type:{0} Message:{1}", type, message));
		}

		private void AdColony_Ads_OnRewardGranted (string zoneId, bool success, string name, int amount)
		{
			Debug.Log (string.Format("AdColony OnRewardGranted ZoneId:{0} Success:{1} Name:{2}, Amount:{3}", zoneId, success, name, amount));
			_OnRewarded (success, name, amount);
		}

		private void AdColony_Ads_OnIAPOpportunity (AdColony.InterstitialAd ad, string productId, AdColony.AdsIAPEngagementType type)
		{
			Debug.Log (string.Format("AdColony OnIAPOpportunity Id:{0} Type:{1}", productId, type.ToString()));
		}

		private void AdColony_Ads_OnExpiring (AdColony.InterstitialAd ad)
		{
			Debug.Log ("AdColony OnExpiring");
		}

		private void AdColony_Ads_OnClosed (AdColony.InterstitialAd ad)
		{
			Debug.Log ("AdColony OnClosed");
			if (ad.ZoneId.Equals (InterstitialZoneId)) {
				_OnFinished (true);
			}
		}

		private void AdColony_Ads_OnOpened (AdColony.InterstitialAd ad)
		{
			Debug.Log ("AdColony OnOpened");
		}

		private void AdColony_Ads_OnRequestInterstitialFailed ()
		{
			_isVideoReady = false;
			_isRewardedVideoReady = false;
			Debug.Log ("AdColony OnRequestInterstitialFailed");
		}

		private void AdColony_Ads_OnRequestInterstitial (AdColony.InterstitialAd ad)
		{
			_ad = ad;
			if (ad.ZoneId.Equals (RewardedVideoZoneId)) {
				_isRewardedVideoReady = true;
			}

			if (ad.ZoneId.Equals(InterstitialZoneId)) {
				_isVideoReady = true;
			}
			Debug.Log ("AdColony OnRequestInterstitial");
		}

		private void AdColony_Ads_OnConfigurationCompleted (List<AdColony.Zone> zones)
		{
			Debug.Log ("AdColony OnConfigurationCompleted called");
			if (zones == null || zones.Count <= 0) {
				Debug.Log ("No available Ad Zones");
			} else {
				Debug.Log (string.Format("{0} Ad Zones available", zones.Count));
			}
		}
		#endif

		void SA.UltimateAds.IVideoAd.Load() {
			#if ADCOLONY_ENABLED
			AdColony.AdOptions adOptions = new AdColony.AdOptions();
			adOptions.ShowPrePopup = false;
			adOptions.ShowPostPopup = false;
			AdColony.Ads.RequestInterstitialAd (InterstitialZoneId, adOptions);
			#endif
		}

		void SA.UltimateAds.IRewardedVideoAd.Load() {
			#if ADCOLONY_ENABLED
			AdColony.AdOptions adOptions = new AdColony.AdOptions();
			adOptions.ShowPrePopup = false;
			adOptions.ShowPostPopup = false;
			AdColony.Ads.RequestInterstitialAd (RewardedVideoZoneId, adOptions);
			#endif
		}

		public bool IsVideoReady() {
			return _isVideoReady;
		}

		public bool IsReady() {
			return _isRewardedVideoReady;
		}

		bool SA.UltimateAds.IRewardedVideoAd.Show() {
			#if ADCOLONY_ENABLED
			if (_isRewardedVideoReady) {
				_isRewardedVideoReady = false;
				AdColony.Ads.ShowAd (_ad);
				return true;
			}
			#endif
			return false;
		}

		bool SA.UltimateAds.IVideoAd.Show() {
			#if ADCOLONY_ENABLED
			if (_isVideoReady) {
				_isVideoReady = false;
				AdColony.Ads.ShowAd (_ad);
				return true;
			}
			#endif
			return false;
		}

		public Action<bool> OnFinished {
			get {
				return _OnFinished;
			}
			set { 
				_OnFinished = value;
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

		#if ADCOLONY_ENABLED
		private string AppId {
			get { 
				#if UNITY_ANDROID
				return _network.AndroidAppId;
				#elif UNITY_IOS
				return _network.iOSAppId;
				#else
				return string.Empty;
				#endif
			}
		}

		private string InterstitialZoneId {
			get { 
				#if UNITY_ANDROID
				return _network.AndroidInterstitialAdZone;
				#elif UNITY_IOS
				return _network.iOSInterstitialAdZone;
				#else
				return string.Empty;
				#endif
			}
		}

		private string RewardedVideoZoneId {
			get { 
				#if UNITY_ANDROID
				return _network.AndroidRewardedVideoAdZone;
				#elif UNITY_IOS
				return _network.iOSRewardedVideoAdZone;
				#else
				return string.Empty;
				#endif
			}
		}
		#endif
	}
}
