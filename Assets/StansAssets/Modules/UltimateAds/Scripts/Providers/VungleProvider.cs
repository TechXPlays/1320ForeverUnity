//#define VUNGLE_ENABLED

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

namespace SA.UltimateAds {
	internal class VungleProvider : IVideoAd {

		#if VUNGLE_ENABLED
		private VungleNetwork _network;
		#endif

		private Action _OnVideoLoaded = delegate {};
		private Action<bool> _OnFinished = delegate {};

		private bool _inited = false;
		private bool _isVideoReady = false;

		public VungleProvider(VungleNetwork network){
			#if VUNGLE_ENABLED
			_network = network;
			#endif
		}

		public void Init() {
			if (_inited) return;

			#if VUNGLE_ENABLED
			Vungle.adPlayableEvent += Vungle_adPlayableEvent;
			Vungle.onAdStartedEvent += Vungle_onAdStartedEvent;
			Vungle.onAdFinishedEvent += Vungle_onAdFinishedEvent;
			Vungle.init (_network.AndroidAppId, _network.iOSAppId, _network.WinAppId);

			_inited = true;
			Debug.Log (string.Format("Vungle Init: {0} | {1} | {2}", _network.AndroidAppId, _network.iOSAppId, _network.WinAppId));
			#endif
		}

		public void Load() {
			_isVideoReady = false;
		}

		public bool IsVideoReady () {
			return _isVideoReady;
		}

		public bool Show() {
			#if VUNGLE_ENABLED
			if (_isVideoReady) {
				Dictionary<string, object> options = new Dictionary<string, object> ();
				options.Add ("orientation", VungleAdOrientation.MatchVideo);
				options.Add ("immersive", true);
				Vungle.playAdWithOptions (options);
				return true;
			}
			#endif
			return false;
		}

		#if VUNGLE_ENABLED
		private void Vungle_adPlayableEvent (bool isAdAvailable)
		{			
			if (isAdAvailable) {
				_isVideoReady = true;
				_OnVideoLoaded ();
				Debug.Log ("Vungle video ad is ready to show");
			} else {
				Debug.Log ("Vungle video ad failed to load");
			}
		}

		private void Vungle_onAdFinishedEvent (AdFinishedEventArgs args)
		{
			_isVideoReady = false;
			_OnFinished (args.IsCompletedView);
			Debug.Log ("Vungle Ad Finished to play: " + args.IsCompletedView);
		}

		private void Vungle_onAdStartedEvent ()
		{
			_isVideoReady = false;
			Debug.Log ("Vungle Ads started to play");
		}
		#endif

		public Action OnLoaded {
			get {
				return _OnVideoLoaded;
			}
			set {
				_OnVideoLoaded = value;
			}
		}

		public Action<bool> OnFinished {
			get {
				return _OnFinished;
			}
			set {
				_OnFinished = value;
			}
		}
	}
}
