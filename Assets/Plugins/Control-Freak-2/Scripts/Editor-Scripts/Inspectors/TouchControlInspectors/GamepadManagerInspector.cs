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
	
[CustomEditor(typeof(ControlFreak2.GamepadManager))]
public class GamepadManagerInspector : Editor
	{
	public AnalogConfigInspector
		leftStickConfig,
		rightStickConfig,
		dpadConfig,
		leftTriggerConfig,
		rightTiggerConfig;




	// ---------------------
	void OnEnable()
		{
		}

	// ---------------
	public override void OnInspectorGUI()
		{
		GamepadManager m = (GamepadManager)this.target;

	
		// Gamepad Manager GUI...
			
		float 
			connectionCheckInterval	= m.connectionCheckInterval;
		bool 
			dontDestroyOnLoad		= m.dontDestroyOnLoad;

			
		GUILayout.Box(GUIContent.none, CFEditorStyles.Inst.headerGamepadManager, GUILayout.ExpandWidth(true));


		EditorGUILayout.Space();

		InspectorUtils.BeginIndentedSection(new GUIContent("Settings"));

			connectionCheckInterval = CFGUI.FloatFieldEx(new GUIContent("Connection Check Interval (ms)", "Gamepad connection Check Interval in milliseconds.\nEach call of Input.GetJoystickNames() allocates at least 40 bytes - that's not much, but..."),
				connectionCheckInterval, 0, 10, 1000, true, 190);
	
			dontDestroyOnLoad = EditorGUILayout.ToggleLeft(new GUIContent("Don't destroy on load.", "This game object will not be destroyed when new scene/level is loaded."),
				dontDestroyOnLoad);
		
		InspectorUtils.EndIndentedSection();

	
		EditorGUILayout.Space();


		// Register undo...

		if (//(gamepadOrderMode			!= m.gamepadOrderMode) 
			(connectionCheckInterval	!= m.connectionCheckInterval) ||
			(dontDestroyOnLoad			!= m.dontDestroyOnLoad)
 
			)
			{

			CFGUI.CreateUndo("CF2 Gamepad Manager modification", m);

			//m.gamepadOrderMode		 = gamepadOrderMode;
				
			m.connectionCheckInterval	= connectionCheckInterval;
			m.dontDestroyOnLoad			= dontDestroyOnLoad;

			CFGUI.EndUndo(m);
			}
		}
			
	
	

	}

		
}
#endif
