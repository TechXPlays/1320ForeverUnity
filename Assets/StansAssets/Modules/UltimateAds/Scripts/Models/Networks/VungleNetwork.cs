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
	public class VungleNetwork : AdNetworkTemplate {

		private const string iosAppId = "";
		private const string androidAppId = "56069b06e15077c231000162";
		private const string winAppId = "";

		private const string VungleProviderFilePath = "StansAssets/Modules/UltimateAds/Scripts/Providers/VungleProvider.cs";
		private const string VungleNetworkEditorFilePath = "StansAssets/Modules/UltimateAds/Scripts/Editor/VungleEditor.cs";

		[SerializeField]
		public bool isOpen = false;

		[SerializeField]
		private string _androidAppId = string.Empty;

		[SerializeField]
		private string _iosAppId = string.Empty;

		[SerializeField]
		private string _winAppId = string.Empty;

		private IBaseAd _provider;

		public VungleNetwork() {
			_name = "Vungle";
			_sdkLink = "https://v.vungle.com/sdk";
			_provider = new VungleProvider (this);
		}

		public override void LoadExampleSettings() {
			_iosAppId = iosAppId;
			_androidAppId = androidAppId;
			_winAppId = winAppId;
		}

		public override IBaseAd Provider {
			get {
				return _provider;
			}
		}

		public override bool IsEnabled {
			get {				
				#if UNITY_EDITOR
				if (SA.Common.Util.Files.IsFileExists ("Plugins/Vungle/Vungle.cs")) {
					if (!_enabled) {
						_enabled = true;
						ChangeDefineState(VungleProviderFilePath, "VUNGLE_ENABLED", true);
						ChangeDefineState(VungleNetworkEditorFilePath, "VUNGLE_ENABLED", true);
					}
					return true;
				} else {
					if (_enabled) {
						_enabled = false;
						ChangeDefineState(VungleProviderFilePath, "VUNGLE_ENABLED", false);
						ChangeDefineState(VungleNetworkEditorFilePath, "VUNGLE_ENABLED", false);
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
			_logo = AssetDatabase.LoadAssetAtPath<Texture2D> ("Assets\\StansAssets\\Modules\\UltimateAds\\Scripts\\Editor\\Icons\\vungle.png");
		}
		#endif

		public string AndroidAppId {
			get { return _androidAppId; }
			set { _androidAppId = value; }
		}

		public string iOSAppId {
			get { return _iosAppId; }
			set { _iosAppId = value; }
		}

		public string WinAppId {
			get { return _winAppId; }
			set { _winAppId = value; }
		}
	}
}
