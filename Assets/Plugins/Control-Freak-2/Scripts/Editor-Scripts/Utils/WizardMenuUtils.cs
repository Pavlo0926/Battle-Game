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
using System.Collections.Generic;

using ControlFreak2Editor.Inspectors;
using System.ComponentModel;


namespace ControlFreak2Editor
{
static public class WizardMenuUtils
	{


	// -------------------------
	public class WizardMenuItem
		{
		public TouchControlPanel	panel;
		public System.Type			wizardType;
		public object				bindingSetup;
		public System.Action		callback;			
			
		// ---------------------
		static public void AddToMenu(
			GenericMenu			menu,
			string				itemLabel,
			TouchControlPanel	panel,
			System.Type			wizardType,
			object				bindingSetup,
			System.Action		onCreationCallback = null)
			{
			WizardMenuItem o = new WizardMenuItem();

			o.wizardType	= wizardType;
			o.panel			= panel;
			o.callback		= onCreationCallback;
			o.bindingSetup	= bindingSetup;
			
			menu.AddItem(new GUIContent(itemLabel), false, o.Execute);
			}


		// -------------------
		public void Execute()
			{
			if (this.wizardType == typeof(TouchButtonCreationWizard))
				TouchButtonCreationWizard.ShowWizard(this.panel, (TouchButtonCreationWizard.BindingSetup)this.bindingSetup, this.callback);

			else if (this.wizardType == typeof(TouchJoystickCreationWizard))
				TouchJoystickCreationWizard.ShowWizard(this.panel, (TouchJoystickCreationWizard.BindingSetup)this.bindingSetup, this.callback);

			else if (this.wizardType == typeof(TouchWheelCreationWizard))
				TouchWheelCreationWizard.ShowWizard(this.panel, (TouchWheelCreationWizard.BindingSetup)this.bindingSetup, this.callback);

			else if (this.wizardType == typeof(TouchTrackPadCreationWizard))
				TouchTrackPadCreationWizard.ShowWizard(this.panel, (TouchTrackPadCreationWizard.BindingSetup)this.bindingSetup, this.callback);


			else if (this.wizardType == typeof(SuperTouchZoneCreationWizard))
				SuperTouchZoneCreationWizard.ShowWizard(this.panel, (SuperTouchZoneCreationWizard.BindingSetup)this.bindingSetup, this.callback);
			}
		}





	// ------------------------
	static public void CreateContextMenuForKeyBinding(Object panelOrRig, KeyCode key, System.Action onRefreshCallback = null)
		{
		InputRig			rig		= (panelOrRig as InputRig);
		TouchControlPanel	panel	= (panelOrRig as TouchControlPanel);
			
		string axisName = null;
	
		//bool displayPanelName = false;

		if (panel != null)
			{
			rig = panel.rig;
			}

		else if (rig != null)
			{		
			// Auto Select panel...

			panel = TouchControlWizardUtils.GetRigPanel(rig);
			//displayPanelName = true;
			}
		else 
			return;

		
		List<TouchControl> controlList = rig.GetTouchControls();
	
		
		GenericMenu menu = new GenericMenu();

		string commandName = (string.IsNullOrEmpty(axisName) ? ("\"" + key.ToString() + "\" keycode ") : ("\"" + axisName + "\" axis "));

		menu.AddDisabledItem(new GUIContent("Actions for : " + commandName));
		menu.AddSeparator("");



	
			{
		
				
			UniversalBindingAssignment.AddBindingContainerToMenu(menu, rig, onRefreshCallback, BindingDescription.BindingType.Digital, commandName,  
				"Bind to Input Rig/", rig, key, null); //, null); 
		
	
			menu.AddSeparator("");
	
			if (panel == null)	
				{
				menu.AddDisabledItem(new GUIContent("Add a Touch Control Panel to this rig to create and bind commands to Touch Controls!"));
				}
			else
				{
		
				UniversalBindingAssignment.AddControlListBindingsToMenu(menu, controlList, onRefreshCallback, 
					BindingDescription.BindingType.Digital, commandName, "Bind to Touch Controls/", key, null); // true); //, null);
		
		
				menu.AddSeparator("");
		
		
				AddTouchControlCreationItemsToMenuForAxisOrKey(menu, panel, null, key, onRefreshCallback);


				}
			}

		menu.ShowAsContext();

		}
		





