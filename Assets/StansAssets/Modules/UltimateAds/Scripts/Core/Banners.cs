////////////////////////////////////////////////////////////////////////////////
//  
// @module Ultimate Ads
// @author Alexey Yaremenko (Stan's Assets) 
// @support support@stansassets.com
// @website https://stansassets.com
//
////////////////////////////////////////////////////////////////////////////////

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA.UltimateAds {
	public class Banners : SA.Common.Pattern.Singleton<Banners> {

		private static List<IBannerAd> _providers = new List<IBannerAd>();
		private static IBannerAd _currentProvider;

		private static bool _inited = false;

		private Banners() {}

		void Awake() {
			DontDestroyOnLoad (gameObject);
		}

		public static void Init() {
			if (_inited)
				return;

			AdsController.Instance.Init ();
			
			for (int i = 0; i < UltimateAdsSettings.Instance.Networks.Count; i++) {
				if (UltimateAdsSettings.Instance.Networks [i].IsEnabled && UltimateAdsSettings.Instance.Networks [i].Provider is IBannerAd) {
					IBannerAd banner = UltimateAdsSettings.Instance.Networks [i].Provider as IBannerAd;
					_providers.Add (banner);
					banner.Init ();
				}
			}

			if (_providers.Count > 0) {
				_inited = true;
				_currentProvider = Randomize ();
			}
		}

		public static bool Show() {
			if (!_inited) {
				Init ();
				return false;
			}
			return _currentProvider.Show ();
		}

		public static void Destroy() {
			if (_currentProvider != null)
				_currentProvider.Destroy ();
		}

		private static IBannerAd Randomize() {
			return _providers[UnityEngine.Random.Range (0, _providers.Count - 1)];
		}
	}
}
