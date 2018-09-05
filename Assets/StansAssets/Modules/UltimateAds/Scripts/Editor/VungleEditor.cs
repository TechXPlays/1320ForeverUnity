//#define VUNGLE_ENABLED

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
	[CustomEditor(typeof(VungleNetwork))]
	internal class VungleEditor : Editor {

		private VungleNetwork network;

		void Awake() {
			network = target as VungleNetwork;
		}

		public override void OnInspectorGUI() {
			EditorGUILayout.Space ();

			GUIStyle style = EditorStyles.foldout;
			style.font = EditorStyles.boldFont;
			if (network.isOpen = EditorGUILayout.Foldout (network.isOpen, network.Name, style)) {
#if VUNGLE_ENABLED
				network.AndroidAppId = EditorGUILayout.TextField ("Android App Id", network.AndroidAppId);
				network.iOSAppId = EditorGUILayout.TextField ("iOS App Id", network.iOSAppId);
				network.WinAppId = EditorGUILayout.TextField ("Windows App Id", network.WinAppId);
#else
				EditorGUILayout.HelpBox ("Vungle SDK DOESN'T exist", MessageType.Warning);				
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
