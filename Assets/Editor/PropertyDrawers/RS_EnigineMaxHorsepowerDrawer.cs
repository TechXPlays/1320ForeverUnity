////////////////////////////////////////////////////////////////////////////////
//  
// @module $(modue_name)
// @author Stanislav Osipov lacost.st@gmail.com
//
////////////////////////////////////////////////////////////////////////////////

using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;


[CustomPropertyDrawer (typeof (RS_EnigineMaxHorsepower))]
public class RS_EnigineMaxHorsepowerDrawer : PropertyDrawer {
	
	const int curveWidth = 50;
    const float min = 0;
    const float max = 1;
   

	//--------------------------------------
	// INITIALIZE
	//--------------------------------------

	//--------------------------------------
	//  PUBLIC METHODS
	//--------------------------------------
	
	 public override void OnGUI (Rect pos, SerializedProperty prop, GUIContent label) {
        SerializedProperty MaxHorsepower = prop.FindPropertyRelative ("MaxHorsepower");
        SerializedProperty RPM = prop.FindPropertyRelative ("RPM");

        // Draw scale
		
		EditorGUI.LabelField(new Rect(  5, pos.y, 100f, pos.height), label );
		MaxHorsepower.floatValue = EditorGUI.FloatField(new Rect(100f, pos.y, 60f, pos.height),
			MaxHorsepower.floatValue
			);
		
	
		
		EditorGUI.LabelField(new Rect( 160f, pos.y, 50, pos.height), "Hp    at" );
		
		
		RPM.intValue = EditorGUI.IntField(new Rect( 205 , pos.y, 60f, pos.height),
			 RPM.intValue
			);
		EditorGUI.LabelField(new Rect( 270, pos.y, 40, pos.height), "RPM" );
		
		/*
		 MaxTorque.floatValue = EditorGUI.FloatField(new Rect(4, pos.y, pos.width / 2f, pos.height),
			label, MaxTorque.floatValue
			);
		
		 EditorGUI.LabelField(new Rect( pos.width / 2f + 5, pos.y, 40, pos.height), "at" );
		
		
		
		 RPM.intValue = EditorGUI.IntField(new Rect( pos.width / 2f + 50, pos.y, 100, pos.height),
			 RPM.intValue
			);
		
		*/
      /*  EditorGUI.Slider (
            new Rect (pos.x, pos.y, pos.width - curveWidth, pos.height),
            scale, min, max, label); */

        // Draw curve
      /*  int indent = EditorGUI.indentLevel;
        EditorGUI.indentLevel = 0;
        EditorGUI.PropertyField (
            new Rect (pos.width - curveWidth, pos.y, curveWidth, pos.height),
            curve, GUIContent.none);
        EditorGUI.indentLevel = indent; */
    }
	
	//--------------------------------------
	//  GET/SET
	//--------------------------------------
	
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
