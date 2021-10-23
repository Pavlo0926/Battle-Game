// -------------------------------------------
// Control Freak 2
// Copyright (C) 2013-2020 Dan's Game Tools
// http://DansGameTools.blogspot.com
// -------------------------------------------

#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

using ControlFreak2Editor;
using ControlFreak2;
using ControlFreak2.Internal;
using ControlFreak2Editor.Internal;

namespace ControlFreak2Editor.Inspectors
{
	


[CustomEditor(typeof(InputRig))]
public class InputRigInspector : Editor
	{
	public InputRig 
		rig;		

	private GeneralSettingsInspector
		generalInsp;

	private AxisListInspector 
		axisListInsp;
	private JoystickListInspector 
		joyListInsp;
	//private KeyCodeRemapListInspector 
	//	keyRemapListInsp;
	private RigSwitchListInspector
		rigSwitchListInsp;
	private BlockedKeyCodeListInspector
		blockedKeyCodeListInsp;
		
	private GamepadsSectionInspector
		gamepadsSectionInspector;

	private AutoInputConfigListInspector
		autoInputListInsp;

	private MouseConfigInspector
		mouseInsp;
	private ScrollWheelConfigInspector
		scrollWheelInsp;
	private TiltConfigInspector
		tiltInsp;

	private bool
		isAdvancedSectionFoldedOut;
		

	// ---------------------
	void OnEnable()
		{
		this.rig = (InputRig)this.target;

		this.generalInsp		= new GeneralSettingsInspector(rig);
		this.axisListInsp		= new AxisListInspector(rig);
		this.joyListInsp		= new JoystickListInspector(rig);
		//this.keyRemapListInsp	= new KeyCodeRemapListInspector(rig, this);
		this.rigSwitchListInsp	= new RigSwitchListInspector(rig);
		this.blockedKeyCodeListInsp = new BlockedKeyCodeListInspector(rig);
		this.mouseInsp 			= new MouseConfigInspector(rig);
		this.scrollWheelInsp	= new ScrollWheelConfigInspector(rig);
		this.tiltInsp			= new TiltConfigInspector(rig);
		this.gamepadsSectionInspector = new GamepadsSectionInspector(rig);
		this.autoInputListInsp = new AutoInputConfigListInspector(rig);

		}


	// --------------------
	private static bool IsRigUsingGamepads(InputRig rig)
		{
		if (rig.anyGamepad.enabled) 
			return true;

		for (int i = 0; i < rig.gamepads.Length; ++i)
			{
			if (rig.gamepads[i].enabled)
				return true;
			}
			
		return false;
		}


	// -------------------
	private void DrawWarnings()
		{
		if ((this.rig == null) || !this.rig.gameObject.activeInHierarchy)
			return;

		if ((GamepadManager.activeManager == null) && IsRigUsingGamepads(this.rig))
			EditorGUILayout.HelpBox("This rig is configured to use gamepad input, but there's no active Gamepad Manager in the scene!", MessageType.Warning); 
	

		if (!CFUtils.forcedMobileModeEnabled && (this.rig.GetTouchControls().Count > 0))
			EditorGUILayout.HelpBox("This rig contains touch controls, but Mobile Mode is not forced on current platform! " + 
				"Run CF2 Installer to enable forced Mobile Mode.", MessageType.Info);
		}

		
	private bool drawDefault;
	// ---------------
	public override void OnInspectorGUI()
		{
		
//		EditorGUILayout.BeginHorizontal();
			GUILayout.Box(GUIContent.none, CFEditorStyles.Inst.headerRig, GUILayout.ExpandWidth(true));


					
		EditorGUILayout.Space();

		this.DrawWarnings();


		this.generalInsp.DrawGUI();

		EditorGUILayout.Space();
		this.axisListInsp.DrawGUI();
			
		if (this.rig.joysticks.list.Count > 0)
			{
			EditorGUILayout.Space();
			this.joyListInsp.DrawGUI();
			}

		EditorGUILayout.Space();
		this.gamepadsSectionInspector.DrawGUI();


		EditorGUILayout.Space();
		this.mouseInsp.DrawGUI();

		EditorGUILayout.Space();
		this.scrollWheelInsp.DrawGUI();

		EditorGUILayout.Space();
		this.tiltInsp.DrawGUI();

		EditorGUILayout.Space();

		
		//EditorGUILayout.Space();
		//this.keyRemapListInsp.DrawGUI();

		if (InspectorUtils.DrawSectionHeader(new GUIContent("Advanced Settings", ""), ref this.isAdvancedSectionFoldedOut))
			{
			CFGUI.BeginIndentedVertical();

			EditorGUILayout.Space();
			this.rigSwitchListInsp.DrawGUI();

			EditorGUILayout.Space();
			this.blockedKeyCodeListInsp.DrawGUI();


			EditorGUILayout.Space();
			this.autoInputListInsp.DrawGUI();

			CFGUI.EndIndentedVertical();
			}

		EditorGUILayout.Space();

		}

		
		

	



	// ----------------------
	// General Settings Inspector.
	// ----------------------
	private class GeneralSettingsInspector
		{
		private bool isFoldedOut;
		private InputRig rig;

	
		// ----------------------
		public GeneralSettingsInspector(InputRig rig) //, Editor mainEditor)
			{
			this.rig = rig;
			}



