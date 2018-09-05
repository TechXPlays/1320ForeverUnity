//#define ADCOLONY_ENABLED

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
	[CustomEditor(typeof(AdColonyNetwork))]
	internal class AdColonyEditor : Editor {

		private AdColonyNetwork network;

		void Awake() {
			network = target as AdColonyNetwork;
		}

		public override void OnInspectorGUI() {
			EditorGUILayout.Space ();

			GUIStyle style = EditorStyles.foldout;
			style.font = EditorStyles.boldFont;
			if (network.isOpen = EditorGUILayout.Foldout (network.isOpen, network.Name, style)) {
#if ADCOLONY_ENABLED
				EditorGUI.indentLevel++;
				EditorGUILayout.HelpBox ("iOS Platform Settings", MessageType.None);
				EditorGUI.indentLevel++;
				network.iOSAppId = EditorGUILayout.TextField ("App Id", network.iOSAppId);
				network.iOSInterstitialAdZone = EditorGUILayout.TextField ("Interstitial Zone Id", network.iOSInterstitialAdZone);
				network.iOSRewardedVideoAdZone = EditorGUILayout.TextField ("Rewarded Video Zone Id", network.iOSRewardedVideoAdZone);
				EditorGUI.indentLevel--;

				EditorGUILayout.HelpBox ("Android Platform Settings", MessageType.None);
				EditorGUI.indentLevel++;
				network.AndroidAppId = EditorGUILayout.TextField ("App Id", network.AndroidAppId);
				network.AndroidInterstitialAdZone = EditorGUILayout.TextField ("Interstitial Zone Id", network.AndroidInterstitialAdZone);
				network.AndroidRewardedVideoAdZone = EditorGUILayout.TextField ("Rewarded Video Zone Id", network.AndroidRewardedVideoAdZone);
				EditorGUI.indentLevel--;

				EditorGUILayout.HelpBox ("Amazon Platform Settings", MessageType.None);
				EditorGUI.indentLevel++;
				network.AmazonAppId = EditorGUILayout.TextField ("App Id", network.AmazonAppId);
				network.AmazonInterstitialAdZone = EditorGUILayout.TextField ("Interstitial Zone Id", network.AmazonInterstitialAdZone);
				network.AmazonRewardedVideoAdZone = EditorGUILayout.TextField ("Rewarded Video Zone Id", network.AmazonRewardedVideoAdZone);
				EditorGUI.indentLevel--;
				EditorGUI.indentLevel--;
#else
				EditorGUILayout.HelpBox ("AdColony SDK DOESN'T exist", MessageType.Warning);
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
