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

public class AnalogConfigInspector
	{
	//public Editor mainEditor;
	public Object undoObject;
	public GUIContent titleContent;

	public bool 
		isFoldable,
		isFoldedOut;		
		
	private bool
		drawDigitalSection;


	// -----------------------
	public AnalogConfigInspector(Object undoObject, GUIContent titleContent, bool isFoldable = true)
		{
		//this.mainEditor = mainEditor;
		this.undoObject = undoObject;
		this.titleContent = titleContent;
		this.isFoldable = isFoldable;
		this.drawDigitalSection = true;
		}
			
	// ----------------------
	public void SetDigitalSectionVisibility(bool visible)
		{
		this.drawDigitalSection = visible;
		}

																										



	// -------------------
	public void DrawGUI(AnalogConfig target)
		{
		EditorGUILayout.BeginVertical();

		if (!string.IsNullOrEmpty(this.titleContent.text))
			{
	
//EditorGUILayout.LabelField("Fodable: " + this.isFoldable  + " Folded:" + this.isFoldedOut);

			if (this.isFoldable)
				{
				InspectorUtils.DrawSectionHeader(this.titleContent, ref this.isFoldedOut);
				}
			else
				{
				EditorGUILayout.LabelField(this.titleContent, CFEditorStyles.Inst.boldText, GUILayout.ExpandWidth(true), GUILayout.MinWidth(30));
				this.isFoldedOut = true;
				}	
			}				
		else
			this.isFoldedOut = true;

			
		if (this.isFoldedOut)
			{
			CFGUI.BeginIndentedVertical(CFEditorStyles.Inst.transpSunkenBG);		
	
			JoystickConfig joyConfig = target as JoystickConfig;
			if (joyConfig != null)
				this.DrawJoystickConfigGUI(joyConfig);
	
			this.DrawAnalogConfigGUI(target);

			CFGUI.EndIndentedVertical();
			}

		EditorGUILayout.EndVertical();
		EditorGUILayout.Space();
		}

		
//private float buttonHeight = 40;
	// ---------------------
	protected void DrawJoystickConfigGUI(JoystickConfig target)
		{
		bool 
			blockX			= target.blockX,
			blockY			= target.blockY,
			perAxisDeadzones = target.perAxisDeadzones;

		JoystickConfig.ClampMode
			clampMode		= target.clampMode;
			//circularClamp	= target.circularClamp;
		JoystickConfig.StickMode
			stickMode		= target.stickMode;
		DirectionState.OriginalDirResetMode
			originalDirResetMode	= target.originalDirResetMode;	
		JoystickConfig.DigitalDetectionMode
			digitalDetectionMode		= target.digitalDetectionMode;
	
		float
			angularMagnet		= target.angularMagnet;

	

				
			stickMode = (JoystickConfig.StickMode)CFGUI.EnumPopup(new GUIContent("Mode", "Joystick mode"), stickMode, 120); // GUILayout.MinWidth(30));
			
			clampMode = (JoystickConfig.ClampMode)CFGUI.EnumPopup(new GUIContent("Vector Range", "Vector Range Mode"), clampMode, 120); //GUILayout.MinWidth(30));
			
			digitalDetectionMode = (JoystickConfig.DigitalDetectionMode)CFGUI.EnumPopup(new GUIContent("Digi. Detect. Mode", "Digital Detection Mode"), 
				digitalDetectionMode, 120); 

			angularMagnet = CFGUI.Slider(new GUIContent("Digi. Angular Magnet", "Angular Magnet Strength used when changing digital direction. Higher value will make changing direction harder, which is recommended for Touch Digital Detection Mode."),
				angularMagnet, 0f, 1f, 100);



			if (stickMode == JoystickConfig.StickMode.Analog)
				{
				perAxisDeadzones = EditorGUILayout.ToggleLeft(new GUIContent("Per-Axis Analog Range", "Per-Axis deadzone, endzone and ramp transformations."), 
					perAxisDeadzones, GUILayout.MinWidth(30));
				}
	
				blockX = EditorGUILayout.ToggleLeft(new GUIContent("Block X"), blockX, GUILayout.MinWidth(30));
				blockY = EditorGUILayout.ToggleLeft(new GUIContent("Block Y"), blockY, GUILayout.MinWidth(30));

			originalDirResetMode = (DirectionState.OriginalDirResetMode)CFGUI.EnumPopup(new GUIContent("Dir. Reset Mode", "Original Direction Reset Mode - choose when original stick direction will be reset. This option is used mainly by the Direction Binding..."), 
				originalDirResetMode, 120); // GUILayout.MinWidth(30));
	

	


		if ((blockX				!= target.blockX) ||
			(blockY				!= target.blockY) ||
			(perAxisDeadzones	!= target.perAxisDeadzones) ||
			(clampMode			!= target.clampMode) ||
			(originalDirResetMode!= target.originalDirResetMode) ||
			(digitalDetectionMode!= target.digitalDetectionMode) ||
			(stickMode			!= target.stickMode) ||
			(angularMagnet		!= target.angularMagnet) )
			{
			CFGUI.CreateUndo("Joy Config modification", this.undoObject);
	
  
			target.perAxisDeadzones			= perAxisDeadzones;
			target.blockX						= blockX;
			target.blockY						= blockY;
			//target.circularClamp		= circularClamp;
			target.clampMode					= clampMode;
			target.stickMode					= stickMode;
			target.originalDirResetMode	= originalDirResetMode;
			target.angularMagnet				= angularMagnet;
			target.digitalDetectionMode	= digitalDetectionMode;

			CFGUI.EndUndo(this.undoObject);
			}
		}
	
	// -----------------------
	protected void DrawAnalogConfigGUI(AnalogConfig target)
		{

		float
			analogDeadZone			= target.analogDeadZone,
			analogEndZone			= target.analogEndZone,
			analogRangeStartValue	= target.analogRangeStartValue,
			digitalEnterThresh		= target.digitalEnterThresh,
			digitalLeaveThresh		= target.digitalLeaveThresh;
			//digitalMagnet			= target.digitalMagnetStrength;
		bool
			useRamp			= target.useRamp;
		AnimationCurve
			ramp			= target.ramp;

		
	
		CFGUI.MinMaxSlider(new GUIContent("Analog range", "Analog deadzone and end-zone."), ref analogDeadZone, ref analogEndZone, 0, 1, 100); //GUILayout.MinWidth(30));
		
		useRamp = EditorGUILayout.ToggleLeft(new GUIContent("Use Ramp", "Use custom ramp for this joystick."), useRamp, GUILayout.ExpandWidth(true));

		if (useRamp)
			ramp = EditorGUILayout.CurveField(new GUIContent("Ramp", "Ramp curve."), target.ramp, GUILayout.ExpandWidth(true));
		else
			analogRangeStartValue = CFGUI.Slider(new GUIContent("Start Value", "Analog Range Start Value.\n\n" +
				"If raw analog value is below the Deadzone, returned value equals zero." +
				" Otherwise, returned value will be interpolated between this value (at analog deadzone) and 1.0 (analog endzone).\n\n" +
				"Default value is zero."), 
				analogRangeStartValue, 0, 1, 100);


		if (this.drawDigitalSection)
			{
			CFGUI.MinMaxSlider(new GUIContent("Digital Thresholds", "Max value is the digital \"enter threshold\" and minimal value is the \"Leave Threshold\""),
				ref digitalLeaveThresh, ref digitalEnterThresh, 0.01f, 0.99f, 100);


			}				


		if ((useRamp			!= target.useRamp) ||
			(ramp				!= target.ramp) ||
			//(circularClamp	!= target.circularClamp) ||
			(analogDeadZone		!= target.analogDeadZone) ||
			(analogRangeStartValue	!= target.analogRangeStartValue) ||
			(analogEndZone		!= target.analogEndZone) ||
			(digitalEnterThresh	!= target.digitalEnterThresh) ||
			(digitalLeaveThresh	!= target.digitalLeaveThresh) )
			//((joyConfig != null) && (digitalFreeDirChangeZone != joyConfig.digitalFreeDirChangeZone)))
			//(digitalMagnet	!= target.digitalMagnetStrength) )
			{
			CFGUI.CreateUndo("Analog Config modification", this.undoObject);
	
			target.analogDeadZone		= analogDeadZone;
			target.analogEndZone		= analogEndZone;
			target.analogRangeStartValue= analogRangeStartValue;
			target.digitalLeaveThresh	= digitalLeaveThresh;
			target.digitalEnterThresh	= digitalEnterThresh;
			//target.digitalMagnetStrength= digitalMagnet;
			target.useRamp				= useRamp;
			target.ramp					= ramp;				
				
//			if (joyConfig != null)
//				joyConfig.digitalFreeDirChangeZone = digitalFreeDirChangeZone;

			CFGUI.EndUndo(this.undoObject);
			}
		}
	}

		
}
#endif