		// --------------------
		public void DrawGUI()
			{
			bool
				autoActivate						= this.rig.autoActivate,
				overrideActiveRig					= this.rig.overrideActiveRig,
				hideWhenTouchScreenIsUnused	= this.rig.hideWhenTouchScreenIsUnused,
				hideWhenGamepadIsActivated		= this.rig.hideWhenGamepadIsActivated,
				disableWhenDisactivated			= this.rig.disableWhenDisactivated,
				hideWhenDisactivated				= this.rig.hideWhenDisactivated;
			float
				hideWhenTouchScreenIsUnusedDelay	= this.rig.hideWhenTouchScreenIsUnusedDelay,
				hideWhenGamepadIsActivatedDelay		= this.rig.hideWhenGamepadIsActivatedDelay;
			float
				ninetyDegTurnMouseDelta			= this.rig.ninetyDegTurnMouseDelta,
				ninetyDegTurnTouchSwipeInCm		= this.rig.ninetyDegTurnTouchSwipeInCm,
				ninetyDegTurnAnalogDuration		= this.rig.ninetyDegTurnAnalogDuration,
				virtualScreenDiameterInches		= this.rig.virtualScreenDiameterInches;

			//	analogDeltaSpeed			= this.rig.analogDeltaSpeed,
			//	touchCmPerMonitorWidth		= this.rig.touchCmPerMonitorWidth;
			//int 	
			//	mouseTargetHorzRes			= this.rig.mouseTargetHorzRes;
			float
				controlBaseAlphaAnimDuration	= this.rig.controlBaseAlphaAnimDuration,
				animatorMaxAnimDuration			= this.rig.animatorMaxAnimDuration;
			float
				fingerRadiusInCm	= this.rig.fingerRadiusInCm;
			bool	
				swipeOverFromNothing = this.rig.swipeOverFromNothing;				

				

			if (InspectorUtils.DrawSectionHeader(new GUIContent("General Settings", "General Settings"), ref this.isFoldedOut))
				{
				CFGUI.BeginIndentedVertical(InspectorUtils.SectionContentStyle);

				InspectorUtils.BeginIndentedSection(new GUIContent("Activation and Visibility Settings"));

					autoActivate = EditorGUILayout.ToggleLeft(new GUIContent("Auto activate as the Main Rig", "Automatically set as the main rig when loaded, created or enabled."),
						autoActivate, GUILayout.MinWidth(30), GUILayout.ExpandWidth(true));
					
					GUI.enabled = autoActivate;

					overrideActiveRig = EditorGUILayout.ToggleLeft(new GUIContent("Override active rig", "If unchecked, this rig will the auto-activated only if there's no active rig in the scene."),
						overrideActiveRig, GUILayout.MinWidth(30), GUILayout.ExpandWidth(true));

					GUI.enabled = true;

				//InspectorUtils.EndIndentedSection();
				//InspectorUtils.BeginIndentedSection(new GUIContent("Visibility Settings"));
					EditorGUILayout.Space();

					
					disableWhenDisactivated = EditorGUILayout.ToggleLeft(new GUIContent("Disable when disactivated as the Main Rig", "Disable rig's game object when other Input Rig gets activated as CF2Input's active, main rig."),
						disableWhenDisactivated, GUILayout.MinWidth(30), GUILayout.ExpandWidth(true));
		

					GUI.enabled = !disableWhenDisactivated;

					hideWhenDisactivated = EditorGUILayout.ToggleLeft(new GUIContent("Hide when disactivated as the Main Rig", "Hide On-screen touch controls when other Input Rig gets activated as CF2Input's active, main rig."),
						hideWhenDisactivated, GUILayout.MinWidth(30), GUILayout.ExpandWidth(true));

					GUI.enabled = true;

					EditorGUILayout.Space();

					hideWhenTouchScreenIsUnused = EditorGUILayout.ToggleLeft(new GUIContent("Hide after a period of touch screen inactivity", "Hide On-screen touch controls after a period of touch-screen inactivity."),
						hideWhenTouchScreenIsUnused, GUILayout.MinWidth(30), GUILayout.ExpandWidth(true));
						
					CFGUI.BeginIndentedVertical();
						hideWhenTouchScreenIsUnusedDelay = CFGUI.FloatField(new GUIContent("Min. inactivity", "Time (in seconds) of inactivity to hide touch controls."),  
							hideWhenTouchScreenIsUnusedDelay, 1, 10000000, 100);
					CFGUI.EndIndentedVertical();

					hideWhenGamepadIsActivated = EditorGUILayout.ToggleLeft(new GUIContent("Hide when there's gamepad activated", "Hide On-screen touch controls when at least one gamepad is currently activated."),
						hideWhenGamepadIsActivated, GUILayout.MinWidth(30), GUILayout.ExpandWidth(true));
						
					CFGUI.BeginIndentedVertical();
						hideWhenGamepadIsActivatedDelay = CFGUI.FloatField(new GUIContent("Min. inactivity", "Time (in seconds) of inactivity to hide touch controls."),  
							hideWhenGamepadIsActivatedDelay, 1, 10000000, 100);
					CFGUI.EndIndentedVertical();

				InspectorUtils.EndIndentedSection();

					
				InspectorUtils.BeginIndentedSection(new GUIContent("Touch Input Settings"));
					fingerRadiusInCm 	= CFGUI.FloatFieldEx(new GUIContent("Finger radius (cm)", "Touch radius in centimeters."), 
						fingerRadiusInCm, 0, 2, 1, false, 120);	
						
					swipeOverFromNothing = EditorGUILayout.ToggleLeft(new GUIContent("Swipe Over from nothing", "Allow swipe-over controls from empty space on the screen. If enabled, any other UI elements below will not be reachable."),
						swipeOverFromNothing, GUILayout.MinWidth(30), GUILayout.ExpandWidth(true));

				InspectorUtils.EndIndentedSection();



					
				InspectorUtils.BeginIndentedSection(new GUIContent("Timing Settings"));
					
					controlBaseAlphaAnimDuration = CFGUI.FloatFieldEx(new GUIContent("Control Hide Duration", "Time in milliseconds to show and hide touch controls."),
						 controlBaseAlphaAnimDuration, 0.01f, 10, 1000, true, 150);

					animatorMaxAnimDuration = CFGUI.FloatFieldEx(new GUIContent("Animator Ref. Duration.", "Time in milliseconds used by touch control animators as a reference animation duration."),
						 animatorMaxAnimDuration, 0.01f, 10, 1000, true, 150);


				InspectorUtils.EndIndentedSection();					


				InspectorUtils.BeginIndentedSection(new GUIContent("Delta input reference settings"));

					virtualScreenDiameterInches = CFGUI.FloatField(new GUIContent("Virtual Scr. Diameter (Inches)", "Virtual screen diameter in Inches that will be used to calculate fake DPI when testing touch input in editor/webplayer."),
						virtualScreenDiameterInches, 4, 12, 150);
					
					EditorGUILayout.Space();

					ninetyDegTurnMouseDelta = CFGUI.FloatField(new GUIContent("90-Turn Mouse Delta", "Reference unscaled mouse delta (in points) that would turn the camera in a typical FPP game by 90 degrees."),
						ninetyDegTurnMouseDelta, 200, 10000, 150);

					ninetyDegTurnTouchSwipeInCm = CFGUI.FloatField(new GUIContent("90-Turn Swipe Dist. (cm)", "Swipe distance in centimeters that would result in 90 degree turn in a typical FPP game."),
						ninetyDegTurnTouchSwipeInCm, 0.25f, 10, 150);
					
					ninetyDegTurnAnalogDuration = CFGUI.FloatFieldEx(new GUIContent("90-Turn Analog Duration (ms)", "Amount of time (in milliseconds) that would take to turn the camera by 90 degrees with fully tilted analog stick or other analog input source."),
						ninetyDegTurnAnalogDuration, 0.1f, 10.0f, 1000, true, 150);

					
		

		
		
				InspectorUtils.EndIndentedSection();
	
				CFGUI.EndIndentedVertical();
				}



			if ((hideWhenTouchScreenIsUnused			!= this.rig.hideWhenTouchScreenIsUnused) ||
				(hideWhenTouchScreenIsUnusedDelay	!= this.rig.hideWhenTouchScreenIsUnusedDelay) ||
				(hideWhenGamepadIsActivated			!= this.rig.hideWhenGamepadIsActivated) ||
				(hideWhenGamepadIsActivatedDelay		!= this.rig.hideWhenGamepadIsActivatedDelay) ||
				(hideWhenDisactivated					!= this.rig.hideWhenDisactivated) ||
				(disableWhenDisactivated				!= this.rig.disableWhenDisactivated) ||

				(autoActivate								!= this.rig.autoActivate) ||
				(overrideActiveRig						!= this.rig.overrideActiveRig) ||

				//(analogDeltaSpeed						!= this.rig.analogDeltaSpeed) ||
				//(touchCmPerMonitorWidth					!= this.rig.touchCmPerMonitorWidth) ||
				(controlBaseAlphaAnimDuration			!= this.rig.controlBaseAlphaAnimDuration) ||
				(animatorMaxAnimDuration				!= this.rig.animatorMaxAnimDuration) ||
				(fingerRadiusInCm							!= this.rig.fingerRadiusInCm) ||
				(swipeOverFromNothing					!= this.rig.swipeOverFromNothing) ||

				//(mouseTargetHorzRes						!= this.rig.mouseTargetHorzRes) ||
				(ninetyDegTurnMouseDelta				!= this.rig.ninetyDegTurnMouseDelta) ||
				(ninetyDegTurnTouchSwipeInCm			!= this.rig.ninetyDegTurnTouchSwipeInCm) ||
				(ninetyDegTurnAnalogDuration			!= this.rig.ninetyDegTurnAnalogDuration) ||
				(virtualScreenDiameterInches			!= this.rig.virtualScreenDiameterInches)
 
				)
				{
				CFGUI.CreateUndo("Rig Settings Modification", this.rig);
					

				this.rig.autoActivate							= autoActivate;
				this.rig.overrideActiveRig						= overrideActiveRig;
				this.rig.hideWhenTouchScreenIsUnused		= hideWhenTouchScreenIsUnused;
				this.rig.hideWhenTouchScreenIsUnusedDelay	= hideWhenTouchScreenIsUnusedDelay;
				this.rig.hideWhenGamepadIsActivated			= hideWhenGamepadIsActivated;
				this.rig.hideWhenGamepadIsActivatedDelay	= hideWhenGamepadIsActivatedDelay;
				this.rig.hideWhenDisactivated					= hideWhenDisactivated;
				this.rig.disableWhenDisactivated				= disableWhenDisactivated;

				//this.rig.analogDeltaSpeed				= analogDeltaSpeed;
				//this.rig.touchCmPerMonitorWidth			= touchCmPerMonitorWidth;
				//this.rig.mouseTargetHorzRes				= mouseTargetHorzRes;

				this.rig.controlBaseAlphaAnimDuration	= controlBaseAlphaAnimDuration;
				this.rig.animatorMaxAnimDuration		= animatorMaxAnimDuration;
				this.rig.fingerRadiusInCm				= fingerRadiusInCm;
				this.rig.swipeOverFromNothing			= swipeOverFromNothing;

				this.rig.ninetyDegTurnMouseDelta			= ninetyDegTurnMouseDelta;
				this.rig.ninetyDegTurnTouchSwipeInCm		= ninetyDegTurnTouchSwipeInCm;
				this.rig.ninetyDegTurnAnalogDuration		= ninetyDegTurnAnalogDuration;
				this.rig.virtualScreenDiameterInches		= virtualScreenDiameterInches;

				this.rig.RecalcPixelConversionParams();

				CFGUI.EndUndo(this.rig);
				}
	
			}
		}





	// ----------------------
	// Mouse Config Inspector.
	// ----------------------
	private class MouseConfigInspector
		{
		private bool isFoldedOut;
		private InputRig rig;
			
		public AxisBindingInspector
			horzDeltaInsp,
			vertDeltaInsp;

	
		// ----------------------
		public MouseConfigInspector(InputRig rig) //, Editor mainEditor)
			{
			this.rig = rig;

			this.horzDeltaInsp = new AxisBindingInspector(this.rig, new GUIContent("Bind Mouse Horz . Delta", "Bind physical mouse's horizontal delta to an axis."), true, InputRig.InputSource.MouseDelta);
			this.vertDeltaInsp = new AxisBindingInspector(this.rig, new GUIContent("Bind Mouse Vert . Delta", "Bind physical mouse's vertical delta to an axis."), true, InputRig.InputSource.MouseDelta);
			}



		// --------------------
		public void DrawGUI()
			{
			if (InspectorUtils.DrawSectionHeader(new GUIContent("Mouse", "Mouse Configuration"), ref this.isFoldedOut))
				{
				CFGUI.BeginIndentedVertical(InspectorUtils.SectionContentStyle);
					
					
				this.horzDeltaInsp.Draw(this.rig.mouseConfig.horzDeltaBinding, this.rig);
				this.vertDeltaInsp.Draw(this.rig.mouseConfig.vertDeltaBinding, this.rig);



				
				CFGUI.EndIndentedVertical();
				}
			}
		}



	// ----------------------
	// Mouse Scroll Config Inspector.
	// ----------------------
	private class ScrollWheelConfigInspector
		{
		private bool isFoldedOut;
		private InputRig rig;

	
		//public AxisBindingInspector
		//	sourceVertAxisInsp,
		//	sourceHorzAxisInsp;

		public ScrollDeltaBindingInspector
			vertScrollDeltaInsp,
			horzScrollDeltaInsp;

	
		// ----------------------
		public ScrollWheelConfigInspector(InputRig rig) //, Editor mainEditor)
			{
			this.rig = rig;
		

				
			this.vertScrollDeltaInsp = new ScrollDeltaBindingInspector(this.rig, 
				new GUIContent("Bind Vertical/Primary Scroll Wheel Delta", "Bind hardware scroll wheel delta to axis and/or keys."));
			this.horzScrollDeltaInsp = new ScrollDeltaBindingInspector(this.rig, 
				new GUIContent("Bind Horizontal/Secondary Scroll Wheel Delta", "Bind hardware scroll wheel delta to axis and/or keys."));

			}


