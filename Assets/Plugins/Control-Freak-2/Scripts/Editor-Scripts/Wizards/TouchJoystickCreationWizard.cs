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
// Joystick Creation Wizard	
// ----------------------
public class TouchJoystickCreationWizard : ControlCreationWizardBase
	{
	private enum AnimatorStyle
		{
		BaseAndHat,
		Simple
		}
	

	private AnimatorStyle
		animatorStyle;

	private Sprite
		hatSprite,
		baseSprite;

	private float
		hatScale = 0.75f;
	
	

	protected DigitalBinding
		pressBinding;
	private DigitalBindingInspector
		pressBindingInsp;

	protected AxisBinding
		horzAxisBinding,
		vertAxisBinding;
	private AxisBindingInspector
		horzAxisBindingInsp,
		vertAxisBindingInsp;
	
	private AxisBinding
		touchPressureBinding;
	private AxisBindingInspector
		touchPressureBindingInsp;



	// -----------------
	public TouchJoystickCreationWizard() : base()
		{
		this.minSize = new Vector2(this.minSize.x, 500);

		this.pressBinding = new DigitalBinding();
		this.pressBindingInsp = new DigitalBindingInspector(null, new GUIContent("Press binding"));

		this.horzAxisBinding	= new AxisBinding();
		this.vertAxisBinding	= new AxisBinding();
		
		this.horzAxisBindingInsp = new AxisBindingInspector(null, new GUIContent("Horizontal Axis Binding"), false, InputRig.InputSource.Analog);
		this.vertAxisBindingInsp = new AxisBindingInspector(null, new GUIContent("Vertical Axis Binding"), false, InputRig.InputSource.Analog);

		this.touchPressureBinding = new AxisBinding();
		this.touchPressureBindingInsp = new AxisBindingInspector(null, new GUIContent("Touch Pressure Binding"), false, 
			InputRig.InputSource.Analog, this.DrawPressureBindingExtraGUI);
		}

	// ----------------
	static public void ShowWizard(TouchControlPanel panel, TouchJoystickCreationWizard.BindingSetup bindingSetup = null, System.Action onCreationCallback = null)
		{
		TouchJoystickCreationWizard w = (TouchJoystickCreationWizard)EditorWindow.GetWindow(typeof(TouchJoystickCreationWizard), true, "CF2 Joystick Wizard");
		if (w != null)
			{
			w.Init(panel, bindingSetup, onCreationCallback);
			w.ShowPopup();
			}
		}			

		

	// ------------------
	protected override void DrawHeader ()
		{
		GUILayout.Box(GUIContent.none, CFEditorStyles.Inst.headerJoystick, GUILayout.ExpandWidth(true));
		}



	// ------------------
	public void Init(TouchControlPanel panel, TouchJoystickCreationWizard.BindingSetup bindingSetup, System.Action onCreationCallback)
		{
		base.Init(panel, onCreationCallback);
		
		this.controlName =  //(((bindingSetup != null) && !string.IsNullOrEmpty(bindingSetup.joyName)) ? ("Joystick-" + bindingSetup.joyName) : 
			TouchControlWizardUtils.GetUniqueJoystickName(panel.rig);
			
		this.defaultControlName = this.controlName;
			
		this.dynamicMode = ControlMode.DynamicWithRegion;
		this.regionRect = RegionRectPreset.LeftHalf;

		this.hatScale = 0.75f;
		this.defaultSprite = TouchControlWizardUtils.GetDefaultDpadSprite();
		this.hatSprite = TouchControlWizardUtils.GetDefaultAnalogJoyHatSprite();	
		this.baseSprite = TouchControlWizardUtils.GetDefaultAnalogJoyBaseSprite();
		
		if (bindingSetup != null)
			bindingSetup.Apply(this);
		}


	// ---------------------
	override protected void DrawBindingGUI()
		{
		InspectorUtils.BeginIndentedSection(new GUIContent("Binding"));

			this.pressBindingInsp.Draw(this.pressBinding, this.panel.rig);
			this.touchPressureBindingInsp.Draw(this.touchPressureBinding, this.panel.rig);
		
			//this.joyNameBindingInsp.Draw(this.joyNameBinding, this.panel.rig);
			
			this.horzAxisBindingInsp.Draw(this.horzAxisBinding, this.panel.rig);
			this.vertAxisBindingInsp.Draw(this.vertAxisBinding, this.panel.rig);

		InspectorUtils.EndIndentedSection(); 		
		}

	
	// ---------------------
	override protected void DrawPresentationGUI()	
		{
		InspectorUtils.BeginIndentedSection(new GUIContent("Presentation"));
		
			this.animatorStyle = (AnimatorStyle)CFGUI.EnumPopup(new GUIContent("Style", "Animator Style"), this.animatorStyle, 		
				ControlCreationWizardBase.MAX_LABEL_WIDTH);
		
			if (this.animatorStyle == AnimatorStyle.BaseAndHat)
				{	
				this.hatScale = CFGUI.Slider(new GUIContent("Hat Scale", "Hat's Scale relative to joystick's size"), this.hatScale, 0.1f, 1.0f,
					ControlCreationWizardBase.MAX_LABEL_WIDTH);
				}

			EditorGUILayout.Space();


			EditorGUILayout.BeginHorizontal(GUILayout.ExpandWidth(true));
		
				EditorGUILayout.Space();
				if (this.animatorStyle == AnimatorStyle.BaseAndHat)
					{	
					this.baseSprite	= DrawSpriteBox(new GUIContent("Base", "Base Sprite for new joystick."), this.baseSprite);
					this.hatSprite	= DrawSpriteBox(new GUIContent("Hat", "Hat sprite for new joystick."), 	this.hatSprite);						
					}
				else
					{
					this.defaultSprite	= DrawSpriteBox(new GUIContent("Sprite", "Sprite for new joystick."), this.defaultSprite);
					}
		
				EditorGUILayout.Space();

			EditorGUILayout.EndHorizontal();

		InspectorUtils.EndIndentedSection();
		
		}


	// --------------------
	override protected void OnCreatePressed(bool selectAfterwards)
		{
		TouchJoystick newObj = (TouchJoystick)this.CreateDynamicTouchControl(typeof(ControlFreak2.TouchJoystick));


		newObj.stickyMode = true;

		newObj.pressBinding.CopyFrom(this.pressBinding);
		newObj.touchPressureBinding.CopyFrom(this.touchPressureBinding);
		
		if (this.touchPressureBinding.enabled)
			newObj.emulateTouchPressure = this.emulateTouchPressure;


		//newObj.joyNameBinding.CopyFrom(this.joyNameBinding);
		newObj.joyStateBinding.horzAxisBinding.CopyFrom(this.horzAxisBinding);
		newObj.joyStateBinding.vertAxisBinding.CopyFrom(this.vertAxisBinding);


		if (this.animatorStyle == AnimatorStyle.Simple)
			TouchControlWizardUtils.CreateTouchJoystickSimpleAnimator(newObj, "-Sprite", this.defaultSprite, 1.0f, 0.0f);
		else
			TouchControlWizardUtils.CreateTouchJoystickAnalogAnimators(newObj, this.baseSprite, this.hatSprite, this.hatScale);


		Undo.RegisterCreatedObjectUndo(newObj.gameObject, "Create CF2 Joystick");	
		
		if (selectAfterwards)
			Selection.activeObject = newObj;	
		}
	

	

	// -------------------------
	// Joystick Wizard Binding Setup class.
	// ------------------------
	public class BindingSetup
		{
		public string
			pressAxis;
		public KeyCode 
			pressKey;
		//public string
		//	joyName;
		public string
			horzAxis,
			vertAxis;

		// -----------------
		//static public BindingSetup JoyName			(string joyName)						{ return (new BindingSetup(joyName)); }
		static public BindingSetup PressAxis		(string pressAxis)						{ return (new BindingSetup(pressAxis)); }
		static public BindingSetup PressKey			(KeyCode pressKey)						{ return (new BindingSetup(null, pressKey)); }
		static public BindingSetup PressKeyOrAxis	(KeyCode pressKey, string pressAxis)	{ return (new BindingSetup(pressAxis, pressKey)); }
		static public BindingSetup HorzAxis			(string horzAxis)						{ return (new BindingSetup(null, KeyCode.None, horzAxis)); }
		static public BindingSetup VertAxis			(string vertAxis)						{ return (new BindingSetup(null, KeyCode.None, null, vertAxis)); }

		// --------------------			
		public BindingSetup(/*string joyName = null,*/ string pressAxis = null, KeyCode pressKey = KeyCode.None, string horzAxis = null, string vertAxis = null)
			{
			//this.joyName = joyName;
			this.pressAxis = pressAxis;
			this.pressKey = pressKey;
			this.horzAxis	= horzAxis;
			this.vertAxis	= vertAxis;
			}

		// ----------------------
		public void Apply(TouchJoystickCreationWizard wizard)
			{

			SetupDigitalBinding(wizard.pressBinding, this.pressAxis, this.pressKey);

			SetupAxisBinding(wizard.horzAxisBinding, this.horzAxis);
			SetupAxisBinding(wizard.vertAxisBinding, this.vertAxis);
			
			}
		}

	
	}



}

#endif
