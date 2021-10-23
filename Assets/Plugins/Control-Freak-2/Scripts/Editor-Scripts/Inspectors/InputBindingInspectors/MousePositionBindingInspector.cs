
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
// Emulated Mouse Position Binding Inspector.
// ----------------------
public class MousePositionBindingInspector
	{	
	private GUIContent				labelContent;
	private Object					undoObject;

	// ------------------
	public MousePositionBindingInspector(Object undoObject, GUIContent labelContent)
		{	
		this.labelContent			= labelContent;
		this.undoObject				= undoObject;
		}


	// ------------------
	public void Draw( MousePositionBinding bind, InputRig rig)
		{
		bool		bindingEnabled	= bind.enabled;
		int 		priority		= bind.priority;
	
		EditorGUILayout.BeginVertical();
		if (bindingEnabled = EditorGUILayout.ToggleLeft(this.labelContent, bindingEnabled, GUILayout.MinWidth(30)))
			{
			InspectorUtils.BeginIndentedSection();

			priority = EditorGUILayout.IntSlider(new GUIContent("Pos. prio.", "Position priority used to pick mouse position if there is more than one mouse position source in the rig at given frame."),
				priority, 0, 100, GUILayout.MinWidth(30));

			InspectorUtils.EndIndentedSection();

			}
		EditorGUILayout.EndVertical();
		
		
		if ((bindingEnabled != bind.enabled) ||
			(priority		!=	bind.priority) )
			{		
			CFGUI.CreateUndo("Mouse Position Binding modification.", this.undoObject);
			
			bind.enabled		= bindingEnabled;
			bind.priority		= priority;
					
			CFGUI.EndUndo(this.undoObject);
			}

		}
	}

		
}
#endif
