// -------------------------------------------
// Control Freak 2
// Copyright (C) 2013-2020 Dan's Game Tools
// http://DansGameTools.blogspot.com
// -------------------------------------------

#if UNITY_EDITOR

using UnityEngine;
using UnityEditor;

using ControlFreak2;
using ControlFreak2.Internal;

using ControlFreak2Editor;
using ControlFreak2Editor.Internal;
using UnityEditor.VersionControl;

namespace ControlFreak2Editor.Inspectors
{

abstract public class TouchControlInspectorBase : UnityEditor.Editor
	{
	public const float 
		LABEL_WIDTH = 60;


	protected DisablingConditionSetInspector
		disablingConditionSetInsp;

	protected ObjectListInspector
		swipeOverTargetListInsp;
		
	protected bool 
		emulateTouchPressure;




	
	// ------------------
	protected void InitTouchControlInspector()
		{
		TouchControl c = (TouchControl)this.target;

		this.disablingConditionSetInsp = new DisablingConditionSetInspector(new GUIContent("Disabling Conditions"), c.disablingConditions, c.rig, c);

		this.swipeOverTargetListInsp = new ObjectListInspector(new GUIContent("Swipe-Over Target List", "List of touch controls that can be swiped over from this control."),
			c, typeof(TouchControl), c.swipeOverTargetList); 
		}

	// ----------------
	protected void DrawPressureBindingExtraGUI()
		{
		this.emulateTouchPressure = EditorGUILayout.ToggleLeft(new GUIContent("Emulate Touch Pressure", "When enabled and controlling touch is not pressure sensitive, digital state will be applied to analog targets."),
			this.emulateTouchPressure, GUILayout.ExpandWidth(true), GUILayout.MinWidth(30));
		}


	// -------------------
	public void DrawWarnings(TouchControl c)
		{
		if (!c.gameObject.activeInHierarchy)
			EditorGUILayout.HelpBox("This control is inactive!", MessageType.Info);
		else
			{
			if (c.rig == null)
				InspectorUtils.DrawErrorBox("This control is not connected to a CF2 Input Rig!!");
			if (c.panel == null)
				InspectorUtils.DrawErrorBox("This control is not connected to a CF2 Touch Control Panel!");
			}
		}

