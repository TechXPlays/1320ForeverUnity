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

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace SA.UltimateAds {
	[Serializable]
	public class AdMobNetwork : AdNetworkTemplate {

		private const string iosBannerAdUnitId = 			"ca-app-pub-6101605888755494/1852640761";
		private const string iosInterstitialAdUnitId = 		"ca-app-pub-6101605888755494/3329373962";
		private const string iosRewardedVideoAdUnitId = 	"ca-app-pub-6101605888755494/3513401162";

		private const string androidBannerAdUnitId = 		"ca-app-pub-6101605888755494/1824764765";
		private const string androidInterstitialAdUnitId = 	"ca-app-pub-6101605888755494/3301497967";
		private const string androidRewardedVideoAdUnitId = "ca-app-pub-6101605888755494/8922005165";

		private const string AdMobNetworkEditorFilePath = "StansAssets/Modules/UltimateAds/Scripts/Editor/AdMobEditor.cs";
		private const string AdMobProviderFilePath = "StansAssets/Modules/UltimateAds/Scripts/Providers/AdMobProvider.cs";

		[SerializeField]
		public bool isOpen = false;

		[SerializeField]
		private string _iosBannerId = string.Empty;
		[SerializeField]
		private string _iosInterstitialId = string.Empty;
		[SerializeField]
		private string _iosRewardedVideoId = string.Empty;

		[SerializeField]
		private string _androidBannerId = string.Empty;
		[SerializeField]
		private string _androidInterstitialId = string.Empty;
		[SerializeField]
		private string _androidRewardedVideoId = string.Empty;

		private IBaseAd _provider;

		public AdMobNetwork() {
			_name = "Google AdMob";
			_sdkLink = "https://github.com/googleads/googleads-mobile-unity";
			_provider = new AdMobProvider (this);
		}

		public override void LoadExampleSettings() {
			_iosBannerId = iosBannerAdUnitId;
			_iosInterstitialId = iosInterstitialAdUnitId;
			_iosRewardedVideoId = iosBannerAdUnitId;

			_androidBannerId = androidBannerAdUnitId;
			_androidInterstitialId = androidInterstitialAdUnitId;
			_androidRewardedVideoId = androidRewardedVideoAdUnitId;
		}

		public override IBaseAd Provider {
			get {
				return _provider;
			}
		}

		public override bool IsEnabled {
			get {				
				#if UNITY_EDITOR
				if (SA.Common.Util.Files.IsFolderExists ("GoogleMobileAds")) {
					if (!_enabled) {
						_enabled = true;
						ChangeDefineState(AdMobProviderFilePath, "ADMOB_ENABLED", true);
						ChangeDefineState(AdMobNetworkEditorFilePath, "ADMOB_ENABLED", true);
					}
					return true;
				} else {
					if (_enabled) {
						_enabled = false;
						ChangeDefineState(AdMobProviderFilePath, "ADMOB_ENABLED", false);
						ChangeDefineState(AdMobNetworkEditorFilePath, "ADMOB_ENABLED", false);
					}
				}
				return false;
				#else
				return _enabled;
				#endif
			}
		}

		#if UNITY_EDITOR
		public void OnEnable() {
			_logo = AssetDatabase.LoadAssetAtPath<Texture2D> ("Assets\\StansAssets\\Modules\\UltimateAds\\Scripts\\Editor\\Icons\\admob.png");
		}
		#endif

		public string iOSBannerAdUnitId {
			get { return _iosBannerId; }
			set { _iosBannerId = value; }
		}

		public string iOSInterstitialAdUnitId {
			get { return _iosInterstitialId; }
			set { _iosInterstitialId = value; }
		}

		public string iOSRewardedVideoAdUnitId {
			get { return _iosRewardedVideoId; }
			set { _iosRewardedVideoId = value; }
		}

		public string AndroidBannerAdUnitId {
			get { return _androidBannerId; }
			set { _androidBannerId = value; }
		}

		public string AndroidInterstitialAdUnitId {
			get { return _androidInterstitialId; }
			set { _androidInterstitialId = value; }
		}

		public string AndroidRewardedVideoAdUnitId {
			get { return _androidRewardedVideoId; }
			set { _androidRewardedVideoId = value; }
		}
	}
}
