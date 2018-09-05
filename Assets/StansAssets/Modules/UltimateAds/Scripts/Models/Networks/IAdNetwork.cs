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
	public interface IAdNetwork {

		string Name { get; }
		string SDKLink { get; }
		bool IsEnabled { get; }
		IBaseAd Provider { get; }

		#if UNITY_EDITOR
		Texture2D Logo { get; }
		#endif
	}
}