	// -------------------
	public void DrawTouchContolGUI(TouchControl c)
		{
		if (c == null) return;
			
		bool 
			ignoreFingerRadius			= c.ignoreFingerRadius,				
			cantBeControlledDirectly	= c.cantBeControlledDirectly,	
			shareTouch					= c.shareTouch,
			dontAcceptSharedTouches		= c.dontAcceptSharedTouches,
			canBeSwipedOver				= c.canBeSwipedOver,
			restictSwipeOverTargets		= c.restictSwipeOverTargets;
		TouchControl.SwipeOverOthersMode
			swipeOverOthersMode			= c.swipeOverOthersMode;
		TouchControl.SwipeOffMode
			swipeOffMode				= c.swipeOffMode;
		TouchControl.Shape
			shape					= c.shape;
			
			
		// GUI...
			
		InspectorUtils.BeginIndentedSection(new GUIContent("Basic Settings"));
		
			shape = (TouchControl.Shape) CFGUI.EnumPopup(new GUIContent("Shape", "Control's shape"), shape, LABEL_WIDTH);
	
			ignoreFingerRadius = EditorGUILayout.ToggleLeft(new GUIContent("Ignore finger radius", "When enabled, this can only be hit by finger's center - finger's radius defined in Touch Control Panel will be ignored when hit testing."),
				ignoreFingerRadius, GUILayout.MinWidth(30));

			cantBeControlledDirectly = EditorGUILayout.ToggleLeft(new GUIContent("Can't be touched directly", "When enabled, this control will not respond to direct touches, only to touches passed by it's dynamic region or other controls."), 
				cantBeControlledDirectly, GUILayout.MinWidth(30));
			shareTouch = EditorGUILayout.ToggleLeft(new GUIContent("Touch-Through", "When touched, this control will pass it's controling touch down to control other controls below (if they accept shared touches)."), 
				shareTouch, GUILayout.MinWidth(30));
			dontAcceptSharedTouches = EditorGUILayout.ToggleLeft(new GUIContent("Don't accept shared touches", "When enabled, this control will not respond to touches already controlling other controls."), 
				dontAcceptSharedTouches, GUILayout.MinWidth(30));

			EditorGUILayout.Space();

			canBeSwipedOver = EditorGUILayout.ToggleLeft(new GUIContent("Can be swiped over", "This control can be touched by swiping over it."), 
				canBeSwipedOver, GUILayout.MinWidth(30));

			swipeOffMode = (TouchControl.SwipeOffMode)CFGUI.EnumPopup(new GUIContent("Swipe Off", "Release when finger swipes off this control?"), 
				swipeOffMode, 100);

			swipeOverOthersMode = (TouchControl.SwipeOverOthersMode)CFGUI.EnumPopup(new GUIContent("Swipe Over Others", "When to allow swiping over other controls?"), 
				swipeOverOthersMode, 100);



			if (swipeOverOthersMode != TouchControl.SwipeOverOthersMode.Disabled)
				{	
				restictSwipeOverTargets = EditorGUILayout.ToggleLeft(new GUIContent("Restrict Swipe Over Targets", "Choose specific controls that can be swiped over from this control..."),
					restictSwipeOverTargets, GUILayout.MinWidth(30));
		
				if (restictSwipeOverTargets)
					this.swipeOverTargetListInsp.DrawGUI();
				}

	
		InspectorUtils.EndIndentedSection();
	
			

		// Hiding conditions section...

		this.disablingConditionSetInsp.DrawGUI();


	
				
		// Refister undo...
	
		if (
			(ignoreFingerRadius			!= c.ignoreFingerRadius) ||				
			(cantBeControlledDirectly	!= c.cantBeControlledDirectly) ||
			(shareTouch					!= c.shareTouch) ||
			(shareTouch					!= c.shareTouch) ||
			(dontAcceptSharedTouches	!= c.dontAcceptSharedTouches) ||

			(canBeSwipedOver			!= c.canBeSwipedOver) ||
			(swipeOffMode				!= c.swipeOffMode) ||
			(swipeOverOthersMode		!= c.swipeOverOthersMode) ||
			(restictSwipeOverTargets	!= c.restictSwipeOverTargets) ||

			(shape						!= c.shape))
			{
	
			CFGUI.CreateUndo("Touch Control Base Param modification", c);
		
			c.ignoreFingerRadius			= ignoreFingerRadius;
			c.cantBeControlledDirectly		= cantBeControlledDirectly;
			c.shareTouch					= shareTouch;
			c.dontAcceptSharedTouches		= dontAcceptSharedTouches;
			c.shape							= shape;

			c.canBeSwipedOver				= canBeSwipedOver;
			c.swipeOffMode					= swipeOffMode;
			c.swipeOverOthersMode			= swipeOverOthersMode;
			c.restictSwipeOverTargets		= restictSwipeOverTargets;

	
			CFGUI.EndUndo(c);
	
			}
		}

		

