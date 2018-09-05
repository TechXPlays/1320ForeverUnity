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

namespace SA.UltimateAds {
	public class TriggerAd {

		public enum Ad {
			Banner,
			Interstitial,
			Video
		}

		public Ad AdType;
		public Trigger Trigger;

		public TriggerAd (Ad ad, Trigger trigger) {
			AdType = ad;
			Trigger = trigger;
		}

		public void ShowAd() {
			switch (AdType) {
			case Ad.Banner:
				Banners.Show ();
				break;
			case Ad.Interstitial:
				Interstitial.Show ();
				break;
			case Ad.Video:
				if (!Video.Show ())
					RewardedVideo.Show ();
				break;
			default: break;
			}
		}
	}
}
