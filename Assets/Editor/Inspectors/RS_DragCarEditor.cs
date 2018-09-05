////////////////////////////////////////////////////////////////////////////////
//  
// @module $(modue_name)
// @author Stanislav Osipov lacost.st@gmail.com
//
////////////////////////////////////////////////////////////////////////////////

using UnityEngine;
using UnityEditor;
using System;
using System.Xml;
using System.Collections;
using System.Collections.Generic;


[CustomEditor(typeof(RS_DragCarController))]
public class RS_DragCarEditor : Editor {
	
	public SerializedProperty _TorgueCureve;
	
	public SerializedProperty  _FinalDrive;
	public SerializedProperty  _DriveEfficiency;
	

	public SerializedProperty  _wheelsRadius;
	public SerializedProperty  _wheelsGrip;
	public SerializedProperty  _weight;
	public SerializedProperty  _weightOverDriveWheels;
	
	public SerializedProperty  _carData;
	public SerializedProperty  _autoCaclulatedCurve;
	public SerializedProperty  _MaxRPM;
	public SerializedProperty  _boost;
	

	private SerializedProperty _torgue;
	private SerializedProperty _horsepower;
	
	
 	
	//--------------------------------------
	// INITIALIZE
	//--------------------------------------
	
	
	void Awake() {
		
	}
	
	public virtual void OnEnable () {
		
	}	

	//--------------------------------------
	//  PUBLIC METHODS
	//--------------------------------------
	
	public override void OnInspectorGUI() {
		Car.UpdatePowerCurve();
		base.OnInspectorGUI();
	}
	
	//--------------------------------------
	//  GET/SET
	//--------------------------------------
	
	public RS_DragCarController Car {
		get {
			return target as RS_DragCarController;
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