		// --------------------
		public void DrawGUI()
			{
			if (InspectorUtils.DrawSectionHeader(new GUIContent("Scroll Wheel", "Scroll Wheel Configuration"), ref this.isFoldedOut))
				{
				CFGUI.BeginIndentedVertical();

				EditorGUILayout.HelpBox("Target Scroll Delta Axes will also be used as sources when calling CF2Input.mouseScrollDelta.", UnityEditor.MessageType.Info);
 
					
					this.vertScrollDeltaInsp.Draw(this.rig.scrollWheel.vertScrollDeltaBinding, this.rig);	
					this.horzScrollDeltaInsp.Draw(this.rig.scrollWheel.horzScrollDeltaBinding, this.rig);	


				
				CFGUI.EndIndentedVertical();
				}
			}
		}




	// ----------------------
	// Tilt Config Inspector.
	// ----------------------
	private class TiltConfigInspector
		{
		private bool isFoldedOut;
		private InputRig rig;

		public AxisBindingInspector
			rollInsp,
			pitchInsp;

		public DigitalBindingInspector
			rollLeftBindingInsp,
			rollRightBindingInsp,
			pitchForwardBindingInsp,
			pitchBackwardBindingInsp;
	
		public AnalogConfigInspector
			rollConfigInsp,
			pitchConfigInsp;

//		public HidingRigSwitchSetInspector
//			hidingRigSwitchSetInsp;
		public DisablingConditionSetInspector
			disablingConditionSetInsp;

	
		// ----------------------
		public TiltConfigInspector(InputRig rig) //, Editor mainEditor)
			{
			this.rig = rig;

			this.rollInsp = new AxisBindingInspector(this.rig, 
				new GUIContent("Bind Roll (Analog)", "Bind normalized Roll rotation (side-to-side) an axis."), true, InputRig.InputSource.Analog); //, this.DrawRollBindingExtraGUI);
			this.pitchInsp = new AxisBindingInspector(this.rig, 
				new GUIContent("Bind Pitch (Analog)", "Bind normalized Pitch rotation (front-to-back) an axis."), true, InputRig.InputSource.Analog); //, this.DrawPitchBindingExtraGUI);
				
			this.rollLeftBindingInsp = new DigitalBindingInspector(this.rig,
				new GUIContent("Bind Roll Left (Digital)")); 
			this.rollRightBindingInsp = new DigitalBindingInspector(this.rig,
				new GUIContent("Bind Roll Right (Digital)")); 

			this.pitchForwardBindingInsp = new DigitalBindingInspector(this.rig,
				new GUIContent("Pitch Forward (Digital)")); 
			this.pitchBackwardBindingInsp = new DigitalBindingInspector(this.rig,
				new GUIContent("Pitch Backward (Digital)")); 


			this.rollConfigInsp = new AnalogConfigInspector(this.rig, new GUIContent("Roll Analog Configuration"), false);
			this.pitchConfigInsp = new AnalogConfigInspector(this.rig, new GUIContent("Pitch Analog Configuration"), false);
				
			//this.hidingRigSwitchSetInsp = new HidingRigSwitchSetInspector(this.rig.tilt.disablingSwitchSet, this.rig, this.rig, "Tilt Disabling Switch Set", "Disable");

			this.disablingConditionSetInsp = new DisablingConditionSetInspector(new GUIContent("Tilt Disabling Conditions"), 
				this.rig.tilt.disablingConditions, this.rig, this.rig);
			}




		// --------------------
		public void DrawGUI()
			{
			Vector2 
				analogRange = this.rig.tilt.tiltState.analogRange;
			//Vector2 
			//	deadzone	= this.rig.tilt.tiltState.deadzone;
			
			
			if (InspectorUtils.DrawSectionHeader(new GUIContent("Tilt", "Tilt Configuration"), ref this.isFoldedOut))
				{
				CFGUI.BeginIndentedVertical(InspectorUtils.SectionContentStyle);
					

				//this.hidingRigSwitchSetInsp.DrawGUI();
				this.disablingConditionSetInsp.DrawGUI();

				EditorGUILayout.Space();


				InspectorUtils.BeginIndentedSection(new GUIContent("Tilt to Analog Parameters"));

					//deadzone.x = CFGUI.Slider(new GUIContent("Roll Deadzone", "Roll Tilt angle deadzone in degrees."), deadzone.x, 0, 45, 100); //GUILayout.MinWidth(30), GUILayout.ExpandWidth(true));
					//deadzone.y = CFGUI.Slider(new GUIContent("Pitch Deadzone", "Pitch Tilt angle deadzone in degrees."), deadzone.y, 0, 45, 100); //GUILayout.MinWidth(30), GUILayout.ExpandWidth(true));
					analogRange.x = CFGUI.Slider(new GUIContent("Roll Angle Range", "Range of rotation around Roll axis (side-to-side) in degrees."), analogRange.x, 5, 90, 110); //GUILayout.MinWidth(30), GUILayout.ExpandWidth(true));
					analogRange.y = CFGUI.Slider(new GUIContent("Pitch Angle Range", "Range of rotation around Pitch axis (front-to-back) in degrees."), analogRange.y, 5, 90, 110); //GUILayout.MinWidth(30), GUILayout.ExpandWidth(true));
			
					EditorGUILayout.Space();
								
					this.rollConfigInsp.DrawGUI(this.rig.tilt.rollAnalogConfig);
					

					this.pitchConfigInsp.DrawGUI(this.rig.tilt.pitchAnalogConfig);
					
				InspectorUtils.EndIndentedSection();
				

				InspectorUtils.BeginIndentedSection(new GUIContent("Tilit Binding Settings"));					

					this.rollInsp.Draw(this.rig.tilt.rollBinding, this.rig);
					this.rollLeftBindingInsp.Draw(this.rig.tilt.rollLeftBinding, this.rig);
					this.rollRightBindingInsp.Draw(this.rig.tilt.rollRightBinding, this.rig);

					EditorGUILayout.Space();

					if (this.rig.tilt.pitchBinding.enabled || 
						this.rig.tilt.pitchForwardBinding.enabled ||
						this.rig.tilt.pitchBackwardBinding.enabled)
						{
						EditorGUILayout.HelpBox("Pitch must be calibrated before it can be used!\n\n" +	
							"Ask the player to hold his device in neutral position and call CalibtareTilt() method.", MessageType.Info);
						EditorGUILayout.Space();
						}					


					this.pitchInsp.Draw(this.rig.tilt.pitchBinding, this.rig);
					this.pitchForwardBindingInsp.Draw(this.rig.tilt.pitchForwardBinding, this.rig);
					this.pitchBackwardBindingInsp.Draw(this.rig.tilt.pitchBackwardBinding, this.rig);
					
				InspectorUtils.EndIndentedSection();



				if ((analogRange 	!= this.rig.tilt.tiltState.analogRange) )
	//				(deadzone		!= this.rig.tilt.tiltState.deadzone))
					{
					CFGUI.CreateUndo("Tilt Config Modification", this.rig);
						
					this.rig.tilt.tiltState.analogRange	= analogRange;
 	//				this.rig.tilt.tiltState.deadzone = deadzone;

					CFGUI.EndUndo(this.rig);
					}
				
				CFGUI.EndIndentedVertical();
				}
			}
		}



	// -----------------------
	// Joystick list Inspector.
	// ------------------------
	private class JoystickListInspector : ListInspector
		{
		private InputRig rig;

		// -------------------
		public JoystickListInspector(InputRig rig) : base(rig, rig.joysticks.list)
			{		
			this.rig = rig;	
			this.Rebuild();
			}

		// -------------------
		override public string GetUndoPrefix()								{	return "Rig's Joy List - "; }
		override public GUIContent GetListTitleContent()					{	return (new GUIContent("Virtual Joysticks (" + this.GetElemCount() + ")", "Configure rig's virtual joysticks (Not to be confused with physical joysticks/gamepads!)"));}
		override protected ElemInspector CreateElemInspector(object obj)	{	return new JoystickElemInspector((InputRig.VirtualJoystickConfig)obj, this.rig, this); }

		// -------------------
		override protected void OnNewElemClicked()
			{
			// TODO : show content menu, but for now...

			this.AddNewElem(new InputRig.VirtualJoystickConfig());
			}


			
		// ---------------------
		// Joystick Elem Inspector.		
		// ----------------------	
		private class JoystickElemInspector : ListInspector.ElemInspector
			{
			public InputRig
				rig;
			public InputRig.VirtualJoystickConfig 
				joy;

			public AnalogConfigInspector 
				joyConfig;
			public JoystickStateBindingInspector 
				joyBinding;
				

			// -------------------
			public JoystickElemInspector(InputRig.VirtualJoystickConfig joy, InputRig rig, JoystickListInspector listInsp) : base(listInsp)
				{
				this.joy = joy;
				this.rig = rig;

				this.joyBinding = new JoystickStateBindingInspector(listInsp.undoObject, new GUIContent("Bind Joystick State"));
				this.joyConfig = new AnalogConfigInspector(listInsp.undoObject, new GUIContent("Joy Config"), false);
				}


			// --------------
			override protected GUIContent GetElemTitleContent()
				{
				return (new GUIContent(this.joy.name));
				}

