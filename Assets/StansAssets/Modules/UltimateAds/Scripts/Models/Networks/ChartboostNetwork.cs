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
	public class ChartboostNetwork : AdNetworkTemplate {

		[SerializeField]
		public bool isOpen = false;

		private const string ChartboostNetworkEditorFilePath = "StansAssets/Modules/UltimateAds/Scripts/Editor/ChartboostEditor.cs";
		private const string ChartboostProviderFilePath = "StansAssets/Modules/UltimateAds/Scripts/Providers/ChartboostProvider.cs";

		private IBaseAd _provider;

		public ChartboostNetwork() {
			_name = "Chartboost";
			_sdkLink = "https://answers.chartboost.com/hc/en-us/articles/200780379-Download-Integrate-the-Chartboost-SDK-for-Unity";
			_provider = new ChartboostProvider (this);
		}

		public override void LoadExampleSettings() {

		}

		public override IBaseAd Provider {
			get {
				return _provider;
			}
		}

		public override bool IsEnabled {
			get {				
				#if UNITY_EDITOR
				if (SA.Common.Util.Files.IsFileExists ("Chartboost/Scripts/Chartboost.cs")) {
					if (!_enabled) {
						_enabled = true;
						ChangeDefineState(ChartboostProviderFilePath, "CHARTBOOST_ENABLED", true);
						ChangeDefineState(ChartboostNetworkEditorFilePath, "CHARTBOOST_ENABLED", true);
					}
					return true;
				} else {
					if (_enabled) {
						_enabled = false;
						ChangeDefineState(ChartboostProviderFilePath, "CHARTBOOST_ENABLED", false);
						ChangeDefineState(ChartboostNetworkEditorFilePath, "CHARTBOOST_ENABLED", false);
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
			_logo = AssetDatabase.LoadAssetAtPath<Texture2D> ("Assets\\StansAssets\\Modules\\UltimateAds\\Scripts\\Editor\\Icons\\chartboost.png");
		}
		#endif

	}
}
