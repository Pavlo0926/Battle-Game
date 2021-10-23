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
public class RigJoystickNameDrawer
	{
	//private Editor	editor;
	private int		cachedJoyId;
	private string	menuSelectedName;

		
	// -----------------
	public RigJoystickNameDrawer(/*Editor editor*/)
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
		
			

		if ((curName.Length > 0) && !rig.IsJoystickDefined(curName, ref this.cachedJoyId))
			{
			menu.AddSeparator("");		
			menu.AddItem(new GUIContent("Create \"" + curName + "\" joy config"), 	false, this.OnMenuCreateJoy, new JoyCreationParams(rig, curName));
			}



		menu.AddSeparator("");		
	
		foreach (InputRig.VirtualJoystickConfig joy in rig.joysticks.list)
			menu.AddItem(new GUIContent("Use \""  + joy.name + "\""), (joy.name == curName), this.OnMenuNameSelected, joy.name);
				

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
	private void OnMenuCreateJoy(object joyParamsObj)
		{
		JoyCreationParams joyParams = (JoyCreationParams)joyParamsObj;
		if (joyParams.rig == null)
			return;
			
		joyParams.rig.joysticks.Add(joyParams.name, true);
		}


	// -------------------
	private struct JoyCreationParams
		{
		public InputRig rig;
		public string name;

		// -----------------
		public JoyCreationParams(InputRig rig, string name)
			{
			this.rig = rig;
			this.name = name;
			}
		}




	// ------------------	
	public string Draw(string label, string curName, InputRig rig)
		{
		EditorGUILayout.BeginHorizontal();

		string s = EditorGUILayout.TextField(label, curName, GUILayout.MinWidth(30));

		bool buttonPressed = false;

		if (rig == null)
			GUILayout.Button(new GUIContent(string.Empty, "No rig attached!"), CFEditorStyles.Inst.iconError);
		else if (!rig.IsJoystickDefined(s, ref this.cachedJoyId))	
			buttonPressed = GUILayout.Button(new GUIContent(string.Empty, "Joystick not found!"), CFEditorStyles.Inst.iconError);
		else
			buttonPressed = GUILayout.Button(new GUIContent(string.Empty, "Joystick name is ok!"), CFEditorStyles.Inst.iconOk);

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
			}

		return s;
		}
	}
	

		
}
#endif