			// --------------
			protected override void DrawGUIContent() //object targetObj)
				{
				string
					name 		= this.joy.name;
				KeyCode
					keyUp		= this.joy.keyboardUp,
					keyRight	= this.joy.keyboardRight,
					keyDown		= this.joy.keyboardDown,
					keyLeft		= this.joy.keyboardLeft;
					


				// Name...
				
				name = EditorGUILayout.TextField(new GUIContent("Name", "Joystick name"), name, GUILayout.MinWidth(30));

				// Keyboard input...

				InspectorUtils.BeginIndentedSection(new GUIContent("Keyboard Input"));
		
					keyUp		= (KeyCode)CFGUI.EnumPopup(new GUIContent("Up", "Up Key Code"),	keyUp, 80); //GUILayout.MinWidth(30));
					keyRight	= (KeyCode)CFGUI.EnumPopup(new GUIContent("Right", "Right Key Code"), keyRight, 80); //GUILayout.MinWidth(30));
					keyDown		= (KeyCode)CFGUI.EnumPopup(new GUIContent("Down", "Down Key Code"), keyDown, 80); //GUILayout.MinWidth(30));
					keyLeft		= (KeyCode)CFGUI.EnumPopup(new GUIContent("Left", "Left Key Code"), keyLeft, 80); //GUILayout.MinWidth(30));
					
				InspectorUtils.EndIndentedSection();
					
				// Joy state and state binding...

				this.joyConfig.DrawGUI(this.joy.joystickConfig);
				this.joyBinding.Draw(this.joy.joyStateBinding, this.rig);
					
				if ((name		!= this.joy.name) ||
					(keyUp		!= this.joy.keyboardUp) ||
					(keyRight	!= this.joy.keyboardRight) ||
					(keyDown	!= this.joy.keyboardDown) ||
					(keyLeft	!= this.joy.keyboardLeft) )
					{
					CFGUI.CreateUndo("Rig Joy Config modification", this.listInsp.undoObject);
						
					this.joy.name			= name;
					this.joy.keyboardUp		= keyUp;
					this.joy.keyboardRight	= keyRight;
					this.joy.keyboardDown	= keyDown;
					this.joy.keyboardLeft	= keyLeft;

					CFGUI.EndUndo(this.listInsp.undoObject);
					}

				}

			}
		}



	// -----------------------
	// Axis list Inspector.
	// ------------------------
	private class AxisListInspector : ListInspector
		{
		private InputRig rig;

		// -------------------
		public AxisListInspector(InputRig rig) : base(rig, rig.axes.list)
			{		
			this.rig = rig;	
			this.Rebuild();
			}

		// -------------------
		override public string GetUndoPrefix()								{	return "Rig Axes - "; }
		override public GUIContent GetListTitleContent()					{	return (new UnityEngine.GUIContent("Virtual Axes (" + this.GetElemCount() + ")"));}
		override protected ElemInspector CreateElemInspector(object obj)	{	return new AxisElemInspector(this.rig, (InputRig.AxisConfig)obj, this); }

		// -------------------
		override protected void OnNewElemClicked()
			{
			// TODO : show content menu, but for now...

			this.AddNewElem(new InputRig.AxisConfig());
			}


			
		// ---------------------
		// Axis Elem Inspector.		
		// ----------------------	
		private class AxisElemInspector : ListInspector.ElemInspector
			{
			public InputRig.AxisConfig target;	
			public InputRig rig;
	
			// -------------------
			public AxisElemInspector(InputRig rig, InputRig.AxisConfig axis, AxisListInspector listInsp) : base(listInsp)
				{
				this.rig = rig;
				this.target = axis;
				}


			// --------------
			override protected GUIContent GetElemTitleContent()
				{
				return (new GUIContent(this.target.name));
				}

			// ---------------
			override protected void DrawExtraHeaderGUI()
				{
				if (GUILayout.Button(new GUIContent(CFEditorStyles.Inst.texWrench, "Bind this axis..."), CFEditorStyles.Inst.iconButtonStyle, GUILayout.Width(ListInspector.BUTTON_WIDTH)))
					WizardMenuUtils.CreateContextMenuForAxisBinding(this.rig, this.target.name, null);
				}

