﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

#if UNITY_EDITOR
using UnityEditor;
public class AxisDefiner
{
	public enum AxisType
	{
		KeyOrMouseButton = 0,
		MouseMovement = 1,
		JoystickAxis = 2
	};


	public class AxisDefinition
	{
		public string
			name,
			descriptiveName,
			descriptiveNegativeName,
			negativeButton,
			positiveButton,
			altNegativeButton,
			altPositiveButton;

		public float
			gravity,
			dead,
			sensitivity;

		public bool
			snap = false,
			invert = false;

		public AxisType type;

		public int
			axis,
			joyNum;
	}


	public static bool AxisDefined( string axisName )
	{
		SerializedObject serializedObject = new SerializedObject( AssetDatabase.LoadAllAssetsAtPath( "ProjectSettings/InputManager.asset" )[0] );
		SerializedProperty axes = serializedObject.FindProperty( "m_Axes" );

		axes.Next( true );
		axes.Next( true );

		while( axes.Next( false ) )
		{
			SerializedProperty axis = axes.Copy();
			axis.Next( true );

			if( axis.stringValue == axisName )
			{
				return true;
			}
		}

		return false;
	}

	public static SerializedProperty GetChildProperty( SerializedProperty parent, string name )
	{
		SerializedProperty child = parent.Copy();
		child.Next( true );

		do
		{
			if( child.name == name )
			{
				return child;
			}
		}
		while( child.Next( false ) );

		return null;
	}


	public static bool AddAxis( AxisDefinition axis )
	{
		if( AxisDefined( axis.name ) )
		{
			return false;
		}

		SerializedObject serializedObject = new SerializedObject( AssetDatabase.LoadAllAssetsAtPath( "ProjectSettings/InputManager.asset" )[0] );
		SerializedProperty axesProperty = serializedObject.FindProperty( "m_Axes" );

		axesProperty.arraySize++;
		serializedObject.ApplyModifiedProperties();

		SerializedProperty axisProperty = axesProperty.GetArrayElementAtIndex( axesProperty.arraySize - 1 );

		GetChildProperty( axisProperty, "m_Name" ).stringValue = axis.name;
		GetChildProperty( axisProperty, "descriptiveName" ).stringValue = axis.descriptiveName;
		GetChildProperty( axisProperty, "descriptiveNegativeName" ).stringValue = axis.descriptiveNegativeName;
		GetChildProperty( axisProperty, "negativeButton" ).stringValue = axis.negativeButton;
		GetChildProperty( axisProperty, "positiveButton" ).stringValue = axis.positiveButton;
		GetChildProperty( axisProperty, "altNegativeButton" ).stringValue = axis.altNegativeButton;
		GetChildProperty( axisProperty, "altPositiveButton" ).stringValue = axis.altPositiveButton;
		GetChildProperty( axisProperty, "gravity" ).floatValue = axis.gravity;
		GetChildProperty( axisProperty, "dead" ).floatValue = axis.dead;
		GetChildProperty( axisProperty, "sensitivity" ).floatValue = axis.sensitivity;
		GetChildProperty( axisProperty, "snap" ).boolValue = axis.snap;
		GetChildProperty( axisProperty, "invert" ).boolValue = axis.invert;
		GetChildProperty( axisProperty, "type" ).intValue = (int)axis.type;
		GetChildProperty( axisProperty, "axis" ).intValue = axis.axis - 1;
		GetChildProperty( axisProperty, "joyNum" ).intValue = axis.joyNum;

		serializedObject.ApplyModifiedProperties();
		return true;
	}
}
#endif
