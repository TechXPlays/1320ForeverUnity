//#define UNITY_ADS

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

#if UNITY_ADS
using UnityEngine.Advertisements;
#endif

namespace SA.UltimateAds {
	internal class UnityAdsProvider : IVideoAd, IRewardedVideoAd {

		#if UNITY_ADS
		private UnityAdsNetwork _network;
		#endif

		private Action _OnVideoLoaded = delegate {};
		private Action _OnRewardedLoaded = delegate {};
		private Action<bool> _OnFinished = delegate {};
		private Action<bool, string, int> _OnRewarded = delegate {};

		private bool _inited = false;

		public UnityAdsProvider(UnityAdsNetwork network) {
			#if UNITY_ADS
			_network = network;
			#endif
		}

		public void Init() {
			if (_inited) return;

			#if UNITY_ADS
			if (Advertisement.isSupported) {
				if (!Advertisement.isInitialized) {
					Advertisement.Initialize (GameId);
					Debug.Log ("Unity Ads Initialize!");
				} else {
					Debug.Log ("Unity Ads is already initialized");
				}
			} else {
				Debug.Log ("Unable to initialize Unity Ads. Platform is NOT supported");
			}
			_inited = true;
			#endif
		}

		void SA.UltimateAds.IVideoAd.Load() {
			//Just empty for Unity Ads
			if (IsVideoReady ())
				_OnVideoLoaded ();
		}

		bool SA.UltimateAds.IVideoAd.Show() {
			#if UNITY_ADS
			if (IsVideoReady ()) {
				ShowOptions options = new ShowOptions ();
				options.resultCallback = OnVideoShowResult;
				Advertisement.Show (options);
				return true;
			}
			#endif
			return false;
		}

		public bool IsVideoReady() {
			#if UNITY_ADS
			return Advertisement.IsReady();
			#else 
			return false;
			#endif
		}

		public bool IsReady() {
			#if UNITY_ADS
			return Advertisement.IsReady (RewardedVideoPlacementId);
			#else
			return false;
			#endif
		}

		void SA.UltimateAds.IRewardedVideoAd.Load() {
			//Just empty for Unity Ads
			if (IsReady ())
				_OnRewardedLoaded ();
		}

		bool SA.UltimateAds.IRewardedVideoAd.Show() {
			#if UNITY_ADS
			if (IsReady ()) {
				ShowOptions options = new ShowOptions ();
				options.resultCallback = OnRewardedVideoShowResult;
				Advertisement.Show (RewardedVideoPlacementId, options);
				return true;
			}
			#endif
			return false;
		}

		#if UNITY_ADS
		private void OnVideoShowResult (ShowResult result) {
			_OnFinished (result == ShowResult.Finished);

			switch (result) {
			case ShowResult.Failed:
				{
					Debug.Log ("Video Ads failed to show");
				}
				break;
			case ShowResult.Finished:
				{
					Debug.Log ("Video Ads finished");
				}
				break;
			case ShowResult.Skipped:
				{
					Debug.Log ("Video Ads was skipped"); 
				}
				break;
			default:
				break;
			}
		}
		#endif

		#if UNITY_ADS
		private void OnRewardedVideoShowResult (UnityEngine.Advertisements.ShowResult result) {
			_OnRewarded (result == ShowResult.Finished, "Reward", 1);

			switch (result) {
			case ShowResult.Failed:
				{
					Debug.Log ("Rewarded Video Ads failed to show");
				}
				break;
			case ShowResult.Finished:
				{
					Debug.Log ("Rewarded Video Ads finished. User rewarded!");
				}
				break;
			case ShowResult.Skipped:
				{
					Debug.Log ("Rewarded Video Ads was skipped"); 
				}
				break;
			default:
				break;
			}
		}
		#endif

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

		#if UNITY_ADS
		private string GameId {
			get { 
				#if UNITY_ANDROID
				return _network.AndroidGameId;
				#elif UNITY_IOS
				return _network.iOSGameId;
				#else
				return string.Empty;
				#endif
			}
		}

		private string RewardedVideoPlacementId {
			get {
				#if UNITY_ANDROID
				return _network.AndroidRewardedVideoPlacementId;
				#elif UNITY_IOS
				return _network.iOSRewardedVideoPlacementId;
				#else
				return string.Empty;
				#endif
			}
		}
		#endif
	}
}
