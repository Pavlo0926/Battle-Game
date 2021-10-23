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
	
[CustomEditor(typeof(ControlFreak2.TouchButton))]
public class TouchButtonInspector : TouchControlInspectorBase
	{

	public DigitalBindingInspector
		pressBindingInsp,
		//releasedBindingInsp,
		toggleBindingInsp;

	private AxisBindingInspector
		touchPressureBindingInsp;

	public RigSwitchNameDrawer
		toggleRigSwitchInsp;		




	// ---------------------
	void OnEnable()
		{
		this.pressBindingInsp 		= new DigitalBindingInspector(this.target, new GUIContent("Pressed or Toggled Binding"));
		this.toggleBindingInsp		= new DigitalBindingInspector(this.target, new GUIContent("Toggle Only Binding"));
		this.touchPressureBindingInsp = new AxisBindingInspector(null, new GUIContent("Touch Pressure Binding"), false, 
			InputRig.InputSource.Analog, this.DrawPressureBindingExtraGUI);
			
		this.toggleRigSwitchInsp = new RigSwitchNameDrawer();

		base.InitTouchControlInspector();
		}




	// ---------------
	public override void OnInspectorGUI()
		{
		TouchButton c = (TouchButton)this.target;
			
		GUILayout.Box(GUIContent.none, CFEditorStyles.Inst.headerButton, GUILayout.ExpandWidth(true));

		this.DrawWarnings(c);			

			
		bool 
			toggle					= c.toggle,
			linkToggleToRigSwitch	= c.linkToggleToRigSwitch,
			autoToggleOff			= c.autoToggleOff,
			toggleOffWhenHiding		= c.toggleOffWhenHiding;
		float 
			autoToggleOffTimeOut	= c.autoToggleOffTimeOut;
		TouchButton.ToggleOnAction
			toggleOnAction			= c.toggleOnAction;
		TouchButton.ToggleOffAction
			toggleOffAction			= c.toggleOffAction;
		string
			toggleRigSwitchName		= c.toggleRigSwitchName;

		this.emulateTouchPressure = c.emulateTouchPressure;
		
		// Button specific inspector....

			
			
			const float LABEL_WIDTH = 110;


			InspectorUtils.BeginIndentedSection(new GUIContent("Toggle Settings"));
	
				toggle = EditorGUILayout.ToggleLeft(new GUIContent("Toggle Mode", "Enable toggle mode."), 
					toggle);
				
				if (toggle)
					{
					CFGUI.BeginIndentedVertical();
					
					toggleOnAction = (TouchButton.ToggleOnAction)CFGUI.EnumPopup(new GUIContent("Toggle On Action", "Select when toggle state should change from OFF to ON."), 
						toggleOnAction, LABEL_WIDTH);
					toggleOffAction = (TouchButton.ToggleOffAction)CFGUI.EnumPopup(new GUIContent("Toggle Off Action", "Select when toggle state should change from ON to OFF."), 
						toggleOffAction, LABEL_WIDTH);
						
					if (toggleOffAction != TouchButton.ToggleOffAction.OnTimeout)
						{
						autoToggleOff = EditorGUILayout.ToggleLeft(new GUIContent("Auto Toggle Off", "When enabled, this button will auto-toggle off itself after specified amount of time."),
							autoToggleOff);
						}

					if (autoToggleOff || toggleOffAction == TouchButton.ToggleOffAction.OnTimeout)
						{
						CFGUI.BeginIndentedVertical();

						autoToggleOffTimeOut = CFGUI.FloatFieldEx(new GUIContent("Timeout (ms)", "Auto-toggle off time-out in milliseconds."), 	
							 autoToggleOffTimeOut, 0.1f, 1000, 1000, true, LABEL_WIDTH);

						CFGUI.EndIndentedVertical();
						}

					linkToggleToRigSwitch = EditorGUILayout.ToggleLeft(new GUIContent("Link toggle to Rig Switch", "Links this toggle button to Rig Switch's state.\nExternal modification to assigned flag will also affect this button's toggle state!"), 
						linkToggleToRigSwitch);
					
					if (linkToggleToRigSwitch)
						{
						CFGUI.BeginIndentedVertical();
	
						toggleRigSwitchName = this.toggleRigSwitchInsp.Draw("Switch", toggleRigSwitchName, c.rig, 50);
					
						toggleOffWhenHiding = EditorGUILayout.ToggleLeft(new GUIContent("Turn the Switch Off when hiding", "When enabled, this button will turn off linked Rig Switch when hidden."),
							toggleOffWhenHiding);

						CFGUI.EndIndentedVertical();

						}
	



	
					CFGUI.EndIndentedVertical();
					}
	
			InspectorUtils.EndIndentedSection();			
			
	

		InspectorUtils.BeginIndentedSection(new GUIContent("Button Bindings"));

	
			this.pressBindingInsp.Draw(c.pressBinding, c.rig);
			this.touchPressureBindingInsp.Draw(c.touchPressureBinding, c.rig);
		
			if (toggle)
				{
				this.toggleBindingInsp.Draw(c.toggleOnlyBinding, c.rig);
				}
	
		InspectorUtils.EndIndentedSection();


		// Register undo...

		if ((toggle					!= c.toggle) ||
			(linkToggleToRigSwitch	!= c.linkToggleToRigSwitch) ||
			(toggleOnAction			!= c.toggleOnAction) ||
			(toggleOffAction		!= c.toggleOffAction) ||
			(autoToggleOff			!= c.autoToggleOff) ||
			(autoToggleOffTimeOut	!= c.autoToggleOffTimeOut) ||
			(this.emulateTouchPressure != c.emulateTouchPressure) ||
			(toggleOffWhenHiding	!= c.toggleOffWhenHiding) ||
			(toggleRigSwitchName	!= c.toggleRigSwitchName) )
			{
			CFGUI.CreateUndo("CF2 Button modification", c);

			c.toggle				= toggle;
			c.linkToggleToRigSwitch	= linkToggleToRigSwitch;
			c.toggleOnAction		= toggleOnAction;
			c.toggleOffAction		= toggleOffAction;
			c.toggleRigSwitchName	= toggleRigSwitchName;
			c.autoToggleOff			= autoToggleOff;
			c.autoToggleOffTimeOut	= autoToggleOffTimeOut;
			c.toggleOffWhenHiding	= toggleOffWhenHiding;	
			c.emulateTouchPressure	= this.emulateTouchPressure;


			CFGUI.EndUndo(c);
			}


		// Draw Shared Dynamic Control Params...

		this.DrawDynamicTouchControlGUI(c);
		}
			
	
	

	}

		
}
#endif