	// -----------------
	public void DrawDynamicTouchControlGUI(DynamicTouchControl c)
		{
		// Draw basic TouchControl GUI...

		this.DrawTouchContolGUI(c);			


		// Dynamic-only GUI...

		bool 
			fadeOutWhenReleased		= c.fadeOutWhenReleased,
			startFadedOut			= c.startFadedOut;
		float
			fadeOutTargetAlpha		= c.fadeOutTargetAlpha,
			fadeInDuration			= c.fadeInDuration,
			fadeOutDelay			= c.fadeOutDelay,
			fadeOutDuration			= c.fadeOutDuration;
			
		bool
			centerOnDirectTouch		= c.centerOnDirectTouch,
			centerOnIndirectTouch	= c.centerOnIndirectTouch,
			centerWhenFollowing		= c.centerWhenFollowing,

			stickyMode				= c.stickyMode,
			clampInsideRegion		= c.clampInsideRegion,
			clampInsideCanvas		= c.clampInsideCanvas,

			returnToStartingPosition= c.returnToStartingPosition;

		Vector2
			directInitialVector		= c.directInitialVector,			
			indirectInitialVector	= c.indirectInitialVector;			
		float
			originSmoothTime		= c.originSmoothTime,
			touchSmoothing			= c.touchSmoothing;
		DynamicRegion 
			targetDynamicRegion		= c.targetDynamicRegion;	

			
		// GUI...

		
		
		InspectorUtils.BeginIndentedSection(new GUIContent("Dynamic Control Settings"));
		
		if (targetDynamicRegion == null)
			EditorGUILayout.HelpBox("Connect this control to a Dynamic Region to make it dynamic.", MessageType.Info);
		//else
		//	{
		//	GUI.enabled = false;
		targetDynamicRegion = (DynamicRegion)CFGUI.ObjectField(new GUIContent("Region", "Control's Dynamic Region.\n\nWARNING: Only one control can be assigned to a dynamic region!"), 
			targetDynamicRegion, typeof(DynamicRegion), LABEL_WIDTH);
		//	GUI.enabled = true;
		//	}
			
		EditorGUILayout.Space();

		if (targetDynamicRegion != null)
			{
			if ((fadeOutWhenReleased = EditorGUILayout.ToggleLeft(new GUIContent("Fade-out when released", "Fade-out control when released (Dynamic mode ONLY)."),
				fadeOutWhenReleased)))
				{
				CFGUI.BeginIndentedVertical();
			
				startFadedOut = EditorGUILayout.ToggleLeft(new GUIContent("Start faded out", "This tontrol will start invisible at the begining."), 
					startFadedOut);

				fadeOutTargetAlpha = CFGUI.Slider(new GUIContent("F.O. Alpha", "Fade-Out Target Alpha. Default value is ZERO (completely invisible)."), 
					fadeOutTargetAlpha, 0, 1, LABEL_WIDTH);		

				EditorGUILayout.Space();

				fadeInDuration = CFGUI.FloatFieldEx(new GUIContent("Fade-In Dur.", "Fade-In duration (ms) : 0 = instant fade-in."), 
					fadeInDuration, 0, 2, 1000, true, 100);		
				fadeOutDuration = CFGUI.FloatFieldEx(new GUIContent("Fade-Out Dur.", "Fade-Out duration (ms) : 0 = instant fade-out."),
					fadeOutDuration, 0, 2, 1000, true, 100);		

				fadeOutDelay = CFGUI.FloatFieldEx(new GUIContent("Fade-Out Delay.", "Fade-Out delay (ms)."), 
					fadeOutDelay, 0, 2, 1000, true, 100);		

				CFGUI.EndIndentedVertical();

				EditorGUILayout.Space();
				}
			}
			
	
		if (targetDynamicRegion != null)
			{
			centerOnDirectTouch = EditorGUILayout.ToggleLeft(new GUIContent("Center On Direct Touch"), 
				centerOnDirectTouch);
			centerOnIndirectTouch = EditorGUILayout.ToggleLeft(new GUIContent("Center On Indirect Touch"), 
				centerOnIndirectTouch);

			const string initialPosTooltip = 
				"Normalized position that the control will center itself after being activated by a touch.\n" +
				"Default if (0,0) (center).";
	
			if (centerOnDirectTouch)
				{
				EditorGUILayout.LabelField(new GUIContent("Direct Touch Initial Pos", initialPosTooltip) );
				CFGUI.BeginIndentedVertical();
					directInitialVector.x = CFGUI.Slider(new GUIContent("X", initialPosTooltip),  directInitialVector.x, -1, 1, 20);
					directInitialVector.y = CFGUI.Slider(new GUIContent("Y", initialPosTooltip),  directInitialVector.y, -1, 1, 20);
				CFGUI.EndIndentedVertical();							
				}

			if (centerOnIndirectTouch)
				{
				EditorGUILayout.LabelField(new GUIContent("Indirect Touch Initial Pos", initialPosTooltip) );
				CFGUI.BeginIndentedVertical();
					indirectInitialVector.x = CFGUI.Slider(new GUIContent("X", initialPosTooltip),  indirectInitialVector.x, -1, 1, 20);
					indirectInitialVector.y = CFGUI.Slider(new GUIContent("Y", initialPosTooltip),  indirectInitialVector.y, -1, 1, 20);
				CFGUI.EndIndentedVertical();							
				}		

			EditorGUILayout.Space();
			}

				

		//GUI.enabled = (c.swipeOffMode == TouchControl.SwipeOffMode.Disabled);
		GUI.enabled = (c.swipeOffMode != TouchControl.SwipeOffMode.Enabled);
				
		
		stickyMode = EditorGUILayout.ToggleLeft(new GUIContent("Sticky Mode", "Control will follow the finger is it goes out of control bounds. This option will be disabled if control's Swipe Off Mode is det to Enabled!"), 
			stickyMode);

		if (stickyMode)	
			{
			CFGUI.BeginIndentedVertical();
				centerWhenFollowing = EditorGUILayout.ToggleLeft(new GUIContent("Center when following.", "WARNING! Don't use this option on Joysticks or Steering Wheels - it will make them unusable!"), 
					centerWhenFollowing);
			CFGUI.EndIndentedVertical();
			}

		GUI.enabled = true;
			
		EditorGUILayout.Space();


		clampInsideRegion = EditorGUILayout.ToggleLeft(new GUIContent("Clamp Inside Region", "Control will be clamped inside Dynamic Region when in Sticky Mode or when started by indirect touch."),
			clampInsideRegion);
			
		clampInsideCanvas = EditorGUILayout.ToggleLeft(new GUIContent("Clamp Inside Canvas", "Control will be clamped inside parent canvas when in Sticky Mode or when started by indirect touch."),
			clampInsideCanvas);
			

		EditorGUILayout.Space();

		if (targetDynamicRegion != null)
			{
			returnToStartingPosition = EditorGUILayout.ToggleLeft(new GUIContent("Return to start position", "Control will return to starting position after being released (Dynamic Mode)"),
				returnToStartingPosition);

			EditorGUILayout.Space();
			}


		originSmoothTime = CFGUI.Slider(new GUIContent("Move. smoothing", "Movement smoothing time (sticky mode, dynamic mode).\n\n0.0 = no smoothing/no animation.\n1.0 = slow movement."),
			originSmoothTime, 0, 1,  110);

		touchSmoothing = CFGUI.Slider(new GUIContent("Touch smoothing", "Amount of smoothing applied to controlling touch position. "),
			touchSmoothing, 0, 1,  110);
		

		InspectorUtils.EndIndentedSection();
	


	
		// Register Undo...
		
		if ((fadeOutWhenReleased	!= c.fadeOutWhenReleased) ||
			(startFadedOut			!= c.startFadedOut) ||
			(fadeOutTargetAlpha		!= c.fadeOutTargetAlpha) ||
			(fadeInDuration			!= c.fadeInDuration) ||
			(fadeOutDelay			!= c.fadeOutDelay) ||
			(fadeOutDuration		!= c.fadeOutDuration) ||
			(centerOnDirectTouch	!= c.centerOnDirectTouch) ||
			(centerOnIndirectTouch	!= c.centerOnIndirectTouch) ||
			(stickyMode				!= c.stickyMode) ||
			(clampInsideRegion		!= c.clampInsideRegion) ||
			(clampInsideCanvas		!= c.clampInsideCanvas) ||
			(targetDynamicRegion	!= c.targetDynamicRegion) ||
			(returnToStartingPosition!= c.returnToStartingPosition) ||
			(directInitialVector	!= c.directInitialVector) ||
			(indirectInitialVector	!= c.indirectInitialVector) ||
			(touchSmoothing			!= c.touchSmoothing) ||
			(originSmoothTime		!= c.originSmoothTime) )
			{
			CFGUI.CreateUndo("Dynamic Touch Control modification.", c);
	
			c.fadeOutWhenReleased		= fadeOutWhenReleased;
			c.startFadedOut				= startFadedOut;
			c.fadeOutTargetAlpha		= fadeOutTargetAlpha;
			c.fadeInDuration			= fadeInDuration;
			c.fadeOutDelay				= fadeOutDelay;
			c.fadeOutDuration			= fadeOutDuration;
			c.centerOnDirectTouch		= centerOnDirectTouch;
			c.centerOnIndirectTouch		= centerOnIndirectTouch;
			c.stickyMode				= stickyMode;
			c.clampInsideRegion			= clampInsideRegion;
			c.clampInsideCanvas			= clampInsideCanvas;
			c.returnToStartingPosition	= returnToStartingPosition;
			c.directInitialVector		= directInitialVector;
			c.indirectInitialVector		= indirectInitialVector;
			c.originSmoothTime			= originSmoothTime;
			
			if (c.touchSmoothing != touchSmoothing)
				c.SetTouchSmoothing(touchSmoothing);

			if (targetDynamicRegion	!= c.targetDynamicRegion)
				c.SetTargetDynamicRegion(targetDynamicRegion);

			CFGUI.EndUndo(c);
			}

		}






	}
}

#endif
