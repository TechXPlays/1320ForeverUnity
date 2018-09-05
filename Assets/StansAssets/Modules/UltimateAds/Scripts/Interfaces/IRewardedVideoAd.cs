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
	internal interface IRewardedVideoAd : IBaseAd {
		void Load ();
		bool Show ();
		bool IsReady ();

		Action<bool, string, int> OnRewarded { get; set; }
	}
}
