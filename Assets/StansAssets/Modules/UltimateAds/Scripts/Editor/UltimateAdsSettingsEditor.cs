////////////////////////////////////////////////////////////////////////////////
//  
// @module Ultimate Ads
// @author Alexey Yaremenko (Stan's Assets) 
// @support support@stansassets.com
// @website https://stansassets.com
//
////////////////////////////////////////////////////////////////////////////////

#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using System;
using System.IO;
using System.Collections.Generic;
using SA.Common.Editor;

namespace SA.UltimateAds
{
	[CustomEditor (typeof(UltimateAdsSettings))]
	internal class SettingsEditor : Editor
	{
		private UltimateAdsSettings settings;

		private List<string> scenes = new List<string> ();

		private ReorderableList bannersList;
		private ReorderableList interstitialsList;
		private ReorderableList videosList;

		private ReorderableList rulesList;
		private ReorderableList triggersList;

		void Awake ()
		{
			settings = target as UltimateAdsSettings;
		}

		void OnEnable ()
		{
			scenes.Clear ();
			foreach (EditorBuildSettingsScene scene in EditorBuildSettings.scenes) {
				scenes.Add (Path.GetFileNameWithoutExtension (scene.path));
			}

			bannersList = new ReorderableList (settings.BannerTriggers, typeof(Trigger), true, true, true, true);
			bannersList.drawHeaderCallback = (Rect rect) => {
				GUIStyle style = EditorStyles.boldLabel;
				EditorGUI.LabelField (rect, "Banner Ads Triggers", style);
			};
			bannersList.drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) => {
				DrawSceneAdsConstructor(settings.BannerTriggers, rect, index);
			};

			bannersList.onSelectCallback = (ReorderableList l) => {
				//Just empty callback here
			};
			bannersList.onCanRemoveCallback = (ReorderableList l) => {
				return l.count > 0;
			};
			bannersList.onRemoveCallback = (ReorderableList l) => {
				ReorderableList.defaultBehaviours.DoRemoveButton (l);
			};
			bannersList.onAddCallback = (ReorderableList l) => {
				//Just empty callback here
			};
			bannersList.onAddDropdownCallback = (Rect buttonRect, ReorderableList l) => {				
				var menu = new GenericMenu ();
				foreach (string scene in scenes) {
					menu.AddItem (new GUIContent (scene), false, AddNewBannersTriggerClick, scene as object);
				}
				menu.ShowAsContext ();
			};

			interstitialsList = new ReorderableList (settings.InterstitialTriggers, typeof(Trigger), true, true, true, true);
			interstitialsList.drawHeaderCallback = (Rect rect) => {
				GUIStyle style = EditorStyles.boldLabel;
				EditorGUI.LabelField (rect, "Interstitial Ads Triggers", style);
			};
			interstitialsList.drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) => {
				DrawSceneAdsConstructor(settings.InterstitialTriggers, rect, index);
			};

			interstitialsList.onSelectCallback = (ReorderableList l) => {
				//Just empty callback here
			};
			interstitialsList.onCanRemoveCallback = (ReorderableList l) => {
				return l.count > 0;
			};
			interstitialsList.onRemoveCallback = (ReorderableList l) => {
				ReorderableList.defaultBehaviours.DoRemoveButton (l);
			};
			interstitialsList.onAddCallback = (ReorderableList l) => {
				//Just empty callback here
			};
			interstitialsList.onAddDropdownCallback = (Rect buttonRect, ReorderableList l) => {				
				var menu = new GenericMenu ();
				foreach (string scene in scenes) {
					menu.AddItem (new GUIContent (scene), false, AddNewInterstitialTriggerClick, scene as object);
				}
				menu.ShowAsContext ();
			};

