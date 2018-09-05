////////////////////////////////////////////////////////////////////////////////
//  
// @module Ultimate Ads
// @author Alexey Yaremenko (Stan's Assets) 
// @support support@stansassets.com
// @website https://stansassets.com
//
////////////////////////////////////////////////////////////////////////////////

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SA.UltimateAds {
	public class AdsController : SA.Common.Pattern.Singleton<AdsController> {

		private bool _initialised = false;
		private Scene _currentScene;

		private Dictionary<string, List<TriggerAd>> _sortedTriggers = new Dictionary<string, List<TriggerAd>>();

		private AdsController() { }

		void Awake () {
			DontDestroyOnLoad (gameObject);
		}

		public void Init() {
			if (_initialised)
				return;

			Debug.Log ("It's time to initialize Ultimate Ads Controller");
			ReloadTriggers ();

			//Subscribe to scene manager events
			SceneManager.sceneLoaded += SceneManager_sceneLoaded;
			SceneManager.sceneUnloaded += SceneManager_sceneUnloaded;

			_initialised = true;
		}

		void Update() {
			if (!_initialised) return;
			if (!_currentScene.IsValid() || !_sortedTriggers.ContainsKey (_currentScene.name)) return;

			foreach (TriggerAd triggerAd in _sortedTriggers[_currentScene.name]) {
				if (triggerAd.Trigger.WhenShow == TriggerEvent.Rule && triggerAd.Trigger.Condition.IsCorrect ()) {
					if (triggerAd.Trigger.HasRule) {
						if (triggerAd.Trigger.Rule.IsCorrect ()) {
							triggerAd.ShowAd ();
							_sortedTriggers [_currentScene.name].Remove (triggerAd);
							break;
						}
					} else {
						triggerAd.ShowAd ();
						_sortedTriggers [_currentScene.name].Remove (triggerAd);
						break;
					}
				}
			}
		}

		private void ReloadTriggers() {
			_sortedTriggers.Clear ();

			AddAndSortTriggers (UltimateAdsSettings.Instance.BannerTriggers, TriggerAd.Ad.Banner);
			AddAndSortTriggers (UltimateAdsSettings.Instance.InterstitialTriggers, TriggerAd.Ad.Interstitial);
			AddAndSortTriggers (UltimateAdsSettings.Instance.VideoTriggers, TriggerAd.Ad.Video);
		}

		private void AddAndSortTriggers (List<Trigger> collection, TriggerAd.Ad adType) {
			foreach (Trigger trigger in collection) {
				if (!_sortedTriggers.ContainsKey (trigger.Scene)) {
					List<TriggerAd> triggers = new List<TriggerAd> ();
					_sortedTriggers.Add (trigger.Scene, triggers);
				}
				_sortedTriggers [trigger.Scene].Add (new TriggerAd(adType, trigger));
			}
		}

		private void SceneManager_sceneUnloaded (Scene scene)
		{			
			if (!_sortedTriggers.ContainsKey(scene.name)) return;

			foreach (TriggerAd triggerAd in _sortedTriggers[scene.name]) {
				if (triggerAd.Trigger.WhenShow == TriggerEvent.LevelFinished) {
					if (triggerAd.Trigger.HasRule) {
						if (triggerAd.Trigger.Rule.IsCorrect ()) {
							triggerAd.ShowAd ();
							_sortedTriggers [scene.name].Remove (triggerAd);
							break;
						}
					} else {
						triggerAd.ShowAd ();
						_sortedTriggers [scene.name].Remove (triggerAd);
						break;
					}
				}
			}
		}

		private void SceneManager_sceneLoaded (Scene scene, LoadSceneMode loadMode)
		{
			ReloadTriggers ();
			_currentScene = scene;
			if (!_sortedTriggers.ContainsKey(scene.name)) return;

			foreach (TriggerAd triggerAd in _sortedTriggers[scene.name]) {
				if (triggerAd.Trigger.WhenShow == TriggerEvent.LevelLoaded) {
					if (triggerAd.Trigger.HasRule) {
						if (triggerAd.Trigger.Rule.IsCorrect ()) {
							triggerAd.ShowAd ();
							_sortedTriggers [scene.name].Remove (triggerAd);
							break;
						}
					} else {
						triggerAd.ShowAd ();
						_sortedTriggers [scene.name].Remove (triggerAd);
						break;
					}
				}
			}
		}
	}
}
