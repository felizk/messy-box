using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public static class Gamepad
{
	const int MaxPads = 4;

	/// <summary>
	/// Returns true if the button is pressed down on any of the connected gamepads
	/// </summary>
	public static bool GetButton( Buttons button )
	{
		if( Time.frameCount != lastUpdateFrame )
			Update();

		foreach( var pad in pads )
			if( pad.GetButton( button ) )
				return true;

		return false;
	}

	/// <summary>
	/// Returns true if the button was pressed down this frame on any of the connected gamepads
	/// </summary>
	public static bool GetButtonDown( Buttons button )
	{
		if( Time.frameCount != lastUpdateFrame )
			Update();

		foreach( var pad in pads )
			if( pad.GetButtonDown( button ) )
				return true;

		return false;
	}

	/// <summary>
	/// Returns true if the button was released this frame on any of the connected gamepads
	/// </summary>
	public static bool GetButtonUp( Buttons button )
	{
		if( Time.frameCount != lastUpdateFrame )
			Update();

		foreach( var pad in pads )
			if( pad.GetButtonUp( button ) )
				return true;

		return false;
	}

	/// <summary>
	/// Returns true if the button is pressed down on the specified gamepad
	/// </summary>
	public static bool GetButton( int inputDevice, Buttons button )
	{
		if( Time.frameCount != lastUpdateFrame )
			Update();

		return pads[inputDevice].GetButton( button );
	}

	/// <summary>
	/// Returns true if the button was pressed down this frame on the specified gamepad
	/// </summary>
	public static bool GetButtonDown( int inputDevice, Buttons button )
	{
		if( Time.frameCount != lastUpdateFrame )
			Update();
		return pads[inputDevice].GetButtonDown( button );
	}

	/// <summary>
	/// Returns true if the button was released this frame on the specified gamepad
	/// </summary>
	public static bool GetButtonUp( int inputDevice, Buttons button )
	{
		if( Time.frameCount != lastUpdateFrame )
			Update();

		return pads[inputDevice].GetButtonUp( button );
	}

	/// <summary>
	/// Get the value for an axis from any controller.
	/// Returns the first value that is non-zero
	/// </summary>
	public static float GetAxis( Axes axis )
	{
		if( Time.frameCount != lastUpdateFrame )
			Update();

		foreach( var pad in pads )
		{
			var value = pad.GetAxis( axis );
			if( value != 0f )
				return value;
		}
		return 0f;
	}

	/// <summary>
	/// Get the value of an axis from the specified controller
	/// </summary>
	public static float GetAxis( int inputDevice, Axes axis )
	{
		if( Time.frameCount != lastUpdateFrame )
			Update();
		return pads[inputDevice].GetAxis( axis );
	}

	/// <summary>
	/// The aggregated state of all the controllers
	/// </summary>
	public static PadState Any = new PadState( -1 );

	/// <summary>
	/// The state of controller 1
	/// </summary>
	public static PadState One = new PadState( 0 );

	/// <summary>
	/// The state of controller 2
	/// </summary>
	public static PadState Two = new PadState( 1 );

	/// <summary>
	/// The state of controller 3
	/// </summary>
	public static PadState Three = new PadState( 2 );

	/// <summary>
	/// The state of controller 4
	/// </summary>
	public static PadState Four = new PadState( 3 );

	/// <summary>
	/// The current amount of connected controllers
	/// </summary>
	public static int ConnectedDeviceCount { get { return 0; } }

	private static int lastUpdateFrame = 0;
	private static List<Gamepad.Base> pads = new List<Gamepad.Base>();
	private static void Update()
	{
		if( lastUpdateFrame == 0 )
		{
#if UNITY_EDITOR
			SetupAxes();
#endif
		}

		lastUpdateFrame = Time.frameCount;

		// Update controller objects
		var joysticks = Input.GetJoystickNames();
		for( int i = 0; i < joysticks.Length; i++ )
		{
			if( i >= pads.Count )
				pads.Add( null );

			if( pads[i] == null || joysticks[i] != pads[i].Name )
			{
				pads[i] = CreateGamepad( joysticks[i], i );
			}
		}

		// Fill out the rest of the joypads with default gamepads.
		for( int i = joysticks.Length; i < MaxPads; i++ )
		{
			if( i >= pads.Count )
				pads.Add( null );

			if( pads[i] == null || !( pads[i] is DefaultGamepad ) )
			{
				pads[i] = CreateGamepad( null, i );
			}
		}
	}

	private static Gamepad.Base CreateGamepad( string joystickName, int index )
	{
		switch( joystickName )
		{
			case XboxWirelessForWindows.ID:
			return new XboxWirelessForWindows( index );
			case OuyaController.ID:
			return new OuyaController( index );
			case PS3Controller.ID:
			return new PS3Controller( index );
			default:
			return new DefaultGamepad( index, joystickName );
		}
	}

#if UNITY_EDITOR
	[UnityEditor.MenuItem( "Edit/Project Settings/Setup Gamepad Axes" )]
	private static void SetupAxes()
	{
		var methods = System.Reflection.Assembly.GetExecutingAssembly().GetTypes()
			.Where( x => typeof( Gamepad.Base ).IsAssignableFrom( x ) )
			.Select( x => x.GetMethod( "DefineAxes", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public ) )
			.Where( x => x != null && x.GetParameters().Length == 0 );
		foreach( var method in methods )
		{
			method.Invoke( null, null );
		}
	}
#endif


	public abstract class Base
	{
		public string Name { get; private set; }
		public int Index { get; set; }

		public Base( int index, string padName )
		{
			this.Name = padName;
			this.Index = index;
			this.axisPrefix = GetType().Name + index;
		}

		public bool GetButtonDown( Buttons button )
		{
			int key;
			if( TryGetKeyForButton( button, out key ) )
				return GetKeyDown( key );

			if( !GetButton( button ) )
				return false;
			else
				return !buttonPressFrames.ContainsKey( button ) || buttonPressFrames[button] == Time.frameCount - 1;
		}

		public bool GetButton( Buttons button )
		{
			int key;
			if( TryGetKeyForButton( button, out key ) )
				return GetKey( key );

			if( GetButtonState( button ) )
			{
				if( !buttonPressFrames.ContainsKey( button ) || buttonPressFrames[button] == -1 )
					buttonPressFrames[button] = Time.frameCount;

				buttonReleaseFrames[button] = -1;
				return true;
			}
			else
			{
				if( !buttonReleaseFrames.ContainsKey( button ) || buttonReleaseFrames[button] == -1 )
					buttonReleaseFrames[button] = Time.frameCount;

				buttonPressFrames[button] = -1;
				return false;
			}
		}
		public bool GetButtonUp( Buttons button )
		{
			int key;
			if( TryGetKeyForButton( button, out key ) )
				return GetKeyUp( key );

			if( GetButton( button ) )
				return false;
			else
				return !buttonReleaseFrames.ContainsKey( button ) || buttonReleaseFrames[button] == Time.frameCount - 1;
		}

		protected abstract bool TryGetKeyForButton( Buttons button, out int key );
		protected virtual bool GetButtonState( Buttons button ) { return false; }

		protected static StringBuilder buttonBuilder = new StringBuilder();

		protected string GetButtonName( int button )
		{
			buttonBuilder.Length = 0;
			buttonBuilder.Append( "joystick " );
			buttonBuilder.Append( Index + 1 );
			buttonBuilder.Append( " button " );
			buttonBuilder.Append( button );
			return buttonBuilder.ToString();
		}

		protected bool GetKeyDown( int buttonIndex )
		{
			return Input.GetKeyDown( GetButtonName( buttonIndex ) );
		}
		protected bool GetKey( int buttonIndex )
		{
			return Input.GetKey( GetButtonName( buttonIndex ) );
		}
		protected bool GetKeyUp( int buttonIndex )
		{
			return Input.GetKeyUp( GetButtonName( buttonIndex ) );
		}

		protected float GetAxis( string axis )
		{
			buttonBuilder.Length = 0;
			buttonBuilder.Append( axisPrefix );
			buttonBuilder.Append( axis );
			return Input.GetAxis( buttonBuilder.ToString() );
		}

		public virtual float GetAxis( Axes axis )
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
			}

			return 0f;
		}

		protected readonly string axisPrefix;

		private Dictionary<Buttons, int>
			buttonPressFrames = new Dictionary<Buttons, int>(),
			buttonReleaseFrames = new Dictionary<Buttons, int>();