			videosList = new ReorderableList (settings.VideoTriggers, typeof(Trigger), true, true, true, true);
			videosList.drawHeaderCallback = (Rect rect) => {
				GUIStyle style = EditorStyles.boldLabel;
				EditorGUI.LabelField (rect, "Video Ads Triggers", style);
			};
			videosList.drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) => {
				DrawSceneAdsConstructor(settings.VideoTriggers, rect, index);
			};

			videosList.onSelectCallback = (ReorderableList l) => {
				//Just empty callback here
			};
			videosList.onCanRemoveCallback = (ReorderableList l) => {
				return l.count > 0;
			};
			videosList.onRemoveCallback = (ReorderableList l) => {
				ReorderableList.defaultBehaviours.DoRemoveButton (l);
			};
			videosList.onAddCallback = (ReorderableList l) => {
				//Just empty callback here
			};
			videosList.onAddDropdownCallback = (Rect buttonRect, ReorderableList l) => {				
				var menu = new GenericMenu ();
				foreach (string scene in scenes) {
					menu.AddItem (new GUIContent (scene), false, AddNewVideoTriggerClick, scene as object);
				}
				menu.ShowAsContext ();
			};

			rulesList = new ReorderableList (settings.Rules, typeof(Trigger), true, true, true, true);
			rulesList.drawHeaderCallback = (Rect rect) => {
				GUIStyle style = EditorStyles.boldLabel;
				EditorGUI.LabelField (rect, "Rules", style);
			};
			rulesList.drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) => {
				float x = rect.x;
				float y = rect.y + 2.0f;
				Rule rule = settings.Rules.ToArray () [index];
				DrawUserRule(x, y, ref rule.Name, rule);
			};

			rulesList.onSelectCallback = (ReorderableList l) => {
				//Just empty callback here
			};
			rulesList.onCanRemoveCallback = (ReorderableList l) => {
				return l.count > 0;
			};
			rulesList.onRemoveCallback = (ReorderableList l) => {
				ReorderableList.defaultBehaviours.DoRemoveButton (l);
			};
			rulesList.onAddCallback = (ReorderableList l) => {
				//Just empty callback here
			};
			rulesList.onAddDropdownCallback = (Rect buttonRect, ReorderableList l) => {				
				var menu = new GenericMenu ();
				Array array = Enum.GetValues(typeof(Rule.ValType));
				foreach (object data in array) {
					menu.AddItem (new GUIContent (data.ToString()), false, DropDownRulesMenuItemClickHandler, data);
				}
				menu.ShowAsContext ();
			};

			triggersList = new ReorderableList (settings.Triggers, typeof(Trigger), true, true, true, true);
			triggersList.drawHeaderCallback = (Rect rect) => {
				GUIStyle style = EditorStyles.boldLabel;
				EditorGUI.LabelField (rect, "Triggers", style);
			};
			triggersList.drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) => {
				float x = rect.x;
				float y = rect.y + 2.0f;
				Trigger trigger = settings.Triggers.ToArray () [index];
				DrawUserRule(x, y, ref trigger.Name, trigger.Condition);
			};

			triggersList.onSelectCallback = (ReorderableList l) => {
				//Just empty callback here
			};
			triggersList.onCanRemoveCallback = (ReorderableList l) => {
				return l.count > 0;
			};
			triggersList.onRemoveCallback = (ReorderableList l) => {
				ReorderableList.defaultBehaviours.DoRemoveButton (l);
			};
			triggersList.onAddCallback = (ReorderableList l) => {
				//Just empty callback here
			};
			triggersList.onAddDropdownCallback = (Rect buttonRect, ReorderableList l) => {				
				var menu = new GenericMenu ();
				Array array = Enum.GetValues(typeof(Rule.ValType));
				foreach (object data in array) {
					menu.AddItem (new GUIContent (data.ToString()), false, DropDownTriggerMenuItemClickHandler, data);
				}
				menu.ShowAsContext ();
			};
		}

		public override void OnInspectorGUI ()
		{
			AboutGUI ();
		}

		private void DrawSceneAdsConstructor(List<Trigger> collection, Rect rect, int index) {
			Trigger trigger = collection.ToArray () [index];

			float x = rect.x + 5.0f;
			float y = rect.y + 2.0f;

			EditorGUI.LabelField (new Rect (x, y, 120.0f, EditorGUIUtility.singleLineHeight), trigger.Scene); x += 130.0f;
			EditorGUI.LabelField (new Rect (x + 10.0f, y, 55.0f, EditorGUIUtility.singleLineHeight), "Trigger:"); x += 60.0f;

			List<string> triggers = new List<string>();
			triggers.Add("Level Loaded");
			triggers.Add("Level Finished");
			foreach (Trigger tr in settings.Triggers) {
				triggers.Add(tr.Name);
			}

			trigger.SelectedIndex = EditorGUI.Popup (new Rect (x, y, 100.0f, EditorGUIUtility.singleLineHeight), trigger.SelectedIndex, triggers.ToArray()); x += 110.0f;
			if (trigger.SelectedIndex == 0)
				trigger.WhenShow = TriggerEvent.LevelLoaded;
			else if (trigger.SelectedIndex == 1)
				trigger.WhenShow = TriggerEvent.LevelFinished;
			else {
				trigger.WhenShow = TriggerEvent.Rule;
				foreach (Trigger tr in settings.Triggers) {
					if (tr.Name.Equals (triggers[trigger.SelectedIndex])) {
						trigger.Condition = tr.Condition;
						break;
					}
				}
			}
			EditorGUI.LabelField (new Rect (x + 10.0f, y, 55.0f, EditorGUIUtility.singleLineHeight), "Rule:"); x += 45.0f;

			List<string> rules = new List<string>();
			rules.Add("[Empty]");
			foreach (Rule r in settings.Rules) {
				rules.Add(r.Name);
			}
			trigger.RuleIndex = EditorGUI.Popup (new Rect (x, y, 100.0f, EditorGUIUtility.singleLineHeight), trigger.RuleIndex, rules.ToArray()); x += 110.0f;
			trigger.HasRule = trigger.RuleIndex != 0;
			if (trigger.HasRule) {
				foreach (Rule r in settings.Rules) {
					if (r.Name.Equals(rules[trigger.RuleIndex])) {
						trigger.Rule = r;
						break;
					}
				}
			}
		}

		private void DrawUserRule (float x, float y, ref string name, Rule rule) {
			EditorGUI.LabelField (new Rect (x, y, 45.0f, EditorGUIUtility.singleLineHeight), "Name:"); x += 45.0f;
			name = EditorGUI.TextField(new Rect (x, y, 90.0f, EditorGUIUtility.singleLineHeight), name); x += 93.0f;

			GUI.enabled = false;
			EditorGUI.LabelField (new Rect (x, y, 10.0f, EditorGUIUtility.singleLineHeight), "|"); x += 12.0f;
			GUI.enabled = true;
			EditorGUI.LabelField (new Rect (x, y, 60.0f, EditorGUIUtility.singleLineHeight), "Variable:"); x += 60.0f;
			rule.VariableName = EditorGUI.TextField (new Rect (x, y, 90.0f, EditorGUIUtility.singleLineHeight), rule.VariableName); x += 93.0f;
			rule.ValueType = (Rule.ValType)EditorGUI.EnumPopup (new Rect (x, y, 60.0f, EditorGUIUtility.singleLineHeight), rule.ValueType); x += 60.0f;

			if (rule.ValueType == Rule.ValType.Bool) {
				SA_YesNoBool variable = rule.Value.BoolValue ? SA_YesNoBool.Yes : SA_YesNoBool.No;
				variable = (SA_YesNoBool)EditorGUI.EnumPopup (new Rect (x + 10.0f, y, 40.0f, EditorGUIUtility.singleLineHeight), variable); x += 40.0f;
				rule.Value.BoolValue = variable == SA_YesNoBool.Yes ? true : false;
			} else if (rule.ValueType == Rule.ValType.Integer) {
				rule.Operation = (Rule.LogicOp)EditorGUI.EnumPopup (new Rect (x + 10.0f, y, 60.0f, EditorGUIUtility.singleLineHeight), rule.Operation); x += 70.0f;
				rule.Value.IntValue = EditorGUI.IntField(new Rect (x + 10.0f, y, 90.0f, EditorGUIUtility.singleLineHeight), rule.Value.IntValue); x += 90.0f;
			} else if (rule.ValueType == Rule.ValType.String) {
				rule.Value.StrValue = EditorGUI.TextField(new Rect (x + 10.0f, y, 200.0f, EditorGUIUtility.singleLineHeight), rule.Value.StrValue); x += 200.0f;
			}
		}

		private void AddNewBannersTriggerClick (object data) {
			settings.BannerTriggers.Add (new Trigger() { Scene = data as string, WhenShow = TriggerEvent.Rule });
		}

		private void AddNewInterstitialTriggerClick (object data) {
			settings.InterstitialTriggers.Add (new Trigger() { Scene = data as string, WhenShow = TriggerEvent.Rule });
		}

		private void AddNewVideoTriggerClick (object data) {
			settings.VideoTriggers.Add (new Trigger() { Scene = data as string, WhenShow = TriggerEvent.Rule });
		}

		private void DropDownTriggerMenuItemClickHandler (object data)
		{
			Trigger trigger = new Trigger ();
			trigger.WhenShow = TriggerEvent.Rule;
			trigger.Condition.ValueType = ((Rule.ValType)data);
			settings.Triggers.Add (trigger);
		}

		private void DropDownRulesMenuItemClickHandler (object data)
		{
			Rule rule = new Rule ();
			rule.ValueType = ((Rule.ValType)data);
			settings.Rules.Add (rule);
		}

		private void AboutGUI ()
		{
			GUILayoutOption[] toolbarSize = new GUILayoutOption[]{ GUILayout.Width (Width - 5), GUILayout.Height (30) };

			EditorGUILayout.BeginHorizontal ();
			GUILayout.FlexibleSpace ();

			string[] ToolbarTabs = { "General", "Settings", "Ads Constructor", "User Rules" };
			UltimateAdsSettings.Instance.ToolbarSelectedIndex = GUILayout.Toolbar (UltimateAdsSettings.Instance.ToolbarSelectedIndex, ToolbarTabs, toolbarSize);
			GUILayout.FlexibleSpace ();

			EditorGUILayout.EndHorizontal ();

			switch (UltimateAdsSettings.Instance.ToolbarSelectedIndex) {
			case 0:
				GeneralTab ();
				break;
			case 1:
				SettingsTab ();
				break;
			case 2:
				AdsConstructorTab ();
				break;
			case 3:
				UserRules ();
				break;
			default:
				break;
			}

			if (GUI.changed) {
				EditorUtility.SetDirty (settings);
			}
		}

		private void UserRules ()
		{
			EditorGUILayout.Space ();
			rulesList.DoLayoutList ();

			EditorGUILayout.Space ();
			triggersList.DoLayoutList ();
		}

		private void GeneralTab ()
		{
			EditorGUILayout.Space ();
			EditorGUILayout.HelpBox ("Advertising SDKs in this project", MessageType.None);
			EditorGUILayout.Space ();

			for (int i = 0; i < settings.Networks.Count; i++) {
				IAdNetwork network = settings.Networks [i];

				EditorGUILayout.BeginHorizontal ();

				GUIStyle s = new GUIStyle (GUI.skin.label);
				s.fixedWidth = 30.0f;
				s.fixedHeight = 30.0f;
				GUILayout.Label (network.Logo, s);

				EditorGUILayout.LabelField (network.Name, GUILayout.Width (150.0f));

				if (network.IsEnabled) {
					GUIStyle style = new GUIStyle (GUI.skin.label);
					style.fixedWidth = 150.0f;
					style.normal.textColor = Color.green;
					EditorGUILayout.LabelField ("[Detected]", style);
				} else {
					GUIStyle style = new GUIStyle (GUI.skin.label);
					style.fixedWidth = 150.0f;
					style.normal.textColor = Color.red;
					EditorGUILayout.LabelField ("[Not detected]", style);
				}

				if (!network.IsEnabled) {
					if (GUILayout.Button ("[Download SDK]", GUILayout.Width (120.0f))) {
						Application.OpenURL (network.SDKLink);
					}
				} else {
					if (GUILayout.Button ("[Go to Settings]", GUILayout.Width (120.0f))) {
						//Go to Settings tab
						UltimateAdsSettings.Instance.ToolbarSelectedIndex = 1;
					}
				}
				EditorGUILayout.Space ();
				EditorGUILayout.EndHorizontal ();
				EditorGUILayout.Space ();
			}

			EditorGUILayout.Space ();
			EditorGUILayout.HelpBox ("About the Plugin", MessageType.None);
			EditorGUILayout.Space ();

			SA.Common.Editor.Tools.SelectableLabelField (new GUIContent ("Plugin Version [?]", ""), UltimateAdsSettings.VERSION_NUMBER);
			SA.Common.Editor.Tools.SupportMail ();
			SA.Common.Editor.Tools.DrawSALogo ();
		}

		private void SettingsTab ()
		{
			EditorGUILayout.LabelField ("[Settings Tab goes here]");

			foreach (AdNetworkTemplate net in settings.Networks) {
				Editor editor = Editor.CreateEditor (net);
				editor.OnInspectorGUI ();
			}

			EditorGUILayout.Space ();
			EditorGUILayout.BeginHorizontal ();
			GUILayout.FlexibleSpace ();
			if (GUILayout.Button ("Load Example Settings", GUILayout.Width (200.0f))) {
				foreach (AdNetworkTemplate net in settings.Networks) {
					net.LoadExampleSettings ();
				}
			}
			GUILayout.FlexibleSpace ();
			EditorGUILayout.EndHorizontal ();
		}

		private void AdsConstructorTab ()
		{
			EditorGUILayout.Space ();
			bannersList.DoLayoutList ();

			EditorGUILayout.Space ();
			interstitialsList.DoLayoutList ();

			EditorGUILayout.Space ();
			videosList.DoLayoutList ();
		}

		private int _Width = 500;

		public int Width {
			get {
				EditorGUILayout.BeginHorizontal ();
				GUILayout.FlexibleSpace ();
				EditorGUILayout.EndHorizontal ();
				Rect scale = GUILayoutUtility.GetLastRect ();

				if (scale.width != 1) {
					_Width = System.Convert.ToInt32 (scale.width);
				}

				return _Width;
			}
		}
	}

	public class UltimateAdEditorMenu : EditorWindow
	{
		
		[MenuItem ("Window/Stan's Assets/Ultimate Ads/Edit Settings")]
		public static void Edit ()
		{
			Selection.activeObject = UltimateAdsSettings.Instance;
		}

	}
}

#endif