	// ------------------------
	static public void CreateContextMenuForAxisBinding(Object panelOrRig, string axisName, /*InputRig.InputSource sourceType, */System.Action onRefreshCallback = null)
		{
		InputRig			rig		= (panelOrRig as InputRig);
		TouchControlPanel	panel	= (panelOrRig as TouchControlPanel);
			
		//bool displayPanelName = false;

		if (panel != null)
			{
			rig = panel.rig;
			}

		else if (rig != null)
			{		
			// Auto Select panel...

			panel = TouchControlWizardUtils.GetRigPanel(rig);
			//displayPanelName = true;
			}
		else 
			return;

		
		List<TouchControl> controlList = rig.GetTouchControls();
	
		
		GenericMenu menu = new GenericMenu();
	

		string commandName = ("\"" + axisName + "\" axis ");

		menu.AddDisabledItem(new GUIContent("Actions for : " + commandName));
		menu.AddSeparator("");
	
			
		InputRig.AxisConfig axisConfig = rig.GetAxisConfig(axisName);
		if (axisConfig == null)
			{
			AxisCreationMenuItem.AddAllMenuItems(menu, axisName, "", rig, onRefreshCallback);
			}

		else
			{
			int 
				axisSourceTypeMask	= axisConfig.GetSupportedInputSourceMask();
			BindingDescription.BindingType	
				bindingTypeMask		= BindingDescription.BindingType.Axis | BindingDescription.BindingType.Digital;
			

			UniversalBindingAssignment.AddBindingContainerToMenu(menu, rig, onRefreshCallback, bindingTypeMask, commandName,  
				"Bind to Input Rig/", rig, KeyCode.None, axisName, axisSourceTypeMask); //, true); //, null); 
		
			menu.AddSeparator("");
	
			if (panel == null)	
				{
				menu.AddDisabledItem(new GUIContent("Add a Touch Control Panel to this rig to create and bind commands to Touch Controls!"));
				}
			else
				{
		
				UniversalBindingAssignment.AddControlListBindingsToMenu(menu, controlList, onRefreshCallback, 
					bindingTypeMask, commandName, "Bind to Touch Controls/", KeyCode.None, axisName); //, true); //, null);
		
				menu.AddSeparator("");

				AddTouchControlCreationItemsToMenuForAxisOrKey(menu, panel, axisName, KeyCode.None, onRefreshCallback);

				}
			}

		menu.ShowAsContext();
		}
		

