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
	[Serializable]
	public class Trigger {

		[SerializeField]
		public string Name = "_Trigger";

		[SerializeField]
		public string Scene = string.Empty;

		[SerializeField]
		public TriggerEvent WhenShow = TriggerEvent.LevelLoaded;

		[SerializeField]
		public int RefreshRate = 60; //Banner refresh rate in seconds

		#if UNITY_EDITOR
		[SerializeField]
		public int SelectedIndex = 0;
		#endif

		[SerializeField]
		public int RuleIndex = 0;

		[SerializeField]
		public bool HasRule = false;

		[SerializeField]
		public Rule Condition = new Rule ();

		[SerializeField]
		public Rule Rule = new Rule ();

		public Trigger () {
		}

		public bool IsFired () {
			if (WhenShow == TriggerEvent.LevelLoaded || WhenShow == TriggerEvent.LevelFinished)
				return true;
			return Condition.IsCorrect ();
		}
	}
}
