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
// Trackpad Creation Wizard	
// ----------------------
public class TouchTrackPadCreationWizard : NonDynamicControlWizardBase
	{
	protected DigitalBinding
		pressBinding;
	protected AxisBinding 
		horzDeltaBinding,
		vertDeltaBinding;		

	private DigitalBindingInspector
		pressBindingInsp;
	private AxisBindingInspector 
		horzDeltaBindingInsp,
		vertDeltaBindingInsp;		
	
	private AxisBinding
		touchPressureBinding;
	private AxisBindingInspector
		touchPressureBindingInsp;


	// --------------
	public TouchTrackPadCreationWizard() : base()
		{
		this.minSize = new Vector2(this.minSize.x, 500);

		this.pressBinding = new DigitalBinding();
		this.horzDeltaBinding = new AxisBinding();
		this.vertDeltaBinding = new AxisBinding();

		this.pressBindingInsp = new DigitalBindingInspector(null, new GUIContent("Press binding"));
		this.horzDeltaBindingInsp = new AxisBindingInspector(null, new GUIContent("Horizontal Delta Binding"), false, InputRig.InputSource.TouchDelta);
		this.vertDeltaBindingInsp = new AxisBindingInspector(null, new GUIContent("Vertical Delta Binding"), false, InputRig.InputSource.TouchDelta);

		this.touchPressureBinding = new AxisBinding();
		this.touchPressureBindingInsp = new AxisBindingInspector(null, new GUIContent("Touch Pressure Binding"), false, 
			InputRig.InputSource.Analog, this.DrawPressureBindingExtraGUI);
		}


	// ----------------
	static public void ShowWizard(TouchControlPanel panel, BindingSetup bindignSetup = null, System.Action onCreationCallback = null)
		{
		TouchTrackPadCreationWizard w = (TouchTrackPadCreationWizard)EditorWindow.GetWindow(typeof(TouchTrackPadCreationWizard), true, "CF2 Trackpad Wizard");
		if (w != null)
			{
			w.Init(panel, bindignSetup, onCreationCallback);
			w.ShowPopup();
			}
		}			


		

	// ------------------
	protected void Init(TouchControlPanel panel, BindingSetup bindignSetup, System.Action onCreationCallback)
		{
		base.Init(panel, onCreationCallback);

		this.controlName = TouchControlWizardUtils.GetUniqueTrackPadName(this.panel.rig);
		this.defaultControlName = this.controlName;

		this.defaultSprite = TouchControlWizardUtils.GetDefaultTrackPadSprite();
		this.controlShape = TouchControl.Shape.Rectangle;
		this.positionMode = PositionMode.Stretch;
		this.regionRect	= RegionRectPreset.RightHalf;

		if (bindignSetup != null)
			bindignSetup.Apply(this);
		}

	

	// ------------------
	protected override void DrawHeader ()
		{
		GUILayout.Box(GUIContent.none, CFEditorStyles.Inst.headerTrackPad, GUILayout.ExpandWidth(true));
		}


	

	// ---------------------
	override protected void DrawBindingGUI()
		{
		InspectorUtils.BeginIndentedSection(new GUIContent("Binding Settings"));
		
			this.pressBindingInsp.Draw(this.pressBinding, this.panel.rig);
			this.touchPressureBindingInsp.Draw(this.touchPressureBinding, this.panel.rig);
		
			this.horzDeltaBindingInsp.Draw(this.horzDeltaBinding, this.panel.rig);
			this.vertDeltaBindingInsp.Draw(this.vertDeltaBinding, this.panel.rig);

		InspectorUtils.EndIndentedSection(); 		
		}


	// --------------------
	override protected void OnCreatePressed(bool selectAfterwards)
		{
		TouchTrackPad newObj = (TouchTrackPad)this.CreateNonDynamicTouchControl(typeof(TouchTrackPad));

		newObj.pressBinding.CopyFrom(this.pressBinding);
		newObj.touchPressureBinding.CopyFrom(this.touchPressureBinding);

		if (this.touchPressureBinding.enabled)
			newObj.emulateTouchPressure = this.emulateTouchPressure;

		newObj.horzSwipeBinding.CopyFrom(this.horzDeltaBinding);
		newObj.vertSwipeBinding.CopyFrom(this.vertDeltaBinding);

		if (this.createSpriteAnimator)
			TouchControlWizardUtils.CreateTouchTrackPadAnimator(newObj, "-Sprite", this.defaultSprite, 1.0f);

		Undo.RegisterCreatedObjectUndo(newObj.gameObject, "Create CF2 Trackpad");	
		
		if (selectAfterwards)
			Selection.activeObject =  newObj;	
		}	

	// -------------------------
	// TouchTrackPad Wizard Binding Setup class.
	// ------------------------
	public class BindingSetup
		{
		public string
			pressAxis;
		public KeyCode 
			pressKey;
		public string
			horzDeltaAxis,
			vertDeltaAxis;
		
		// -----------------
		static public BindingSetup HorzAxis			(string horzAxis)						{ return (new BindingSetup(horzAxis)); }
		static public BindingSetup VertAxis			(string vertAxis)						{ return (new BindingSetup(null, vertAxis)); }
		static public BindingSetup PressAxis		(string pressAxis)						{ return (new BindingSetup(null, null, pressAxis)); }
		static public BindingSetup PressKey			(KeyCode pressKey)						{ return (new BindingSetup(null, null, null, pressKey)); }
		static public BindingSetup PressKeyOrAxis	(KeyCode pressKey, string	pressAxis)	{ return (new BindingSetup(null, null, pressAxis, pressKey)); }

		// --------------------			
		public BindingSetup(string horzDeltaAxis = null, string vertDeltaAxis = null, string pressAxis = null, KeyCode pressKey = KeyCode.None)
			{
			this.horzDeltaAxis = horzDeltaAxis;
			this.vertDeltaAxis = vertDeltaAxis;
			this.pressAxis = pressAxis;
			this.pressKey = pressKey;
			}

		// ----------------------
		public void Apply(TouchTrackPadCreationWizard wizard)
			{

			SetupDigitalBinding(wizard.pressBinding, this.pressAxis, this.pressKey);
			SetupAxisBinding(wizard.horzDeltaBinding, this.horzDeltaAxis);
			SetupAxisBinding(wizard.vertDeltaBinding, this.vertDeltaAxis);
			
			}
		}


	}
}

#endif
