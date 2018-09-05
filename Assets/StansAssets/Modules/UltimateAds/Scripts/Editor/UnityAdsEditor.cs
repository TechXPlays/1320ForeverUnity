//#define UNITY_ADS

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
	[CustomEditor(typeof(UnityAdsNetwork))]
	internal class UnityAdsEditor : Editor {

		private UnityAdsNetwork network;

		void Awake() {
			network = target as UnityAdsNetwork;
		}

		public override void OnInspectorGUI() {
			EditorGUILayout.Space ();

			GUIStyle style = EditorStyles.foldout;
			style.font = EditorStyles.boldFont;
			if (network.isOpen = EditorGUILayout.Foldout (network.isOpen, network.Name, style)) {
#if UNITY_ADS
				network.iOSGameId = EditorGUILayout.TextField ("iOS Game Id", network.iOSGameId);
				network.iOSRewardedVideoPlacementId = EditorGUILayout.TextField ("iOS Rewarded Video Placement Id", network.iOSRewardedVideoPlacementId);

				EditorGUILayout.Space ();
				network.AndroidGameId = EditorGUILayout.TextField ("Android Game Id", network.AndroidGameId);
				network.AndroidRewardedVideoPlacementId = EditorGUILayout.TextField ("Android Rewarded Video Placement Id", network.AndroidRewardedVideoPlacementId);
#else
				EditorGUILayout.HelpBox ("Unity Ads SDK DOESN'T exist", MessageType.Warning);
#endif
			}

			EditorGUILayout.Space ();
		}
	}
}
#endif
