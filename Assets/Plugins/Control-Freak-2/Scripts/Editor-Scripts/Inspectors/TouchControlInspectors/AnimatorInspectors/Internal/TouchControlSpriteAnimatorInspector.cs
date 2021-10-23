// -------------------------------------------
// Control Freak 2
// Copyright (C) 2013-2020 Dan's Game Tools
// http://DansGameTools.blogspot.com
// -------------------------------------------

#if UNITY_EDITOR

using UnityEngine;
using UnityEditor;

using ControlFreak2.Internal;
using ControlFreak2;

namespace ControlFreak2Editor.Inspectors
{
public class TouchControlSpriteAnimatorInspector 
	{
	
	// -------------------
	static public bool DrawSourceGUI(TouchControlSpriteAnimatorBase target)
		{
		bool 
			autoConnect 		= target.autoConnectToSource;
		TouchControl	
			sourceControl 		= target.sourceControl;
			

		bool initiallyEnabled = GUI.enabled;	

		InspectorUtils.BeginIndentedSection(new GUIContent("Source Control Connection"));

		GUI.enabled = (sourceControl != null);
		if (GUILayout.Button(new GUIContent("Select Source Control"), GUILayout.ExpandWidth(true), GUILayout.Height(20)))
			{
			Selection.activeObject = sourceControl;
			return false;	
			}		

		GUI.enabled = initiallyEnabled;

		autoConnect = EditorGUILayout.ToggleLeft(new GUIContent("Auto Connect To Control", "When enabled, this animator will automatically pick source control whenever this gameobject's hierarchy changes."), 
			autoConnect, GUILayout.MinWidth(30), GUILayout.ExpandWidth(true));

		if (autoConnect)
			GUI.enabled = false;

		sourceControl = (TouchControl)EditorGUILayout.ObjectField(new GUIContent("Source Control"), sourceControl, target.GetSourceControlType(), true,
			GUILayout.MaxWidth(30), GUILayout.ExpandWidth(true));		
	
		GUI.enabled = initiallyEnabled; 

		if (sourceControl == null)
			InspectorUtils.DrawErrorBox("Source Control is not connected!");
		else if (target.IsIllegallyAttachedToSource())
			InspectorUtils.DrawErrorBox("This Animator is attached to the source control's game object!!\nTransformation Animation will not be possible!!");

		InspectorUtils.EndIndentedSection();


		// Register Undo...

		if ((autoConnect != target.autoConnectToSource) ||
			(sourceControl != target.sourceControl))
			{
			CFGUI.CreateUndo("Sprite Animator Source modification", target);

			target.autoConnectToSource 	= autoConnect;
			target.SetSourceControl(sourceControl);
				
			if (target.autoConnectToSource)
				target.AutoConnectToSource();

			CFGUI.EndUndo(target);
			}

		return true;		
		}
	
	
	}
}

#endif
