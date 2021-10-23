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

public class TouchGestureThresholdsInspector
	{

	public GUIContent titleContent;
	//public Editor mainEditor;
	public Object undoObject;
	public bool foldedOut;
	public bool foldable;

	// -----------------------
	public TouchGestureThresholdsInspector(Object undoObject, GUIContent titleContent, bool foldable = false)
		{
		this.titleContent = titleContent;
		//this.mainEditor = mainEditor;
		this.foldable = foldable;
		this.undoObject = undoObject;
		}
		


	// ---------------		
	public void DrawGUI(TouchGestureThresholds target)
		{
		if (this.foldable)
			{
			if (!CFGUI.BoldFoldout(this.titleContent, ref this.foldedOut))
				return;


			InspectorUtils.BeginIndentedSection(); //CFGUI.BeginIndentedVertical(CFEditorStyles.Inst.transpSunkenBG);
			}
		else
			{
			InspectorUtils.BeginIndentedSection(this.titleContent);
			}
			

		this.DrawBasicGUI(target);

		MultiTouchGestureThresholds multiTarget = (target as MultiTouchGestureThresholds);
		if (multiTarget != null)
			{
			EditorGUILayout.Space();
			this.DrawMultiTouchGUI(multiTarget);
			}

		//CFGUI.EndIndentedVertical();
		InspectorUtils.EndIndentedSection();
		}
		

		
	const float LABEL_WIDTH = 160;
		



	// ---------------------
	private void DrawBasicGUI(TouchGestureThresholds target)
		{
		//MultiTouchGestureThresholds multiTarget = (target as MultiTouchGestureThresholds);

	
		float 
			tapMoveThreshCm		= target.tapMoveThreshCm,
			tapPosThreshCm		= target.tapPosThreshCm,
			dragThreshCm		= target.dragThreshCm,
			scrollThreshCm		= target.scrollThreshCm,
			scrollMagnetFactor	= target.scrollMagnetFactor,
	
			swipeSegLenCm		= target.swipeSegLenCm,
			swipeJoystickRadCm	= target.swipeJoystickRadCm,
			//dragJoyDeadZone		= target.dragJoyDeadZone,
				
			tapMaxDur			= target.tapMaxDur,
			multiTapMaxTimeGap	= target.multiTapMaxTimeGap,
	
			//longTapMinTime		= target.longTapMinTime,
			longTapMaxDuration	= target.longTapMaxDuration,
	
			longPressMinTime	= target.longPressMinTime;
	



		// GUI...


		tapMoveThreshCm = CFGUI.FloatField(new GUIContent("Static Thresh. (cm)", "Static touch threshold - maximal allowed movement distance for touch to be counted as a static tap."), 
			tapMoveThreshCm, 0.01f, 1.0f, LABEL_WIDTH);

		dragThreshCm = CFGUI.FloatField(new GUIContent("Swipe Thresh (cm)", "Minimal swipe distance in centimeters to activate that gesture. This value should be grater then the Static Threshold!"), 
			dragThreshCm, 0.01f, 5.0f, LABEL_WIDTH);
		
		EditorGUILayout.Space();

		scrollThreshCm = CFGUI.FloatField(new GUIContent("Scroll Step (cm)", "Swipe distance in centimeters for one scroll step"),
			scrollThreshCm, 0.01f, 5.0f, LABEL_WIDTH);

		scrollMagnetFactor = CFGUI.Slider(new GUIContent("Scroll Magnet", "Scroll magnet factor."),
			scrollMagnetFactor, 0, 0.5f, LABEL_WIDTH);


		EditorGUILayout.Space();
		swipeSegLenCm = CFGUI.FloatField(new GUIContent("Swipe Segment Len. (cm)", "Swipe segment length in centimeters used to determinate changing direction of a single swipe."),
			swipeSegLenCm, 0.1f, 10.0f, LABEL_WIDTH);

		swipeJoystickRadCm = CFGUI.FloatField(new GUIContent("Swipe Joy Rad (cm)", "Swipe joystick radius in centimeters."),
			swipeJoystickRadCm, 0.1f, 10.0f, LABEL_WIDTH);


		EditorGUILayout.Space();


		tapPosThreshCm = CFGUI.FloatField(new GUIContent("Max Tap Dist. (cm)", "Maximal allowed distance between consecutive taps."), 
			tapPosThreshCm, 0.1f, 10.0f, LABEL_WIDTH);
			

		tapMaxDur = CFGUI.FloatFieldEx(new GUIContent("Tap Max. Duration", "Maximal allowed tap touch duration in milliseconds."), 
			tapMaxDur, 0.02f, 1.0f, 1000, true, LABEL_WIDTH);

		multiTapMaxTimeGap = CFGUI.FloatFieldEx(new GUIContent("Tap Max. Gap", "Maximal allowed time \'gap\' between consecutive taps."),
			multiTapMaxTimeGap, 0.02f, 2.0f, 1000, true, LABEL_WIDTH);


			
		longPressMinTime = CFGUI.FloatFieldEx(new GUIContent("Long Press Min Time", "Minimal \'hold\' touch duration in milliseconds."),
			longPressMinTime, 0.02f, 1000f, 1000, true, LABEL_WIDTH);

		longTapMaxDuration = CFGUI.FloatFieldEx(new GUIContent("Long Tap Max Duration", "Maximal duration of a long tap in milliseconds."),
			longTapMaxDuration, 0.02f, 1000f, 1000, true, LABEL_WIDTH);


			
		// Register Undo...

		if ((tapMoveThreshCm	!= target.tapMoveThreshCm) ||
			(tapPosThreshCm		!= target.tapPosThreshCm) ||
			(dragThreshCm		!= target.dragThreshCm) ||
			(scrollThreshCm		!= target.scrollThreshCm) ||
			(scrollMagnetFactor	!= target.scrollMagnetFactor) ||
			(swipeSegLenCm		!= target.swipeSegLenCm) ||
			(swipeJoystickRadCm	!= target.swipeJoystickRadCm) ||
			(tapMaxDur			!= target.tapMaxDur) ||
			(multiTapMaxTimeGap	!= target.multiTapMaxTimeGap) ||
			//(longTapMinTime		!= target.longTapMinTime) ||
			(longTapMaxDuration	!= target.longTapMaxDuration) ||
			(longPressMinTime	!= target.longPressMinTime) )
//			(chargeMinTime		!= target.chargeMinTime) ||
//			(repeatDelay		!= target.repeatDelay) ||
//			(repeatInterval		!= target.repeatInterval) ||
//			(velSustainTime		!= target.velSustainTime))
			{
			CFGUI.CreateUndo("Touch Gesture Thresholds modification", this.undoObject);
	
			target.tapMoveThreshCm		= tapMoveThreshCm;
			target.tapPosThreshCm		= tapPosThreshCm;
			target.dragThreshCm			= dragThreshCm;
			target.scrollThreshCm		= scrollThreshCm;
			target.scrollMagnetFactor	= scrollMagnetFactor;
			target.swipeSegLenCm		= swipeSegLenCm;
			target.swipeJoystickRadCm	= swipeJoystickRadCm;
			target.tapMaxDur			= tapMaxDur;
			target.multiTapMaxTimeGap	= multiTapMaxTimeGap;
			target.longTapMaxDuration	= longTapMaxDuration;
			target.longPressMinTime		= longPressMinTime;

			CFGUI.EndUndo(this.undoObject);
			}
		}


	// ---------------------
	private void DrawMultiTouchGUI(MultiTouchGestureThresholds target)
		{
		float 
			pinchDistThreshCm		= target.pinchDistThreshCm,
			pinchAnalogRangeCm		= target.pinchAnalogRangeCm,	
			//pinchAnalogDeadzone		= target.pinchAnalogDeadzone,
			pinchDeltaRangeCm		= target.pinchDeltaRangeCm,
			pinchDigitalThreshCm	= target.pinchDigitalThreshCm,
	
			twistMinDistCm			= target.twistMinDistCm,
			twistAngleThresh		= target.twistAngleThresh,
			twistAnalogRange		= target.twistAnalogRange,
			//twistAnalogDeadzone		= target.twistAnalogDeadzone,
			twistDeltaRange			= target.twistDeltaRange,
			twistDigitalThresh		= target.twistDigitalThresh,

			pinchScrollMagnet		= target.pinchScrollMagnet,
			pinchScrollStepCm		= target.pinchScrollStepCm,
			twistScrollMagnet		= target.twistScrollMagnet,
			twistScrollStep			= target.twistScrollStep,
			
	
			multiFingerTapMaxFingerDistCm	= target.multiFingerTapMaxFingerDistCm;



		// GUI...

		pinchDistThreshCm = CFGUI.FloatField(new GUIContent("Pinch Thresh (cm)", "Pinch distance threshold in centimeters."),
			pinchDistThreshCm, 0.01f, 5.0f, LABEL_WIDTH);

		pinchAnalogRangeCm = CFGUI.FloatField(new GUIContent("Pinch Analog Range (cm)", "Pinch distance analog range in centimeters. This is used when binding pinch state to analog axes."),
			pinchAnalogRangeCm, 0.1f, 10.0f, LABEL_WIDTH);
			

		pinchDeltaRangeCm = CFGUI.FloatField(new GUIContent("Pinch Delta Range (cm)", "Pinch distance delta range in centimeters. This is used when binding pinch state to analog axes."),
			pinchDeltaRangeCm, 0.1f, 10.0f, LABEL_WIDTH);


		pinchDigitalThreshCm = CFGUI.FloatField(new GUIContent("Pinch Digital Thresh (cm)", "Pinch distance threshold to detect digital states of pinch or spread."),
			pinchDigitalThreshCm, 0.1f, 5.0f, LABEL_WIDTH);


		pinchScrollStepCm = CFGUI.FloatField(new GUIContent("Pinch Scroll Step (cm)", "Pinch scroll step in centimeters."),
			pinchScrollStepCm, 0.1f, 5.0f, LABEL_WIDTH);

		pinchScrollMagnet = CFGUI.FloatField(new GUIContent("Pinch Scroll Magnet", "Pinch scroll magnet strength."),
			pinchScrollMagnet, 0, 1, LABEL_WIDTH);


		EditorGUILayout.Space();
			
		twistMinDistCm = CFGUI.FloatField(new GUIContent("Twist Min. Dist (cm)", "Minimal safe finger distance for twist gesture in centimeters."),
			twistMinDistCm, 0.01f, 5.0f, LABEL_WIDTH);

		twistAngleThresh = CFGUI.FloatField(new GUIContent("Twist Thresh (deg)", "Twist threshold in degrees."),
			twistAngleThresh, 0.01f, 180f, LABEL_WIDTH);

		twistAnalogRange = CFGUI.FloatField(new GUIContent("Twist Analog Range (deg)", "Twist analog range in degrees. This is used when binding twist state to analog axis."),
			twistAnalogRange, 1, 360, LABEL_WIDTH);
			

		twistDeltaRange = CFGUI.FloatField(new GUIContent("Twist Delta Range (deg)", "Twist angle range in degrees. This is used when binding twist tate to analog axes."),
			twistDeltaRange, 0.1f, 360.0f, LABEL_WIDTH);


		twistDigitalThresh = CFGUI.FloatField(new GUIContent("Twist Digital Thresh (deg)", "Twist angle threshold to detect digital states of twist right or left."),
			twistDigitalThresh, 1f, 180, LABEL_WIDTH);

		twistScrollStep = CFGUI.FloatField(new GUIContent("Twist Scroll Step (deg)", "Twist scroll step in degrees."),
			twistScrollStep, 1, 180, LABEL_WIDTH);

		twistScrollMagnet = CFGUI.FloatField(new GUIContent("Twist Scroll Magnet", "Twsit scroll magnet strength."),
			twistScrollMagnet, 0, 1, LABEL_WIDTH);
	
		EditorGUILayout.Space();

		multiFingerTapMaxFingerDistCm = CFGUI.FloatField(new GUIContent("Multi-finger max tap dist (cm)", "Maximal allowed distance between two or more fingers forming a multi-finger when detecting taps."),
			multiFingerTapMaxFingerDistCm, 0.01f, 5.0f, LABEL_WIDTH);

	
			
			
			
		// Register Undo...

		if ((pinchDistThreshCm			!= target.pinchDistThreshCm) ||
			(pinchAnalogRangeCm			!= target.pinchAnalogRangeCm) ||
			//(pinchAnalogDeadzone		!= target.pinchAnalogDeadzone) ||
			(pinchDeltaRangeCm			!= target.pinchDeltaRangeCm) ||
			(pinchDigitalThreshCm		!= target.pinchDigitalThreshCm) ||
			(twistMinDistCm				!= target.twistMinDistCm) ||
			(twistAngleThresh			!= target.twistAngleThresh) ||
			(twistAnalogRange			!= target.twistAnalogRange) ||
			//(twistAnalogDeadzone		!= target.twistAnalogDeadzone) ||
			(twistDeltaRange			!= target.twistDeltaRange) ||
			(twistDigitalThresh			!= target.twistDigitalThresh) ||

			(pinchScrollMagnet			!= target.pinchScrollMagnet) ||
			(pinchScrollStepCm			!= target.pinchScrollStepCm) ||
			(twistScrollMagnet			!= target.twistScrollMagnet) ||
			(twistScrollStep			!= target.twistScrollStep) ||

			(multiFingerTapMaxFingerDistCm	!= target.multiFingerTapMaxFingerDistCm) )

			{
			CFGUI.CreateUndo("Touch Gesture Thresholds modification", this.undoObject);
	
			
			target.pinchDistThreshCm				= pinchDistThreshCm;
			target.pinchAnalogRangeCm				= pinchAnalogRangeCm;
			//target.pinchAnalogDeadzone				= pinchAnalogDeadzone;
			target.pinchDeltaRangeCm				= pinchDeltaRangeCm;
			target.pinchDigitalThreshCm				= pinchDigitalThreshCm;
			target.twistMinDistCm					= twistMinDistCm;
			target.twistAngleThresh					= twistAngleThresh;
			target.twistAnalogRange					= twistAnalogRange;
			//target.twistAnalogDeadzone				= twistAnalogDeadzone;
			target.twistDeltaRange					= twistDeltaRange;
			target.twistDigitalThresh				= twistDigitalThresh;
			target.multiFingerTapMaxFingerDistCm	= multiFingerTapMaxFingerDistCm;

			target.pinchScrollMagnet				= pinchScrollMagnet;
			target.pinchScrollStepCm				= pinchScrollStepCm;
			target.twistScrollMagnet				= twistScrollMagnet;
			target.twistScrollStep					= twistScrollStep;
		
			CFGUI.EndUndo(this.undoObject);
			}
		}

	}
		
}
#endif
