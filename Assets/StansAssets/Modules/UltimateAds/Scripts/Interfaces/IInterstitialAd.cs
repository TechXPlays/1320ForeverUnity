﻿////////////////////////////////////////////////////////////////////////////////
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
	internal interface IInterstitialAd : IBaseAd {
		void Load();
		bool Show ();
		bool IsReady ();

		Action OnClosed { get; set; }
	}
}