	// -----------------
	static private void AddTouchControlCreationItemsToMenuForAxisOrKey(GenericMenu menu, TouchControlPanel panel, string axisName, KeyCode key,
		System.Action onRefreshCallback)
		{
		string commandName = (string.IsNullOrEmpty(axisName) ? ("\"" + key.ToString() + "\" keycode ") : ("\"" + axisName + "\" axis "));


		menu.AddSeparator("Create a Button/");

			WizardMenuItem.AddToMenu(menu, "Create a Button/Button with " + commandName + "bound to [Press] ...", panel, 
			typeof(TouchButtonCreationWizard), TouchButtonCreationWizard.BindingSetup.PressKeyOrAxis(key, axisName), onRefreshCallback);
			WizardMenuItem.AddToMenu(menu, "Create a Button/Button with " + commandName + "bound to [Toggle]...", panel, 
			typeof(TouchButtonCreationWizard), TouchButtonCreationWizard.BindingSetup.ToggleKeyOrAxis(key, axisName), onRefreshCallback);

		WizardMenuItem.AddToMenu(menu, "Create a Button/Button...", panel, 
			typeof(TouchButtonCreationWizard), null, onRefreshCallback);


	//	menu.AddSeparator("");


			WizardMenuItem.AddToMenu(menu, "Create a Joystick/Joystick with " + commandName + "bound to [Press]...", panel, 
			typeof(TouchJoystickCreationWizard), TouchJoystickCreationWizard.BindingSetup.PressKeyOrAxis(key, axisName), onRefreshCallback);

		if (!string.IsNullOrEmpty(axisName))
			{
			WizardMenuItem.AddToMenu(menu, "Create a Joystick/Joystick with " + commandName + "bound to [Horizontal Axis]...", panel, 
				typeof(TouchJoystickCreationWizard), TouchJoystickCreationWizard.BindingSetup.HorzAxis(axisName), onRefreshCallback);
			WizardMenuItem.AddToMenu(menu, "Create a Joystick/Joystick with " + commandName + "bound to [Vertical Axis]...", panel, 
				typeof(TouchJoystickCreationWizard), TouchJoystickCreationWizard.BindingSetup.VertAxis(axisName), onRefreshCallback);
			}

		menu.AddSeparator("Create a Joystick/");

		WizardMenuItem.AddToMenu(menu, "Create a Joystick/Joystick...", panel, 
			typeof(TouchJoystickCreationWizard), null, onRefreshCallback);
			


		WizardMenuItem.AddToMenu(menu, "Create a Wheel/Wheel with " + commandName + "bound to [Press]...", panel, 
			typeof(TouchWheelCreationWizard), TouchWheelCreationWizard.BindingSetup.PressKeyOrAxis(key, axisName), onRefreshCallback);

		if (!string.IsNullOrEmpty(axisName))
			{
				WizardMenuItem.AddToMenu(menu, "Create a Wheel/Wheel with " + commandName + "bound to [Analog Turn]...", panel, 
				typeof(TouchWheelCreationWizard), TouchWheelCreationWizard.BindingSetup.TurnAxis(axisName), onRefreshCallback);
			}

		menu.AddSeparator("Create a Wheel/");

		WizardMenuItem.AddToMenu(menu, "Create a Wheel/Wheel...", panel, 
			typeof(TouchWheelCreationWizard), null, onRefreshCallback);


		WizardMenuItem.AddToMenu(menu, "Create a TrackPad/TrackPad with " + commandName + "bound to [Press] bound to " + commandName + "...", panel, 
			typeof(TouchTrackPadCreationWizard), TouchTrackPadCreationWizard.BindingSetup.PressKeyOrAxis(key, axisName), onRefreshCallback);

		if (!string.IsNullOrEmpty(axisName))
			{
			WizardMenuItem.AddToMenu(menu, "Create a TrackPad/TrackPad with " + commandName + "bound to [Horz. Swipe Delta]...", panel, 
				typeof(TouchTrackPadCreationWizard), TouchTrackPadCreationWizard.BindingSetup.HorzAxis(axisName), onRefreshCallback);
	
			WizardMenuItem.AddToMenu(menu, "Create a TrackPad/TrackPad with " + commandName + "bound to [Vert. Swipe Delta]...", panel, 
				typeof(TouchTrackPadCreationWizard), TouchTrackPadCreationWizard.BindingSetup.VertAxis(axisName), onRefreshCallback);
			}

		menu.AddSeparator("Create a TrackPad/");

		WizardMenuItem.AddToMenu(menu, "Create a TrackPad/TrackPad...", panel, 
			typeof(TouchTrackPadCreationWizard), null, onRefreshCallback);


		WizardMenuItem.AddToMenu(menu, "Create a Super Touch Zone/Super Touch Zone with " + commandName + "bound to [Single-finger Press]...", panel, 
			typeof(SuperTouchZoneCreationWizard), SuperTouchZoneCreationWizard.BindingSetup.PressKeyOrAxis(1, key, axisName), onRefreshCallback);
		WizardMenuItem.AddToMenu(menu, "Create a Super Touch Zone/Super Touch Zone with " + commandName + "bound to [Single-finger Tap]...", panel, 
			typeof(SuperTouchZoneCreationWizard), SuperTouchZoneCreationWizard.BindingSetup.TapKeyOrAxis(1, key, axisName), onRefreshCallback);
		WizardMenuItem.AddToMenu(menu, "Create a Super Touch Zone/Super Touch Zone with " + commandName + "bound to [Two-finger Press]...", panel, 
			typeof(SuperTouchZoneCreationWizard), SuperTouchZoneCreationWizard.BindingSetup.PressKeyOrAxis(2, key, axisName), onRefreshCallback);
		WizardMenuItem.AddToMenu(menu, "Create a Super Touch Zone/Super Touch Zone with " + commandName + "bound to [Two-finger Tap]...", panel, 
			typeof(SuperTouchZoneCreationWizard), SuperTouchZoneCreationWizard.BindingSetup.TapKeyOrAxis(2, key, axisName), onRefreshCallback);
			
		if (!string.IsNullOrEmpty(axisName))
			{
			WizardMenuItem.AddToMenu(menu, "Create a Super Touch Zone/Super Touch Zone with " + commandName + "bound to [Single-finger Horz. Swipe Delta]...", panel, 
				typeof(SuperTouchZoneCreationWizard), SuperTouchZoneCreationWizard.BindingSetup.HorzSwipeAxis(1, axisName), onRefreshCallback);
			WizardMenuItem.AddToMenu(menu, "Create a Super Touch Zone/Super Touch Zone with " + commandName + "bound to [Single-finger Vert. Swipe Delta]...", panel, 
				typeof(SuperTouchZoneCreationWizard), SuperTouchZoneCreationWizard.BindingSetup.VertSwipeAxis(1, axisName), onRefreshCallback);
			WizardMenuItem.AddToMenu(menu, "Create a Super Touch Zone/Super Touch Zone with " + commandName + "bound to [Two-finger Horz. Swipe Delta]...", panel, 
				typeof(SuperTouchZoneCreationWizard), SuperTouchZoneCreationWizard.BindingSetup.HorzSwipeAxis(2, axisName), onRefreshCallback);
			WizardMenuItem.AddToMenu(menu, "Create a Super Touch Zone/Super Touch Zone with " + commandName + "bound to [Two-finger Vert. Swipe Delta]...", panel, 
				typeof(SuperTouchZoneCreationWizard), SuperTouchZoneCreationWizard.BindingSetup.VertSwipeAxis(2, axisName), onRefreshCallback);
	
			WizardMenuItem.AddToMenu(menu, "Create a Super Touch Zone/Super Touch Zone with " + commandName + "bound to [Single-finger Horz. Scroll]...", panel, 
				typeof(SuperTouchZoneCreationWizard), SuperTouchZoneCreationWizard.BindingSetup.HorzScrollAxis(1, axisName), onRefreshCallback);
			WizardMenuItem.AddToMenu(menu, "Create a Super Touch Zone/Super Touch Zone with " + commandName + "bound to [Single-finger Vert. Scroll]...", panel, 
				typeof(SuperTouchZoneCreationWizard), SuperTouchZoneCreationWizard.BindingSetup.VertScrollAxis(1, axisName), onRefreshCallback);
			WizardMenuItem.AddToMenu(menu, "Create a Super Touch Zone/Super Touch Zone with " + commandName + "bound to [Two-finger Horz. Scroll]...", panel, 
				typeof(SuperTouchZoneCreationWizard), SuperTouchZoneCreationWizard.BindingSetup.HorzScrollAxis(2, axisName), onRefreshCallback);
			WizardMenuItem.AddToMenu(menu, "Create a Super Touch Zone/Super Touch Zone with " + commandName + "bound to [Two-finger Vert. Scroll]...", panel, 
				typeof(SuperTouchZoneCreationWizard), SuperTouchZoneCreationWizard.BindingSetup.VertScrollAxis(2, axisName), onRefreshCallback);
			}

		menu.AddSeparator("Create a Super Touch Zone/");

		WizardMenuItem.AddToMenu(menu, "Create a Super Touch Zone/Super Touch Zone...", panel, 
			typeof(SuperTouchZoneCreationWizard), null, onRefreshCallback);
		

		}




