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
// Button Creation Wizard	
// ----------------------
public class TouchButtonCreationWizard : ControlCreationWizardBase
	{

	private DigitalBinding
		pressBinding,
		toggleBinding;
	
	private DigitalBindingInspector
		pressBindingInsp,
		toggleBindingInsp;

	private AxisBinding
		touchPressureBinding;

	private AxisBindingInspector
		touchPressureBindingInsp;


	// ----------------
	static public void ShowWizard(TouchControlPanel panel, BindingSetup bindingSetup = null, System.Action onCreationCallback = null)
		{
		TouchButtonCreationWizard w = (TouchButtonCreationWizard)EditorWindow.GetWindow(typeof(TouchButtonCreationWizard), true, "CF2 Button Wizard");
		if (w != null)
			{
			w.Init(panel, bindingSetup, onCreationCallback);
			w.ShowPopup();
			}
		}			
	


	// -------------------
	public TouchButtonCreationWizard() : base()
		{	
		this.minSize = new Vector2(this.minSize.x, 500);
		this.pressBinding = new DigitalBinding();
		this.pressBindingInsp = new DigitalBindingInspector(null, new GUIContent("Press Binding"));
		this.toggleBinding = new DigitalBinding();
		this.toggleBindingInsp = new DigitalBindingInspector(null, new GUIContent("Toggle Binding"));
		this.touchPressureBinding = new AxisBinding();
		this.touchPressureBindingInsp = new AxisBindingInspector(null, new GUIContent("Touch Pressure Binding"), false, 
			InputRig.InputSource.Analog, this.DrawPressureBindingExtraGUI);
		}


		
	// ------------------
	protected override void DrawHeader ()
		{
		GUILayout.Box(GUIContent.none, CFEditorStyles.Inst.headerButton, GUILayout.ExpandWidth(true));
		}


	// ------------------
	protected void Init(TouchControlPanel panel, BindingSetup bindingSetup, System.Action onCreationCallback)
		{
		base.Init(panel, onCreationCallback);

		if (bindingSetup != null)
			{
			bindingSetup.Apply(this);
			}

		this.controlName = TouchControlWizardUtils.GetUniqueButtonName(panel.rig);
		this.defaultControlName = this.controlName;
			
		string uniqueNameSuffix = "";

		if ((bindingSetup != null) && !string.IsNullOrEmpty(bindingSetup.pressAxis))
			uniqueNameSuffix = bindingSetup.pressAxis;
		else if ((bindingSetup != null) && !string.IsNullOrEmpty(bindingSetup.toggleAxis))
			uniqueNameSuffix = bindingSetup.toggleAxis;

		if (!string.IsNullOrEmpty(uniqueNameSuffix))
			this.controlName = (uniqueNameSuffix + "-Button");

		this.defaultSprite = TouchControlWizardUtils.GetDefaultButtonSprite(uniqueNameSuffix);
		}


	// ---------------------
	override protected void DrawBindingGUI()
		{
		InspectorUtils.BeginIndentedSection(new GUIContent("Binding"));
		
			this.pressBindingInsp.Draw(this.pressBinding, this.panel.rig);
			this.touchPressureBindingInsp.Draw(this.touchPressureBinding, this.panel.rig);
			this.toggleBindingInsp.Draw(this.toggleBinding, this.panel.rig);

		InspectorUtils.EndIndentedSection(); 		
		}


	// --------------------
	override protected void OnCreatePressed(bool selectAfterwards)
		{
		ControlFreak2.TouchButton button = (ControlFreak2.TouchButton)this.CreateDynamicTouchControl(typeof(ControlFreak2.TouchButton));


		TouchControlWizardUtils.CreateButtonAnimator(button, "-Sprite", this.defaultSprite, 1.0f);
		
		if (this.pressBinding.enabled)
			button.pressBinding.CopyFrom(this.pressBinding);
		
		if (this.touchPressureBinding.enabled)
			{
			button.touchPressureBinding.CopyFrom(this.touchPressureBinding);
			button.emulateTouchPressure = this.emulateTouchPressure;
			}

		if (this.toggleBinding.enabled)
			{
			button.toggle = true;
			button.toggleOnlyBinding.CopyFrom(this.toggleBinding);
			}

		Undo.RegisterCreatedObjectUndo(button.gameObject, "Create Touch Button");	
		
		if (selectAfterwards)
			Selection.activeObject =  button;	
		}		



	// -------------------------
	// Button Wizard Binding Setup class.
	// ------------------------
	public class BindingSetup
		{
		public string
			pressAxis,
			toggleAxis;
		public KeyCode 
			pressKey,
			toggleKey;
		
		// -----------------
		static public BindingSetup PressAxis		(string	pressAxis)						{ return (new BindingSetup(pressAxis)); }
		static public BindingSetup PressKey			(KeyCode pressKey)						{ return (new BindingSetup(null, pressKey)); }
		static public BindingSetup PressKeyOrAxis	(KeyCode pressKey, string	pressAxis)	{ return (new BindingSetup(pressAxis, pressKey)); }
		static public BindingSetup ToggleAxis		(string	toggleAxis)						{ return (new BindingSetup(null, KeyCode.None, toggleAxis)); }
		static public BindingSetup ToggleKey		(KeyCode toggleKey)						{ return (new BindingSetup(null, KeyCode.None, null, toggleKey)); }
		static public BindingSetup ToggleKeyOrAxis	(KeyCode toggleKey, string	toggleAxis)	{ return (new BindingSetup(null, KeyCode.None, toggleAxis, toggleKey)); }

		// --------------------			
		public BindingSetup(string pressAxis = null, KeyCode pressKey = KeyCode.None, string toggleAxis = null, KeyCode toggleKey = KeyCode.None)
			{
			this.pressAxis	= pressAxis;
			this.pressKey	= pressKey;
			this.toggleAxis	= toggleAxis;
			this.toggleKey	= toggleKey;
			}

		// ----------------------
		public void Apply(TouchButtonCreationWizard wizard)
			{

			SetupDigitalBinding(wizard.pressBinding, this.pressAxis, this.pressKey);
			SetupDigitalBinding(wizard.toggleBinding, this.toggleAxis, this.toggleKey);
			
			}
		}

	}




}

#endif
