//#define CHARTBOOST_ENABLED

////////////////////////////////////////////////////////////////////////////////
//  
// @module Ultimate Ads
// @author Alexey Yaremenko (Stan's Assets) 
// @support support@stansassets.com
// @website https://stansassets.com
//
////////////////////////////////////////////////////////////////////////////////

#if UNITY_EDITOR
using System;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace SA.UltimateAds {
	[CustomEditor(typeof(ChartboostNetwork))]
	internal class ChartboostEditor : Editor {

		private ChartboostNetwork network;

		void Awake() {
			network = target as ChartboostNetwork;
		}

		public override void OnInspectorGUI() {
			EditorGUILayout.Space ();

			GUIStyle style = EditorStyles.foldout;
			style.font = EditorStyles.boldFont;
			if (network.isOpen = EditorGUILayout.Foldout (network.isOpen, network.Name, style)) {
				
#if CHARTBOOST_ENABLED
				Type type = typeof(ChartboostSDK.CBSettings);
				PropertyInfo property = type.GetProperty ("Instance", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.NonPublic);
				if (property != null) {
					object instance = property.GetValue (null, null);
					if (instance != null) {
						Editor editor = Editor.CreateEditor (instance as UnityEngine.Object);
						editor.OnInspectorGUI ();
					} else {
						Debug.Log ("Sorry, Chartboost Instance value equals null");
					}
				} else {
					Debug.Log ("Ohh fuck, Chartboost Instance property equals null");
				}
#else
				EditorGUILayout.HelpBox ("Chartboost SDK DOESN'T exist", MessageType.Warning);
#endif
			}

			EditorGUILayout.Space ();
		}
	}
}
#endif