	// ------------------------
	static public void CreateContextMenuForMousePositionBinding(Object panelOrRig, System.Action onRefreshCallback = null)
		{
		InputRig			rig		= (panelOrRig as InputRig);
		TouchControlPanel	panel	= (panelOrRig as TouchControlPanel);
			
		//bool displayPanelName = false;

		if (panel != null)
			{
			rig = panel.rig;
			}

		else if (rig != null)
			{		
			// Auto Select panel...

			panel = TouchControlWizardUtils.GetRigPanel(rig);
			//displayPanelName = true;
			}
		else 
			return;

		
		List<TouchControl> controlList = rig.GetTouchControls();
	
		
		GenericMenu menu = new GenericMenu();

	
		string commandName = ("Mouse Position ");
		menu.AddDisabledItem(new GUIContent("Actions for : " + commandName));
		menu.AddSeparator("");
			
		UniversalBindingAssignment.AddBindingContainerToMenu(menu, rig, onRefreshCallback, BindingDescription.BindingType.MousePos, commandName,  
			"Bind to Input Rig/", rig); 
	
		menu.AddSeparator("");

		if (panel == null)	
			{
			menu.AddDisabledItem(new GUIContent("Add a Touch Control Panel to this rig to create and bind commands to Touch Controls!"));
			}
		else
			{
	
			UniversalBindingAssignment.AddControlListBindingsToMenu(menu, controlList, onRefreshCallback, 
				BindingDescription.BindingType.MousePos, commandName, "Bind to Touch Controls/");
	
			menu.AddSeparator("");
	
	
	
			int mouseTargetMaxId = CFUtils.GetEnumMaxValue(typeof(SuperTouchZoneCreationWizard.BindingSetup.MouseBindingTarget));

			for (int i = 1; i <= 2; ++i)
				{	
				for (int j = 1; j <= mouseTargetMaxId; ++j)
					{	
					SuperTouchZoneCreationWizard.BindingSetup.MouseBindingTarget 
						mouseTarget = (SuperTouchZoneCreationWizard.BindingSetup.MouseBindingTarget)j;

					WizardMenuItem.AddToMenu(menu, "Create a Super Touch Zone/Super Touch Zone with " + commandName + "bound to [" +
						((i == 1) ? "Single-finger" : "Two-finger") + mouseTarget.ToString() + "]...", panel, typeof(SuperTouchZoneCreationWizard), 
						SuperTouchZoneCreationWizard.BindingSetup.MousePos(i, mouseTarget), onRefreshCallback);
					}
				}
	
			menu.AddSeparator("Create a Super Touch Zone/");
	
			WizardMenuItem.AddToMenu(menu, "Create a Super Touch Zone/Super Touch Zone...", panel, 
				typeof(SuperTouchZoneCreationWizard), null, onRefreshCallback);
			}

		menu.ShowAsContext();
		}