			// --------------
			protected override void DrawGUIContent() //object targetObj)
				{
				string
					name		= this.target.name;
				InputRig.AxisType
					axisType	= this.target.axisType;
				InputRig.DeltaTransformMode
					deltaMode 	= this.target.deltaMode;

					
				bool			
					digitalToScrollAutoRepeat	= this.target.digitalToScrollAutoRepeat;

				float			
					analogToDigitalThresh		= this.target.analogToDigitalThresh,
					digitalToAnalogAccelTime	= this.target.digitalToAnalogAccelTime,
					digitalToAnalogDecelTime	= this.target.digitalToAnalogDecelTime,
					digitalToScrollDelay		= this.target.digitalToScrollDelay,
					digitalToScrollRepeatInterval = this.target.digitalToScrollRepeatInterval,
					rawSmoothingTime			= this.target.rawSmoothingTime,
					smoothingTime				= this.target.smoothingTime,
					scale						= this.target.scale,
					scrollToAnalogSmoothingTime	= this.target.scrollToAnalogSmoothingTime,
					scrollToAnalogStepDuration	= this.target.scrollToAnalogStepDuration,					
					scrollToDeltaSmoothingTime	= this.target.scrollToDeltaSmoothingTime;

				//bool
				//	useRawMouseDelta		= this.target.useRawMouseDelta;
					
				bool
					affectSourceKeys		= this.target.affectSourceKeys,
					snap					= this.target.snap; 

				KeyCode
					keyboardNegative = this.target.keyboardNegative,
					keyboardPositive = this.target.keyboardPositive,	
					keyboardNegativeAlt0 = this.target.keyboardNegativeAlt0,
					keyboardNegativeAlt1 = this.target.keyboardNegativeAlt1,
					keyboardNegativeAlt2 = this.target.keyboardNegativeAlt2,
					keyboardPositiveAlt0 = this.target.keyboardPositiveAlt0,
					keyboardPositiveAlt1 = this.target.keyboardPositiveAlt1,
					keyboardPositiveAlt2 = this.target.keyboardPositiveAlt2,
		
					affectedKeyNegative	= this.target.affectedKeyNegative,
					affectedKeyPositive	= this.target.affectedKeyPositive;	

	
				name = CFGUI.TextField(new GUIContent("Name", "Axis name"), name, 90); //GUILayout.MinWidth(30), GUILayout.ExpandWidth(true));

				axisType = (InputRig.AxisType)CFGUI.EnumPopup(new GUIContent("Type", "Axis type"), axisType, 90); // GUILayout.MinWidth(30), GUILayout.ExpandWidth(true));
				
				if (axisType == InputRig.AxisType.Delta)
					{
					deltaMode = (InputRig.DeltaTransformMode)CFGUI.EnumPopup(new GUIContent("Delta Mode", "Controls how delta input is transformed.\n\nCheck the 'General Settings' section for parameters controlling delta transformation..."), 
						deltaMode, 90); // GUILayout.MinWidth(30), GUILayout.ExpandWidth(true));
				
					//useRawMouseDelta = EditorGUILayout.ToggleLeft(new GUIContent("Use Raw Mouse", "When enabled, mouse delta input passed to this axis wil not be transformed.\nEnable this is scripts using this axis need real mouse delta..."),
					//	useRawMouseDelta, GUILayout.MinWidth(30), GUILayout.ExpandWidth(true));
					}
					

				
				InspectorUtils.BeginIndentedSection(new GUIContent("Keyboard Input"));
					
				if ((axisType == InputRig.AxisType.UnsignedAnalog) || (axisType == InputRig.AxisType.Digital))
					{
					keyboardPositive 	= (KeyCode)CFGUI.EnumPopup(new GUIContent("Key", "Key"), 					keyboardPositive, 90); //,  GUILayout.MinWidth(30), GUILayout.ExpandWidth(true));
					keyboardPositiveAlt0 = (KeyCode)CFGUI.EnumPopup(new GUIContent("Key (Alt)", "Alternative Key"),	keyboardPositiveAlt0, 90); //, GUILayout.MinWidth(30), GUILayout.ExpandWidth(true));
					keyboardPositiveAlt1 = (KeyCode)CFGUI.EnumPopup(new GUIContent("Key (Alt)", "Alternative Key"),	keyboardPositiveAlt1, 90); //, GUILayout.MinWidth(30), GUILayout.ExpandWidth(true));
					keyboardPositiveAlt2 = (KeyCode)CFGUI.EnumPopup(new GUIContent("Key (Alt)", "Alternative Key"),	keyboardPositiveAlt2, 90); //, GUILayout.MinWidth(30), GUILayout.ExpandWidth(true));
					}	
				else
					{
					keyboardPositive 		= (KeyCode)CFGUI.EnumPopup(new GUIContent("Pos. Key", "Positive Key"), 						keyboardPositive, 90); //, GUILayout.MinWidth(30), GUILayout.ExpandWidth(true));
					keyboardPositiveAlt0 = (KeyCode)CFGUI.EnumPopup(new GUIContent("Pos. Key (Alt)", "Alternative Positive Key"),	keyboardPositiveAlt0, 90); //, GUILayout.MinWidth(30), GUILayout.ExpandWidth(true));
					keyboardPositiveAlt1 = (KeyCode)CFGUI.EnumPopup(new GUIContent("Pos. Key (Alt)", "Alternative Positive Key"),	keyboardPositiveAlt1, 90); //, GUILayout.MinWidth(30), GUILayout.ExpandWidth(true));
					keyboardPositiveAlt2 = (KeyCode)CFGUI.EnumPopup(new GUIContent("Pos. Key (Alt)", "Alternative Positive Key"),	keyboardPositiveAlt2, 90); //, GUILayout.MinWidth(30), GUILayout.ExpandWidth(true));
					keyboardNegative 		= (KeyCode)CFGUI.EnumPopup(new GUIContent("Neg. Key", "Negative Key"),						keyboardNegative, 90); //, GUILayout.MinWidth(30), GUILayout.ExpandWidth(true));
					keyboardNegativeAlt0 = (KeyCode)CFGUI.EnumPopup(new GUIContent("Neg. Key (Alt)", "Alternative Negative Key"),	keyboardNegativeAlt0, 90); //, GUILayout.MinWidth(30), GUILayout.ExpandWidth(true));
					keyboardNegativeAlt1 = (KeyCode)CFGUI.EnumPopup(new GUIContent("Neg. Key (Alt)", "Alternative Negative Key"),	keyboardNegativeAlt1, 90); //, GUILayout.MinWidth(30), GUILayout.ExpandWidth(true));
					keyboardNegativeAlt2 = (KeyCode)CFGUI.EnumPopup(new GUIContent("Neg. Key (Alt)", "Alternative Negative Key"),	keyboardNegativeAlt2, 90); //, GUILayout.MinWidth(30), GUILayout.ExpandWidth(true));
					}
				InspectorUtils.EndIndentedSection();

				if ((axisType == InputRig.AxisType.UnsignedAnalog) || (axisType == InputRig.AxisType.SignedAnalog) || (axisType == InputRig.AxisType.Digital))
					{
					InspectorUtils.BeginIndentedSection(new GUIContent("Affected Keys", "Virtual Keycodes affected by this axis.")); 
					
					affectSourceKeys = EditorGUILayout.ToggleLeft(new GUIContent("Affect Source Keys", "Affect (primary) source key codes."), 
						affectSourceKeys);

					if (!affectSourceKeys)
						{ 
						if ((axisType == InputRig.AxisType.UnsignedAnalog) || (axisType == InputRig.AxisType.Digital))
							{
							affectedKeyPositive 	= (KeyCode)CFGUI.EnumPopup(new GUIContent("Key", "Key"), 				affectedKeyPositive, 90); //,  GUILayout.MinWidth(30), GUILayout.ExpandWidth(true));
							}	
						else
							{
							affectedKeyPositive 	= (KeyCode)CFGUI.EnumPopup(new GUIContent("Pos. Key", "Positive Key"), 	affectedKeyPositive, 90); //, GUILayout.MinWidth(30), GUILayout.ExpandWidth(true));
							affectedKeyNegative 	= (KeyCode)CFGUI.EnumPopup(new GUIContent("Neg. Key", "Negative Key"),	affectedKeyNegative, 90); //, GUILayout.MinWidth(30), GUILayout.ExpandWidth(true));
							}
						}

					InspectorUtils.EndIndentedSection(); 
					}


				//if (this.target.axisType != InputRig.AxisType.Digital)
					{
					InspectorUtils.BeginIndentedSection(new GUIContent("Smoothing And Thresholds"));
					
					if ((this.target.axisType != InputRig.AxisType.ScrollWheel) && (this.target.axisType != InputRig.AxisType.Digital))
						{
						scale = CFGUI.FloatField(new GUIContent("Scale", "Returned Axis Value Scale."),
							scale, -10000, 10000, 120); //GUILayout.MinWidth(30), GUILayout.ExpandWidth(true));
							
						snap = EditorGUILayout.ToggleLeft(new GUIContent("Snap", "Snap digital-to-analog value to zero whenever opposite digital source is activated."), 
							snap, GUILayout.MinWidth(30), GUILayout.ExpandWidth(true));

						smoothingTime = CFGUI.FloatFieldEx(new GUIContent("Sm. Time (ms)", "Smoothing time used for smoothing the final axis value later returned by GetAxis()."), 
							smoothingTime, 0, 10, 1000, true, 120); 
						rawSmoothingTime = CFGUI.FloatFieldEx(new GUIContent("Raw Sm. Time (ms)", "Smoothing time used for smoothing RAW axis value later returned by GetRawAxis().\nIt's recommended to use at least minimal raw smoothing, especially for DELTA axis type."), 
							rawSmoothingTime, 0, 10, 1000, true, 120); 
	
						digitalToAnalogAccelTime = CFGUI.FloatFieldEx(new GUIContent("Digital accel. (ms)", "Time to reach maximal analog value starting from zero when usign digital inut source."),
							digitalToAnalogAccelTime, 0, 10, 1000, true, 120); 
						digitalToAnalogDecelTime = CFGUI.FloatFieldEx(new GUIContent("Digital decel. (ms)", "Time to get from maximal analog value back to zero, when digital input source is released."),
							digitalToAnalogDecelTime, 0, 10, 1000, true, 120); 
						}

					analogToDigitalThresh = CFGUI.Slider(new GUIContent("Digital thresh.", "Threshold used to convert analog input sources to digital state."),
						analogToDigitalThresh, 0, 1, 100); //GUILayout.MinWidth(30), GUILayout.ExpandWidth(true));


					if ((this.target.axisType == InputRig.AxisType.UnsignedAnalog) || (this.target.axisType == InputRig.AxisType.SignedAnalog))
						{
						scrollToAnalogStepDuration = CFGUI.FloatFieldEx(new GUIContent("Scroll To Analog Step Dur.", "Scroll To Analog Step Duration in milliseconds. Each scroll step applied to this Analog axis will turn on this axis to 100% for amount of time specified here."),
							scrollToAnalogStepDuration, 0, 1, 1000, true, 160); //GUILayout.MinWidth(30), GUILayout.ExpandWidth(true));
						scrollToAnalogSmoothingTime = CFGUI.FloatFieldEx(new GUIContent("Scroll To Analog Sm.", "Scroll To Analog Smoothing Time..."),
							scrollToAnalogSmoothingTime, 0, 1, 1000, true, 160); //GUILayout.MinWidth(30), GUILayout.ExpandWidth(true));

						}

					if (this.target.axisType == InputRig.AxisType.Delta)
						{
						scrollToDeltaSmoothingTime = CFGUI.FloatFieldEx(new GUIContent("Scroll To Delta Sm.", "Scroll To Delta Smoothing Time..."),
							scrollToDeltaSmoothingTime, 0, 1, 1000, true, 160); //GUILayout.MinWidth(30), GUILayout.ExpandWidth(true));
						}



					if (this.target.axisType == InputRig.AxisType.ScrollWheel)
						{
						
						digitalToScrollAutoRepeat = EditorGUILayout.ToggleLeft(new GUIContent("Digital/Analog Auto Scroll", "Automatically scroll this axis while holding digital/analog source."), 
							digitalToScrollAutoRepeat, GUILayout.MinWidth(30), GUILayout.ExpandWidth(true));

						GUI.enabled = digitalToScrollAutoRepeat;

						digitalToScrollDelay = CFGUI.FloatFieldEx(new GUIContent("Scroll Delay (ms)", "Delay in milliseconds after which emulated scroll value should start changing when sourcing from digital or analog sources..."),
							digitalToScrollDelay, 0.1f, 1000, 1000, true, 120);
						digitalToScrollRepeatInterval = CFGUI.FloatFieldEx(new GUIContent("Scroll Interval (ms)", "Interval in milliseconds between emulated increments of scroll value when sourcingt from digital or analog sources..."),
							digitalToScrollRepeatInterval, 0.1f, 1000, 1000, true, 120);

						GUI.enabled = true;
								
						}

					InspectorUtils.EndIndentedSection();
					}


		
					
				if ((name					!= this.target.name) ||
					(axisType				!= this.target.axisType) ||
					(deltaMode 				!= this.target.deltaMode) ||
					(analogToDigitalThresh	!= this.target.analogToDigitalThresh) ||
					(digitalToAnalogAccelTime!= this.target.digitalToAnalogAccelTime) ||
					(digitalToAnalogDecelTime!= this.target.digitalToAnalogDecelTime) ||
					(rawSmoothingTime		!= this.target.rawSmoothingTime) ||
					(smoothingTime			!= this.target.smoothingTime) ||
					(scale					!= this.target.scale) ||
					(snap					!= this.target.snap) ||
					(scrollToAnalogSmoothingTime	!= this.target.scrollToAnalogSmoothingTime) ||
					(scrollToAnalogStepDuration		!= this.target.scrollToAnalogStepDuration) ||
					(scrollToDeltaSmoothingTime		!= this.target.scrollToDeltaSmoothingTime) ||
					//(useRawMouseDelta		!= this.target.useRawMouseDelta) ||
					(digitalToScrollDelay	!= this.target.digitalToScrollDelay) ||
					(digitalToScrollRepeatInterval	!= this.target.digitalToScrollRepeatInterval) ||
					(digitalToScrollAutoRepeat		!= this.target.digitalToScrollAutoRepeat) ||
					(affectSourceKeys		!= this.target.affectSourceKeys) ||
					(affectedKeyNegative 	!= this.target.affectedKeyNegative) ||
					(affectedKeyPositive 	!= this.target.affectedKeyPositive) ||
					(keyboardNegative 		!= this.target.keyboardNegative) ||
					(keyboardNegativeAlt0	!= this.target.keyboardNegativeAlt0) ||
					(keyboardNegativeAlt1	!= this.target.keyboardNegativeAlt1) ||
					(keyboardNegativeAlt2	!= this.target.keyboardNegativeAlt2) ||
					(keyboardPositive 		!= this.target.keyboardPositive) ||
					(keyboardPositiveAlt0	!= this.target.keyboardPositiveAlt0) ||
					(keyboardPositiveAlt1	!= this.target.keyboardPositiveAlt1) ||
					(keyboardPositiveAlt2	!= this.target.keyboardPositiveAlt2) )
					{
						
					CFGUI.CreateUndo("Rig Axis Modification", this.rig);

					this.target.name					= name;
					this.target.axisType				= axisType;
					this.target.deltaMode 				= deltaMode;
					this.target.analogToDigitalThresh	= analogToDigitalThresh;
					this.target.digitalToAnalogAccelTime= digitalToAnalogAccelTime;
					this.target.digitalToAnalogDecelTime= digitalToAnalogDecelTime;
					this.target.digitalToScrollDelay 	= digitalToScrollDelay;
					this.target.digitalToScrollRepeatInterval 	= digitalToScrollRepeatInterval;
					this.target.digitalToScrollAutoRepeat		= digitalToScrollAutoRepeat;

					this.target.rawSmoothingTime		= rawSmoothingTime;
					this.target.smoothingTime			= smoothingTime;
					this.target.scale					= scale;
					this.target.snap					= snap;
					this.target.scrollToAnalogSmoothingTime	= scrollToAnalogSmoothingTime;
					this.target.scrollToAnalogStepDuration	= scrollToAnalogStepDuration;
					this.target.scrollToDeltaSmoothingTime	= scrollToDeltaSmoothingTime;
					//this.target.useRawMouseDelta		= useRawMouseDelta;
					this.target.keyboardNegative 		= keyboardNegative;
					this.target.keyboardNegativeAlt0	= keyboardNegativeAlt0;
					this.target.keyboardNegativeAlt1	= keyboardNegativeAlt1;
					this.target.keyboardNegativeAlt2	= keyboardNegativeAlt2;
					this.target.keyboardPositive 		= keyboardPositive;
					this.target.keyboardPositiveAlt0	= keyboardPositiveAlt0;
					this.target.keyboardPositiveAlt1	= keyboardPositiveAlt1;
					this.target.keyboardPositiveAlt2	= keyboardPositiveAlt2;

					this.target.affectSourceKeys		= affectSourceKeys;
					this.target.affectedKeyNegative 	= affectedKeyNegative;
					this.target.affectedKeyPositive 	= affectedKeyPositive;


					CFGUI.EndUndo(this.rig);
					}	
					
	
				}

			}
		}

		
	// ------------------------
	// Gamepads Section Inspector
	// ------------------------
	private class GamepadsSectionInspector
		{
		private InputRig rig;
			
