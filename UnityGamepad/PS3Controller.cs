using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class PS3Controller : Gamepad.Base
{
	public const string ID = "PLAYSTATION(R)3 Controller";

	public PS3Controller( int index )
		: base( index, ID )
	{
	}

	protected override bool TryGetKeyForButton( Buttons button, out int key )
	{
		switch( button )
		{
			case Buttons.FaceN:
			{
				key = 12;
				return true;
			}
			case Buttons.FaceS:
			{
				key = 14;
				return true;
			}
			case Buttons.FaceW:
			{
				key = 15;
				return true;
			}
			case Buttons.FaceE:
			{
				key = 13;
				return true;
			}
			case Buttons.LeftShoulder:
			{
				key = 10;
				return true;
			}
			case Buttons.RightShoulder:
			{
				key = 11;
				return true;
			}
			case Buttons.LeftStick:
			{
				key = 1;
				return true;
			}
			case Buttons.RightStick:
			{
				key = 2;
				return true;
			}
			case Buttons.DpadN:
			{
				key = 4;
				return true;
			}
			case Buttons.DpadS:
			{
				key = 6;
				return true;
			}
			case Buttons.DpadW:
			{
				key = 7;
				return true;
			}
			case Buttons.DpadE:
			{
				key = 5;
				return true;
			}
			case Buttons.Start:
			{
				key = 3;
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
		string stickName = typeof( PS3Controller ).Name;
		DefineMultiAxis( stickName, "LeftStickX", 1 );
		DefineMultiAxis( stickName, "LeftStickY", 2 );
		DefineMultiAxis( stickName, "RightStickX", 3 );
		DefineMultiAxis( stickName, "RightStickY", 4 );
		DefineMultiAxis( stickName, "LeftTrigger", 5 );
		DefineMultiAxis( stickName, "RightTrigger", 6 );
	}
#endif
}