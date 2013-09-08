using UnityEngine;
using System.Collections;
using System;

public class GamepadTest : MonoBehaviour
{
	void OnGUI()
	{
		// Write out the values of all the gamepad buttons
		var names = Enum.GetNames( typeof( Buttons ) );
		var buttonCount = names.Length;
		for( int i = 0; i < buttonCount; i++ )
		{
			writeText( 100, 100 + i * 20, names[i] + ": " + Gamepad.GetButton( (Buttons)i ) );
		}

		// Write out the values for all the gamepad axes.
		names = Enum.GetNames( typeof( Axes ) );
		for( int i = 0; i < names.Length; i++ )
		{
			writeText( 100, 100 + ( i + buttonCount ) * 20, names[i] + ": " + Gamepad.GetAxis( (Axes)i ) );
		}

		// Write out all the debugging axes
		for( int i = 1; i <= 10; i++ )
		{
			writeText( 300, 100 + i * 20, "Axis" + i + ": " + Input.GetAxis( "DefaultGamepadAxis" + i ) );
		}

		// Write out all the debugging buttons.
		for( int i = 0; i < 20; i++ )
		{
			writeText( 400, 100 + i * 20, "Button " + i + ": " + Input.GetKey( "joystick button " + i ) );
		}

		// Write out all joystick names.
		var joysticks = Input.GetJoystickNames();
		for( int i = 0; i < joysticks.Length; i++ )
		{
			writeText( 600, 80 + i * 20, joysticks[i] );
		}
	}

	void writeText( int x, int y, string text )
	{
		GUI.Label( new Rect( x, y, 1000, 100 ), text );
	}
}
