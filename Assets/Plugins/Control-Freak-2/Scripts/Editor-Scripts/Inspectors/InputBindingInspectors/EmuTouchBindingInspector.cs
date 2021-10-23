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
// Emulated Touch Binding Inspector.
// ----------------------
public class EmuTouchBindingInspector
	{	
	private GUIContent				labelContent;
	private Object					undoObject;

	// ------------------
	public EmuTouchBindingInspector(Object undoObject, GUIContent labelContent)
		{	
		this.labelContent			= labelContent;
		this.undoObject				= undoObject;
		}


	// ------------------
	public void Draw(EmuTouchBinding bind, InputRig rig)
		{
		bool		bindingEnabled	= bind.enabled;
	
		EditorGUILayout.BeginVertical();
		if (bindingEnabled = EditorGUILayout.ToggleLeft(this.labelContent, bindingEnabled, GUILayout.MinWidth(30)))
			{
			}
		EditorGUILayout.EndVertical();
		
		
		if ((bindingEnabled != bind.enabled) )
			{		
			CFGUI.CreateUndo("Emu Touch Binding modification.", this.undoObject);
			
			bind.enabled		= bindingEnabled;
					
			CFGUI.EndUndo(this.undoObject);
			}

		}
	}

		
}
#endif
