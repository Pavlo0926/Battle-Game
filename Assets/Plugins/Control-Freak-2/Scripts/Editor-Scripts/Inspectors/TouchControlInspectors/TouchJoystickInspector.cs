// -------------------------------------------
// Control Freak 2
// Copyright (C) 2013-2020 Dan's Game Tools
// http://DansGameTools.blogspot.com
// -------------------------------------------

#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

using ControlFreak2Editor;
using ControlFreak2;
using ControlFreak2.Internal;
using ControlFreak2Editor.Internal;

namespace ControlFreak2Editor.Inspectors
{
	
[CustomEditor(typeof(ControlFreak2.TouchJoystick))]
public class TouchJoystickInspector : TouchControlInspectorBase
	{

	public DigitalBindingInspector
		pressBindingInsp;
		
	private AxisBindingInspector
		touchPressureBindingInsp;

	public AnalogConfigInspector
		joyConfigInsp;


	public JoystickStateBindingInspector
		joyStateBindingInsp;


	// ---------------------
	void OnEnable()
		{
		this.pressBindingInsp = new DigitalBindingInspector(this.target, new GUIContent("Press Binding"));

		this.touchPressureBindingInsp = new AxisBindingInspector(null, new GUIContent("Touch Pressure Binding"), false, 
			InputRig.InputSource.Analog, this.DrawPressureBindingExtraGUI);
			
		this.joyConfigInsp = new AnalogConfigInspector(this.target, new GUIContent(""), false); //Joystick Configuration"));
			
		this.joyStateBindingInsp = new JoystickStateBindingInspector(this.target, new GUIContent("Joystick State Binding"));

		base.InitTouchControlInspector();
		}

	// ---------------
	public override void OnInspectorGUI()
		{
		TouchJoystick c = (TouchJoystick)this.target;

		this.emulateTouchPressure = c.emulateTouchPressure;

		this.DrawWarnings(c);			
	
		// Joystick GUI...
			
		
		GUILayout.Box(GUIContent.none, CFEditorStyles.Inst.headerJoystick, GUILayout.ExpandWidth(true));

		// Steering Wheel specific inspector....

		InspectorUtils.BeginIndentedSection(new GUIContent("Joystick Settings"));
			
			this.joyConfigInsp.DrawGUI(c.config);

		InspectorUtils.EndIndentedSection();


		InspectorUtils.BeginIndentedSection(new GUIContent("Joystick Bindings"));

	
			this.pressBindingInsp.Draw(c.pressBinding, c.rig);
			this.touchPressureBindingInsp.Draw(c.touchPressureBinding, c.rig);
	
			this.joyStateBindingInsp.Draw(c.joyStateBinding, c.rig);

		InspectorUtils.EndIndentedSection();


		// Register Undo...

		if ((this.emulateTouchPressure != c.emulateTouchPressure) )
			{
			CFGUI.CreateUndo("Dynamic Touch Control modification.", c);

			c.emulateTouchPressure	= this.emulateTouchPressure;

			CFGUI.EndUndo(c);
			}


		// Draw Shared Dynamic Control Params...

		this.DrawDynamicTouchControlGUI(c);



		}
			
	
	

	}

		
}
#endif