#if UNITY_EDITOR
		protected static void DefineMultiAxis( string controllerName, string axisName, int axis, float dead = 0.19f, float sensitivity = 1f, bool invert = false )
		{
			for( int i = 0; i < Gamepad.MaxPads; i++ )
			{

				if( AxisDefiner.AddAxis(
					new AxisDefiner.AxisDefinition()
					{
						name = controllerName + i + axisName,
						dead = 0.19f,
						sensitivity = 1.0f,
						type = AxisDefiner.AxisType.JoystickAxis,
						axis = axis,
						joyNum = i + 1
					} ) )
				{
					if( !Application.isPlaying )
						Debug.Log( "Added axis: " + controllerName + i + axisName );
				}
			}
		}
#endif
	}
}

public struct ButtonState
{
	private int padIndex;
	private Buttons button;

	public ButtonState( int padIndex, Buttons button )
	{
		this.padIndex = padIndex;
		this.button = button;
	}

	public bool Down { get { return padIndex >= 0 ? Gamepad.GetButtonDown( padIndex, button ) : Gamepad.GetButtonDown( button ); } }
	public bool Up { get { return padIndex >= 0 ? Gamepad.GetButtonUp( padIndex, button ) : Gamepad.GetButtonUp( button ); ; } }

	public static implicit operator bool( ButtonState state )
	{
		return state.padIndex >= 0 ? Gamepad.GetButton( state.padIndex, state.button ) : Gamepad.GetButton( state.button );
	}

