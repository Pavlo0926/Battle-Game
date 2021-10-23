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
	
[CustomEditor(typeof(ControlFreak2.TouchSplitter))]
public class TouchSplitterInspector : TouchControlInspectorBase
	{
	public SplitterTargetControlListInspector
		targetControlListInsp;



	// ---------------------
	void OnEnable()
		{
		this.targetControlListInsp = new SplitterTargetControlListInspector(new GUIContent("Target Control List"), (TouchSplitter)this.target); //, typeof(TouchControl), ((TouchSplitter)this.target).targetControlList);

		base.InitTouchControlInspector();
		}

	// ---------------
	public override void OnInspectorGUI()
		{
		TouchSplitter c = (TouchSplitter)this.target;

		this.DrawWarnings(c);			
	
		

		GUILayout.Box(GUIContent.none, CFEditorStyles.Inst.headerTouchSplitter, GUILayout.ExpandWidth(true));

		// Steering Wheel specific inspector....

		InspectorUtils.BeginIndentedSection(new GUIContent("Target Controls"));
			
			this.targetControlListInsp.DrawGUI();

		InspectorUtils.EndIndentedSection();



		// Draw Shared Touch Control Params...

		this.DrawTouchContolGUI(c);
		}
			
	
	// ------------------
	public class SplitterTargetControlListInspector : ObjectListInspector
		{
		public TouchSplitter
			splitter;

		// ----------------
		public SplitterTargetControlListInspector(GUIContent titleContent, TouchSplitter splitter) : 
			base(titleContent, splitter, typeof(TouchControl), splitter.targetControlList)
			{
			this.splitter = splitter;	
			}

		// ---------------
		override protected Object HandleObjectChange(Object originalObj, Object newObj)
			{
			if (newObj == this.splitter)
				return ((originalObj == this.splitter) ? null : originalObj);
				
			if (this.splitter.targetControlList.Contains((TouchControl)newObj))
				return originalObj;
			
			return newObj;
			}	
			
		}



	}

		
}
#endif
