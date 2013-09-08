using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class OuyaController : Gamepad.Base
{
	public const string ID = "OUYA Game Controller";

	public OuyaController( int index )
		: base( index, ID )
	{
	}

	protected override bool TryGetKeyForButton( Buttons button, out int key )
	{
		switch( button )
		{
			case Buttons.FaceN:
			{
				key = 2;
				return true;
			}
			case Buttons.FaceS:
			{
				key = 0;
				return true;
			}
			case Buttons.FaceW:
			{
				key = 1;
				return true;
			}
			case Buttons.FaceE:
			{
				key = 3;
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
				key = 6;
				return true;
			}
			case Buttons.RightStick:
			{
				key = 7;
				return true;
			}
			case Buttons.DpadN:
			{
				key = 8;
				return true;
			}
			case Buttons.DpadS:
			{
				key = 9;
				return true;
			}
			case Buttons.DpadW:
			{
				key = 10;
				return true;
			}
			case Buttons.DpadE:
			{
				key = 11;
				return true;
			}
			default:
			{
				key = 0;
				return false;
			}
		}
	}

	public override float GetAxis( Axes axis )
	{
		switch( axis )
		{
			case Axes.LeftStickX:
			return GetAxis( "LeftStickX" );
			case Axes.LeftStickY:
			return GetAxis( "LeftStickY" );
			case Axes.RightStickX:
			return GetAxis( "RightStickX" );
			case Axes.RightStickY:
			return GetAxis( "RightStickY" );
			case Axes.LeftTrigger:
			return GetAxis( "LeftTrigger" );
			case Axes.RightTrigger:
			return GetAxis( "RightTrigger" );
			default:
			return 0f;
		}
	}

#if UNITY_EDITOR
	public static void DefineAxes()
	{
		string stickName = typeof( OuyaController ).Name;
		DefineMultiAxis( stickName, "LeftStickX", 1 );
		DefineMultiAxis( stickName, "LeftStickY", 2 );
		DefineMultiAxis( stickName, "RightStickX", 3 );
		DefineMultiAxis( stickName, "RightStickY", 4 );
		DefineMultiAxis( stickName, "LeftTrigger", 5 );
		DefineMultiAxis( stickName, "RightTrigger", 6 );
	}
#endif
}