	// ------------------------
	static public void CreateContextMenuForEmuTouchBinding(Object panelOrRig, System.Action onRefreshCallback = null)
		{
		InputRig			rig		= (panelOrRig as InputRig);
		TouchControlPanel	panel	= (panelOrRig as TouchControlPanel);
			
		//bool displayPanelName = false;

		if (panel != null)
			{
			rig = panel.rig;
			}

		else if (rig != null)
			{		
			// Auto Select panel...

			panel = TouchControlWizardUtils.GetRigPanel(rig);
			//displayPanelName = true;
			}
		else 
			return;

		
		List<TouchControl> controlList = rig.GetTouchControls();
	
		
		GenericMenu menu = new GenericMenu();

	
		string commandName = ("Emuulated Touch ");
		menu.AddDisabledItem(new GUIContent("Actions for : " + commandName));
		menu.AddSeparator("");
			
		UniversalBindingAssignment.AddBindingContainerToMenu(menu, rig, onRefreshCallback, BindingDescription.BindingType.EmuTouch, commandName,  
			"Bind to Input Rig/", rig); 
	
		menu.AddSeparator("");

		if (panel == null)	
			{
			menu.AddDisabledItem(new GUIContent("Add a Touch Control Panel to this rig to create and bind commands to Touch Controls!"));
			}
		else
			{
	
			UniversalBindingAssignment.AddControlListBindingsToMenu(menu, controlList, onRefreshCallback, 
				BindingDescription.BindingType.EmuTouch, commandName, "Bind to Touch Controls/");
	
			menu.AddSeparator("");
	
	
	
	
			WizardMenuItem.AddToMenu(menu, "Create a Super Touch Zone/Super Touch Zone with " + commandName + "bound to [Single-finger Touch]...", panel, 
				typeof(SuperTouchZoneCreationWizard), SuperTouchZoneCreationWizard.BindingSetup.EmuTouch(1), onRefreshCallback);
			WizardMenuItem.AddToMenu(menu, "Create a Super Touch Zone/Super Touch Zone with " + commandName + "bound to [Two-finger Touch]...", panel, 
				typeof(SuperTouchZoneCreationWizard), SuperTouchZoneCreationWizard.BindingSetup.EmuTouch(2), onRefreshCallback);

	
			menu.AddSeparator("Create a Super Touch Zone/");
	
			WizardMenuItem.AddToMenu(menu, "Create a Super Touch Zone/Super Touch Zone...", panel, 
				typeof(SuperTouchZoneCreationWizard), null, onRefreshCallback);
			}

		menu.ShowAsContext();
		}


		

	// -------------------
	private class AxisCreationMenuItem
		{
		public string 				axisName;
		public InputRig.AxisType	axisType;
		public System.Action		callback;
		public InputRig				rig;

			

		// ----------------------
		static public void AddAllMenuItems(GenericMenu menu, string axisName, string menuPath, InputRig rig, System.Action callback)
			{
			AddMenuItem(menu, axisName, menuPath, InputRig.AxisType.Digital,			rig, callback);
			AddMenuItem(menu, axisName, menuPath, InputRig.AxisType.UnsignedAnalog,	rig, callback);
			AddMenuItem(menu, axisName, menuPath, InputRig.AxisType.SignedAnalog,	rig, callback);
			AddMenuItem(menu, axisName, menuPath, InputRig.AxisType.Delta, 			rig, callback);
			AddMenuItem(menu, axisName, menuPath, InputRig.AxisType.ScrollWheel,		rig, callback);
			}

		// ----------------------
		static public void AddMenuItem(GenericMenu menu, string axisName, string menuPath, InputRig.AxisType axisType, InputRig rig, System.Action callback)
			{
			AxisCreationMenuItem o = new AxisCreationMenuItem();

			string itemName = "";

			o.axisName	= axisName;
			o.axisType	= axisType;				
			o.callback	= callback;
			o.rig		= rig;
				
			itemName = "Create \"" + axisName + "\" " + axisType.ToString() + " axis!";

			menu.AddItem(new GUIContent(menuPath + itemName), false, o.Execute);
			}


		// ---------------------
		public void Execute()
			{	
			if (this.rig == null)
				return;

			if (!this.rig.IsAxisDefined(this.axisName))
				{
				this.rig.axes.Add(this.axisName, this.axisType, true);
				}	

			if (this.callback != null)
				this.callback();
			}
		}


