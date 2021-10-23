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
	
[CustomEditor(typeof(TouchControlPanelUnityUiAddOn))]
public class TouchControlPanelUnityUiAddOnInspector : Editor
	{

	// ---------------------
	void OnEnable()
		{
		}

	// ---------------
	public override void OnInspectorGUI()
		{
		TouchControlPanelUnityUiAddOn c = (TouchControlPanelUnityUiAddOn)this.target;

		if (c.IsConnectedToPanel())
			EditorGUILayout.HelpBox("Add-on connected to Touch Control Panel.", MessageType.Info);
		else
			EditorGUILayout.HelpBox("Add-on is not connected to Touch Control Panel!\nAttach this component to a game object with Touch Control Panel!", MessageType.Error);


		}
			
	
	

	}

		
}
#endif
