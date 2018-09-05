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
	public class UnityAdsNetwork : AdNetworkTemplate {

		private const string iosGameId = "106653";
		private const string iosRewardedVideoPlacementId = "rewardedVideoZone";
		private const string androidGameId = "106654";
		private const string androidRewardedVideoPlacementId = "rewardedVideoZone";

		private const string UnityProviderFilePath = "StansAssets/Modules/UltimateAds/Scripts/Providers/UnityAdsProvider.cs";
		private const string UnityNetworkEditorFilePath = "StansAssets/Modules/UltimateAds/Scripts/Editor/UnityAdsEditor.cs";

		[SerializeField]
		public bool isOpen = false;

		[SerializeField]
		private string _iosGameId = string.Empty;

		[SerializeField]
		private string _iosRewardedVideoPlacementId = string.Empty;

		[SerializeField]
		private string _androidGameId = string.Empty;

		[SerializeField]
		private string _androidRewardedVideoPlacementId = string.Empty;

		private IBaseAd _provider;

		public UnityAdsNetwork() {
			_name = "UnityAds";
			_sdkLink = "https://unity3d.com/ru/services/ads";
			_provider = new UnityAdsProvider (this);
		}

		public override void LoadExampleSettings() {
			_iosGameId = iosGameId;
			_iosRewardedVideoPlacementId = iosRewardedVideoPlacementId;
			_androidGameId = androidGameId;
			_androidRewardedVideoPlacementId = androidRewardedVideoPlacementId;
		}

		public override IBaseAd Provider {
			get {
				return _provider;
			}
		}

		public override bool IsEnabled {
			get {				
				#if UNITY_EDITOR
				if (SA.Common.Util.Files.IsFileExists ("UnityAds/UnityEngine.Advertisements.Android.dll")
					&& SA.Common.Util.Files.IsFileExists ("UnityAds/UnityEngine.Advertisements.iOS.dll")
					&& SA.Common.Util.Files.IsFileExists ("UnityAds/UnityEngine.Advertisements.Editor.dll")) {
					if (!_enabled) {
						_enabled = true;
						ChangeDefineState(UnityProviderFilePath, "UNITY_ADS", true);
						ChangeDefineState(UnityNetworkEditorFilePath, "UNITY_ADS", true);
					}
					return true;
				} else {
					if (_enabled) {
						_enabled = false;
						ChangeDefineState(UnityProviderFilePath, "UNITY_ADS", false);
						ChangeDefineState(UnityNetworkEditorFilePath, "UNITY_ADS", false);
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
			_logo = AssetDatabase.LoadAssetAtPath<Texture2D> ("Assets\\StansAssets\\Modules\\UltimateAds\\Scripts\\Editor\\Icons\\unityads.png");
		}
		#endif

		public string iOSGameId {
			get {
				return _iosGameId;
			}
			set {
				_iosGameId = value;
			}
		}

		public string iOSRewardedVideoPlacementId {
			get {
				return _iosRewardedVideoPlacementId;
			}
			set {
				_iosRewardedVideoPlacementId = value;
			}
		}

		public string AndroidGameId {
			get {
				return _androidGameId;
			}
			set {
				_androidGameId = value;
			}
		}

		public string AndroidRewardedVideoPlacementId {
			get {
				return _androidRewardedVideoPlacementId;
			}
			set {
				_androidRewardedVideoPlacementId = value;
			}
		}
	}
}