	// ------------------
	private class UniversalBindingAssignment // : BindingAssignmentBase
		{
		public System.Action 	onRefreshCallback;
		public Object			undoObject;
		public string			undoLabel;
		public InputBindingBase	binding;

		public string			digiAxisName;
		public int				digiAxisElemId;
		public KeyCode			digiKey;
		public int				digiKeyElemId;
		public bool				digiBindToPositiveAxis;

		public string			analogAxisName;
		public int 				analogElemId;
		public bool				analogSeparate,
								analogPositiveSide,
								analogFlip;

		//public string			joyName;
			
		// ---------------------
		static public void AddControlListBindingsToMenu(
			GenericMenu						menu,
			List<TouchControl>				controlList,	
			System.Action					onRefreshCallback,
			BindingDescription.BindingType	typeMask,
			string 							commandName,
			string							menuPath,
			KeyCode							digiKey					= KeyCode.None,
			string							axisName				= null)
			//bool 							digiBindToPositiveAxis	= true)
			//string							joyName					= null)
			{
			
			for (int i = 0; i < controlList.Count; ++i)
				{
				TouchControl c = controlList[i];

				AddBindingContainerToMenu(menu, c, onRefreshCallback,typeMask, commandName, 
					(menuPath + c.name + " (" + c.GetType().Name + ")/"), c, digiKey, axisName); 
				}
			}

		

		// -----------------------
		static private string FormatBindingMenuLabel(InputBindingBase binding, string name)
			{
			if (!binding.enabled)
				return (name + " (Disabled)");
			if (!binding.IsEnabledInHierarchy())
				return (name + " (Disabled by hierarchy)");
			else
				return name;
			}

		// ----------------------
		static public void AddBindingContainerToMenu(
			GenericMenu						menu,
			IBindingContainer				bc,	
			System.Action					onRefreshCallback,
			BindingDescription.BindingType	typeMask,
			string							commandName,
			string							menuPath,
			Object							undoObject,
			KeyCode							digiKey					= KeyCode.None,
			string							axisName				= null,
			int 								axisInputSourceMask		= ((1<<16)-1))
			//bool 							digiBindToPositiveAxis	= true)
			//string							joyName					= null)
			{
			
			BindingDescriptionList bindingDescList = new BindingDescriptionList(typeMask, false, axisInputSourceMask, FormatBindingMenuLabel);
			//bc.GetSubBindingDescriptions(bindingDescList, typeMask, undoObject, menuPath, false, axisInputSourceMask);
			bc.GetSubBindingDescriptions(bindingDescList, undoObject, menuPath);

			for (int i = 0; i < bindingDescList.Count; ++i)
				{
				BindingDescription desc = bindingDescList[i];

				string itemLabel = "";	
				bool itemEnabled = false;			
						

				DigitalBinding			digiBinding		= (desc.binding as DigitalBinding);
				AxisBinding				analogBinding	= (desc.binding as AxisBinding);
				EmuTouchBinding			touchBinding	= (desc.binding as EmuTouchBinding);
				MousePositionBinding 	mouseBinding	= (desc.binding as MousePositionBinding);

				// Digi binding...
	
				if (digiBinding != null)
					{
//					o.digiKey					= digiKey;
//					o.digiAxisName				= axisName;
//					o.digiBindToPositiveAxis	= true;
				
					if (digiKey != KeyCode.None)
						{
						AddDigitalBindingKeySubMenu(menu, digiKey, desc, onRefreshCallback);

						}		
					else if (!string.IsNullOrEmpty(axisName))
						{	
						AddDigitalBindingAxisSubMenu(menu, axisName, desc, onRefreshCallback);

						}
					}

				// Analog binding...

				else if (analogBinding != null)
					{
					AddAnalogBindingAxisSubMenu(menu, axisName, desc, onRefreshCallback);

					}

				// Touch binding...
				
				else if (touchBinding != null)
					{
					UniversalBindingAssignment o = new UniversalBindingAssignment();
				
					o.undoObject			= desc.undoObject;
					o.binding				= desc.binding;
					o.onRefreshCallback 	= onRefreshCallback;

					o.undoLabel = "Bind [" + desc.nameFormatted + "] to Eumlated Touch.";
					itemLabel = o.undoLabel;
			
					menu.AddItem(new GUIContent(desc.menuPath + itemLabel), itemEnabled, o.Execute);
					}

				// Mouse binding...

				else if (mouseBinding != null)
					{
					UniversalBindingAssignment o = new UniversalBindingAssignment();
				
					o.undoObject			= desc.undoObject;
					o.binding				= desc.binding;
					o.onRefreshCallback 	= onRefreshCallback;

					o.undoLabel = "Bind [" + desc.nameFormatted + "] to Mouse Position."; 
					itemLabel = o.undoLabel;

					menu.AddItem(new GUIContent(desc.menuPath + itemLabel), itemEnabled, o.Execute);

					}
						
				
				else
					continue;

				}
			}



