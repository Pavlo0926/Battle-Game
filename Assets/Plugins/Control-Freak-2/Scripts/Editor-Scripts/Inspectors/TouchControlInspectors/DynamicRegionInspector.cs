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
	
[CustomEditor(typeof(DynamicRegion))]
public class DynamicRegionInspector : TouchControlInspectorBase
	{

	// ---------------------
	public DynamicRegionInspector() : base()
		{
		}



	// ---------------------
	void OnEnable()
		{
		base.InitTouchControlInspector();
		}


	// ---------------
	public override void OnInspectorGUI()
		{
		DynamicRegion c = (DynamicRegion)this.target;
			
		GUILayout.Box(GUIContent.none, CFEditorStyles.Inst.headerDynamicRegion, GUILayout.ExpandWidth(true));

		this.DrawWarnings(c);			

		DynamicTouchControl tc = c.GetTargetControl();
	
		if (tc == null)
			{
			EditorGUILayout.HelpBox("There's no control linked to this Dynamic Region!", MessageType.Warning);
			}
		else
			{
			EditorGUILayout.HelpBox("Region linked to [" + tc.name + "]!", MessageType.Info);
			}

		this.DrawTouchContolGUI(c);
			
		}
			
	
	


	

	}

		
}
#endif
