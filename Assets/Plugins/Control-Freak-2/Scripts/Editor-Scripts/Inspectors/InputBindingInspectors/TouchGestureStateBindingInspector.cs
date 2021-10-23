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



// ----------------------
// Touch State Binding Inspector.
// ----------------------
public class TouchGestureStateBindingInspector
	{	
	private GUIContent			labelContent;
	private Object				undoObject;
	//private Editor				editor;
		
	private DigitalBindingInspector
		rawPressBinding,
		normalPressBinding,
		longPressBinding,
		longTapBinding,
		tapBinding,
		doubleTapBinding;	
	private AxisBindingInspector 
		normalPressSwipeHorzAxisBinding,
		normalPressSwipeVertAxisBinding,
		longPressSwipeHorzAxisBinding,
		longPressSwipeVertAxisBinding;
	private ScrollDeltaBindingInspector
		normalPressScrollHorzBinding,
		normalPressScrollVertBinding,			
		longPressScrollHorzBinding,
		longPressScrollVertBinding;			
	public DirectionBindingInspector
		normalPressSwipeDirBinding,		
		longPressSwipeDirBinding;		
	public JoystickStateBindingInspector
		normalPressSwipeJoyBinding,		
		longPressSwipeJoyBinding;		
	public EmuTouchBindingInspector
		rawPressEmuTouchInsp,
		normalPressEmuTouchInsp,
		longPressEmuTouchInsp;

	public MousePositionBindingInspector
		rawPressMousePosBindingInsp,
		normalPressMousePosBindingInsp,
		longPressMousePosBindingInsp,
		tapMousePosBindingInsp,
		doubleTapMousePosBindingInsp,
		longTapMousePosBindingInsp,
		normalPressSwipeMousePosBindingInsp, 
		longPressSwipeMousePosBindingInsp; 
		
	// ------------------

	private System.Action
		customPostGUICallback;


	// ------------------
	public TouchGestureStateBindingInspector(/*Editor editor,*/ Object undoObject, GUIContent labelContent)
		{
		this.labelContent	= labelContent;
		//this.editor		= editor;
		this.undoObject	= undoObject;


		this.rawPressBinding				= new DigitalBindingInspector(undoObject, new GUIContent("Raw Press", "Bind Raw Press state to an axis and/or a key."));
		this.rawPressMousePosBindingInsp	= new MousePositionBindingInspector(undoObject, new GUIContent("Raw Press Mouse Pos", 	"Bind raw press position as virtual mouse position."));

		this.normalPressBinding				= new DigitalBindingInspector(undoObject, new GUIContent("Normal Press",	"Bind Normal Press (after a potential tap has been rulled out) state to an axis and/or a key."));
		this.normalPressMousePosBindingInsp= new MousePositionBindingInspector(undoObject, new GUIContent("Normal Press Mouse Pos","Bind Normal press position as virtual mouse position."));

		this.longPressBinding				= new DigitalBindingInspector(undoObject, new GUIContent("Long Press",	"Bind Long Press state to an axis and/or a key."));
		this.longPressMousePosBindingInsp 	= new MousePositionBindingInspector(undoObject, new GUIContent("Long Press Mouse Pos",	"Bind long press position as virtual mouse position."));
		
		this.tapBinding					= new DigitalBindingInspector(undoObject, new GUIContent("Tap", 	"Bind Single Tap to an axis and/or a key."));
		this.tapMousePosBindingInsp		= new MousePositionBindingInspector(undoObject, new GUIContent("Tap Mouse Pos", 		"Bind tap position as virtual mouse position."));

		this.doubleTapBinding			= new DigitalBindingInspector(undoObject, new GUIContent("Double Tap", 	"Bind Double Tap to an axis and/or a key."));	
		this.doubleTapMousePosBindingInsp= new MousePositionBindingInspector(undoObject, new GUIContent("Double Tap Mouse Pos", "Bind double tap position as virtual mouse position."));

		this.longTapBinding				= new DigitalBindingInspector(undoObject, new GUIContent("Long Tap", 		"Bind Long Tap state to an axis and/or a key."));
		this.longTapMousePosBindingInsp	= new MousePositionBindingInspector(undoObject, new GUIContent("Long Tap Mouse Pos",	"Bind long tap position as virtual mouse position."));

		this.normalPressSwipeHorzAxisBinding		= new AxisBindingInspector(undoObject, new GUIContent("Horizontal Swipe Delta (Normal Press)", "Bind Horizontal swipe delta to an axis."), true, InputRig.InputSource.TouchDelta);
		this.normalPressSwipeVertAxisBinding		= new AxisBindingInspector(undoObject, new GUIContent("Vertical Swipe Delta (Normal Press)", "Bind Vertical swipe delta to an axis."), true, InputRig.InputSource.TouchDelta);
		this.normalPressSwipeDirBinding				= new DirectionBindingInspector(undoObject, new GUIContent("Swipe Segment's Direction (Normal Press)", "Swipe segment's direction."));
		this.normalPressSwipeMousePosBindingInsp	= new MousePositionBindingInspector(undoObject, new GUIContent("Swipe Mouse Pos (Normal Press)",		"Bind swipe position as virtual mouse position."));

		this.normalPressSwipeJoyBinding				= new JoystickStateBindingInspector(undoObject, new GUIContent("Swipe Joystick (Normal Press)", "Swipe Joystick-like state binding."));
		this.longPressSwipeJoyBinding				= new JoystickStateBindingInspector(undoObject, new GUIContent("Swipe Joystick (Long Press)", "Swipe Joystick-like state binding."));
			 
		this.normalPressScrollHorzBinding		= new ScrollDeltaBindingInspector(undoObject, new GUIContent("Horizontal Scoll Delta (Normal Press)", "Bind Horizontal scroll delta.")); //true, , InputRig.InputSource.SCROLL);
		this.normalPressScrollVertBinding		= new ScrollDeltaBindingInspector(undoObject, new GUIContent("Vertical Scroll Delta (Normal Press)", "Bind Vertical scroll delta.")); // true, InputRig.InputSource.SCROLL);


		this.longPressSwipeHorzAxisBinding		= new AxisBindingInspector(undoObject, new GUIContent("Horizontal Swipe Delta (Long Press)", "Bind Horizontal swipe delta to an axis."), true, InputRig.InputSource.TouchDelta);
		this.longPressSwipeVertAxisBinding		= new AxisBindingInspector(undoObject, new GUIContent("Vertical Swipe Delta (Long Press)", "Bind Vertical swipe delta to an axis."), true, InputRig.InputSource.TouchDelta);
		this.longPressSwipeDirBinding				= new DirectionBindingInspector(undoObject, new GUIContent("Swipe Segment's Direction (Long Press)", "Swipe segment's direction."));
		this.longPressSwipeMousePosBindingInsp	= new MousePositionBindingInspector(undoObject, new GUIContent("Swipe Mouse Pos (Long Press)",		"Bind swipe position as virtual mouse position."));
			 
		this.longPressScrollHorzBinding		= new ScrollDeltaBindingInspector(undoObject, new GUIContent("Horizontal Scoll Delta (Long Press)", "Bind Horizontal scroll delta.")); //true, , InputRig.InputSource.SCROLL);
		this.longPressScrollVertBinding		= new ScrollDeltaBindingInspector(undoObject, new GUIContent("Vertical Scroll Delta (Long Press)", "Bind Vertical scroll delta.")); // true, InputRig.InputSource.SCROLL);

		//this.swipeDirBinding.SetCustomPreGUI(this.DrawSwipeBindModeGUI);


		this.rawPressEmuTouchInsp		= new EmuTouchBindingInspector(undoObject, new GUIContent("Raw Press as Emulated Touch", "Bind touch's position and raw press state as emulated touch (Input.touches[])."));
		this.normalPressEmuTouchInsp	= new EmuTouchBindingInspector(undoObject, new GUIContent("Normal Press as Emulated Touch", "Bind touch's position and normal press state as emulated touch (Input.touches[])."));
		this.longPressEmuTouchInsp		= new EmuTouchBindingInspector(undoObject, new GUIContent("Long Press as Emulated Touch", "Bind touch's position and long press state as emulated touch (Input.touches[])."));

		}


	// -----------------
	public void SetCustomPostGUI(System.Action callback)
		{
		this.customPostGUICallback = callback;
		}

		

	// ------------------
	public void Draw(TouchGestureStateBinding bind, TouchGestureConfig config, InputRig rig)
		{
		bool	bindingEnabled	= bind.enabled;


		//EditorGUILayout.BeginVertical();

		//if (bindingEnabled = EditorGUILayout.ToggleLeft(this.labelContent, bindingEnabled, GUILayout.MinWidth(30), GUILayout.ExpandWidth(true)))
		//	{
		//	CFGUI.BeginIndentedVertical();

		if (InspectorUtils.BeginIndentedCheckboxSection(this.labelContent, ref bindingEnabled))
			{
				InspectorUtils.BeginIndentedSection(new GUIContent("Press bindings"));

					this.rawPressBinding				.Draw(bind.rawPressBinding, rig);
					this.rawPressMousePosBindingInsp	.Draw(bind.rawPressMousePosBinding, rig);
					EditorGUILayout.Space();
		
					this.normalPressBinding				.Draw(bind.normalPressBinding, rig);
					this.normalPressMousePosBindingInsp	.Draw(bind.normalPressMousePosBinding, rig);
					EditorGUILayout.Space();
		
					if ((config == null) || config.detectLongPress)
						{
						this.longPressBinding				.Draw(bind.longPressBinding, rig);
						this.longPressMousePosBindingInsp	.Draw(bind.longPressMousePosBinding, rig);
						}

					EditorGUILayout.Space();

					this.rawPressEmuTouchInsp	.Draw(bind.rawPressEmuTouchBinding, rig);
					this.normalPressEmuTouchInsp.Draw(bind.normalPressEmuTouchBinding, rig);

					if ((config == null) || config.detectLongPress)
						{
						this.longPressEmuTouchInsp	.Draw(bind.longPressEmuTouchBinding, rig);
						}


				InspectorUtils.EndIndentedSection();

			if ((config == null) || (config.maxTapCount >= 1) || (config.detectLongTap))
				{
				InspectorUtils.BeginIndentedSection(new GUIContent("Tap bindings"));
				
				if ((config == null) || (config.maxTapCount >= 1))
					{
					this.tapBinding					.Draw(bind.tapBinding, rig);
					this.tapMousePosBindingInsp		.Draw(bind.tapMousePosBinding, rig);
					EditorGUILayout.Space();
					}
	
				if ((config == null) || (config.maxTapCount >= 2))
					{
					this.doubleTapBinding				.Draw(bind.doubleTapBinding, rig);
					this.doubleTapMousePosBindingInsp	.Draw(bind.doubleTapMousePosBinding, rig);
					EditorGUILayout.Space();
					}
	
	
				if ((config == null) || (config.detectLongPress && config.detectLongTap))
					{
					this.longTapBinding					.Draw(bind.longTapBinding, rig);
					this.longTapMousePosBindingInsp		.Draw(bind.longTapMousePosBinding, rig);
					EditorGUILayout.Space();
					}

				InspectorUtils.EndIndentedSection();
				}
		

			InspectorUtils.BeginIndentedSection(new GUIContent("Swipe bindings"));

				this.normalPressSwipeHorzAxisBinding	.Draw(bind.normalPressSwipeHorzAxisBinding, rig);
				this.normalPressSwipeVertAxisBinding	.Draw(bind.normalPressSwipeVertAxisBinding, rig);
				this.normalPressSwipeMousePosBindingInsp.Draw(bind.normalPressSwipeMousePosBinding, rig);	
				this.normalPressSwipeDirBinding			.Draw(bind.normalPressSwipeDirBinding, rig);
				this.normalPressSwipeJoyBinding			.Draw(bind.normalPressSwipeJoyBinding, rig);

				if ((config == null) || config.detectLongPress)
					{
					this.longPressSwipeHorzAxisBinding		.Draw(bind.longPressSwipeHorzAxisBinding, rig);
					this.longPressSwipeVertAxisBinding		.Draw(bind.longPressSwipeVertAxisBinding, rig);
					this.longPressSwipeMousePosBindingInsp	.Draw(bind.longPressSwipeMousePosBinding, rig);	
					this.longPressSwipeDirBinding			.Draw(bind.longPressSwipeDirBinding, rig);
					this.longPressSwipeJoyBinding			.Draw(bind.longPressSwipeJoyBinding, rig);
					}

			InspectorUtils.EndIndentedSection();

			InspectorUtils.BeginIndentedSection(new GUIContent("Scroll bindings"));
				this.normalPressScrollHorzBinding.Draw(bind.normalPressScrollHorzBinding, rig);
				this.normalPressScrollVertBinding.Draw(bind.normalPressScrollVertBinding, rig);

				if ((config == null) || config.detectLongPress)
					{
					this.longPressScrollHorzBinding.Draw(bind.longPressScrollHorzBinding, rig);
					this.longPressScrollVertBinding.Draw(bind.longPressScrollVertBinding, rig);
					}
			InspectorUtils.EndIndentedSection();

			

			if (this.customPostGUICallback != null)
				{
				InspectorUtils.BeginIndentedSection(new GUIContent("Other"));

				this.customPostGUICallback();
		
				InspectorUtils.EndIndentedSection();
				}


			InspectorUtils.EndIndentedSection();
			//CFGUI.EndIndentedVertical();
			//GUILayout.Space(InputBindingGUIUtils.VERT_MARGIN);
			}

		//EditorGUILayout.EndVertical();



		if ((bindingEnabled	!= bind.enabled)  )
			{
			CFGUI.CreateUndo("Touch Binding modification.", this.undoObject);
				
			bind.enabled			= bindingEnabled;
				
			CFGUI.EndUndo(this.undoObject);
			}
			
			

		}

	

	} 


		
}
#endif