		// ---------------
		public void Execute()
			{
			if (this.binding == null)
				return;

			CFGUI.CreateUndo(this.undoLabel, this.undoObject);
				
			DigitalBinding			digiBinding		= (this.binding as DigitalBinding);
			AxisBinding				analogBinding	= (this.binding as AxisBinding);
			EmuTouchBinding			touchBinding	= (this.binding as EmuTouchBinding);
			MousePositionBinding 	mouseBinding	= (this.binding as MousePositionBinding);
			//JoystickNameBinding		joyNameBinding	= (this.binding as JoystickNameBinding);

			// Digi binding...

			if (digiBinding != null)
				{
				if (this.digiKey != KeyCode.None)
					{

					digiBinding.Enable();
	
					if (this.digiKeyElemId < 0)
						digiBinding.AddKey(this.digiKey);	
					else 	
						digiBinding.ReplaceKey(this.digiKeyElemId, this.digiKey);
					}
	
				if (!string.IsNullOrEmpty(this.digiAxisName))
					{
					digiBinding.Enable();

					DigitalBinding.AxisElem elem = null;


					if (this.digiAxisElemId < 0)
						elem = digiBinding.AddAxis();	
					else 	
						elem = digiBinding.GetAxisElem(this.digiAxisElemId);

					if (elem != null)
						elem.SetAxis(this.digiAxisName, this.digiBindToPositiveAxis);
					} 
				}
				

			// Analog Binding...

			else if (analogBinding != null)
				{
				if (!string.IsNullOrEmpty(this.analogAxisName))
					{
					analogBinding.Enable();
					
					AxisBinding.TargetElem elem = null;

					if (this.analogElemId < 0)
						elem = analogBinding.AddTarget();
					else
						elem = analogBinding.GetTarget(this.analogElemId);

					if (elem != null)
						{
						if (this.analogSeparate)
							elem.SetSeparateAxis(this.analogAxisName, this.analogPositiveSide, !this.analogFlip);
						else
							elem.SetSingleAxis(this.analogAxisName, this.analogFlip);
						}
					}
				}

			// Touch Binding...

			else if (touchBinding != null)	
				{
				touchBinding.Enable();
				}

			// Mouse Binding...

			else if (mouseBinding != null)
				{
				mouseBinding.Enable();
				}


			CFGUI.EndUndo(this.undoObject);

			if (this.onRefreshCallback != null)
				this.onRefreshCallback();
			}



		// ----------------------
		static private void AddDigitalBindingKeySubMenu(GenericMenu menu, KeyCode key, BindingDescription desc, System.Action onRefreshCallback)
			{
			DigitalBinding digiBinding = (DigitalBinding)desc.binding;
				
			string menuPath = desc.menuPath + desc.nameFormatted + "/";
				
			for (int i = -1; i < digiBinding.keyList.Count; ++i)
				{
				UniversalBindingAssignment o = new UniversalBindingAssignment();
			
				o.undoLabel				= "Bind " + key + " key to " + desc.name;
				o.undoObject			= desc.undoObject;
				o.binding				= desc.binding;
				o.onRefreshCallback 	= onRefreshCallback;
				o.digiKey				= key;
				o.digiKeyElemId			= i;
	
				string menuItemLabel = "";

				if (i < 0)
					{
					menuItemLabel = "Add new as new key target.";
					}					
				else
					{
					menuItemLabel = "Replace \"" + digiBinding.keyList[i] + "\" (" + i + ").";
					}
				
				menu.AddItem(new GUIContent(menuPath + menuItemLabel), true, o.Execute);
	
				if ((i < 0) && (digiBinding.keyList.Count > 0))
					menu.AddSeparator(menuPath);
				}
			}
	
