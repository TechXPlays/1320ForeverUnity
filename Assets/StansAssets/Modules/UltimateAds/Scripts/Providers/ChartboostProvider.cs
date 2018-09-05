//#define CHARTBOOST_ENABLED

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

#if CHARTBOOST_ENABLED
using ChartboostSDK;
#endif

namespace SA.UltimateAds {
	internal class ChartboostProvider : IInterstitialAd, IRewardedVideoAd {

		private bool _inited = false;
		private bool _isInterstitialReady = false;
		private bool _isRewardedVideoReady = false;

		private Action _OnClosed = delegate {};
		private Action<bool, string, int> _OnRewarded = delegate {};

		public ChartboostProvider(ChartboostNetwork network) {}

		public void Init() {
			if (_inited) return;

			#if CHARTBOOST_ENABLED
			Chartboost.Create ();
			Chartboost.didCacheInterstitial += OnDidCacheInterstitial;
			Chartboost.didCloseInterstitial += OnDidCloseInterstitial;
			Chartboost.didCompleteRewardedVideo += OnDidCloseRewardedVideo;
			Chartboost.didCacheRewardedVideo += OnDidCacheRewardedVideo;

			_inited = true;
			#endif
		}

		void SA.UltimateAds.IInterstitialAd.Load() {
			#if CHARTBOOST_ENABLED
			Chartboost.cacheInterstitial (CBLocation.Default);
			#endif
		}

		void SA.UltimateAds.IRewardedVideoAd.Load() {
			#if CHARTBOOST_ENABLED
			Chartboost.cacheRewardedVideo (CBLocation.Default);
			#endif
		}

		bool SA.UltimateAds.IInterstitialAd.IsReady() {
			return _isInterstitialReady;
		}

		bool SA.UltimateAds.IRewardedVideoAd.IsReady() {
			return _isRewardedVideoReady;
		}

		bool SA.UltimateAds.IRewardedVideoAd.Show() {
			#if CHARTBOOST_ENABLED
			if (_isRewardedVideoReady) {
				Chartboost.showRewardedVideo (CBLocation.Default);
				_isRewardedVideoReady = false;
				return true;
			}
			#endif
			return false;
		}

		bool SA.UltimateAds.IInterstitialAd.Show() {
			#if CHARTBOOST_ENABLED
			if (_isInterstitialReady) {
				Chartboost.showInterstitial (CBLocation.Default);
				_isInterstitialReady = false;
				return true;
			}
			#endif
			return false;
		}

		#if CHARTBOOST_ENABLED
		private void OnDidCloseRewardedVideo(CBLocation location, int reward) {
			Debug.Log ("Chartboost did close rewarded video");
			_isRewardedVideoReady = true;
			_OnRewarded (true, "Reward", reward);
		}

		private void OnDidCacheRewardedVideo(CBLocation location) {
			Debug.Log ("Chartboost did cache rewarded video");
			_isRewardedVideoReady = true;
		}

		private void OnDidCacheInterstitial(CBLocation location) {
			Debug.Log ("Chartboost did cache interstitial");
			_isInterstitialReady = true;
		}

		private void OnDidCloseInterstitial(CBLocation location) {
			Debug.Log ("Chartboost did close interstitial");
			_isRewardedVideoReady = true;
			_OnClosed();
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
	}
}