		public bool isFoldedOut;

		private List<GamepadConfigInspector>
			gamepadInspectors;			


		public AnalogConfigInspector
			leftStickConfig,
			rightStickConfig,
			dpadConfig,
			leftTriggerConfig,
			rightTiggerConfig;




		
		// ------------------
		public GamepadsSectionInspector(InputRig rig)
			{
			this.rig = rig;
			this.isFoldedOut = false;

			this.gamepadInspectors = new List<GamepadConfigInspector>(5);
			
			this.gamepadInspectors.Add(new GamepadConfigInspector(rig, rig.anyGamepad, new GUIContent("Combined Gamepad State", "Configure input bindings for combined state of all connected gamepads.")) );
				
			for (int i = 0; i < this.rig.gamepads.Length; ++i)
				{
				this.gamepadInspectors.Add(new GamepadConfigInspector(rig, rig.gamepads[i], 
					new GUIContent("Gamepad #" + (i + 1), "Configure input bindings for a gamepad connected to port #" + (i + 1) + ".")) );
				}		


			this.leftStickConfig 	= new AnalogConfigInspector(rig, new GUIContent("Left Stick Configuartion"));
			this.rightStickConfig 	= new AnalogConfigInspector(rig, new GUIContent("Right Stick Configuartion"));
			this.dpadConfig 		= new AnalogConfigInspector(rig, new GUIContent("D-Pad Configuartion"));
			this.leftTriggerConfig 	= new AnalogConfigInspector(rig, new GUIContent("Left Analog Trigger Configuartion"));
			this.rightTiggerConfig 	= new AnalogConfigInspector(rig, new GUIContent("Right Analog Trigger Configuartion"));
	
			}


		// --------------------
		public void DrawGUI()
			{

			if (InspectorUtils.DrawSectionHeader(new GUIContent("Gamepad Settings"), ref this.isFoldedOut))
				{
				EditorGUILayout.HelpBox("To enable gamepad input, Control Freak Gamepad Manager must be present in the scene.", MessageType.Info);

				InspectorUtils.BeginIndentedSection(new GUIContent("Analog Settings"));
		
					this.leftStickConfig	.DrawGUI(this.rig.leftStickConfig);
					this.rightStickConfig	.DrawGUI(this.rig.rightStickConfig);
					this.dpadConfig			.DrawGUI(this.rig.dpadConfig);
					this.leftTriggerConfig	.DrawGUI(this.rig.leftTriggerAnalogConfig);
					this.rightTiggerConfig	.DrawGUI(this.rig.rightTriggerAnalogConfig);
		
				InspectorUtils.EndIndentedSection();


				//CFGUI.BeginIndentedVertical();
				InspectorUtils.BeginIndentedSection(new GUIContent("Gamepad Bindings"));
					
					for (int i = 0; i < this.gamepadInspectors.Count; ++i)
						this.gamepadInspectors[i].Draw();

				InspectorUtils.EndIndentedSection();
				//CFGUI.EndIndentedVertical();
				}

			}

		// -----------------
		private class GamepadConfigInspector
			{
			private InputRig	
				rig;
			private InputRig.GamepadConfig	
				target;
			private GUIContent
				titleContent;
			public bool
				isFoldedOut;


			public DigitalBindingInspector
				digiFaceDownBinding,
				digiFaceRightBinding,
				digiFaceLeftBinding,
				digiFaceUpBinding,
				digiStartBinding,	
				digiSelectBinding,
				digiL1Binding,
				digiR1Binding,
				digiL2Binding,
				digiR2Binding,
				digiL3Binding,
				digiR3Binding;
	
			public AxisBindingInspector
				analogL2Binding,
				analogR2Binding;
	
			//public JoystickNameBindingInspector
			//	leftStickJoyBinding,
			//	rightStickJoyBinding,
			//	dpadJoyBinding;
	
			public JoystickStateBindingInspector
				leftStickStateBinding,
				rightStickStateBinding,
				dpadStateBinding;

			// ---------------------
			public GamepadConfigInspector(InputRig rig, InputRig.GamepadConfig target, GUIContent titleContent )
				{
				this.rig			= rig;
				this.target			= target;
				this.titleContent	= titleContent;
				this.isFoldedOut	= false;

				this.digiFaceDownBinding	= new DigitalBindingInspector(rig, new GUIContent("Bind Bottom Face Button"));
				this.digiFaceRightBinding	= new DigitalBindingInspector(rig, new GUIContent("Bind Right Face Button"));
				this.digiFaceLeftBinding	= new DigitalBindingInspector(rig, new GUIContent("Bind Left Face Button"));
				this.digiFaceUpBinding		= new DigitalBindingInspector(rig, new GUIContent("Bind Top Face Button"));
				this.digiStartBinding		= new DigitalBindingInspector(rig, new GUIContent("Bind START Button"));
				this.digiSelectBinding		= new DigitalBindingInspector(rig, new GUIContent("Bind SELECT Button"));
				this.digiL1Binding			= new DigitalBindingInspector(rig, new GUIContent("Bind L1 Button"));
				this.digiR1Binding			= new DigitalBindingInspector(rig, new GUIContent("Bind R1 Button"));
				this.digiL2Binding			= new DigitalBindingInspector(rig, new GUIContent("Bind L2 Button"));
				this.digiR2Binding			= new DigitalBindingInspector(rig, new GUIContent("Bind R2 Button"));
				this.digiL3Binding			= new DigitalBindingInspector(rig, new GUIContent("Bind L3 Button"));
				this.digiR3Binding			= new DigitalBindingInspector(rig, new GUIContent("Bind R3 Button"));
				
				this.analogL2Binding		= new AxisBindingInspector(rig, new GUIContent("Bind L2 Analog", "Bind Left analog trigger to an axis or a key"), false, InputRig.InputSource.Analog);
				this.analogR2Binding		= new AxisBindingInspector(rig, new GUIContent("Bind R2 Analog", "Bind Right analog trigger to an axis or a key"), false, InputRig.InputSource.Analog);

			
				this.leftStickStateBinding	= new JoystickStateBindingInspector(rig, new GUIContent("Bind Left Stick's State", "Bind joystick's state to axes and/or keys directly."));
				this.rightStickStateBinding	= new JoystickStateBindingInspector(rig, new GUIContent("Bind Right Stick's State", "Bind joystick's state to axes and/or keys directly."));
				this.dpadStateBinding		= new JoystickStateBindingInspector(rig, new GUIContent("Bind DPAD's State", "Bind joystick's state to axes and/or keys directly."));				
				}

				 
			// -----------------------
			public void Draw()
				{
				
				bool enabled = this.target.enabled;
					

				EditorGUILayout.BeginHorizontal(CFEditorStyles.Inst.transpBevelBG);
					this.isFoldedOut = GUILayout.Toggle(this.isFoldedOut, this.titleContent, CFEditorStyles.Inst.foldout, GUILayout.MinWidth(30), GUILayout.ExpandWidth(true));
					enabled = GUILayout.Toggle(enabled, new GUIContent("", "Enable this gamepad config."), GUILayout.Width(20));
				EditorGUILayout.EndHorizontal();

				if (this.isFoldedOut)
					{
					InspectorUtils.BeginIndentedSection();

					bool guiEnabled = GUI.enabled;
					GUI.enabled = enabled;
			
					// Joystick and dpad bindings...

					InspectorUtils.BeginIndentedSection(new GUIContent("Sticks and D-Pad"));
				
						//this.leftStickJoyBinding	.Draw(this.target.leftStickJoyBinding, this.rig);
						this.leftStickStateBinding	.Draw(this.target.leftStickStateBinding, this.rig);

						EditorGUILayout.Space();
	
						//this.rightStickJoyBinding	.Draw(this.target.rightStickJoyBinding, this.rig);
						this.rightStickStateBinding	.Draw(this.target.rightStickStateBinding, this.rig);

						EditorGUILayout.Space();
							
						//this.dpadJoyBinding			.Draw(this.target.dpadJoyBinding, this.rig);
						this.dpadStateBinding		.Draw(this.target.dpadStateBinding, this.rig);

					InspectorUtils.EndIndentedSection();

					// Key bindings...

					InspectorUtils.BeginIndentedSection(new GUIContent("Face buttons"));
	
						this.digiFaceDownBinding	.Draw(this.target.digiFaceDownBinding, this.rig);
						this.digiFaceRightBinding	.Draw(this.target.digiFaceRightBinding, this.rig);
						this.digiFaceLeftBinding	.Draw(this.target.digiFaceLeftBinding, this.rig);
						this.digiFaceUpBinding		.Draw(this.target.digiFaceUpBinding, this.rig);

						EditorGUILayout.Space();
	
						this.digiStartBinding		.Draw(this.target.digiStartBinding, this.rig);
						this.digiSelectBinding		.Draw(this.target.digiSelectBinding, this.rig);

					InspectorUtils.EndIndentedSection();
						


					InspectorUtils.BeginIndentedSection(new GUIContent("Triggers"));

						this.digiL1Binding			.Draw(this.target.digiL1Binding, this.rig);
						this.digiR1Binding			.Draw(this.target.digiR1Binding, this.rig);

						EditorGUILayout.Space();

						this.digiL2Binding			.Draw(this.target.digiL2Binding, this.rig);
						this.digiR2Binding			.Draw(this.target.digiR2Binding, this.rig);

						EditorGUILayout.Space();

						this.digiL3Binding			.Draw(this.target.digiL3Binding, this.rig);
						this.digiR3Binding			.Draw(this.target.digiR3Binding, this.rig);
	
						EditorGUILayout.Space();

						this.analogL2Binding		.Draw(this.target.analogL2Binding, this.rig);			
						this.analogR2Binding		.Draw(this.target.analogR2Binding, this.rig);			

					InspectorUtils.EndIndentedSection();

	

					GUI.enabled = guiEnabled;
					
					InspectorUtils.EndIndentedSection();
					}
		

		
				// Undo...

				if ((enabled != this.target.enabled))
					{
					CFGUI.CreateUndo("Input Rig's Gamepad Config modification", this.rig);
						
					this.target.enabled = enabled;

					CFGUI.EndUndo(this.rig);
					}

				

				}
			}
		}


