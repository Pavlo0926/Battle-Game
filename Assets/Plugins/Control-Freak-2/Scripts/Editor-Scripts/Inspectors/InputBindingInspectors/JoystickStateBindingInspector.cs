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
// Joystick State Binding Inspector.
// ----------------------
public class JoystickStateBindingInspector
	{	
	private GUIContent			labelContent;
	private Object				undoObject;
	private DirectionBindingInspector
		dirInspector;
	private AxisBindingInspector
		horzAxisInspector,
		vertAxisInspector;


	// ------------------
	public JoystickStateBindingInspector(Object undoObject, GUIContent labelContent)
		{
		this.labelContent	= labelContent;
		this.undoObject	= undoObject;
		this.dirInspector = new DirectionBindingInspector(undoObject, new GUIContent("Bind Joy direction", 	"Bind Joystick's digital directions to axes and/or keys."));
		this.horzAxisInspector = new AxisBindingInspector(undoObject, new GUIContent("Bind Horz. axis", "Bind Joystick's horizontal tilt to an axis."), true, InputRig.InputSource.Analog);
		this.vertAxisInspector = new AxisBindingInspector(undoObject, new GUIContent("Bind Vert. axis", "Bind Joystick's vertical tilt to an axis."), true, InputRig.InputSource.Analog);
		}



	// ------------------
	public void Draw(JoystickStateBinding bind, InputRig rig)
		{
		bool	bindingEnabled	= bind.enabled;

		EditorGUILayout.BeginVertical();

		if (bindingEnabled = EditorGUILayout.ToggleLeft(this.labelContent, bindingEnabled, GUILayout.MinWidth(30)))
			{
			InspectorUtils.BeginIndentedSection();

				this.horzAxisInspector.Draw(bind.horzAxisBinding, rig);
				this.vertAxisInspector.Draw(bind.vertAxisBinding, rig);

				
				this.dirInspector.Draw(bind.dirBinding, rig);

			InspectorUtils.EndIndentedSection();

			GUILayout.Space(InputBindingGUIUtils.VERT_MARGIN);

			}

		EditorGUILayout.EndVertical();


		if ((bindingEnabled	!= bind.enabled) )
			{
			CFGUI.CreateUndo("Joy State Binding modification.", this.undoObject);
			
			bind.enabled		= bindingEnabled;
			
			CFGUI.EndUndo(this.undoObject);
			}
		}
	} 


		
}
#endif
