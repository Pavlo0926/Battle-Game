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


// ---------------------
// Touch Splitter Wizard	
// ----------------------
public class TouchSplitterCreationWizard : NonDynamicControlWizardBase
	{
	// -----------------	
	public TouchSplitterCreationWizard() : base()
		{	
		}
		

	// ----------------
	override protected void DrawPresentationGUI()
		{
		}
	


	// ----------------
	static public void ShowWizard(TouchControlPanel panel, System.Action onCreationCallback = null)
		{
		TouchSplitterCreationWizard w = (TouchSplitterCreationWizard)EditorWindow.GetWindow(typeof(TouchSplitterCreationWizard), true, "CF2 Touch Splitter Wizard");
		if (w != null)
			{
			w.InitWizard(panel, onCreationCallback);
			w.ShowPopup();
			}
		}			


		

	// ------------------
	protected void InitWizard(TouchControlPanel panel, System.Action onCreationCallback)
		{
		base.Init(panel, onCreationCallback);

		this.controlName = TouchControlWizardUtils.GetUniqueTouchSplitterName(this.panel.rig);
		this.defaultControlName = this.controlName;
			
		this.controlDepth = 60;
		this.regionDepth = 60; 

		this.positionMode = NonDynamicControlWizardBase.PositionMode.ConstantSize;

		}


	// ------------------
	protected override void DrawHeader ()
		{
		GUILayout.Box(GUIContent.none, CFEditorStyles.Inst.headerTouchSplitter, GUILayout.ExpandWidth(true));
		}


	// ---------------------
	override protected void DrawBindingGUI()
		{
		}


	// --------------------
	override protected void OnCreatePressed(bool selectAfterwards)
		{
		ControlFreak2.TouchSplitter newObj = (ControlFreak2.TouchSplitter)this.CreateNonDynamicTouchControl(typeof(ControlFreak2.TouchSplitter));


		Undo.RegisterCreatedObjectUndo(newObj.gameObject, "Create CF2 Touch Splitter");	
		
		if (selectAfterwards)
			Selection.activeObject =  newObj;	
		}	


	}


}

#endif
