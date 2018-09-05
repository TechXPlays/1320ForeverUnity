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
	public class Rule {

		[Serializable]
		public enum ValType {
			Bool = 0,
			Integer = 1,
			String = 2
		}

		[Serializable]
		public enum LogicOp {
			Greater = 1,
			Less = 2,
			Equals = 3,
			NotEqual = 4
		}

		[Serializable]
		public class Val
		{
			public bool BoolValue = true;
			public int IntValue = 0;
			public string StrValue = string.Empty;

			public Val() {}
		}

		[SerializeField]
		public string Name = "_UserRule";

		[SerializeField]
		public string VariableName = "_Name";

		[SerializeField]
		public LogicOp Operation = LogicOp.Equals;

		[SerializeField]
		public ValType ValueType = Rule.ValType.Bool;

		[SerializeField]
		public Val Value;

		public Rule() {
			Value = new Val ();
		}

		public bool IsCorrect() {
			switch (ValueType) {
			case ValType.Bool:
				return PlayerPrefs.HasKey(VariableName) ? Convert.ToBoolean(PlayerPrefs.GetInt(VariableName)).Equals(Value.BoolValue) : false;
			case ValType.Integer:
				return ValidateInteger ();
			case ValType.String:
				return PlayerPrefs.HasKey(VariableName) ? PlayerPrefs.GetString (VariableName).Equals (Value.StrValue) : false;
			default : return false;
			}
		}

		private bool ValidateInteger() {
			int variable = PlayerPrefs.HasKey(VariableName) ? PlayerPrefs.GetInt (VariableName) : -1;
			switch (Operation) {
			case LogicOp.Equals:
				return variable == Value.IntValue;
			case LogicOp.Greater:
				return variable > Value.IntValue;
			case LogicOp.Less:
				return variable < Value.IntValue;
			case LogicOp.NotEqual:
				return variable != Value.IntValue;
			default:
				return false;
			}
		}
	}
}
