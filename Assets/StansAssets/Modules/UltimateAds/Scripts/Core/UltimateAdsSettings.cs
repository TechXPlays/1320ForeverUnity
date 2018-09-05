////////////////////////////////////////////////////////////////////////////////
//  
// @module Ultimate Ads
// @author Alexey Yaremenko (Stan's Assets) 
// @support support@stansassets.com
// @website https://stansassets.com
//
////////////////////////////////////////////////////////////////////////////////

using UnityEngine;
using System;
using System.IO;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace SA.UltimateAds {
	#if UNITY_EDITOR
	[InitializeOnLoad]
	#endif
	public class UltimateAdsSettings : ScriptableObject {

		public int ToolbarSelectedIndex = 0;

		public const string VERSION_NUMBER = "1.1/19";

		public const string UltimateAdSettingsAssetName = "UltimateAdSettings";
		public const string UltimateAdSettingsAssetExtension = ".asset";

		[SerializeField]
		private List<AdNetworkTemplate> _networks = new List<AdNetworkTemplate>();

		[SerializeField]
		private List<Trigger> _bannerTriggers = new List<Trigger>();

		[SerializeField]
		private List<Trigger> _interstitialTriggers = new List<Trigger>();

		[SerializeField]
		private List<Trigger> _videoTriggers = new List<Trigger>();

		[SerializeField]
		private List<Trigger> _triggers = new List<Trigger> ();

		[SerializeField]
		private List<Rule> _rules = new List<Rule>();

		private static UltimateAdsSettings instance = null;

		public static UltimateAdsSettings Instance {

			get {
				if (instance == null) {
					instance = Resources.Load(UltimateAdSettingsAssetName) as UltimateAdsSettings;

					if (instance == null) {
						instance = CreateInstance<UltimateAdsSettings>();
						#if UNITY_EDITOR
						SA.Common.Util.Files.CreateFolder(SA.Common.Config.SETTINGS_PATH);

						string fullPath = Path.Combine(Path.Combine("Assets", SA.Common.Config.SETTINGS_PATH),
							UltimateAdSettingsAssetName + UltimateAdSettingsAssetExtension
						);
						AssetDatabase.CreateAsset(instance, fullPath);

						List<AdNetworkTemplate> _nets = new List<AdNetworkTemplate>();
						AdNetworkTemplate network = ScriptableObject.CreateInstance<AdMobNetwork> ();
						network.hideFlags = HideFlags.HideInInspector | HideFlags.HideInHierarchy;
						AssetDatabase.AddObjectToAsset (network, instance);
						_nets.Add (network);

						network = ScriptableObject.CreateInstance<ChartboostNetwork> ();
						network.hideFlags = HideFlags.HideInInspector | HideFlags.HideInHierarchy;
						AssetDatabase.AddObjectToAsset (network, instance);
						_nets.Add (network);

						network = ScriptableObject.CreateInstance<AdColonyNetwork> ();
						network.hideFlags = HideFlags.HideInInspector | HideFlags.HideInHierarchy;
						AssetDatabase.AddObjectToAsset (network, instance);
						_nets.Add (network);

						network = ScriptableObject.CreateInstance<VungleNetwork> ();
						network.hideFlags = HideFlags.HideInInspector | HideFlags.HideInHierarchy;
						AssetDatabase.AddObjectToAsset (network, instance);
						_nets.Add (network);

						network = ScriptableObject.CreateInstance<UnityAdsNetwork> ();
						network.hideFlags = HideFlags.HideInInspector | HideFlags.HideInHierarchy;
						AssetDatabase.AddObjectToAsset (network, instance);
						_nets.Add (network);

						AssetDatabase.ImportAsset(fullPath);

						instance.Networks = _nets;
						#endif
					}
				}
				return instance;
			}
		}

		public List<AdNetworkTemplate> Networks {
			get { 
				return _networks;
			}
			private set { 
				_networks = value;
			}
		}

		public List<Trigger> BannerTriggers {
			get {
				return _bannerTriggers;
			}
		}

		public List<Trigger> InterstitialTriggers {
			get {
				return _interstitialTriggers;
			}
		}

		public List<Trigger> VideoTriggers {
			get {
				return _videoTriggers;
			}
		}

		public List<Trigger> Triggers {
			get {
				return _triggers;
			}
		}

		public List<Rule> Rules {
			get {
				return _rules;
			}
		}
	}
}