	// -----------------------
	// Blocked Keycodes list Inspector.
	// ------------------------
	private class BlockedKeyCodeListInspector : ListInspector
		{
		private InputRig rig;

		// -------------------
		public BlockedKeyCodeListInspector(InputRig rig) : base(rig, rig.keyboardBlockedCodes)
			{		
			this.rig = rig;	
			this.Rebuild();
			}

		// -------------------
		override public string GetUndoPrefix()								{	return "Rig's Blocked Keys - "; }
		override public GUIContent GetListTitleContent()					{	return (new UnityEngine.GUIContent("Blocked Hardware Key Codes (" + this.GetElemCount() + ")", "GetKey() combines InputRig's virtual keys with hardware keys. Use this list to block hardware keys..."));}
		override protected ElemInspector CreateElemInspector(object obj)	{	return new KeyCodeElemInspector((KeyCode)obj, this.rig, this); }

		// -------------------
		override protected void OnNewElemClicked()
			{
			// TODO : show content menu, but for now...

			this.AddNewElem((KeyCode.None));
			}


		
		// ---------------------
		// KeyCode Elem Inspector.		
		// ----------------------	
		private class KeyCodeElemInspector : ListInspector.ElemInspector
			{
			public InputRig
				rig;
			//public InputRig.KeyCodeRemap 
			//	keyRemap;

							

			// -------------------
			public KeyCodeElemInspector(KeyCode keycode, InputRig rig, BlockedKeyCodeListInspector listInsp) : base(listInsp)
				{
				//this.keyRemap = keyRemap;
				this.rig = rig;

				}


			// --------------
			override protected GUIContent GetElemTitleContent()
				{
				return (GUIContent.none); //new GUIContent(this.keyRemap.nativeKeyCode.ToString()));
				}

			// ---------------
			override public bool IsFoldable()
				{
				return false;
				}

			// ----------------
			override protected void DrawGUIContent()
				{
				}

			// --------------
			override public bool DrawGUI() //object targetObj)
				{
				bool retVal = true;

				KeyCode keycode = this.rig.keyboardBlockedCodes[this.elemId];
				

				EditorGUILayout.BeginHorizontal(CFEditorStyles.Inst.transpBevelBG);

				// Keyboard input...

		//		EditorGUILayout.LabelField(new GUIContent("Keyboard Input"));
		//		CFGUI.BeginIndentedVertical(CFEditorStyles.Inst.transpSunkenBG);
			
					EditorGUILayout.LabelField(new GUIContent("KeyCode", "KeyCode to block."), GUILayout.Width(60));


					keycode = (KeyCode)EditorGUILayout.EnumPopup(new GUIContent("", "KeyCode to block."), keycode, GUILayout.MinWidth(30), GUILayout.ExpandWidth(true));

					GUILayout.Space(10);
					
		//		CFGUI.EndIndentedVertical();
					
					
				if (!this.DrawDefaultButtons(false, true))
					retVal = false;
					

				EditorGUILayout.EndHorizontal();

					
				if (retVal)
					{
					if ((keycode	!= this.rig.keyboardBlockedCodes[this.elemId]) )
						{
						CFGUI.CreateUndo("Blocked Keycode modification", this.listInsp.undoObject);
	
						this.rig.keyboardBlockedCodes[this.elemId] = keycode;

						CFGUI.EndUndo(this.listInsp.undoObject);
						}
					}

				return retVal;
				}

			}
		}


	// -----------------------
	// Rig Switches list Inspector.
	// ------------------------
	private class RigSwitchListInspector : ListInspector
		{
		private InputRig rig;

		// -------------------
		public RigSwitchListInspector(InputRig rig) : base(rig, rig.rigSwitches.list)
			{		
			this.rig = rig;	
			this.Rebuild();
			}

		// -------------------
		override public string GetUndoPrefix()								{	return "Rig Switches - "; }
		override public GUIContent GetListTitleContent()					{	return (new UnityEngine.GUIContent("Switches (" + this.GetElemCount() + ")"));}
		override protected ElemInspector CreateElemInspector(object obj)	{	return new RigSwitchElemInspector((InputRig.RigSwitch)obj, this.rig, this); }

		// -------------------
		override protected void OnNewElemClicked()
			{
			// TODO : show content menu, but for now...

			this.AddNewElem(new InputRig.RigSwitch("NEW SWITCH"));
			}


			
		// ---------------------
		// Rig Switch Elem Inspector.		
		// ----------------------	
		private class RigSwitchElemInspector : ListInspector.ElemInspector
			{
			public InputRig
				rig;
			public InputRig.RigSwitch 
				target;

							

			// -------------------
			public RigSwitchElemInspector(InputRig.RigSwitch obj, InputRig rig, RigSwitchListInspector listInsp) : base(listInsp)
				{
				this.target = obj;
				this.rig = rig;

				}


			// --------------
			override protected GUIContent GetElemTitleContent()
				{
				return (new GUIContent(this.target.name));
				}

			// ---------------
			override public bool IsFoldable()
				{
				return false;
				}
				
			// ----------------
			override protected void DrawGUIContent()
				{
				}

			// --------------
			override public bool DrawGUI() //object targetObj)
				{
				bool retVal = true;

				string name	= this.target.name;
				bool	defaultState = this.target.defaultState;
					
				

				EditorGUILayout.BeginHorizontal(CFEditorStyles.Inst.transpBevelBG);

				// Keyboard input...

		//		EditorGUILayout.LabelField(new GUIContent("Keyboard Input"));
		//		CFGUI.BeginIndentedVertical(CFEditorStyles.Inst.transpSunkenBG);
			
					EditorGUILayout.LabelField(new GUIContent("Name", "Switch's name"), GUILayout.Width(60));


					name = EditorGUILayout.TextField(new GUIContent("", "Switch's name"), name, GUILayout.ExpandWidth(true), GUILayout.MinWidth(20));

					defaultState = EditorGUILayout.ToggleLeft(new GUIContent("", "Switch's default state"), defaultState, GUILayout.Width(15));
		
					GUILayout.Space(10);
					
		//		CFGUI.EndIndentedVertical();
					
					
				if (!this.DrawDefaultButtons(false, true))
					retVal = false;
					

				EditorGUILayout.EndHorizontal();

					
				if (retVal)
					{
					if ((name 			!= this.target.name) ||
						(defaultState	!= this.target.defaultState) )
						{
						CFGUI.CreateUndo("Rig Switch modification", this.listInsp.undoObject);
	
						this.target.name			= name;
						this.target.defaultState	= defaultState;

						CFGUI.EndUndo(this.listInsp.undoObject);
						}
					}

				return retVal;
				}

			} 
		}
		




	// -----------------------
	// Auto Input List Inspector.
	// ------------------------
	private class AutoInputConfigListInspector : ListInspector
		{
		private InputRig rig;

