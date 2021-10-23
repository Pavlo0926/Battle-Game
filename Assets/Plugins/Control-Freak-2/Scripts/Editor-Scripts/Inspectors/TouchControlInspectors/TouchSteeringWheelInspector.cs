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
	
[CustomEditor(typeof(ControlFreak2.TouchSteeringWheel))]
public class TouchSteeringWheelInspector : TouchControlInspectorBase
	{

	public DigitalBindingInspector
		pressBindingInsp,
		turnLeftBindingInsp,
		turnRightBindingInsp;
		
	public AxisBindingInspector
		analogTurnBindingInsp;

	public AnalogConfigInspector
		analogConfigInsp;

	private AxisBindingInspector
		touchPressureBindingInsp;


	// ---------------------
	void OnEnable()
		{
		this.pressBindingInsp = new DigitalBindingInspector(this.target, new GUIContent("Press Binding"));
		this.analogTurnBindingInsp = new AxisBindingInspector(this.target, new GUIContent("Analog Turn Binding"), true, InputRig.InputSource.Analog);
		this.turnLeftBindingInsp = new DigitalBindingInspector(this.target, new GUIContent("Digital Turn Left"));
		this.turnRightBindingInsp = new DigitalBindingInspector(this.target, new GUIContent("Digital Turn Right"));

		this.touchPressureBindingInsp = new AxisBindingInspector(null, new GUIContent("Touch Pressure Binding"), false, 
			InputRig.InputSource.Analog, this.DrawPressureBindingExtraGUI);

		this.analogConfigInsp = new AnalogConfigInspector(this.target, new GUIContent("Analog Config"), false);
			
		base.InitTouchControlInspector();
		}

	// ---------------
	public override void OnInspectorGUI()
		{
		TouchSteeringWheel c = (TouchSteeringWheel)this.target;
			
		GUILayout.Box(GUIContent.none, CFEditorStyles.Inst.headerWheel, GUILayout.ExpandWidth(true));

		this.DrawWarnings(c);			
	
		// Steerign wheel GUI...
			
		TouchSteeringWheel.WheelMode
			wheelMode			= c.wheelMode;

		bool 
			limitTurnSpeed 		= c.limitTurnSpeed;
		float
			minTurnTime			= c.minTurnTime,
			maxReturnTime		= c.maxReturnTime,
			maxTurnAngle		= c.maxTurnAngle,
			turnModeDeadZone	= c.turnModeDeadZone;
		//	digitalThresh		= c.digitalThresh;
		bool
			physicalMode		= c.physicalMode,
			sendInputWhileReturning = c.sendInputWhileReturning;
		float 
			physicalMoveRangeCm	= c.physicalMoveRangeCm;

		this.emulateTouchPressure = c.emulateTouchPressure;


		const float LABEL_WIDTH = 120;

		
		// Steering Wheel specific inspector....

		InspectorUtils.BeginIndentedSection(new GUIContent("Steering Wheel Settings"));
		
			wheelMode = (TouchSteeringWheel.WheelMode)CFGUI.EnumPopup(new GUIContent("Wheel Mode"), wheelMode, LABEL_WIDTH);

			if (c.wheelMode == TouchSteeringWheel.WheelMode.Turn)
				{
				maxTurnAngle = CFGUI.FloatField(new GUIContent("Max. Turn Angle", "Maximal Turn angle in degrees (TURN Wheel Mode)."), 
					maxTurnAngle, 1, 3600, LABEL_WIDTH);

				turnModeDeadZone = CFGUI.Slider(new GUIContent("Dead-zone", "Dead-zone - how far from wheel's center a finger must be to interact with it."), 
					turnModeDeadZone, 0.01f, 0.9f, LABEL_WIDTH);
				}
			else
				{
				physicalMode = EditorGUILayout.ToggleLeft(new GUIContent("Physical Swipe Mode", "Define wheel's range in centimeters."), physicalMode);
				if (physicalMode)
					{
					InspectorUtils.BeginIndentedSection();
						physicalMoveRangeCm = CFGUI.FloatFieldEx(new GUIContent("Range (cm)", "Physical Mode's range in centimeters."),
							physicalMoveRangeCm, 0.1f, 10, 1, false, LABEL_WIDTH);
					InspectorUtils.EndIndentedSection();
					}
				}

			limitTurnSpeed = EditorGUILayout.ToggleLeft(new GUIContent("Limit turn speed", "Limit how fast the wheel can turn and how fast it can return to neutral position."), 
				limitTurnSpeed);

			
			if (limitTurnSpeed)
				{
				InspectorUtils.BeginIndentedSection();
					minTurnTime = CFGUI.FloatFieldEx(new GUIContent("Turn Time", "Turn Time in milliseconds - how quick can this wheel move from neutral position to maximal turn angle.\nSet to zero to remove speed limit."),
						minTurnTime, 0, 5, 1000, true, LABEL_WIDTH);
					maxReturnTime = CFGUI.FloatFieldEx(new GUIContent("Return Time", "Time in milliseconds needed to return from maximal turn angle to neutral position."),
						maxReturnTime, 0, 5, 1000, true, LABEL_WIDTH);
				InspectorUtils.EndIndentedSection();
				}
			
		

			EditorGUILayout.Space();

			this.analogConfigInsp.DrawGUI(c.analogConfig);

		InspectorUtils.EndIndentedSection();



		InspectorUtils.BeginIndentedSection(new GUIContent("Steering Wheel Bindings"));

			sendInputWhileReturning = EditorGUILayout.ToggleLeft(new GUIContent("Send input while returning", "If enabled, wheel's analog state will be sent to the rig while the wheel is returning to the neutral position."),
				sendInputWhileReturning, GUILayout.MinWidth(30)); 

			EditorGUILayout.Space();

			this.pressBindingInsp.Draw(c.pressBinding, c.rig);
			this.touchPressureBindingInsp.Draw(c.touchPressureBinding, c.rig);
			this.analogTurnBindingInsp.Draw(c.analogTurnBinding, c.rig);
			this.turnLeftBindingInsp.Draw(c.turnLeftBinding, c.rig);
			this.turnRightBindingInsp.Draw(c.turnRightBinding, c.rig);
	
		InspectorUtils.EndIndentedSection();


		// Register undo...


		if ((limitTurnSpeed 		!= c.limitTurnSpeed) ||
			(minTurnTime			!= c.minTurnTime) ||
			(maxReturnTime			!= c.maxReturnTime) ||
			(wheelMode				!= c.wheelMode) ||
			(maxTurnAngle			!= c.maxTurnAngle) ||
			(turnModeDeadZone		!= c.turnModeDeadZone) ||
			(sendInputWhileReturning != c.sendInputWhileReturning) ||
			(this.emulateTouchPressure != c.emulateTouchPressure) ||
			(physicalMode			!= c.physicalMode) ||
			(physicalMoveRangeCm	!= c.physicalMoveRangeCm) )
			{
			if ((wheelMode == TouchSteeringWheel.WheelMode.Turn) && (wheelMode != c.wheelMode) && (c.analogConfig.analogDeadZone != 0))
				c.analogConfig.analogDeadZone = 0;


			CFGUI.CreateUndo("CF2 Steering Wheel modification", c);

			c.limitTurnSpeed 		= limitTurnSpeed;
			c.minTurnTime			= minTurnTime;
			c.maxReturnTime			= maxReturnTime;
			c.emulateTouchPressure	= this.emulateTouchPressure;
			c.physicalMode				= physicalMode;
			c.physicalMoveRangeCm	= physicalMoveRangeCm;
			c.wheelMode					= wheelMode;
			c.maxTurnAngle				= maxTurnAngle;
			c.turnModeDeadZone		= turnModeDeadZone;
			c.sendInputWhileReturning = sendInputWhileReturning;

			
			CFGUI.EndUndo(c);
			}

		// Draw Shared Dynamic Control Params...

		this.DrawDynamicTouchControlGUI(c);
		}
			
	
	

	}

		
}
#endif
