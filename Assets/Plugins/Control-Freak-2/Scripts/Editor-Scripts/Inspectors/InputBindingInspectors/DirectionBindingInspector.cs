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
// Direction Binding Inspector.
// ----------------------
public class DirectionBindingInspector
	{	
	private GUIContent			labelContent;
	private Object				undoObject;
	private DigitalBindingInspector
		dirN,
		dirAny,
		dirU,
		dirUR,
		dirR,
		dirDR,
		dirD,
		dirDL,
		dirL,
		dirUL;
		


	private System.Action
		customPreGUI;


	// ------------------
	public DirectionBindingInspector(/*Editor editor,*/ Object undoObject, GUIContent labelContent)
		{
		this.labelContent	= labelContent;
		this.undoObject	= undoObject;
		this.dirN	= new DigitalBindingInspector(undoObject, new GUIContent("Neutral", 	"Neutral direction binding."));
		this.dirAny	= new DigitalBindingInspector(undoObject, new GUIContent("Any Non-neutral", 	"Any Non-neutral direction binding."));
		this.dirU	= new DigitalBindingInspector(undoObject, new GUIContent("Up", 			CFEditorStyles.Inst.arrowUpTex,		"Up direction binding."));
		this.dirUR	= new DigitalBindingInspector(undoObject, new GUIContent("Up-right", 	CFEditorStyles.Inst.arrowUpRightTex,"Up-right diagonal direction binding."));
		this.dirR	= new DigitalBindingInspector(undoObject, new GUIContent("Right", 		CFEditorStyles.Inst.arrowRightTex,	"Right direction binding."));
		this.dirDR	= new DigitalBindingInspector(undoObject, new GUIContent("Down-right",	CFEditorStyles.Inst.arrowDownRightTex,"Down-right diagonal direction binding."));
		this.dirD	= new DigitalBindingInspector(undoObject, new GUIContent("Down", 		CFEditorStyles.Inst.arrowDownTex,	"Down direction binding."));
		this.dirDL	= new DigitalBindingInspector(undoObject, new GUIContent("Down-left", 	CFEditorStyles.Inst.arrowDownLeftTex,"Down-left diagonal direction binding."));
		this.dirL	= new DigitalBindingInspector(undoObject, new GUIContent("Left", 		CFEditorStyles.Inst.arrowLeftTex,	"Left direction binding."));
		this.dirUL	= new DigitalBindingInspector(undoObject, new GUIContent("Up-left", 	CFEditorStyles.Inst.arrowUpLeftTex,	"Up-left diagonal direction binding."));
		}

	
	// ---------------------	
	public void SetCustomPreGUI(System.Action callback)
		{
		this.customPreGUI = callback;
		}		


	// ------------------
	public void Draw(DirectionBinding bind, InputRig rig)
		{
		bool
			bindingEnabled	= bind.enabled,
			bindDiagonals	= bind.bindDiagonals;
		DirectionBinding.BindMode 
			bindMode		= bind.bindMode;

		EditorGUILayout.BeginVertical();

		if (bindingEnabled = EditorGUILayout.ToggleLeft(this.labelContent, bindingEnabled, GUILayout.MinWidth(30)))
			{

			CFGUI.BeginIndentedVertical(CFEditorStyles.Inst.transpSunkenBG);
				
				if (this.customPreGUI != null)
					this.customPreGUI();

				bindMode = (DirectionBinding.BindMode)CFGUI.EnumPopup(new GUIContent("Bind mode"),
					bindMode, 70);

				bindDiagonals = EditorGUILayout.ToggleLeft(new GUIContent("Bind diagonals", 
					"Bind diagonal directions separately. When turned off, diagonal directions are translated into combinations of main directions."), 	
					bindDiagonals, GUILayout.MinWidth(30));
									
				EditorGUILayout.Space();
					
				this.dirN.Draw(bind.dirBindingN, rig);
				this.dirAny.Draw(bind.dirBindingAny, rig);
	
				this.dirU.Draw(bind.dirBindingU, rig);
				if (bindDiagonals)	this.dirUR.Draw(bind.dirBindingUR, rig);
	
				this.dirR.Draw(bind.dirBindingR, rig);
				if (bindDiagonals)	this.dirDR.Draw(bind.dirBindingDR, rig);
	
				this.dirD.Draw(bind.dirBindingD, rig);
				if (bindDiagonals)	this.dirDL.Draw(bind.dirBindingDL, rig);
	
				this.dirL.Draw(bind.dirBindingL, rig);
				if (bindDiagonals)	this.dirUL.Draw(bind.dirBindingUL, rig);
	
			CFGUI.EndIndentedVertical();

			GUILayout.Space(InputBindingGUIUtils.VERT_MARGIN);
			}

		EditorGUILayout.EndVertical();


		if ((bindingEnabled	!= bind.enabled) ||
			(bindMode		!= bind.bindMode) ||
			(bindDiagonals	!= bind.bindDiagonals) )
			{
			CFGUI.CreateUndo("Direction Binding modification.", this.undoObject);
			
			bind.enabled		= bindingEnabled;
			bind.bindDiagonals	= bindDiagonals;
			bind.bindMode		= bindMode;

			CFGUI.EndUndo(this.undoObject);
			}
		}
	} 

	
		
}
#endif
