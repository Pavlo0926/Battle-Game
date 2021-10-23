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
// Rig's Joystick name field drawer.
// ----------------------
public class RigSwitchNameDrawer
	{
	//private Editor	editor;
	private int		cachedId;
	private string	menuSelectedName;

		
	// -----------------
	public RigSwitchNameDrawer(/*Editor editor*/)
		{			
		//this.editor = editor;
		this.menuSelectedName = null;
		}



	// ------------------
	private void ShowContextMenu(string curName, InputRig rig)
		{
		if (rig == null)
			return;


		GenericMenu menu = new GenericMenu();
			
		menu.AddItem(new GUIContent("Select rig"), false, this.OnMenuSelectRig, rig);

		//menu.AddDisabledItem(new GUIContent("Available axes:"));
		
			

		if ((curName.Length > 0) && !rig.IsSwitchDefined(curName, ref this.cachedId))
			{
			menu.AddSeparator("");		
			menu.AddItem(new GUIContent("Create \"" + curName + "\" rig switch"), 	false, this.OnMenuCreateNew, new SwitchCreationParams(rig, curName));
			}



		menu.AddSeparator("");		
	
		foreach (InputRig.RigSwitch rigSwitch in rig.rigSwitches.list)
			menu.AddItem(new GUIContent("Use \""  + rigSwitch.name + "\""), (rigSwitch.name == curName), this.OnMenuNameSelected, rigSwitch.name);
				

		menu.ShowAsContext();
		}

	// -------------------
	private void OnMenuNameSelected(object name)
		{
		this.menuSelectedName = name as string;
		}
		
	// --------------------
	private void OnMenuSelectRig(object rig)
		{
		if (rig != null)
			Selection.activeObject = rig as InputRig;
		}

	// -------------------
	private void OnMenuCreateNew(object switchParamsObj)
		{
		SwitchCreationParams switchParams = (SwitchCreationParams)switchParamsObj;
		if (switchParams.rig == null)
			return;
			
		switchParams.rig.rigSwitches.Add(switchParams.name, true);
		}


	// -------------------
	private struct SwitchCreationParams
		{
		public InputRig rig;
		public string name;

		// -----------------
		public SwitchCreationParams(InputRig rig, string name)
			{
			this.rig = rig;
			this.name = name;
			}
		}




	// ------------------	
	public string Draw(string label, string curName, InputRig rig, float labelWidth)
		{
		EditorGUILayout.BeginHorizontal();
			

		if (label.Length > 0)
			{
			if (labelWidth > 0)
				EditorGUILayout.LabelField(label, GUILayout.Width(labelWidth));
			else
				EditorGUILayout.LabelField(label);
			}					

		string s = EditorGUILayout.TextField("", curName, GUILayout.MinWidth(30));

		bool buttonPressed = false;

		if (rig == null)
			GUILayout.Button(new GUIContent(string.Empty, "No rig attached!"), CFEditorStyles.Inst.iconError);
		else if (!rig.IsSwitchDefined(s, ref this.cachedId))	
			buttonPressed = GUILayout.Button(new GUIContent(string.Empty, "Switch not found!"), CFEditorStyles.Inst.iconError);
		else
			buttonPressed = GUILayout.Button(new GUIContent(string.Empty, "Switch name is ok!"), CFEditorStyles.Inst.iconOk);

		EditorGUILayout.EndHorizontal();  
		
		// Show context menu...
	
		if (buttonPressed)
			this.ShowContextMenu(s, rig);

		// Apply the name selected via context menu..
 
		if (this.menuSelectedName != null)
			{
			s = this.menuSelectedName;
			this.menuSelectedName = null;

			EditorGUI.FocusTextInControl("");

			//if (this.editor != null)
			//	this.editor.Repaint();
			}

		return s;
		}
	}
	

		
}
#endif
