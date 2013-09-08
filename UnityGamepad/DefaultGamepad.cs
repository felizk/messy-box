using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class DefaultGamepad : Gamepad.Base
{
	public DefaultGamepad( int index, string name )
		: base( index, name )
	{

	}

	public override float GetAxis( Axes axis )
	{
		return 0f;
	}

	protected override bool TryGetKeyForButton( Buttons button, out int key )
	{
		switch( button )
		{
			case Buttons.FaceN:
			{
				key = 3;
				return true;
			}
			case Buttons.FaceS:
			{
				key = 0;
				return true;
			}
			case Buttons.FaceW:
			{
				key = 2;
				return true;
			}
			case Buttons.FaceE:
			{
				key = 1;
				return true;
			}
			case Buttons.Start:
			{
				key = 7;
				return true;
			}
			case Buttons.Select:
			{
				key = 6;
				return true;
			}
			case Buttons.LeftShoulder:
			{
				key = 4;
				return true;
			}
			case Buttons.RightShoulder:
			{
				key = 5;
				return true;
			}
			case Buttons.LeftStick:
			{
				key = 8;
				return true;
			}
			case Buttons.RightStick:
			{
				key = 9;
				return true;
			}
			default:
			{
				key = 0;
				return false;
			}
		}
	}

#if UNITY_EDITOR
	public static void DefineAxes()
	{
		string stickName = typeof( DefaultGamepad ).Name;
		DefineMultiAxis( stickName, "LeftStickX", 1 );
		DefineMultiAxis( stickName, "LeftStickY", 2 );
		DefineMultiAxis( stickName, "RightStickX", 3 );
		DefineMultiAxis( stickName, "RightStickY", 4 );
		DefineMultiAxis( stickName, "LeftTrigger", 5 );
		DefineMultiAxis( stickName, "RightTrigger", 6 );

		// We use this class to define some testing axes.
		DefineTestAxis( stickName, "Axis1", 1 );
		DefineTestAxis( stickName, "Axis2", 2 );
		DefineTestAxis( stickName, "Axis3", 3 );
		DefineTestAxis( stickName, "Axis4", 4 );
		DefineTestAxis( stickName, "Axis5", 5 );
		DefineTestAxis( stickName, "Axis6", 6 );
		DefineTestAxis( stickName, "Axis7", 7 );
		DefineTestAxis( stickName, "Axis8", 8 );
		DefineTestAxis( stickName, "Axis9", 9 );
		DefineTestAxis( stickName, "Axis10", 10 );
	}

	protected static void DefineTestAxis( string controllerName, string axisName, int axis, float dead = 0.19f, float sensitivity = 1f, bool invert = false )
	{
		if( AxisDefiner.AddAxis(
			new AxisDefiner.AxisDefinition()
			{
				name = controllerName + axisName,
				dead = 0.19f,
				sensitivity = 1.0f,
				type = AxisDefiner.AxisType.JoystickAxis,
				axis = axis,
				joyNum = 0
			} ) )
		{
			if(!Application.isPlaying)
				Debug.Log( "Added axis: " + controllerName + axisName );
		}
	}
#endif
}
