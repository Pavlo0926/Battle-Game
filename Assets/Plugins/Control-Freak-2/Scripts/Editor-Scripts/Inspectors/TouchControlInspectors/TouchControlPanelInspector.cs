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
[CustomEditor(typeof(ControlFreak2.TouchControlPanel))]
public class TouchControlPanelInspector : Editor
	{

	//public TouchControlPanel 
	//	panel;



	// ---------------------
	void OnEnable()
		{
		}

	// ---------------
	public override void OnInspectorGUI()
		{
		TouchControlPanel panel = (TouchControlPanel)this.target;
			

		GUILayout.Box(GUIContent.none, CFEditorStyles.Inst.headerTouchPanel, GUILayout.ExpandWidth(true));
		
		if (panel.rig == null)
			InspectorUtils.DrawErrorBox("This panel is not connected to a Input Rig!");			
			


		if (GUILayout.Button("Add Button"))	
			TouchButtonCreationWizard.ShowWizard(panel);

		if (GUILayout.Button("Add Joystick"))	
			TouchJoystickCreationWizard.ShowWizard(panel);

		if (GUILayout.Button("Add Steering Wheel"))	
			TouchWheelCreationWizard.ShowWizard(panel);

		if (GUILayout.Button("Add Trackpad"))	
			TouchTrackPadCreationWizard.ShowWizard(panel);

		if (GUILayout.Button("Add Touch Zone"))	
			SuperTouchZoneCreationWizard.ShowWizard(panel);

		if (GUILayout.Button("Add Touch Splitter"))	
			TouchSplitterCreationWizard.ShowWizard(panel);


		// Settings GUI...			

		}	
		
	}

}

#endif
