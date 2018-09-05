//#define ADMOB_ENABLED

////////////////////////////////////////////////////////////////////////////////
//  
// @module Ultimate Ads
// @author Alexey Yaremenko (Stan's Assets) 
// @support support@stansassets.com
// @website https://stansassets.com
//
////////////////////////////////////////////////////////////////////////////////

#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace SA.UltimateAds {
	[CustomEditor(typeof(AdMobNetwork))]
	internal class AdMobEditor : Editor {

		private AdMobNetwork network;

		void Awake() {
			network = target as AdMobNetwork;
		}

		public override void OnInspectorGUI() {
			EditorGUILayout.Space ();

			GUIStyle style = EditorStyles.foldout;
			style.font = EditorStyles.boldFont;
			if (network.isOpen = EditorGUILayout.Foldout(network.isOpen, network.Name, style)) {
#if ADMOB_ENABLED
				EditorGUILayout.HelpBox ("iOS Platform", MessageType.None);
				network.iOSBannerAdUnitId = EditorGUILayout.TextField("Banner AdUnit Id", network.iOSBannerAdUnitId);
				network.iOSInterstitialAdUnitId = EditorGUILayout.TextField("Interstitial AdUnit Id", network.iOSInterstitialAdUnitId);
				network.iOSRewardedVideoAdUnitId = EditorGUILayout.TextField("Rewarded Video AdUnit Id", network.iOSRewardedVideoAdUnitId);

				EditorGUILayout.Space ();

				EditorGUILayout.HelpBox ("Android Platform", MessageType.None);
				network.AndroidBannerAdUnitId = EditorGUILayout.TextField("Banner AdUnit Id", network.AndroidBannerAdUnitId);
				network.AndroidInterstitialAdUnitId = EditorGUILayout.TextField("Interstitial AdUnit Id", network.AndroidInterstitialAdUnitId);
				network.AndroidRewardedVideoAdUnitId = EditorGUILayout.TextField("Rewarded Video AdUnit Id", network.AndroidRewardedVideoAdUnitId);
#else
				EditorGUILayout.HelpBox ("Google AdMob SDK DOESN'T exist", MessageType.Warning);
#endif
			}
			EditorGUILayout.Space ();

			if (GUI.changed) {
				EditorUtility.SetDirty (network);
			}
		}

	}
}
#endif
