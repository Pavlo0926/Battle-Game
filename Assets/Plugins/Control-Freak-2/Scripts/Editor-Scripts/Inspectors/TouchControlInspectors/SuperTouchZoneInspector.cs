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
	
[CustomEditor(typeof(ControlFreak2.SuperTouchZone))]
public class SuperTouchZoneInspector : TouchControlInspectorBase
	{
		

	public DigitalBindingInspector
		pinchDigitalBindingInsp,
		spreadDigitalBindingInsp,
		//pinchKeyBindingInsp,
			
		//twistKeyBindingInsp,
		twistRightDigitalBindingInsp,
		twistLeftDigitalBindingInsp;
		
	public AxisBindingInspector
		//twistAxisBindingInsp,
		//pinchAxisBindingInsp;
		twistAnalogBindingInsp,
		twistDeltaBindingInsp,
		pinchAnalogBindingInsp,
		pinchDeltaBindingInsp;

	public ScrollDeltaBindingInspector
		pinchScrollDeltaBindingInsp,
		twistScrollDeltaBindingInsp;


	public AnalogConfigInspector
		pinchAnalogConfigInsp,
		twistAnalogConfigInsp;

	public EmuTouchBindingInspector
		separateFingersAsEmuTouchesBindingInsp;


	public MultiFingerTouchConfigInspector[]
		multiFingerConfigInspArray;

	public TouchGestureThresholdsInspector
		touchThreshInsp;

	public bool 
		settingsFoldedOut,
		keyboardFoldedOut,
		multiFingerBindingsFoldedOut,
		twistAndPinchBindingFoldedOut;
		



	// ---------------------
	void OnEnable()
		{
		this.pinchDigitalBindingInsp = new DigitalBindingInspector( this.target, 
			new GUIContent("Digital Pinch Binding", "Digital pinch state is active when user touches with two fingers and pinch them together past specified threshold."));
		this.spreadDigitalBindingInsp = new DigitalBindingInspector(this.target, 
			new GUIContent("Digital Spread Binding", "Digital spread state is active when user touches with two fingers and spread them apart past specified threshold."));

	

		this.twistRightDigitalBindingInsp = new DigitalBindingInspector(this.target, 
			new GUIContent("Twist Right Digital Binding", "Digital twist to the right state is active when user touches with two fingers and twists them clockwise past specified threshold."));
		this.twistLeftDigitalBindingInsp = new DigitalBindingInspector(this.target, 
			new GUIContent("Twist Left Digital Binding", "Digital twist to the left state is active when user touches with two fingers and twists them counter-clockwise past specified threshold."));
			
		this.twistAnalogBindingInsp = new AxisBindingInspector(this.target, 
			new GUIContent("Twist Analog Binding", "Bind twist analog state to an axis"), true, InputRig.InputSource.Analog, this.DrawTwistAnalogBindingExtraGUI);
		this.twistDeltaBindingInsp = new AxisBindingInspector(this.target, 
			new GUIContent("Twist Delta Binding", "Bind twist delta to an axis"), true, InputRig.InputSource.NormalizedDelta, this.DrawTwistDeltaBindingExtraGUI);

		this.pinchAnalogBindingInsp = new AxisBindingInspector(this.target, 
			new GUIContent("Pinch Analog Binding", "Bind pinch analog state to an axis"), true, InputRig.InputSource.Analog, this.DrawPinchAnalogBindingExtraGUI);
		this.pinchDeltaBindingInsp = new AxisBindingInspector(this.target, 
			new GUIContent("Pinch Delta Binding", "Bind pinch delta to an axis"), true, InputRig.InputSource.NormalizedDelta, this.DrawPinchDeltaBindingExtraGUI);
		

		this.pinchScrollDeltaBindingInsp = new ScrollDeltaBindingInspector(this.target, new GUIContent("Pinch Scroll Delta Binding"));
		this.twistScrollDeltaBindingInsp = new ScrollDeltaBindingInspector(this.target, new GUIContent("Twist Scroll Delta Binding"));
		


		this.pinchAnalogConfigInsp = new AnalogConfigInspector(this.target, new GUIContent("Analog Pinch Config"), false);
		this.twistAnalogConfigInsp = new AnalogConfigInspector(this.target, new GUIContent("Analog Twist Config"), false);
			
		this.pinchAnalogConfigInsp.SetDigitalSectionVisibility(false);
		this.twistAnalogConfigInsp.SetDigitalSectionVisibility(false);
		



		this.touchThreshInsp = new TouchGestureThresholdsInspector(this.target, new GUIContent("Gesture Thresholds"));
		
		this.separateFingersAsEmuTouchesBindingInsp = new EmuTouchBindingInspector(this.target, new GUIContent("Bind Separate Fingers as Emu. Touches", "Bind separate fingers touching this touch zone to rig's emulated touches (Input.touches[])."));
			

		this.multiFingerConfigInspArray = new MultiFingerTouchConfigInspector[ControlFreak2.SuperTouchZone.MAX_FINGERS];
		for (int i = 0; i < this.multiFingerConfigInspArray.Length; ++i)
			{
			string labelPrefix = (
				(i == 0) ? "Single finger" :
				(i == 1) ? "Double finger" :
				(i == 2) ? "Triple finger" :
				((i + 1).ToString() + "-finger"));
			
			this.multiFingerConfigInspArray[i] = new MultiFingerTouchConfigInspector((ControlFreak2.SuperTouchZone)this.target, 
				new GUIContent(labelPrefix + " Touch Configuration", "Configure multi-finger touch"),
				new GUIContent(labelPrefix + " Touch Binding"));
			}



		base.InitTouchControlInspector();
		}



	// ------------------
	private void DrawPinchDeltaBindingExtraGUI()
		{
		}

	// -----------------
	private void DrawPinchAnalogBindingExtraGUI()
		{
		ControlFreak2.SuperTouchZone c = (ControlFreak2.SuperTouchZone)this.target;

			{
			this.pinchAnalogConfigInsp.DrawGUI(c.pinchAnalogConfig);
			}



		}


	// ------------------
	private void DrawTwistDeltaBindingExtraGUI()
		{
		}

	// ------------------
	private void DrawTwistAnalogBindingExtraGUI()
		{
		ControlFreak2.SuperTouchZone c = (ControlFreak2.SuperTouchZone)this.target;

			{
			this.twistAnalogConfigInsp.DrawGUI(c.twistAnalogConfig);
			}


		}


	// ---------------
	public override void OnInspectorGUI()
		{
		ControlFreak2.SuperTouchZone c = (ControlFreak2.SuperTouchZone)this.target;
			
		GUILayout.Box(GUIContent.none, CFEditorStyles.Inst.headerTouchZone, GUILayout.ExpandWidth(true));


		this.DrawWarnings(c);			


			
		int
			maxFingers				= c.maxFingers;
		float
			strictMultiTouchTime	= c.strictMultiTouchTime;
		float
			touchSmoothing			= c.touchSmoothing;
		bool
			strictMultiTouch		= c.strictMultiTouch,
			freezeTwistWhenTooClose	= c.freezeTwistWhenTooClose,
			noPinchAfterDrag		= c.noPinchAfterDrag,
			noPinchAfterTwist		= c.noPinchAfterTwist,
			noTwistAfterDrag		= c.noTwistAfterDrag,
			noTwistAfterPinch		= c.noTwistAfterPinch,
			noDragAfterPinch		= c.noDragAfterPinch,
			noDragAfterTwist		= c.noDragAfterTwist,
			startPinchWhenTwisting	= c.startPinchWhenTwisting,
			startPinchWhenDragging	= c.startPinchWhenDragging,
			startDragWhenPinching	= c.startDragWhenPinching,
			startDragWhenTwisting	= c.startDragWhenTwisting,
			startTwistWhenDragging	= c.startTwistWhenDragging,
			startTwistWhenPinching	= c.startTwistWhenPinching;
			
		ControlFreak2.SuperTouchZone.GestureDetectionOrder
			gestureDetectionOrder	= c.gestureDetectionOrder;
			

		bool disableTouchMarkers	= c.disableTouchMarkers;

		bool
			emuWithKeys				= c.emuWithKeys;
		KeyCode
			emuKeyOneFinger			= c.emuKeyOneFinger,
			emuKeyTwoFingers		= c.emuKeyTwoFingers,
			emuKeyThreeFingers		= c.emuKeyThreeFingers,
			emuKeySwipeU			= c.emuKeySwipeU,
			emuKeySwipeR			= c.emuKeySwipeR,
			emuKeySwipeD			= c.emuKeySwipeD,
			emuKeySwipeL			= c.emuKeySwipeL,
			emuKeyTwistR			= c.emuKeyTwistR,
			emuKeyTwistL			= c.emuKeyTwistL,
			emuKeyPinch				= c.emuKeyPinch,
			emuKeySpread			= c.emuKeySpread,
			emuMouseTwoFingersKey	= c.emuMouseTwoFingersKey,
			emuMouseTwistKey		= c.emuMouseTwistKey,
			emuMousePinchKey		= c.emuMousePinchKey,
			emuMouseThreeFingersKey	= c.emuMouseThreeFingersKey;
			
		ControlFreak2.SuperTouchZone.EmuMouseAxis
			emuMousePinchAxis 		= c.emuMousePinchAxis,
			emuMouseTwistAxis 		= c.emuMouseTwistAxis;

		float 
			emuKeySwipeSpeed 		= c.emuKeySwipeSpeed,
			emuKeyPinchSpeed 		= c.emuKeyPinchSpeed,
			emuKeyTwistSpeed 		= c.emuKeyTwistSpeed,
			mouseEmuTwistScreenFactor	= c.mouseEmuTwistScreenFactor;
			


 

		if (InspectorUtils.BeginIndentedSection(new GUIContent("Touch Zone Settings"), ref this.settingsFoldedOut))
			{
			disableTouchMarkers = EditorGUILayout.ToggleLeft(new GUIContent("Disable touch markers", "Disable emulated touch markers in the Editor Play Mode."),
				disableTouchMarkers);

			maxFingers = CFGUI.IntSlider(new GUIContent("Max finger number", "Maximal number of fingers allowed at the same time."), 
				maxFingers, 1, ControlFreak2.SuperTouchZone.MAX_FINGERS, 120);

			touchSmoothing = CFGUI.Slider(new GUIContent("Touch. smoothing", "Amount of smoothing applied to controlling touch position. "),
				touchSmoothing, 0, 1,  120);


			


			if (maxFingers >= 2)
				{
				EditorGUILayout.Space();

				InspectorUtils.BeginIndentedSection(new GUIContent("Multi-finger Gesture Settings"));

				strictMultiTouch = EditorGUILayout.ToggleLeft(new GUIContent("Strict multi-touch", "Use strict rules when detecting multi-finger touch gestures."), 
					strictMultiTouch);
				
				if (strictMultiTouch)
					{
					strictMultiTouchTime = CFGUI.FloatFieldEx(new GUIContent("Strict multi-touch time", "Time in milliseconds to wait for more fingers when starting a multi-finger touch. Touh zone will wait (and not report the fingers already pressed) until time runs out or any of the fingers already pressed moves."),
						strictMultiTouchTime, 0, 1.0f, 1000, true, 150);
					}

				freezeTwistWhenTooClose = EditorGUILayout.ToggleLeft(new GUIContent("Freeze twist when too close.", "Freeze Twist angle when two fingers get too close."), 
					freezeTwistWhenTooClose);
				
				EditorGUILayout.Space();

				noPinchAfterDrag = EditorGUILayout.ToggleLeft(new GUIContent("No Pinch after Swipe.", "Don't allow Pinch gesture to activate after swipe has been detected."), 
					noPinchAfterDrag);
				noPinchAfterTwist = EditorGUILayout.ToggleLeft(new GUIContent("No Pinch after Twist.", "Don't allow Pinch gesture to activate after twist has been detected."), 
					noPinchAfterTwist);

				EditorGUILayout.Space();

				noTwistAfterDrag = EditorGUILayout.ToggleLeft(new GUIContent("No Twist after Swipe.", "Don't allow Twist gesture to activate after swipe has been detected."), 
					noTwistAfterDrag);
				noTwistAfterPinch = EditorGUILayout.ToggleLeft(new GUIContent("No Twist after Pinch.", "Don't allow Twist gesture to activate after pinch has been detected."), 
					noTwistAfterPinch);

				EditorGUILayout.Space();

				noDragAfterPinch = EditorGUILayout.ToggleLeft(new GUIContent("No Swipe after Pinch.", "Don't allow Swipe gesture to activate after pinch has been detected."), 
					noDragAfterPinch);
				noDragAfterTwist = EditorGUILayout.ToggleLeft(new GUIContent("No Swipe after Twist.", "Don't allow Swipe gesture to activate after twist has been detected."), 
					noDragAfterTwist);

				EditorGUILayout.Space();
		

				startPinchWhenTwisting = EditorGUILayout.ToggleLeft(new GUIContent("Start Pinch when Twisting", "Auto-activate Pinch gesture when Twist gesture has been detected."),
					startPinchWhenTwisting);
				startPinchWhenDragging = EditorGUILayout.ToggleLeft(new GUIContent("Start Pinch when Swiping", "Auto-activate Pinch gesture when Swipe gesture has been detected."),
					startPinchWhenDragging);
				EditorGUILayout.Space();

				startDragWhenPinching = EditorGUILayout.ToggleLeft(new GUIContent("Start Swipe when Pinching", "Auto-activate Swipe gesture when Pinch gesture has been detected."),
					startDragWhenPinching);
				startDragWhenTwisting = EditorGUILayout.ToggleLeft(new GUIContent("Start Swipe when Twisting", "Auto-activate Swipe gesture when Twist gesture has been detected."),
					startDragWhenTwisting);
				EditorGUILayout.Space();

				startTwistWhenDragging = EditorGUILayout.ToggleLeft(new GUIContent("Start Twist when Swiping", "Auto-activate Twist esture when Swipe gesture has been detected."),
					startTwistWhenDragging);
				startTwistWhenPinching = EditorGUILayout.ToggleLeft(new GUIContent("Start Twist when Pinching", "Auto-activate Twist gesture when Pinching gesture has been detected."),
					startTwistWhenPinching);
				EditorGUILayout.Space();

	

				gestureDetectionOrder = (SuperTouchZone.GestureDetectionOrder)CFGUI.EnumPopup(new GUIContent("Gesture detect. order", 
					"Gesture Detection Order. Useful is you don't allow some gesture combinations at the same time."), gestureDetectionOrder, 130);
								
				InspectorUtils.EndIndentedSection();

				}
	
	
			// Thresholds...
	
			EditorGUILayout.Space();
	
			this.touchThreshInsp.DrawGUI(c.customThresh);
	
			// Finger configs...
	
			for (int i = 0; i < Mathf.Min(c.maxFingers, this.multiFingerConfigInspArray.Length); ++i)
				{
				EditorGUILayout.Space();
	
				this.multiFingerConfigInspArray[i].DrawConfigGUI(c.multiFingerConfigs[i]);
				}




			InspectorUtils.EndIndentedSection();
			}
			


		EditorGUILayout.Space();
	
		if (InspectorUtils.BeginIndentedSection(new GUIContent("Touch Zone Bindings"), ref this.multiFingerBindingsFoldedOut))
			{
	

		//InspectorUtils.BeginIndentedSection(new GUIContent("Touch Zone Bindings"));
		//	if (InspectorUtils.BeginIndentedSection(new GUIContent("Multi-Finger Touch Bindings"), ref this.multiFingerBindingsFoldedOut))
		//		{
	
	
				for (int i = 0; i < Mathf.Min(c.maxFingers, this.multiFingerConfigInspArray.Length); ++i)	
					{
					this.multiFingerConfigInspArray[i].DrawBindingGUI(c.multiFingerConfigs[i]);
					EditorGUILayout.Space();
					}

				//InspectorUtils.EndIndentedSection();
				//}
	
				if (maxFingers >= 2)
					{
	
					if (InspectorUtils.BeginIndentedSection(new GUIContent("Twist and Pinch Bindings"), ref this.twistAndPinchBindingFoldedOut))
						{
						this.twistAnalogBindingInsp.Draw(c.twistAnalogBinding, c.rig);
						this.twistDeltaBindingInsp.Draw(c.twistDeltaBinding, c.rig);
						//this.twistKeyBindingInsp.Draw(c.twistKeyBinding, c.rig);
			
						this.twistRightDigitalBindingInsp.Draw(c.twistRightDigitalBinding, c.rig);
						this.twistLeftDigitalBindingInsp.Draw(c.twistLeftDigitalBinding, c.rig);
				
						this.twistScrollDeltaBindingInsp.Draw(c.twistScrollDeltaBinding, c.rig);
	
		
						EditorGUILayout.Space();
		
						this.pinchAnalogBindingInsp.Draw(c.pinchAnalogBinding, c.rig);
						this.pinchDeltaBindingInsp.Draw(c.pinchDeltaBinding, c.rig);
						//this.pinchKeyBindingInsp.Draw(c.pinchKeyBinding, c.rig);
			
						this.pinchDigitalBindingInsp.Draw(c.pinchDigitalBinding, c.rig);
						this.spreadDigitalBindingInsp.Draw(c.spreadDigitalBinding, c.rig);
	
						this.pinchScrollDeltaBindingInsp.Draw(c.pinchScrollDeltaBinding, c.rig);
		
						InspectorUtils.EndIndentedSection();
						}
					}

				
				EditorGUILayout.Space();
	
				InspectorUtils.BeginIndentedSection(new GUIContent("Misc. Touch Zone Bindings"));
	
			
					this.separateFingersAsEmuTouchesBindingInsp.Draw(c.separateFingersAsEmuTouchesBinding, c.rig);
	
				InspectorUtils.EndIndentedSection();

			InspectorUtils.EndIndentedSection();
			}	


		EditorGUILayout.Space();


		if (InspectorUtils.BeginIndentedSection(new GUIContent("Keyboard Settings"), ref this.keyboardFoldedOut))
			{
			
			emuWithKeys = EditorGUILayout.ToggleLeft(new GUIContent("Keyboard control", "Use keyboard to control this touch control."), 
				emuWithKeys);
			
			const float KEY_LABEL_WIDTH = 120;

			if (emuWithKeys)
				{
				emuKeyOneFinger		= (KeyCode)CFGUI.EnumPopup(new GUIContent("One Finger Press", "Static press with one finger."),
		 			emuKeyOneFinger, KEY_LABEL_WIDTH);
				
				if (maxFingers >= 2)
					{
					emuKeyTwoFingers		= (KeyCode)CFGUI.EnumPopup(new GUIContent("Two Finger Press", "Static press with two fingers."),
			 			emuKeyTwoFingers, KEY_LABEL_WIDTH);
					}

				if (maxFingers >= 3)
					{
					emuKeyThreeFingers		= (KeyCode)CFGUI.EnumPopup(new GUIContent("Three Finger Press", "Static press with three fingers."),
			 			emuKeyThreeFingers, KEY_LABEL_WIDTH);
					}
				
				EditorGUILayout.Space();
	
				emuKeySwipeU		= (KeyCode)CFGUI.EnumPopup(new GUIContent("Swipe Up", "Swipe Up.\nNote: you can start a multi-finger press with one of keys defined above and then simulate swipe with one of these keys."),
		 			emuKeySwipeU, KEY_LABEL_WIDTH);
				emuKeySwipeR		= (KeyCode)CFGUI.EnumPopup(new GUIContent("Swipe Right", "Swipe Right.\nNote: you can start a multi-finger press with one of keys defined above and then simulate swipe with one of these keys."),
		 			emuKeySwipeR, KEY_LABEL_WIDTH);
				emuKeySwipeD		= (KeyCode)CFGUI.EnumPopup(new GUIContent("Swipe Down", "Swipe Down.\nNote: you can start a multi-finger press with one of keys defined above and then simulate swipe with one of these keys."),
		 			emuKeySwipeD, KEY_LABEL_WIDTH);
				emuKeySwipeL		= (KeyCode)CFGUI.EnumPopup(new GUIContent("Swipe Left", "Swipe Left.\nNote: you can start a multi-finger press with one of keys defined above and then simulate swipe with one of these keys."),
		 			emuKeySwipeL, KEY_LABEL_WIDTH);


				if (maxFingers >= 2)
					{
					EditorGUILayout.Space();
		
					emuKeyTwistR		= (KeyCode)CFGUI.EnumPopup(new GUIContent("Twist Right", "Start a two-finger press and twist clockwise.\nNote: This can be combined with pinching and swiping!"),
			 			emuKeyTwistR, KEY_LABEL_WIDTH);
					emuKeyTwistL		= (KeyCode)CFGUI.EnumPopup(new GUIContent("Twist Left", "Start a two-finger press and twist counter-colckwise.\nNote: This can be combined with pinching and swiping!"),
			 			emuKeyTwistL, KEY_LABEL_WIDTH);
	
					emuKeyPinch		= (KeyCode)CFGUI.EnumPopup(new GUIContent("Pinch", "Start a two-finger press and pinch them together.\nNote: This can be combined with twisting and swiping!"),
			 			emuKeyPinch, KEY_LABEL_WIDTH);
					emuKeySpread		= (KeyCode)CFGUI.EnumPopup(new GUIContent("Spread", "Start a two-finger press and spread them apart.\nNote: This can be combined with twisting and swiping!"),
			 			emuKeySpread, KEY_LABEL_WIDTH);
					
					emuKeySwipeSpeed = CFGUI.Slider(new GUIContent("Key Swipe Speed", "Keyboard swipe speed in screen widths per second."), 
						emuKeySwipeSpeed, 0.1f, 1.0f, KEY_LABEL_WIDTH);
					emuKeyPinchSpeed = CFGUI.Slider(new GUIContent("Key Pinch Speed", "Keyboard pinch speed in screen widths per second."), 
						emuKeyPinchSpeed, 0.1f, 1.0f, KEY_LABEL_WIDTH);
					emuKeyTwistSpeed = CFGUI.Slider(new GUIContent("Key Twist Speed", "Keyboard twist speed in degrees per second."),
						emuKeyTwistSpeed, 25, 360, KEY_LABEL_WIDTH);
	
	
					EditorGUILayout.Space();
	
					emuMouseTwoFingersKey		= (KeyCode)CFGUI.EnumPopup(new GUIContent("2-finger mouse mod.", "Holding this key and clicking with a mouse will start two virtual touches."),
			 			emuMouseTwoFingersKey, KEY_LABEL_WIDTH);

					if (maxFingers >= 3)
						{
						emuMouseThreeFingersKey		= (KeyCode)CFGUI.EnumPopup(new GUIContent("3-finger mouse mod.", "Holding this key and clicking with a mouse will start three virtual touches."),
			 				emuMouseThreeFingersKey, KEY_LABEL_WIDTH);
						}
	
					EditorGUILayout.Space();
	
					emuMouseTwistKey		= (KeyCode)CFGUI.EnumPopup(new GUIContent("Twist mouse mod.", "Twist mouse modifier. Holding this key and clicking with a mouse will start two virtual touches. Dragging the mouse along axis selected below will simulate twist."),
			 			emuMouseTwistKey, KEY_LABEL_WIDTH);
	
					emuMousePinchKey		= (KeyCode)CFGUI.EnumPopup(new GUIContent("Pinch mouse mod.", "Pinch mouse modifier. Holding this key and clicking with a mouse will start two virtual touches. Dragging the mouse along axis selected below will simulate pinch."),
			 			emuMousePinchKey, KEY_LABEL_WIDTH);
	
					emuMouseTwistAxis = (ControlFreak2.SuperTouchZone.EmuMouseAxis)CFGUI.EnumPopup(new GUIContent("Mouse twist axis", "Select mouse drag axis to simulate twist.\nNote: If twist and pinch axes are not the same, twist and pinch mouse modifers can be combined to twist and pinch with a mouse at the same time."),
						emuMouseTwistAxis, KEY_LABEL_WIDTH);
					emuMousePinchAxis = (ControlFreak2.SuperTouchZone.EmuMouseAxis)CFGUI.EnumPopup(new GUIContent("Mouse pinch axis", "Select mouse drag axis to simulate pinch.\nNote: If twist and pinch axes are not the same, twist and pinch mouse modifers can be combined to twist and pinch with a mouse at the same time."),
						emuMousePinchAxis, KEY_LABEL_WIDTH);
						
	
					mouseEmuTwistScreenFactor = CFGUI.Slider(new GUIContent("Mouse Twist Factor", "Fraction of screen width to drag to twist virtual fingers by 360 degrees."),
						mouseEmuTwistScreenFactor, 0.2f, 1.0f, KEY_LABEL_WIDTH);
					}
				}
			
			InspectorUtils.EndIndentedSection();
			}


		// Register undo...



		if ((maxFingers				!= c.maxFingers) ||
			(disableTouchMarkers		!= c.disableTouchMarkers) ||
			(touchSmoothing			!= c.touchSmoothing) ||
			(strictMultiTouchTime	!= c.strictMultiTouchTime) ||
			(strictMultiTouch		!= c.strictMultiTouch) ||
			(freezeTwistWhenTooClose!= c.freezeTwistWhenTooClose) ||
			(noPinchAfterDrag		!= c.noPinchAfterDrag) ||
			(noPinchAfterTwist		!= c.noPinchAfterTwist) ||
			(noTwistAfterDrag		!= c.noTwistAfterDrag) ||
			(noTwistAfterPinch		!= c.noTwistAfterPinch) ||
			(noDragAfterPinch		!= c.noDragAfterPinch) ||
			(noDragAfterTwist		!= c.noDragAfterTwist) ||
			(startPinchWhenTwisting	!= c.startPinchWhenTwisting) ||
			(startPinchWhenDragging	!= c.startPinchWhenDragging) ||
			(startDragWhenPinching	!= c.startDragWhenPinching) ||
			(startDragWhenTwisting	!= c.startDragWhenTwisting) ||
			(startTwistWhenDragging	!= c.startTwistWhenDragging) ||
			(startTwistWhenPinching	!= c.startTwistWhenPinching) ||
			(gestureDetectionOrder	!= c.gestureDetectionOrder) ||
			//(bindSeparateFingersToEmuTouches	!= c.bindSeparateFingersToEmuTouches) ||
//			(twistBindMode			!= c.twistBindMode) ||
//			(pinchBindMode			!= c.pinchBindMode) ||
			(emuWithKeys			!= c.emuWithKeys) ||
			(emuKeyOneFinger		!= c.emuKeyOneFinger) ||
			(emuKeyTwoFingers		!= c.emuKeyTwoFingers) ||
			(emuKeyThreeFingers		!= c.emuKeyThreeFingers) ||
			(emuKeySwipeU			!= c.emuKeySwipeU) ||
			(emuKeySwipeR			!= c.emuKeySwipeR) ||
			(emuKeySwipeD			!= c.emuKeySwipeD) ||
			(emuKeySwipeL			!= c.emuKeySwipeL) ||
			(emuKeyTwistR			!= c.emuKeyTwistR) ||
			(emuKeyTwistL			!= c.emuKeyTwistL) ||
			(emuKeyPinch			!= c.emuKeyPinch) ||
			(emuKeySpread			!= c.emuKeySpread) ||
			(emuMouseTwoFingersKey	!= c.emuMouseTwoFingersKey) ||
			(emuMouseTwistKey		!= c.emuMouseTwistKey) ||
			(emuMousePinchKey		!= c.emuMousePinchKey) ||
			(emuMouseThreeFingersKey!= c.emuMouseThreeFingersKey) ||
			(emuMousePinchAxis 		!= c.emuMousePinchAxis) ||
			(emuMouseTwistAxis 		!= c.emuMouseTwistAxis) ||
			(emuKeySwipeSpeed 		!= c.emuKeySwipeSpeed) ||
			(emuKeyPinchSpeed 		!= c.emuKeyPinchSpeed) ||
			(emuKeyTwistSpeed 		!= c.emuKeyTwistSpeed) ||
			(mouseEmuTwistScreenFactor!= c.mouseEmuTwistScreenFactor) )
			{
			CFGUI.CreateUndo("CF2 Touch Zone modification", c);

			c.disableTouchMarkers				= disableTouchMarkers;
			c.maxFingers							= maxFingers;
			c.strictMultiTouchTime				= strictMultiTouchTime;
			c.strictMultiTouch					= strictMultiTouch;
			c.freezeTwistWhenTooClose			= freezeTwistWhenTooClose;
			c.noPinchAfterDrag					= noPinchAfterDrag;
			c.noPinchAfterTwist					= noPinchAfterTwist;
			c.noTwistAfterDrag					= noTwistAfterDrag;
			c.noTwistAfterPinch					= noTwistAfterPinch;
			c.noDragAfterPinch					= noDragAfterPinch;
			c.noDragAfterTwist					= noDragAfterTwist;
			c.startPinchWhenTwisting			= startPinchWhenTwisting;
			c.startPinchWhenDragging			= startPinchWhenDragging;
			c.startDragWhenPinching				= startDragWhenPinching;
			c.startDragWhenTwisting				= startDragWhenTwisting;
			c.startTwistWhenDragging			= startTwistWhenDragging;
			c.startTwistWhenPinching			= startTwistWhenPinching;
			c.gestureDetectionOrder				= gestureDetectionOrder;
			
			if (c.touchSmoothing != touchSmoothing)
				c.SetTouchSmoothing(touchSmoothing);

			
			c.emuWithKeys				= emuWithKeys;
			c.emuKeyOneFinger			= emuKeyOneFinger;
			c.emuKeyTwoFingers			= emuKeyTwoFingers;
			c.emuKeyThreeFingers		= emuKeyThreeFingers;
			c.emuKeySwipeU				= emuKeySwipeU;
			c.emuKeySwipeR				= emuKeySwipeR;
			c.emuKeySwipeD				= emuKeySwipeD;
			c.emuKeySwipeL				= emuKeySwipeL;
			c.emuKeyTwistR				= emuKeyTwistR;
			c.emuKeyTwistL				= emuKeyTwistL;
			c.emuKeyPinch				= emuKeyPinch;
			c.emuKeySpread				= emuKeySpread;
			c.emuMouseTwoFingersKey		= emuMouseTwoFingersKey;
			c.emuMouseTwistKey			= emuMouseTwistKey;
			c.emuMousePinchKey			= emuMousePinchKey;
			c.emuMouseThreeFingersKey	= emuMouseThreeFingersKey;
			c.emuMousePinchAxis 		= emuMousePinchAxis;
			c.emuMouseTwistAxis 		= emuMouseTwistAxis;
			c.emuKeySwipeSpeed 			= emuKeySwipeSpeed;
			c.emuKeyPinchSpeed 			= emuKeyPinchSpeed;
			c.emuKeyTwistSpeed 			= emuKeyTwistSpeed;
			c.mouseEmuTwistScreenFactor	= mouseEmuTwistScreenFactor;
		
			

			CFGUI.EndUndo(c);
			}


		// Draw Shared Control Params...

		this.DrawTouchContolGUI(c);
		
		}
			
	



	// --------------------------
	public class MultiFingerTouchConfigInspector
		{
		private GUIContent configTitleContent;
		private ControlFreak2.SuperTouchZone touchZone;
		
		public bool 
			configFoldedOut,
			bindingFoldedOut;
		
		public TouchGestureStateBindingInspector
			touchGestureBindingInsp;
	
		public TouchGestureConfigInspector
			touchGestureConfigInsp;

		public AnalogConfigInspector	
			swipeJoyConfigInsp;



		// -----------------------	
		public MultiFingerTouchConfigInspector(ControlFreak2.SuperTouchZone touchZone, GUIContent configTitleContent, GUIContent bindingTitleContent)
			{
			this.configTitleContent = configTitleContent;
			this.touchZone	= touchZone;
				

			this.touchGestureBindingInsp = new TouchGestureStateBindingInspector(touchZone, bindingTitleContent);
			this.touchGestureConfigInsp = new TouchGestureConfigInspector(touchZone, new GUIContent("Gesture Configuration"));

			this.swipeJoyConfigInsp	= new AnalogConfigInspector(touchZone, new GUIContent("Swipe Joystick Configuration"), false);
				
			}


		// -------------------
		public void DrawConfigGUI(ControlFreak2.SuperTouchZone.MultiFingerTouchConfig config)
			{
			//if (!CFGUI.BoldFoldout(this.configTitleContent, ref this.configFoldedOut))
			//	return;

			InspectorUtils.BeginIndentedSection(this.configTitleContent);

			//CFGUI.BeginIndentedVerticalCFEditorStyles.Inst.boldText);

			bool 
				driveOtherControl	= config.driveOtherControl;
				//swipeToDrive		= config.swipeToDrive;
				//useSwipeJoystick	= config.useSwipeJoystick;
			TouchControl
				//controlToDrive				= config.controlToDrive;
				controlToDriveOnRawPress	= config.controlToDriveOnRawPress,
				controlToDriveOnNormalPress	= config.controlToDriveOnNormalPress,
				controlToDriveOnLongPress	= config.controlToDriveOnLongPress,

				controlToDriveOnNormalPressSwipe	= config.controlToDriveOnNormalPressSwipe,
				controlToDriveOnNormalPressSwipeU	= config.controlToDriveOnNormalPressSwipeU,
				controlToDriveOnNormalPressSwipeR	= config.controlToDriveOnNormalPressSwipeR,
				controlToDriveOnNormalPressSwipeD	= config.controlToDriveOnNormalPressSwipeD,
				controlToDriveOnNormalPressSwipeL	= config.controlToDriveOnNormalPressSwipeL,

				controlToDriveOnLongPressSwipe		= config.controlToDriveOnLongPressSwipe,
				controlToDriveOnLongPressSwipeU		= config.controlToDriveOnLongPressSwipeU,
				controlToDriveOnLongPressSwipeR		= config.controlToDriveOnLongPressSwipeR,
				controlToDriveOnLongPressSwipeD		= config.controlToDriveOnLongPressSwipeD,
				controlToDriveOnLongPressSwipeL		= config.controlToDriveOnLongPressSwipeL;


			//InspectorUtils.BeginIndentedSection(GUIContent.none); //this.configTitleContent);
	
			//CFGUI.BeginIndentedVertical(CFEditorStyles.Inst.transpSunkenBG);



				this.touchGestureConfigInsp.DrawGUI(config.touchConfig);

				EditorGUILayout.Space();
	
				//useSwipeJoystick = EditorGUILayout.ToggleLeft(new GUIContent("Use Swipe Joystick", "When enabled, this option gives access to virtual joystick controlled by swiping."),
				//	useSwipeJoystick);

				//if (useSwipeJoystick)
					this.swipeJoyConfigInsp.DrawGUI(config.swipeJoyConfig);				

				EditorGUILayout.Space();


				driveOtherControl = EditorGUILayout.ToggleLeft(new GUIContent(" Drive other control", "Use this multi-finger touch to drive other touch control, like joystick or even other touch zone!"),
					driveOtherControl, CFEditorStyles.Inst.boldText);

				if (driveOtherControl)
					{
					CFGUI.BeginIndentedVertical(CFEditorStyles.Inst.transpSunkenBG);

					//swipeToDrive = EditorGUILayout.ToggleLeft(new GUIContent("Swipe to drive", "Swipe first to drive other control. This option is useful for joysticks, as it allows to detect many static gestures such as tap without activating the joystick."),
					//	swipeToDrive);


					controlToDriveOnRawPress = (TouchControl)CFGUI.ObjectField(new GUIContent("On Raw Press", "Touch Control to drive with this multi-finger touch.\n\nTIP: It's a good idea to enable \'Can\'t be touched directly\' option on driven control."),
						controlToDriveOnRawPress, typeof(TouchControl), 150); 

					if (controlToDriveOnRawPress == null)
						{
						controlToDriveOnNormalPress = (TouchControl)CFGUI.ObjectField(new GUIContent("On Normal Press", "Touch Control to drive with this multi-finger touch.\n\nTIP: It's a good idea to enable \'Can\'t be touched directly\' option on driven control."),
							controlToDriveOnNormalPress, typeof(TouchControl), 150); 

						if (config.touchConfig.detectLongPress)
							{
							controlToDriveOnLongPress = (TouchControl)CFGUI.ObjectField(new GUIContent("On Long Press", "Touch Control to drive with this multi-finger touch.\n\nTIP: It's a good idea to enable \'Can\'t be touched directly\' option on driven control."),
								controlToDriveOnLongPress, typeof(TouchControl), 150); 
							}

						controlToDriveOnNormalPressSwipe = (TouchControl)CFGUI.ObjectField(new GUIContent("On Swipe in Any dir. (Normal Press)", "Touch Control to drive with this multi-finger touch.\n\nTIP: It's a good idea to enable \'Can\'t be touched directly\' option on driven control."),
							controlToDriveOnNormalPressSwipe, typeof(TouchControl), 150); 

						if (controlToDriveOnNormalPressSwipe == null)
							{
							controlToDriveOnNormalPressSwipeU = (TouchControl)CFGUI.ObjectField(new GUIContent("On Swipe Up (Normal Press)", "Touch Control to drive with this multi-finger touch.\n\nTIP: It's a good idea to enable \'Can\'t be touched directly\' option on driven control."),
								controlToDriveOnNormalPressSwipeU, typeof(TouchControl), 150); 
							controlToDriveOnNormalPressSwipeR = (TouchControl)CFGUI.ObjectField(new GUIContent("On Swipe Right (Normal Press)", "Touch Control to drive with this multi-finger touch.\n\nTIP: It's a good idea to enable \'Can\'t be touched directly\' option on driven control."),
								controlToDriveOnNormalPressSwipeR, typeof(TouchControl), 150); 
							controlToDriveOnNormalPressSwipeD = (TouchControl)CFGUI.ObjectField(new GUIContent("On Swipe Down (Normal Press)", "Touch Control to drive with this multi-finger touch.\n\nTIP: It's a good idea to enable \'Can\'t be touched directly\' option on driven control."),
								controlToDriveOnNormalPressSwipeD, typeof(TouchControl), 150); 
							controlToDriveOnNormalPressSwipeL = (TouchControl)CFGUI.ObjectField(new GUIContent("On Swipe Left (Normal Press)", "Touch Control to drive with this multi-finger touch.\n\nTIP: It's a good idea to enable \'Can\'t be touched directly\' option on driven control."),
								controlToDriveOnNormalPressSwipeL, typeof(TouchControl), 150); 
							}

						if (config.touchConfig.detectLongPress)
							{
							controlToDriveOnLongPressSwipe = (TouchControl)CFGUI.ObjectField(new GUIContent("On Swipe in Any dir. (Long Press)", "Touch Control to drive with this multi-finger touch.\n\nTIP: It's a good idea to enable \'Can\'t be touched directly\' option on driven control."),
								controlToDriveOnLongPressSwipe, typeof(TouchControl), 120); 
	
							if (controlToDriveOnLongPressSwipe == null)
								{
								controlToDriveOnLongPressSwipeU = (TouchControl)CFGUI.ObjectField(new GUIContent("On Swipe Up (Long Press)", "Touch Control to drive with this multi-finger touch.\n\nTIP: It's a good idea to enable \'Can\'t be touched directly\' option on driven control."),
									controlToDriveOnLongPressSwipeU, typeof(TouchControl), 120); 
								controlToDriveOnLongPressSwipeR = (TouchControl)CFGUI.ObjectField(new GUIContent("On Swipe Right (Long Press)", "Touch Control to drive with this multi-finger touch.\n\nTIP: It's a good idea to enable \'Can\'t be touched directly\' option on driven control."),
									controlToDriveOnLongPressSwipeR, typeof(TouchControl), 120); 
								controlToDriveOnLongPressSwipeD = (TouchControl)CFGUI.ObjectField(new GUIContent("On Swipe Down (Long Press)", "Touch Control to drive with this multi-finger touch.\n\nTIP: It's a good idea to enable \'Can\'t be touched directly\' option on driven control."),
									controlToDriveOnLongPressSwipeD, typeof(TouchControl), 120); 
								controlToDriveOnLongPressSwipeL = (TouchControl)CFGUI.ObjectField(new GUIContent("On Swipe Left (Long Press)", "Touch Control to drive with this multi-finger touch.\n\nTIP: It's a good idea to enable \'Can\'t be touched directly\' option on driven control."),
									controlToDriveOnLongPressSwipeL, typeof(TouchControl), 120); 
								}
							}
						}


					CFGUI.EndIndentedVertical();
					}
				

				//EditorGUILayout.Space();


			InspectorUtils.EndIndentedSection();


			// Register Undo...


			if ((driveOtherControl	!= config.driveOtherControl) ||
				//(swipeToDrive		!= config.swipeToDrive) ||
				//(controlToDrive		!= config.controlToDrive) )
				(controlToDriveOnRawPress	!= config.controlToDriveOnRawPress) ||
				(controlToDriveOnNormalPress!= config.controlToDriveOnNormalPress) ||
				(controlToDriveOnLongPress	!= config.controlToDriveOnLongPress) ||

				(controlToDriveOnNormalPressSwipe		!= config.controlToDriveOnNormalPressSwipe) ||
				(controlToDriveOnNormalPressSwipeU		!= config.controlToDriveOnNormalPressSwipeU) ||
				(controlToDriveOnNormalPressSwipeR		!= config.controlToDriveOnNormalPressSwipeR) ||
				(controlToDriveOnNormalPressSwipeD		!= config.controlToDriveOnNormalPressSwipeD) ||
				(controlToDriveOnNormalPressSwipeL		!= config.controlToDriveOnNormalPressSwipeL) ||

				(controlToDriveOnLongPressSwipe			!= config.controlToDriveOnLongPressSwipe) ||
				(controlToDriveOnLongPressSwipeU		!= config.controlToDriveOnLongPressSwipeU) ||
				(controlToDriveOnLongPressSwipeR		!= config.controlToDriveOnLongPressSwipeR) ||
				(controlToDriveOnLongPressSwipeD		!= config.controlToDriveOnLongPressSwipeD) ||
				(controlToDriveOnLongPressSwipeL		!= config.controlToDriveOnLongPressSwipeL)
				)
				//|| 	(useSwipeJoystick	!= config.useSwipeJoystick))
				{					
				CFGUI.CreateUndo("Multi-finger touch config mod.", this.touchZone);

				config.driveOtherControl	= driveOtherControl;
				//config.swipeToDrive			= swipeToDrive;	
				//config.controlToDrive		= controlToDrive;
				//config.useSwipeJoystick		= useSwipeJoystick;
				config.controlToDriveOnRawPress		= controlToDriveOnRawPress;
				config.controlToDriveOnNormalPress	= controlToDriveOnNormalPress;
				config.controlToDriveOnLongPress	= controlToDriveOnLongPress;

				config.controlToDriveOnNormalPressSwipe		= controlToDriveOnNormalPressSwipe;
				config.controlToDriveOnNormalPressSwipeU	= controlToDriveOnNormalPressSwipeU;
				config.controlToDriveOnNormalPressSwipeR	= controlToDriveOnNormalPressSwipeR;
				config.controlToDriveOnNormalPressSwipeD	= controlToDriveOnNormalPressSwipeD;
				config.controlToDriveOnNormalPressSwipeL	= controlToDriveOnNormalPressSwipeL;

				config.controlToDriveOnLongPressSwipe		= controlToDriveOnLongPressSwipe;
				config.controlToDriveOnLongPressSwipeU		= controlToDriveOnLongPressSwipeU;
				config.controlToDriveOnLongPressSwipeR		= controlToDriveOnLongPressSwipeR;
				config.controlToDriveOnLongPressSwipeD		= controlToDriveOnLongPressSwipeD;
				config.controlToDriveOnLongPressSwipeL		= controlToDriveOnLongPressSwipeL;

				CFGUI.EndUndo(this.touchZone);
				}

			}


		// ------------------
		public void DrawBindingGUI(ControlFreak2.SuperTouchZone.MultiFingerTouchConfig config)
			{
			this.touchGestureBindingInsp.Draw(config.binding, config.touchConfig, this.touchZone.rig);
			}


		}

	

	}

		
}
#endif
