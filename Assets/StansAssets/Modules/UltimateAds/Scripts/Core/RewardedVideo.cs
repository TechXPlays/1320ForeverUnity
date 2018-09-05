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
	public class RewardedVideo : SA.Common.Pattern.Singleton<RewardedVideo> {

		public static Action<bool, string, int> OnRewarded = delegate {};
		
		private static List<IRewardedVideoAd> _providers = new List<IRewardedVideoAd>();
		private static IRewardedVideoAd _currentProvider;

		private static bool _inited = false;

		private RewardedVideo() {}

		void Awake() {
			DontDestroyOnLoad (gameObject);
		}

		public static void Init() {
			if (_inited)
				return;

			AdsController.Instance.Init ();

			for (int i = 0; i < UltimateAdsSettings.Instance.Networks.Count; i++) {
				if (UltimateAdsSettings.Instance.Networks [i].IsEnabled && UltimateAdsSettings.Instance.Networks [i].Provider is IRewardedVideoAd) {
					IRewardedVideoAd rewarded = UltimateAdsSettings.Instance.Networks [i].Provider as IRewardedVideoAd;
					_providers.Add (rewarded);
					rewarded.Init ();
				}
			}

			if (_providers.Count > 0) {
				_inited = true;
				Load ();
			}

			Debug.Log (_providers.Count + " Rewarded Video Providers initialized");
		}

		public static void Load() {
			if (!_inited) return;

			#if UNITY_EDITOR
			SA_EditorAd.Instance.LoadVideo();
			#else
			foreach (IRewardedVideoAd rewarded in _providers) {
				if (!rewarded.IsReady())
					rewarded.Load ();
			}
			#endif
		}

		public static bool IsReady() {
			if (!_inited) return false;

			#if UNITY_EDITOR
			return SA_EditorAd.Instance.IsInterstitialReady;
			#else
			foreach (IRewardedVideoAd rewarded in _providers) {
				if (rewarded.IsReady())
					return true;
			}
			return false;
			#endif
		}

		public static bool Show() {
			if (!_inited) {
				Init ();
				return false;
			}

			#if UNITY_EDITOR
			SA_EditorAd.OnVideoFinished += Editor_VideoFinished;
			SA_EditorAd.Instance.ShowVideo();
			return true;
			#else
			foreach (IRewardedVideoAd rewarded in _providers) {
				FreeProvider (_currentProvider);
				SelectProvider (rewarded);
				if (_currentProvider.Show ())
					return true;
			}
			Load ();
			return false;
			#endif
		}

		private static void Editor_VideoFinished(bool status) {
			SA_EditorAd.OnVideoFinished -= Editor_VideoFinished;
			OnRewarded (status, "Reward", 1);
			Load ();
		}

		private static void OnUserRewarded(bool success, string item, int count) {
			OnRewarded (success, item, count);
			FreeProvider (_currentProvider);
			Load ();
		}

		private static void SelectProvider(IRewardedVideoAd provider) {
			_currentProvider = provider;
			if (provider != null)
				provider.OnRewarded += OnUserRewarded;
		}

		private static void FreeProvider(IRewardedVideoAd provider) {
			if (provider != null)
				provider.OnRewarded -= OnUserRewarded;
		}
	}
}
