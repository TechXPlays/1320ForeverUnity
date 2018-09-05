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
	public abstract class AdNetworkTemplate : ScriptableObject, IAdNetwork {

		[SerializeField]
		protected string _sdkLink = string.Empty;
		
		[SerializeField]
		protected string _name = string.Empty;

		[SerializeField]
		protected bool _enabled = false;

		#if UNITY_EDITOR
		protected Texture2D _logo;
		#endif

		public AdNetworkTemplate() {
		}

		public abstract void LoadExampleSettings();

		#if UNITY_EDITOR
		protected void ChangeDefineState(string file, string tag, bool IsEnabled) {
			if(SA.Common.Util.Files.IsFileExists(file)) {
				string content = SA.Common.Util.Files.Read(file);
				int endlineIndex;
				endlineIndex = content.IndexOf(System.Environment.NewLine);
				if(endlineIndex == -1) {
					endlineIndex = content.IndexOf("\n");
				}
				string TagLine = content.Substring(0, endlineIndex);

				if(IsEnabled) {
					content 	= content.Replace(TagLine, "#define " + tag);
				} else {
					content 	= content.Replace(TagLine, "//#define " + tag);
				}
					
				SA.Common.Util.Files.Write(file, content);
			}		
		}
		#endif

		public abstract IBaseAd Provider { get; }

		public virtual string Name {
			get {
				return _name;
			}
		}

		public virtual bool IsEnabled {
			get {
				return _enabled;
			}
		}

		public string SDKLink {
			get {
				return _sdkLink;
			}
		}

		#if UNITY_EDITOR
		public Texture2D Logo {
			get {
				return _logo;
			}
		}
		#endif
	}
}