	public override string ToString()
	{
		return ( (bool)this ).ToString();
	}
}

public struct PadState
{
	int index;

	public PadState( int index )
	{
		this.index = index;
		FaceN = new ButtonState( index, Buttons.FaceN );
		FaceS = new ButtonState( index, Buttons.FaceS );
		FaceW = new ButtonState( index, Buttons.FaceE );
		FaceE = new ButtonState( index, Buttons.FaceE );

		DpadN = new ButtonState( index, Buttons.DpadN );
		DpadS = new ButtonState( index, Buttons.DpadS );
		DpadW = new ButtonState( index, Buttons.DpadW );
		DpadE = new ButtonState( index, Buttons.DpadE );

		Start = new ButtonState( index, Buttons.Start );
		Select = new ButtonState( index, Buttons.Select );

		LeftShoulder = new ButtonState( index, Buttons.LeftShoulder );
		RightShoulder = new ButtonState( index, Buttons.RightShoulder );

		LeftStick = new ButtonState( index, Buttons.LeftStick );
		RightStick = new ButtonState( index, Buttons.RightStick );
	}

	public ButtonState FaceN;
	public ButtonState FaceS;
	public ButtonState FaceW;
	public ButtonState FaceE;

	public ButtonState DpadN;
	public ButtonState DpadS;
	public ButtonState DpadW;
	public ButtonState DpadE;

	public ButtonState Start;
	public ButtonState Select;

	public ButtonState LeftShoulder;
	public ButtonState RightShoulder;

	public ButtonState LeftStick;
	public ButtonState RightStick;

	public float LeftStickX { get { return index < 0 ? Gamepad.GetAxis( Axes.LeftStickX ) : Gamepad.GetAxis( index, Axes.LeftStickX ); } }
	public float LeftStickY { get { return index < 0 ? Gamepad.GetAxis( Axes.LeftStickY ) : Gamepad.GetAxis( index, Axes.LeftStickY ); } }

	public float RightStickX { get { return index < 0 ? Gamepad.GetAxis( Axes.RightStickX ) : Gamepad.GetAxis( index, Axes.RightStickX ); } }
	public float RightStickY { get { return index < 0 ? Gamepad.GetAxis( Axes.RightStickY ) : Gamepad.GetAxis( index, Axes.RightStickY ); } }

	public float LeftTrigger { get { return index < 0 ? Gamepad.GetAxis( Axes.LeftTrigger ) : Gamepad.GetAxis( index, Axes.LeftTrigger ); } }
	public float RightTrigger { get { return index < 0 ? Gamepad.GetAxis( Axes.RightTrigger ) : Gamepad.GetAxis( index, Axes.RightTrigger ); } }
}

public enum Buttons
{
	FaceN,
	FaceS,
	FaceW,
	FaceE,

	DpadN,
	DpadS,
	DpadW,
	DpadE,

	Start,
	Select,

	LeftShoulder,
	RightShoulder,

	LeftStick,
	RightStick,
}

public enum Axes
{
	LeftStickX,
	LeftStickY,
	RightStickX,
	RightStickY,
	LeftTrigger,
	RightTrigger
}