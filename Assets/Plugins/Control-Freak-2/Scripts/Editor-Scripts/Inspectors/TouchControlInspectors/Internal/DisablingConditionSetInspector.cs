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

namespace ControlFreak2Editor.Inspectors
{

public class DisablingConditionSetInspector 
	{
	public Object 
		undoObject;
	public GUIContent 
		titleContent;	
	public DisablingConditionSet
		target;

	public DisablingRigSwitchSetInspector
		switchListInsp;

		


	// -----------------------
	public DisablingConditionSetInspector(GUIContent titleContent, DisablingConditionSet target, InputRig rig, Object undoObject)
		{
		this.undoObject = undoObject;
		this.titleContent = titleContent;
		this.target	= target;

		this.switchListInsp = new DisablingRigSwitchSetInspector(target, rig, undoObject);
		}

	
	// -----------------------
	public void DrawGUI()
		{
		bool 
			disableWhenTouchScreenInactive	= this.target.disableWhenTouchScreenInactive,
			disableWhenCursorIsUnlocked		= this.target.disableWhenCursorIsUnlocked;
			
		DisablingConditionSet.MobileModeRelation
			mobileModeRelation			= this.target.mobileModeRelation;


		InspectorUtils.BeginIndentedSection(this.titleContent);
	
			mobileModeRelation	= (DisablingConditionSet.MobileModeRelation)CFGUI.EnumPopup(new GUIContent("Mobile Mode Relation"), 
				mobileModeRelation, 130);

			disableWhenCursorIsUnlocked = EditorGUILayout.ToggleLeft(new GUIContent("Disable when Cursor is unlocked", "Disable this element when mouse cursor is unlocked. \nUse this option if your game is using cursor lock state to display menus and things like that..."), 
				disableWhenCursorIsUnlocked);
			disableWhenTouchScreenInactive = EditorGUILayout.ToggleLeft(new GUIContent("Disable when Touch Screen is inactive", "Disable this element when touch screen hasn't been used for some time (Check out Input Rig's General Settings)."), 
				disableWhenTouchScreenInactive);
	
			EditorGUILayout.Space();
	
			if (this.switchListInsp != null)
				this.switchListInsp.DrawGUI();
		

		InspectorUtils.EndIndentedSection();

	
				
		// Refister undo...
	
		if ((mobileModeRelation				!= this.target.mobileModeRelation) ||
			(disableWhenCursorIsUnlocked	!= this.target.disableWhenCursorIsUnlocked) ||
			(disableWhenTouchScreenInactive	!= this.target.disableWhenTouchScreenInactive) )
			{
	
			CFGUI.CreateUndo("Disabling Conditions modification", this.undoObject);
		
			this.target.mobileModeRelation				= mobileModeRelation;
			this.target.disableWhenCursorIsUnlocked		= disableWhenCursorIsUnlocked;
			this.target.disableWhenTouchScreenInactive	= disableWhenTouchScreenInactive;
	
			CFGUI.EndUndo(this.undoObject);
	
			}
		}


	// -----------------------
	// Hidding Conditions list Inspector.
	// ------------------------
	
	public class DisablingRigSwitchSetInspector : ListInspector
		{
		private string
			actionName,
			title;
	
		private InputRig
			rig;
	
		// -------------------
		public DisablingRigSwitchSetInspector(DisablingConditionSet target, InputRig rig, Object undoObject, 
			string title = "Disabling Switches", string actionName = "Disable") : base(undoObject, target.switchList)
			{			
			this.title		= title;
			this.actionName	= actionName;
	
			this.isFoldedOut	= true;
			this.isFoldable		= false;
				
			this.rig = rig;
	
			this.Rebuild();
			}
	
		// -------------------
		override public string GetUndoPrefix()								{	return (this.title + " - "); }
		override public GUIContent GetListTitleContent()					{	return (new UnityEngine.GUIContent(this.title + " (" + this.GetElemCount() + ")"));}
		override protected ElemInspector CreateElemInspector(object obj)	{	return new DisablingRigSwitchElemInspector((DisablingConditionSet.DisablingRigSwitch)obj, this.rig, this); }
	
		// -------------------
		override protected void OnNewElemClicked()
			{
			this.AddNewElem(new DisablingConditionSet.DisablingRigSwitch(""));
			}
	
	
			
		// ---------------------
		// Rig Switch Elem Inspector.		
		// ----------------------	
		private class DisablingRigSwitchElemInspector : ListInspector.ElemInspector
			{
			public InputRig
				rig;
			//public Object
			//	undoObject;
			public DisablingConditionSet.DisablingRigSwitch
				target;
				
			private RigSwitchNameDrawer
				flagNameInsp;
							
	
			// -------------------
			public DisablingRigSwitchElemInspector(DisablingConditionSet.DisablingRigSwitch obj, InputRig rig, DisablingRigSwitchSetInspector listInsp) : base(listInsp)
				{
				this.rig		= rig;
				this.target 	= obj;
				//this.undoObject = undoObject;
					
				this.flagNameInsp = new RigSwitchNameDrawer(); //listInsp.mainEditor);
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
				bool	disableWhenSwitchIsOff = this.target.disableWhenSwitchIsOff;
					
				
	
				EditorGUILayout.BeginHorizontal(CFEditorStyles.Inst.transpBevelBG);
	
				// Keyboard input...
	
					
					string actionName = ((DisablingRigSwitchSetInspector)this.listInsp).actionName;
	
					disableWhenSwitchIsOff = CFGUI.PushButton(new GUIContent("When OFF", actionName + " When OFF"), new GUIContent("When ON", actionName + " when ON"), 
						disableWhenSwitchIsOff, CFEditorStyles.Inst.buttonStyle, GUILayout.Width(80));
	
					name = this.flagNameInsp.Draw("", name, this.rig, 0);
	
		
					GUILayout.Space(10);
					
					
					
				if (!this.DrawDefaultButtons(false, true))
					retVal = false;
					
	
				EditorGUILayout.EndHorizontal();
	
					
				if (retVal)
					{
					if ((name 			!= this.target.name) ||
						(disableWhenSwitchIsOff	!= this.target.disableWhenSwitchIsOff) )
						{
						CFGUI.CreateUndo("Disabling Switch modification", this.listInsp.undoObject);
	
						this.target.name			= name;
						this.target.disableWhenSwitchIsOff	= disableWhenSwitchIsOff;
	
						CFGUI.EndUndo(this.listInsp.undoObject);
						}
					}
	
				return retVal;
				}
	
			} 
		}
		
	}
}

#endif
