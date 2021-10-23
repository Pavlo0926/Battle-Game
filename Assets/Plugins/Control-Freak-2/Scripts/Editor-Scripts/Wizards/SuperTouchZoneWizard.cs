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
// Touch Zone Creation Wizard	
// ----------------------
public class SuperTouchZoneCreationWizard : NonDynamicControlWizardBase
	{
	//public int maxFingers;

	public FingerBinding[] fingerBinding;

	// --------------------
	public class FingerBinding
		{
		public string prefix;
		public SuperTouchZoneCreationWizard wizard;

		public TouchGestureStateBinding 
			gestureBinding;
//		public JoystickStateBinding
//			swipeJoyBinding;

		public TouchGestureStateBindingInspector 
			gestureBindingInsp;


		// --------------------
		public FingerBinding(SuperTouchZoneCreationWizard wizard, string prefix)
			{	
			this.wizard = wizard;
			this.prefix = prefix;

			this.gestureBinding = new TouchGestureStateBinding(null);

			this.gestureBindingInsp	= new TouchGestureStateBindingInspector(null, new GUIContent("Gesture Bindings"));

			}

		
		// ----------------
		public void CopyTo(SuperTouchZone.MultiFingerTouchConfig config)
			{
			config.binding.CopyFrom(this.gestureBinding);

			config.touchConfig.maxTapCount = 
				(this.gestureBinding.doubleTapBinding.enabled || this.gestureBinding.doubleTapMousePosBinding.enabled) ? 2 : 
				(this.gestureBinding.tapBinding.enabled || this.gestureBinding.tapMousePosBinding.enabled) ? 1 : 0;

			config.touchConfig.detectLongPress = 
				this.gestureBinding.longPressBinding.enabled ||
				this.gestureBinding.longPressEmuTouchBinding.enabled ||
				this.gestureBinding.longPressScrollHorzBinding.enabled ||
				this.gestureBinding.longPressScrollVertBinding.enabled ||
				this.gestureBinding.longPressSwipeDirBinding.enabled ||
				this.gestureBinding.longPressSwipeHorzAxisBinding.enabled ||
				this.gestureBinding.longPressSwipeVertAxisBinding.enabled ||
				this.gestureBinding.longTapBinding.enabled ||
				this.gestureBinding.longTapMousePosBinding.enabled;

			config.touchConfig.detectLongTap =
				this.gestureBinding.longTapBinding.enabled ||
				this.gestureBinding.longTapMousePosBinding.enabled;

		

			}


		// ---------------------
		public void Draw()
			{
			InspectorUtils.BeginIndentedSection(new GUIContent(this.prefix + " Binding Setup"));
			
				this.gestureBindingInsp.Draw(this.gestureBinding, null, this.wizard.panel.rig);


			InspectorUtils.EndIndentedSection();
			}


		}



	

	// --------------
	public SuperTouchZoneCreationWizard() : base()
		{
		this.fingerBinding = new FingerBinding[SuperTouchZone.MAX_FINGERS];
		for (int i = 0; i < this.fingerBinding.Length; ++i)
			{
			this.fingerBinding[i] = new FingerBinding(this, ((i + 1).ToString() + "-finger"));
			}					

		}



	// ----------------
	static public void ShowWizard(TouchControlPanel panel, BindingSetup bindingSetup = null, System.Action onCreationCallback = null)
		{
		SuperTouchZoneCreationWizard w = (SuperTouchZoneCreationWizard)EditorWindow.GetWindow(typeof(SuperTouchZoneCreationWizard), true, "CF2 Touch Zone Wizard");
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

		this.positionMode = NonDynamicControlWizardBase.PositionMode.Stretch;

		this.controlName = TouchControlWizardUtils.GetUniqueTouchZoneName(panel.rig);
		this.defaultControlName = this.controlName;

		this.defaultSprite = TouchControlWizardUtils.GetDefaultSuperTouchZoneSprite();
		this.controlShape = TouchControl.Shape.Rectangle;

		if (bindingSetup != null)
			bindingSetup.Apply(this);
		}



	// ------------------
	protected override void DrawHeader ()
		{
		GUILayout.Box(GUIContent.none, CFEditorStyles.Inst.headerTouchZone, GUILayout.ExpandWidth(true));
		}



	// ---------------------
	override protected void DrawBindingGUI()
		{
		InspectorUtils.BeginIndentedSection(new GUIContent("Binding Settings"));
		
			for (int i = 0; i < this.fingerBinding.Length; ++i)
				this.fingerBinding[i].Draw();


		InspectorUtils.EndIndentedSection(); 		
		}


	// --------------------
	override protected void OnCreatePressed(bool selectAfterwards)
		{
		SuperTouchZone newObj = (SuperTouchZone)this.CreateNonDynamicTouchControl(typeof(SuperTouchZone));

		
		newObj.maxFingers = 1;


		for (int i = 0; i < this.fingerBinding.Length; ++i)
			{
			if (this.fingerBinding[i].gestureBinding.enabled) // || this.fingerBinding[i].swipeJoyBinding.enabled)
				newObj.maxFingers = i + 1;

			this.fingerBinding[i].CopyTo(newObj.multiFingerConfigs[i]);			
			}
		


		if (this.createSpriteAnimator)
			TouchControlWizardUtils.CreateSuperTouchZoneAnimator(newObj, "-Sprite", this.defaultSprite, 1.0f);

		Undo.RegisterCreatedObjectUndo(newObj.gameObject, "Create CF2 SuperTouchZone");	
		
		if (selectAfterwards)
			Selection.activeObject =  newObj;	
		}		


	// -------------------------
	// TouchZone Wizard Binding Setup class.
	// ------------------------
	public class BindingSetup
		{
		public int
			fingerNum;
		public string
			pressAxis,
			tapAxis;
		public KeyCode 
			pressKey,
			tapKey;
		public string
			horzDeltaAxis,
			vertDeltaAxis;
		public string
			horzScrollAxis,
			vertScrollAxis;

		public bool
			bindToEmuTouch;

		public MouseBindingTarget
			mouseBindingTarget;

		// --------------------
		public enum MouseBindingTarget		
			{
			None,
			RawPress,
			NormalPress,
			LongPress,
			Tap,
			DoubleTap,
			LongTap,
			NormalPressSwipe,
			LongPressSwipe
			}


		
		// ---------------------
		static public BindingSetup MousePos(int fid, MouseBindingTarget target)	{ return (new BindingSetup(fid, false, target)); }
		static public BindingSetup EmuTouch(int fid)	{ return (new BindingSetup(fid, true)); }

		// -----------------
		private BindingSetup(int fid, bool bindToEmuTouch, MouseBindingTarget mouseTarget)
			{
			this.fingerNum 			= fid;
			this.mouseBindingTarget	= mouseTarget;
			this.bindToEmuTouch		= bindToEmuTouch;
			}
		
		// -----------------
		static public BindingSetup HorzSwipeAxis(int fid, string horzAxis)	{ return (new BindingSetup(fid, true, horzAxis)); }
		static public BindingSetup VertSwipeAxis(int fid, string vertAxis)	{ return (new BindingSetup(fid, true, null, vertAxis)); }
		static public BindingSetup HorzScrollAxis(int fid, string horzAxis)	{ return (new BindingSetup(fid, false, horzAxis)); }
		static public BindingSetup VertScrollAxis(int fid, string vertAxis)	{ return (new BindingSetup(fid, false, null, vertAxis)); }
		static public BindingSetup PressAxis	(int fid, string pressAxis)	{ return (new BindingSetup(fid, true, null, null, pressAxis)); }
		static public BindingSetup PressKey		(int fid, KeyCode pressKey)	{ return (new BindingSetup(fid, true, null, null, null, pressKey)); }
		static public BindingSetup PressKeyOrAxis(int fid, KeyCode pressKey, string	pressAxis)	{ return (new BindingSetup(fid, true, null, null, pressAxis, pressKey)); }
		static public BindingSetup TapKeyOrAxis	(int fid, KeyCode tapKey,	string	tapAxis)	{ return (new BindingSetup(fid, true, null, null, null, KeyCode.None, tapAxis, tapKey)); }

		// --------------------			
		public BindingSetup(int fid, bool swipeAxes, string horzAxis = null, string vertAxis = null, 
			string pressAxis = null, KeyCode pressKey = KeyCode.None, string tapAxis = null, KeyCode tapKey = KeyCode.None)
			{
			this.fingerNum = fid;
			
			if (swipeAxes)
				{
				this.horzDeltaAxis = horzAxis;
				this.vertDeltaAxis = vertAxis;
				}
			else
				{
				this.horzScrollAxis = horzAxis;
				this.vertScrollAxis = vertAxis;
				}

			this.pressAxis = pressAxis;
			this.pressKey = pressKey;
			this.tapAxis = tapAxis;
			this.tapKey = tapKey;
			}

		// ----------------------
		public void Apply(SuperTouchZoneCreationWizard wizard)
			{
			if ((this.fingerNum < 1) || (this.fingerNum > SuperTouchZone.MAX_FINGERS))
				return;

			FingerBinding fb = wizard.fingerBinding[this.fingerNum - 1];
			
			SetupDigitalBinding(fb.gestureBinding.normalPressBinding, this.pressAxis, this.pressKey);
			SetupDigitalBinding(fb.gestureBinding.tapBinding, this.tapAxis, this.tapKey);
			SetupAxisBinding(fb.gestureBinding.normalPressSwipeHorzAxisBinding, this.horzDeltaAxis);
			SetupAxisBinding(fb.gestureBinding.normalPressSwipeVertAxisBinding, this.vertDeltaAxis);
			SetupAxisBinding(fb.gestureBinding.normalPressScrollHorzBinding.deltaBinding, this.horzScrollAxis);
			SetupAxisBinding(fb.gestureBinding.normalPressScrollVertBinding.deltaBinding, this.vertScrollAxis);
	
			if (this.bindToEmuTouch)
				fb.gestureBinding.normalPressEmuTouchBinding.Enable();
			
			switch (this.mouseBindingTarget)
				{
				case MouseBindingTarget.RawPress 			: fb.gestureBinding.rawPressMousePosBinding.Enable(); break;
				case MouseBindingTarget.NormalPress			: fb.gestureBinding.normalPressMousePosBinding.Enable(); break;
				case MouseBindingTarget.LongPress 			: fb.gestureBinding.longPressMousePosBinding.Enable(); break;
				case MouseBindingTarget.Tap 				: fb.gestureBinding.tapMousePosBinding.Enable(); break;
				case MouseBindingTarget.DoubleTap 			: fb.gestureBinding.doubleTapMousePosBinding.Enable(); break;
				case MouseBindingTarget.LongTap 			: fb.gestureBinding.longTapMousePosBinding.Enable(); break;
				case MouseBindingTarget.NormalPressSwipe 	: fb.gestureBinding.normalPressSwipeMousePosBinding.Enable(); break;
				case MouseBindingTarget.LongPressSwipe 		: fb.gestureBinding.longPressSwipeMousePosBinding.Enable(); break;
				}	
			}	
		}

	}


}

#endif
