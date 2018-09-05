////////////////////////////////////////////////////////////////////////////////
//  
// @module $(modue_name)
// @author Stanislav Osipov lacost.st@gmail.com
//
////////////////////////////////////////////////////////////////////////////////

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class RS_GearTemplate  {
	
	public int id;
	public float speed;
	public AnimationCurve _curve = null;

	//--------------------------------------
	// INITIALIZE
	//--------------------------------------

	//--------------------------------------
	//  PUBLIC METHODS
	//--------------------------------------
	
	public void AddBenchmark(int rpm, float acceleration) {
		Keyframe kf = new Keyframe(rpm , acceleration );
		curve.AddKey(kf);
	}
	
	
	
	public float GetAcceleration(float rpm) {
		return curve.Evaluate(rpm);
	} 
	
	//--------------------------------------
	//  GET/SET
	//--------------------------------------
	
	public AnimationCurve curve {
		get {
			if(_curve == null) {
				_curve =  new AnimationCurve();
			}
			
			return _curve;
		}
	}
	
	
	//--------------------------------------
	//  EVENTS
	//--------------------------------------
	
	//--------------------------------------
	//  PRIVATE METHODS
	//--------------------------------------
	
	//--------------------------------------
	//  DESTROY
	//--------------------------------------


}
