using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class XboxWirelessForWindows : Gamepad.Base
{
	public const string ID = "Controller (Xbox 360 Wireless Receiver for Windows)";

	public XboxWirelessForWindows( int index )
		: base( index, ID )
	{
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

	protected override bool GetButtonState( Buttons button )
	{
		switch( button )
		{
			case Buttons.DpadN:
			return GetAxis( "DPadY" ) > 0;
			case Buttons.DpadS:
			return GetAxis( "DPadY" ) < 0;
			case Buttons.DpadW:
			return GetAxis( "DPadX" ) < 0;
			case Buttons.DpadE:
			return GetAxis( "DPadX" ) > 0;
		}
		return false;
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
		string stickName = typeof( XboxWirelessForWindows ).Name;
		DefineMultiAxis( stickName, "LeftStickX", 1 );
		DefineMultiAxis( stickName, "LeftStickY", 2 );
		DefineMultiAxis( stickName, "RightStickX", 4 );
		DefineMultiAxis( stickName, "RightStickY", 5 );
		DefineMultiAxis( stickName, "LeftTrigger", 9 );
		DefineMultiAxis( stickName, "RightTrigger", 10 );
		DefineMultiAxis( stickName, "DPadX", 6 );
		DefineMultiAxis( stickName, "DPadY", 7 );
	}
#endif
}