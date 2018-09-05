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
	public class Interstitial : SA.Common.Pattern.Singleton<Interstitial> {

		public static Action OnClosed = delegate {};

		private static List<IInterstitialAd> _providers = new List<IInterstitialAd>();
		private static IInterstitialAd _currentProvider;

		private static bool _inited = false;

		private Interstitial() {}

		void Awake() {
			DontDestroyOnLoad (gameObject);
		}

		public static void Init() {
			if (_inited)
				return;

			AdsController.Instance.Init ();

			for (int i = 0; i < UltimateAdsSettings.Instance.Networks.Count; i++) {
				if (UltimateAdsSettings.Instance.Networks [i].IsEnabled && UltimateAdsSettings.Instance.Networks [i].Provider is IInterstitialAd) {
					IInterstitialAd interstitial = UltimateAdsSettings.Instance.Networks [i].Provider as IInterstitialAd;
					_providers.Add (interstitial);
					interstitial.Init ();
				}
			}

			if (_providers.Count > 0) {
				_inited = true;
				Load ();
			}

			Debug.Log (_providers.Count + " Interstitial Providers initialized");
		}

		public static void Load() {
			if (!_inited) return;

			#if UNITY_EDITOR
			SA_EditorAd.Instance.LoadInterstitial();
			#else
			foreach (IInterstitialAd interstitial in _providers) {
				if (!interstitial.IsReady())
					interstitial.Load ();
			}
			#endif
		}

		public static bool IsReady() {
			if (!_inited) return false;

			#if UNITY_EDITOR
			return SA_EditorAd.Instance.IsInterstitialReady;
			#else
			foreach (IInterstitialAd interstitial in _providers) {
				if (interstitial.IsReady ()) {
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
			SA_EditorAd.OnInterstitialFinished += Editor_InterstitialFinished;
			SA_EditorAd.Instance.ShowInterstitial();
			return true;
			#else
			foreach (IInterstitialAd interstitial in _providers) {
				FreeProvider (_currentProvider);
				SelectProvider (interstitial);
				if (_currentProvider.Show()) {
					return true;
				}
			}
			Load ();
			return false;
			#endif
		}

		private static void Editor_InterstitialFinished(bool status) {
			SA_EditorAd.OnInterstitialFinished -= Editor_InterstitialFinished;
			OnClosed ();
			Load ();
		}

		private static void OnInterstitialClosed() {
			OnClosed ();
			FreeProvider (_currentProvider);
			Load ();
		}

		private static void SelectProvider(IInterstitialAd provider) {
			_currentProvider = provider;
			if (provider != null)
				provider.OnClosed += OnInterstitialClosed;
		}

		private static void FreeProvider(IInterstitialAd provider) {
			if (provider != null)
				provider.OnClosed -= OnInterstitialClosed;
		}
	}
}
