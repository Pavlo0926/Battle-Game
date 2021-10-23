// -------------------------------------------
// Control Freak 2
// Copyright (C) 2013-2020 Dan's Game Tools
// http://DansGameTools.blogspot.com
// -------------------------------------------

#if UNITY_EDITOR

using UnityEngine;
using UnityEditor;

using System.Collections.Generic;

using ControlFreak2;
using ControlFreak2.Internal;
using ControlFreak2Editor.Internal;

namespace ControlFreak2Editor.Inspectors
{


// ---------------------
// Wheel Creation Wizard	
// ----------------------
public class TouchWheelCreationWizard : ControlCreationWizardBase
	{
	private DigitalBinding
		pressBinding,
		turnLeftBinding,
		turnRightBinding;
	private AxisBinding 
		turnBinding;

	private DigitalBindingInspector
		pressBindingInsp,
		turnLeftBindingInsp,
		turnRightBindingInsp;
	private AxisBindingInspector 
		turnBindingInsp;
	
	private AxisBinding
		touchPressureBinding;
	private AxisBindingInspector
		touchPressureBindingInsp;
	

	// -----------------	
	public TouchWheelCreationWizard() : base()
		{	
		this.pressBinding = new DigitalBinding();
		this.turnLeftBinding = new DigitalBinding();
		this.turnRightBinding = new DigitalBinding();
		this.turnBinding = new AxisBinding();

		this.pressBindingInsp = new DigitalBindingInspector(null, new GUIContent("Press Binding"));
		this.turnLeftBindingInsp = new DigitalBindingInspector(null, new GUIContent("Turn Left Binding"));
		this.turnRightBindingInsp = new DigitalBindingInspector(null, new GUIContent("Turn Right Binding"));
		this.turnBindingInsp = new AxisBindingInspector(null, new GUIContent("Turn Analog Binding"), true, InputRig.InputSource.Analog);

		this.touchPressureBinding = new AxisBinding();
		this.touchPressureBindingInsp = new AxisBindingInspector(null, new GUIContent("Touch Pressure Binding"), false, 
			InputRig.InputSource.Analog, this.DrawPressureBindingExtraGUI);
		}

	// ----------------
	static public void ShowWizard(TouchControlPanel panel, BindingSetup bindingSetup = null, System.Action onCreationCallback = null)
		{
		TouchWheelCreationWizard w = (TouchWheelCreationWizard)EditorWindow.GetWindow(typeof(TouchWheelCreationWizard), true, "CF2 Steering Wheel Wizard");
		if (w != null)
			{
			w.Init(panel, bindingSetup, onCreationCallback);
			w.ShowPopup();
			}
		}			


		

	// ------------------
	protected void Init(TouchControlPanel panel, BindingSetup bindingSetup, System.Action onCreationCallback)
		{
		base.Init(panel, onCreationCallback);

		this.controlName = TouchControlWizardUtils.GetUniqueWheelName(this.panel.rig);
		this.defaultControlName = this.controlName;
			
		this.dynamicMode = ControlMode.DynamicWithRegion;
		this.regionRect = RegionRectPreset.LeftHalf;

		this.defaultSprite = TouchControlWizardUtils.GetDefaultWheelSprite(); 

		if (bindingSetup != null)
			bindingSetup.Apply(this);
	
		}


	// ------------------
	protected override void DrawHeader ()
		{
		GUILayout.Box(GUIContent.none, CFEditorStyles.Inst.headerWheel, GUILayout.ExpandWidth(true));
		}


	// ---------------------
	override protected void DrawBindingGUI()
		{
		InspectorUtils.BeginIndentedSection(new GUIContent("Binding Settings"));
		
			this.pressBindingInsp.Draw(this.pressBinding, this.panel.rig);
			this.touchPressureBindingInsp.Draw(this.touchPressureBinding, this.panel.rig);
		
			this.turnBindingInsp.Draw(this.turnBinding, this.panel.rig);
		
			this.turnLeftBindingInsp.Draw(this.turnLeftBinding, this.panel.rig);
			this.turnRightBindingInsp.Draw(this.turnRightBinding, this.panel.rig);
			

		InspectorUtils.EndIndentedSection(); 		
		}


	// --------------------
	override protected void OnCreatePressed(bool selectAfterwards)
		{
		ControlFreak2.TouchSteeringWheel newObj = (ControlFreak2.TouchSteeringWheel)this.CreateDynamicTouchControl(typeof(ControlFreak2.TouchSteeringWheel));
	
		newObj.wheelMode = TouchSteeringWheel.WheelMode.Turn;
		newObj.analogConfig.analogDeadZone = 0;
		newObj.centerOnDirectTouch		= false;
		newObj.centerWhenFollowing		= false;
		newObj.centerOnIndirectTouch	= false;

		newObj.pressBinding		.CopyFrom(this.pressBinding);
		newObj.touchPressureBinding.CopyFrom(this.touchPressureBinding);

		if (this.touchPressureBinding.enabled)
			newObj.emulateTouchPressure = this.emulateTouchPressure;

		newObj.analogTurnBinding.CopyFrom(this.turnBinding);
		newObj.turnRightBinding	.CopyFrom(this.turnRightBinding);
		newObj.turnLeftBinding	.CopyFrom(this.turnLeftBinding);

		TouchControlWizardUtils.CreateWheelAnimator(newObj, "-Sprite", this.defaultSprite, 1.0f);

		Undo.RegisterCreatedObjectUndo(newObj.gameObject, "Create CF2 Wheel");	
		
		if (selectAfterwards)
			Selection.activeObject =  newObj;	
		}	

	// -------------------------
	// Wheel Wizard Binding Setup class.
	// ------------------------
	public class BindingSetup
		{
		public string
			pressAxis;
		public KeyCode 
			pressKey;
		public string
			turnAxis;
		
		// -----------------
		static public BindingSetup TurnAxis			(string turnAxis)						{ return (new BindingSetup(turnAxis)); }
		static public BindingSetup PressAxis		(string pressAxis)						{ return (new BindingSetup(null, pressAxis)); }
		static public BindingSetup PressKey			(KeyCode pressKey)						{ return (new BindingSetup(null, null, pressKey)); }
		static public BindingSetup PressKeyOrAxis	(KeyCode pressKey, string	pressAxis)	{ return (new BindingSetup(null, pressAxis, pressKey)); }

		// --------------------			
		public BindingSetup(string turnAxis = null, string pressAxis = null, KeyCode pressKey = KeyCode.None)
			{
			this.turnAxis = turnAxis;
			this.pressAxis = pressAxis;
			this.pressKey = pressKey;
			}

		// ----------------------
		public void Apply(TouchWheelCreationWizard wizard)
			{

			SetupDigitalBinding(wizard.pressBinding, this.pressAxis, this.pressKey);
			SetupAxisBinding(wizard.turnBinding, this.turnAxis);
			
			}
		}


	}


}

#endif
