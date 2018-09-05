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
	public class AdColonyNetwork : AdNetworkTemplate {

		private const string iosAppId = "appfb92f54ad2e146b49a";
		private const string iosInterstitialAdZone = "vz4afa95a6b7b54cb5b1";
		private const string iosRewardedVideoAdZone = "vz937427139c80463d89";

		private const string androidAppId = "app0c76d14a00174ba9b1";
		private const string androidIntrestitialAdZone = "vzbe002fb30130433cb0";
		private const string androidRewardedVideoAdZone = "vz5bdfadf596a24e4684";

		private const string amazonAppId = "";
		private const string amazonIntrestitialAdZone = "";
		private const string amazonRewardedVideoAdZone = "";

		[SerializeField]
		public bool isOpen = false;

		[SerializeField]
		private string _iosAppId = string.Empty;
		[SerializeField]
		private string _iosInterstitialAdZone = string.Empty;
		[SerializeField]
		private string _iosRewardedVideoAdZone = string.Empty;

		[SerializeField]
		private string _androidAppId = string.Empty;
		[SerializeField]
		private string _androidInterstitialAdZone = string.Empty;
		[SerializeField]
		private string _androidRewardedVideoAdZone = string.Empty;

		[SerializeField]
		private string _amazonAppId = string.Empty;
		[SerializeField]
		private string _amazonInterstitialAdZone = string.Empty;
		[SerializeField]
		private string _amazonRewardedVideoAdZone = string.Empty;

		private const string AdColonyProviderFilePath = "StansAssets/Modules/UltimateAds/Scripts/Providers/AdColonyProvider.cs";
		private const string AdColonyNetworkEditorFilePath = "StansAssets/Modules/UltimateAds/Scripts/Editor/AdColonyEditor.cs";

		private IBaseAd _provider;

		public AdColonyNetwork() {
			_name = "AdColony";
			_sdkLink = "https://github.com/AdColony/AdColony-Unity-SDK-3";
			_provider = new AdColonyProvider (this);
		}

		public override void LoadExampleSettings() {
			_iosAppId = iosAppId;
			_iosInterstitialAdZone = iosInterstitialAdZone;
			_iosRewardedVideoAdZone = iosRewardedVideoAdZone;

			_androidAppId = androidAppId;
			_androidInterstitialAdZone = androidIntrestitialAdZone;
			_androidRewardedVideoAdZone = androidRewardedVideoAdZone;

			_amazonAppId = amazonAppId;
			_amazonInterstitialAdZone = amazonIntrestitialAdZone;
			_amazonRewardedVideoAdZone = amazonRewardedVideoAdZone;
		}

		public override IBaseAd Provider {
			get {
				return _provider;
			}
		}

		public override bool IsEnabled {
			get {				
				#if UNITY_EDITOR
				if (SA.Common.Util.Files.IsFileExists ("AdColony/Scripts/AdColony.cs")) {
					if (!_enabled) {
						_enabled = true;
						ChangeDefineState(AdColonyProviderFilePath, "ADCOLONY_ENABLED", true);
						ChangeDefineState(AdColonyNetworkEditorFilePath, "ADCOLONY_ENABLED", true);
					}
					return true;
				} else {
					if (_enabled) {
						_enabled = false;
						ChangeDefineState(AdColonyProviderFilePath, "ADCOLONY_ENABLED", false);
						ChangeDefineState(AdColonyNetworkEditorFilePath, "ADCOLONY_ENABLED", false);
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
			_logo = AssetDatabase.LoadAssetAtPath<Texture2D> ("Assets\\StansAssets\\Modules\\UltimateAds\\Scripts\\Editor\\Icons\\adcolony.png");
		}
		#endif

		public string iOSAppId {
			get {
				return _iosAppId;
			}
			set {
				_iosAppId = value;
			}
		}

		public string iOSInterstitialAdZone {
			get {
				return _iosInterstitialAdZone;
			}
			set {
				_iosInterstitialAdZone = value;
			}
		}

		public string iOSRewardedVideoAdZone {
			get {
				return _iosRewardedVideoAdZone;
			}
			set {
				_iosRewardedVideoAdZone = value;
			}
		}

		public string AndroidAppId {
			get {
				return _androidAppId;
			}
			set {
				_androidAppId = value;
			}
		}

		public string AndroidInterstitialAdZone {
			get {
				return _androidInterstitialAdZone;
			}
			set {
				_androidInterstitialAdZone = value;
			}
		}

		public string AndroidRewardedVideoAdZone {
			get {
				return _androidRewardedVideoAdZone;
			}
			set {
				_androidRewardedVideoAdZone = value;
			}
		}

		public string AmazonAppId {
			get {
				return _amazonAppId;
			}
			set {
				_amazonAppId = value;
			}
		}

		public string AmazonInterstitialAdZone {
			get {
				return _amazonInterstitialAdZone;
			}
			set {
				_amazonInterstitialAdZone = value;
			}
		}

		public string AmazonRewardedVideoAdZone {
			get {
				return _amazonRewardedVideoAdZone;
			}
			set {
				_amazonRewardedVideoAdZone = value;
			}
		}
	}
}