		// -------------------
		public AutoInputConfigListInspector(InputRig rig) : base(rig, rig.autoInputList.list)
			{		
			this.rig = rig;	
			this.Rebuild();
			}

		// -------------------
		override public string GetUndoPrefix()								{	return "Auto Input List - "; }
		override public GUIContent GetListTitleContent()					{	return (new UnityEngine.GUIContent("Automatic Input (" + this.GetElemCount() + ")"));}
		override protected ElemInspector CreateElemInspector(object obj)	{	return new AutoInputConfigElemInspector((InputRig.AutomaticInputConfig)obj, this.rig, this); }

		// -------------------
		override protected void OnNewElemClicked()
			{
			InputRig.AutomaticInputConfig c = new InputRig.AutomaticInputConfig();
			c.name = "New Auto-Input";

			this.AddNewElem(c);
			}


			
		// ---------------------
		// Auto Input Config Elem Inspector.		
		// ----------------------	
		private class AutoInputConfigElemInspector : ListInspector.ElemInspector
			{
			public InputRig
				rig;
			public InputRig.AutomaticInputConfig 
				target;


			public DigitalBindingInspector
				targetBindingInsp;

			public DisablingConditionSetInspector
				disablingConditionsInsp;

			public RelatedKeyListInspector
				relatedKeyListInsp;
			public RelatedAxisListInspector
				relatedAxisListInsp;
							

			// -------------------
			public AutoInputConfigElemInspector(InputRig.AutomaticInputConfig obj, InputRig rig, AutoInputConfigListInspector listInsp) : base(listInsp)
				{
				this.target = obj;
				this.rig = rig;

				this.targetBindingInsp = new DigitalBindingInspector(rig, new GUIContent("Target Binding"));

				this.disablingConditionsInsp = new DisablingConditionSetInspector(new GUIContent("Disabling Conditions", "When this auto-input should NOT be used..."),
					this.target.disablingConditions, rig, rig);

				this.relatedAxisListInsp = new RelatedAxisListInspector(rig, obj);
				this.relatedKeyListInsp = new RelatedKeyListInspector(rig, obj);

				}


			// --------------
			override protected GUIContent GetElemTitleContent()
				{
				return (new GUIContent(this.target.name));
				}

			// ---------------
			override public bool IsFoldable()
				{
				return true;
				}
				
			// ----------------
			override protected void DrawGUIContent()
				{
				string name = this.target.name;

				name = CFGUI.TextField(new GUIContent("Name"), name, 80);

				if (name != this.target.name)
					{
					CFGUI.CreateUndo("Auto Input Config Modification", this.rig);

					this.target.name = name;

					CFGUI.EndUndo(this.rig);
					}


				this.targetBindingInsp.Draw(this.target.targetBinding, this.rig);

				
				this.relatedAxisListInsp.DrawGUI();
				this.relatedKeyListInsp.DrawGUI();

				this.disablingConditionsInsp.DrawGUI();

				}



			} 






// -----------------------
		// -----------------------
		// Auto Input Related Axis List Inspector.
		// ------------------------
		private class RelatedAxisListInspector : ListInspector
			{
			private InputRig 
				rig;
//			private InputRig.AutomaticInputConfig 
//				parentConfig;
	
			// -------------------
			public RelatedAxisListInspector(InputRig rig, InputRig.AutomaticInputConfig parentConfig) : base(rig, parentConfig.relatedAxisList)
				{		
				this.rig = rig;
				//this.parentConfig = parentConfig;	
				this.Rebuild();
				}
	
			// -------------------
			override public string GetUndoPrefix()								{	return "Related Axes - "; }
			override public GUIContent GetListTitleContent()					{	return (new UnityEngine.GUIContent("Related Axes (" + this.GetElemCount() + ")"));}
			override protected ElemInspector CreateElemInspector(object obj)	{	return new RelatedAxisElemInspector((InputRig.AutomaticInputConfig.RelatedAxis)obj, this.rig, this); }
	
			// -------------------
			override protected void OnNewElemClicked()
				{
				this.AddNewElem(new InputRig.AutomaticInputConfig.RelatedAxis());
				}
	
	
				
			// ---------------------
			// Related Axis Elem Inspector.		
			// ----------------------	
			private class RelatedAxisElemInspector : ListInspector.ElemInspector
				{
				public InputRig
					rig;
				public InputRig.AutomaticInputConfig.RelatedAxis 
					target;
	
				private RigAxisNameDrawer
					axisNameInsp;
								
	
				// -------------------
				public RelatedAxisElemInspector(InputRig.AutomaticInputConfig.RelatedAxis obj, InputRig rig, RelatedAxisListInspector listInsp) : base(listInsp)
					{
					this.target = obj;
					this.rig = rig;
	
					this.axisNameInsp = new RigAxisNameDrawer(InputRig.InputSource.Analog);

					}
	
	
				// --------------
				override protected GUIContent GetElemTitleContent()
					{
					return (new GUIContent(this.target.axisName));
					}
	
				// ---------------
				override public bool IsFoldable()
					{
					return false;
					}
					
				// ----------------
				override protected void DrawGUIContent()
					{
//					float 
//						refVal = this.target.refVal;
//					InputRig.AutomaticInputConfig.AxisValueRelation
//						axisRelation = this.target.axisRelation;
					bool 
						mustBeControlledByInput = this.target.mustBeControlledByInput;
					string 
						axisName = this.target.axisName;

				
					axisName = this.axisNameInsp.Draw("", axisName, this.rig);

					mustBeControlledByInput = EditorGUILayout.ToggleLeft(new GUIContent("Must be pressed", "Axis must be pressed (or actively controlled in some other way) for this auto-input to be active."),
						mustBeControlledByInput, GUILayout.MinWidth(30), GUILayout.ExpandWidth(true));

//					axisRelation = (InputRig.AutomaticInputConfig.AxisValueRelation)CFGUI.EnumPopup(new GUIContent("Active When", "Compare axis value to reference value..."),
//						axisRelation, 120);  
//
//					refVal = CFGUI.FloatField(new GUIContent("Ref. Value", "Reference Value"),
//						refVal, -1, 1, 120);

		
					if (
//						(refVal != this.target.refVal) ||	
//						(axisRelation != this.target.axisRelation) ||
						(mustBeControlledByInput != this.target.mustBeControlledByInput) ||
						(axisName != this.target.axisName))
						{
						CFGUI.CreateUndo("Related Axis Modification", this.rig);

//						this.target.refVal			= refVal;
//						this.target.axisRelation	= axisRelation;
						this.target.mustBeControlledByInput	= mustBeControlledByInput;
						this.target.axisName		= axisName;

						CFGUI.EndUndo(this.rig);
						}
					}
	
	
				} 
	
	
	
			}


		// -----------------------
		// Auto Input Related Key List Inspector.
		// ------------------------
		private class RelatedKeyListInspector : ListInspector
			{
			private InputRig 
				rig;
//			private InputRig.AutomaticInputConfig 
//				parentConfig;
	
			// -------------------
			public RelatedKeyListInspector(InputRig rig, InputRig.AutomaticInputConfig parentConfig) : base(rig, parentConfig.relatedKeyList)
				{		
				this.rig = rig;
				//this.parentConfig = parentConfig;	
				this.Rebuild();
				}
	
			// -------------------
			override public string GetUndoPrefix()								{	return "Related Keys - "; }
			override public GUIContent GetListTitleContent()					{	return (new UnityEngine.GUIContent("Related Keys (" + this.GetElemCount() + ")"));}
			override protected ElemInspector CreateElemInspector(object obj)	{	return new RelatedKeyElemInspector((InputRig.AutomaticInputConfig.RelatedKey)obj, this.rig, this); }
	
			// -------------------
			override protected void OnNewElemClicked()
				{
				this.AddNewElem(new InputRig.AutomaticInputConfig.RelatedKey());
				}
	
	
				
			// ---------------------
			// Related Key Elem Inspector.		
			// ----------------------	
			private class RelatedKeyElemInspector : ListInspector.ElemInspector
				{
				public InputRig
					rig;
				public InputRig.AutomaticInputConfig.RelatedKey 
					target;
	
								
	
				// -------------------
				public RelatedKeyElemInspector(InputRig.AutomaticInputConfig.RelatedKey obj, InputRig rig, RelatedKeyListInspector listInsp) : base(listInsp)
					{
					this.target = obj;
					this.rig = rig;
	
					}
	
	
				// --------------
				override protected GUIContent GetElemTitleContent()
					{
					return (new GUIContent(this.target.key.ToString()));
					}
	
				// ---------------
				override public bool IsFoldable()
					{
					return false;
					}
					
				// ----------------
				override protected void DrawGUIContent()
					{
					KeyCode		
						key = this.target.key; 
					bool	
						mustBeControlledByInput = this.target.mustBeControlledByInput;

				
					key = (KeyCode)CFGUI.EnumPopup(new GUIContent("Key", "Related KeyCode"),
						key, 100);

					mustBeControlledByInput = EditorGUILayout.ToggleLeft(new GUIContent("Must be pressed", "Key must be pressed for this auto-input to be active."),
						mustBeControlledByInput, GUILayout.MinWidth(30), GUILayout.ExpandWidth(true));

		
					if ((key != this.target.key) ||	
						(mustBeControlledByInput != this.target.mustBeControlledByInput))
						{
						CFGUI.CreateUndo("Related Key Modification", this.rig);

						this.target.key			= key;
						this.target.mustBeControlledByInput	= mustBeControlledByInput;

						CFGUI.EndUndo(this.rig);
						}
					}
	
	
				} 
	
	
	
			}

		}
		
 

	}


		
}
#endif