		// --------------------
		static private void AddDigitalBindingAxisSubMenu(GenericMenu menu, string axis, BindingDescription desc, System.Action onRefreshCallback)
			{	
			DigitalBinding digiBinding = (DigitalBinding)desc.binding;
				
			string menuPath = desc.menuPath + desc.nameFormatted + "/";
				
			for (int i = -1; i < digiBinding.axisList.Count; ++i)
				{
	
				string menuItemPath = "";

				DigitalBinding.AxisElem axisElem = digiBinding.GetAxisElem(i);

				if (i < 0)
					{
					menuItemPath = menuPath +  "Add as new axis target/";
					}					
				else
					{
					if (string.IsNullOrEmpty(axisElem.axisName))
						menuItemPath = menuPath + "Replace EMPTY (" + i + ")/";
					else
						menuItemPath = menuPath + "Replace \"" + axisElem.axisName + "\" (" + (axisElem.axisPositiveSide ? "Positive" : "Negative") + ") (" + i + ")/";
					}
				
				for (int sideIndex = 0; sideIndex < 2; ++sideIndex)
					{
					UniversalBindingAssignment o = new UniversalBindingAssignment();
				
					o.undoLabel				= "Bind " + axis + " axis to " + desc.name;
					o.undoObject			= desc.undoObject;
					o.binding				= desc.binding;
					o.onRefreshCallback 	= onRefreshCallback;
					o.digiAxisName			= axis;
					o.digiAxisElemId		= i;
					o.digiBindToPositiveAxis= ((sideIndex == 0) ? true : false);
	
					menu.AddItem(new GUIContent(menuItemPath + ((sideIndex == 0) ? "As Positive" : "As Negative")), false, o.Execute);
					}
			
	
				if ((i < 0) && (digiBinding.keyList.Count > 0))
					menu.AddSeparator(menuPath);
				}

			}

	
		// ------------------
		static private void AddAnalogBindingAxisSubMenu(GenericMenu menu, string axis, BindingDescription desc, System.Action onRefreshCallback)
			{
			AxisBinding bind = (AxisBinding)desc.binding;
				
			string menuPath = desc.menuPath + desc.nameFormatted + "/";
				
			for (int i = -1; i < bind.targetList.Count; ++i)
				{
	
				string menuItemPath = "";

				AxisBinding.TargetElem axisElem = bind.GetTarget(i);

				if (i < 0)
					{
					menuItemPath = menuPath +  "Add as new axis target/";
					}					
				else
					{
					if (axisElem.separateAxes)
						{
						menuItemPath = menuPath + "Replace separated(+,-) " +
							(string.IsNullOrEmpty(axisElem.positiveAxis) ? "EMPTY" : ("\"" + axisElem.positiveAxis + "\"" + 
								(axisElem.positiveAxisAsPositive ? "" : " (as negative)"))) +
							" : " +
							(string.IsNullOrEmpty(axisElem.negativeAxis) ? "EMPTY" : ("\"" + axisElem.negativeAxis + "\"" + 
								(axisElem.negativeAxisAsPositive ? "(as positive)" : ""))) +
							" (" + i + ")/";
						}
					else
						{
						if (string.IsNullOrEmpty(axisElem.singleAxis))
							menuItemPath = menuPath + "Replace single EMPTY (" + i + ")/";
						else
							menuItemPath = menuPath + "Replace single \"" + axisElem.singleAxis + "\"" + (axisElem.reverseSingleAxis ? " (Flipped)" : "") + " (" + i + ")/";
						}
					}
				
				for (int targetModeIndex = 0; targetModeIndex < 6; ++targetModeIndex)
					{
					UniversalBindingAssignment o = new UniversalBindingAssignment();
				
					o.undoLabel				= "Bind " + axis + " axis to " + desc.name;
					o.undoObject			= desc.undoObject;
					o.binding				= desc.binding;
					o.onRefreshCallback 	= onRefreshCallback;
					o.analogAxisName		= axis;
					o.analogElemId			= i;

					string modeName = "";

					switch (targetModeIndex)
						{
						case 0 :
							modeName 			= "Single";
							o.analogSeparate	= false;	
							o.analogFlip		= false;
							break;

						case 1 :
							modeName 			= "Single (Flipped)";
							o.analogSeparate	= false;	
							o.analogFlip		= true;
							break;
	
						case 2 :
							modeName 			= "Separated, Positive Side";
							o.analogSeparate	= true;
							o.analogPositiveSide= true;	
							o.analogFlip		= false;
							break;
						case 3 :
							modeName 			= "Separated, Positive Side (Flipped)";
							o.analogSeparate	= true;
							o.analogPositiveSide= true;	
							o.analogFlip		= true;
							break;

						case 4 :
							modeName 			= "Separated, Negative Side";
							o.analogSeparate	= true;
							o.analogPositiveSide= false;	
							o.analogFlip		= false;
							break;
						case 5 :
							modeName 			= "Separated, Negative Side (Flipped)";
							o.analogSeparate	= true;
							o.analogPositiveSide= false;	
							o.analogFlip		= true;
							break;							
						}
					
	
					menu.AddItem(new GUIContent(menuItemPath + modeName), false, o.Execute);

					if ((targetModeIndex & 1) == 1)
						menu.AddSeparator(menuItemPath);

					}
			
	
				if ((i < 0) && (bind.targetList.Count > 0))
					menu.AddSeparator(menuPath);
				}
			}
		}


	}
}

#endif

