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
// Input Rig's Axis name field drawer.
// ----------------------
public class RigAxisNameDrawer
	{
	private int		cachedAxisId;
	private string	menuSelectedName;
	private InputRig.InputSource inputSource;

		
	// -----------------
	public RigAxisNameDrawer(InputRig.InputSource inputSource)
		{			
		this.inputSource = inputSource;
		this.menuSelectedName = null;
		}

		

	// -----------------
	public void ChangeInputSource(InputRig.InputSource inputSource)
		{
		this.inputSource = inputSource;
		}


	// ------------------
	private void ShowContextMenu(string curName, InputRig rig)
		{
		if (rig == null)
			return;


		GenericMenu menu = new GenericMenu();
			
		menu.AddItem(new GUIContent("Select rig"), false, this.OnMenuSelectRig, rig);

		//menu.AddDisabledItem(new GUIContent("Available axes:"));
		
			
		InputRig.AxisConfig axisConfig = rig.GetAxisConfig(curName, ref this.cachedAxisId);
		if ((curName.Length > 0) && (axisConfig == null))
			{
			menu.AddSeparator("");		
			menu.AddItem(new GUIContent("Create \"" + curName + "\" Digital Axis"), 			false, this.OnMenuCreateAxis, new AxisCreationParams(rig, curName, InputRig.AxisType.Digital));
			menu.AddItem(new GUIContent("Create \"" + curName + "\" Unsigned Analog Axis"), 	false, this.OnMenuCreateAxis, new AxisCreationParams(rig, curName, InputRig.AxisType.UnsignedAnalog));
			menu.AddItem(new GUIContent("Create \"" + curName + "\" Signed Analog Axis"),		false, this.OnMenuCreateAxis, new AxisCreationParams(rig, curName, InputRig.AxisType.SignedAnalog));
			menu.AddItem(new GUIContent("Create \"" + curName + "\" Delta"), 						false, this.OnMenuCreateAxis, new AxisCreationParams(rig, curName, InputRig.AxisType.Delta));
			menu.AddItem(new GUIContent("Create \"" + curName + "\" Scroll"), 					false, this.OnMenuCreateAxis, new AxisCreationParams(rig, curName, InputRig.AxisType.ScrollWheel));
			}

	
		menu.AddSeparator("");		
		for (int i = 0; i < rig.axes.list.Count; ++i)
			{
			InputRig.AxisConfig axis = rig.axes.list[i];
				
			if (axis.DoesSupportInputSource(this.inputSource))
				menu.AddItem(new GUIContent("Use \"" + axis.name + "\""), (axis.name == curName), this.OnMenuNameSelected, axis.name);
			}	

		menu.ShowAsContext();
		}

	// -------------------
	private void OnMenuNameSelected(object name)
		{
		this.menuSelectedName = name as string;
		}
		
		

	// -------------------
	private void OnMenuCreateAxis(object axisParamsObj)
		{
		AxisCreationParams axisParams = (AxisCreationParams)axisParamsObj;
		if (axisParams.rig == null)
			return;
			
		axisParams.rig.axes.Add(axisParams.name, axisParams.axisType, true);
		}


	// -------------------
	private struct AxisCreationParams
		{
		public InputRig rig;
		public string name;
		public InputRig.AxisType axisType;

		// -----------------
		public AxisCreationParams(InputRig rig, string name, InputRig.AxisType axisType)
			{
			this.rig = rig;
			this.name = name;
			this.axisType = axisType;
			}
		}



	
	// --------------------
	private void OnMenuSelectRig(object rig)
		{
		if (rig != null)
			Selection.activeObject = rig as InputRig;
		}

		
	// ------------------	
	public string Draw(string labelText, string curName, InputRig rig)
		{ return Draw(new GUIContent(labelText, "Target Axis Name"), curName, rig); }
		
	public string Draw(GUIContent labelContent, string curName, InputRig rig)
		{
		EditorGUILayout.BeginHorizontal();

		if (labelContent.text.Length > 0)
			EditorGUILayout.LabelField(labelContent); //, GUILayout.Width(40));
		string s = EditorGUILayout.TextField(curName, GUILayout.MinWidth(20), GUILayout.ExpandWidth(true));

		bool buttonPressed = false;

		InputRig.AxisConfig axisConfig = (rig == null) ? null : rig.GetAxisConfig(s, ref this.cachedAxisId);
			
		float buttonWidth = 16;
		//float buttonHeight = 16;
			
		GUIContent buttonContent; 

		if (rig == null)
			buttonContent = new GUIContent(string.Empty, CFEditorStyles.Inst.texError, "No rig attached!");
		else if (axisConfig == null)	
			buttonContent = new GUIContent(string.Empty, CFEditorStyles.Inst.texError, "Axis not found!"); //, CFEditorStyles.Inst.iconError);
		else if (!axisConfig.DoesSupportInputSource(this.inputSource))
			buttonContent = new GUIContent(string.Empty, CFEditorStyles.Inst.texWarning, "Target axis is incompatible!"); //, CFEditorStyles.Inst.iconOkWarning);
		else
			buttonContent = new GUIContent(string.Empty, CFEditorStyles.Inst.texOk, "Axis name is ok!"); //, CFEditorStyles.Inst.iconOk);
			
		buttonPressed = GUILayout.Button(buttonContent, CFEditorStyles.Inst.iconButtonStyle, GUILayout.Width(buttonWidth)); //, GUILayout.Height(buttonHeight));
			

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
