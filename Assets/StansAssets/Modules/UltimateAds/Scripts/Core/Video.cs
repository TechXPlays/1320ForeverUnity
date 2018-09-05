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
	public class Video : SA.Common.Pattern.Singleton<Video> {

		public static Action<bool> OnFinished = delegate {};

		private static List<IVideoAd> _providers = new List<IVideoAd>();
		private static IVideoAd _currentProvider;

		private static bool _inited = false;

		private Video() {}

		void Awake() {
			DontDestroyOnLoad (gameObject);
		}

		public static void Init() {
			if (_inited)
				return;

			AdsController.Instance.Init ();

			for (int i = 0; i < UltimateAdsSettings.Instance.Networks.Count; i++) {
				if (UltimateAdsSettings.Instance.Networks [i].IsEnabled && UltimateAdsSettings.Instance.Networks [i].Provider is IVideoAd) {
					IVideoAd video = UltimateAdsSettings.Instance.Networks [i].Provider as IVideoAd;
					_providers.Add (video);
					video.Init ();
				}
			}

			if (_providers.Count > 0) {
				_inited = true;
				Load ();
			}

			Debug.Log (_providers.Count + " Video Providers initialized");
		}

		public static void Load() {
			if (!_inited) return;

			#if UNITY_EDITOR
			SA_EditorAd.Instance.LoadVideo();
			#else
			foreach (IVideoAd video in _providers) {
				if (!video.IsVideoReady())
					video.Load ();
			}
			#endif
		}

		public static bool IsVideoReady () {
			if (!_inited) return false;

			#if UNITY_EDITOR
			return SA_EditorAd.Instance.IsInterstitialReady;
			#else
			foreach (IVideoAd video in _providers) {
				if (video.IsVideoReady ()) {
					return true;
				}
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
			foreach (IVideoAd video in _providers) {
				FreeProvider (_currentProvider);
				SelectProvider (video);
				if (_currentProvider.Show()) {
					return true;
				}
			}
			Load ();
			return false;
			#endif
		}

		private static void Editor_VideoFinished(bool status) {
			SA_EditorAd.OnVideoFinished -= Editor_VideoFinished;
			OnFinished (status);
			Load ();
		}

		private static void OnVideoFinished(bool success) {
			OnFinished (success);
			FreeProvider (_currentProvider);
			Load ();
		}

		private static void SelectProvider(IVideoAd provider) {
			_currentProvider = provider;
			if (provider != null)
				provider.OnFinished += OnVideoFinished;
		}

		private static void FreeProvider(IVideoAd provider) {
			if (provider != null)
				provider.OnFinished -= OnVideoFinished;
		}
	}
}
