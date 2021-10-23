// -------------------------------------------
// Control Freak 2
// Copyright (C) 2013-2020 Dan's Game Tools
// http://DansGameTools.blogspot.com
// -------------------------------------------

#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

using System.Collections;
using System.Collections.Generic;

using ControlFreak2Editor;
using ControlFreak2;
using ControlFreak2.Internal;
using ControlFreak2Editor.Internal;

namespace ControlFreak2Editor.Inspectors
{
	
public static class InspectorUtils 
	{
	// -----------------
	static public GUIStyle SectionHeaderStyle	{ get { return CFEditorStyles.Inst.whiteBevelBG; } } 
	// -------------------
	static public Color SectionHeaderColor = new Color(0.8f, 0.8f, 0.8f, 1.0f);

	// --------------------
	static public GUIStyle SectionContentStyle	{ get { return CFEditorStyles.Inst.transpSunkenBG; } }	
		


		
	// ------------------
	static public bool DrawSectionHeader(GUIContent titleContent, ref bool foldedOut)
		{
		Color initialBgColor = GUI.backgroundColor;
		
		GUI.backgroundColor = SectionHeaderColor;
		EditorGUILayout.BeginHorizontal(SectionHeaderStyle);				
		GUI.backgroundColor = initialBgColor;
					
			foldedOut = GUILayout.Toggle(foldedOut, titleContent, CFEditorStyles.Inst.foldout, GUILayout.MinWidth(30), GUILayout.ExpandWidth(true));

			
		EditorGUILayout.EndHorizontal();
		
	
		return foldedOut;
		}


	// -----------------
	static public void BeginIndentedSection()
		{
		CFGUI.BeginIndentedVertical(CFEditorStyles.Inst.transpSunkenBG);
		}

	static public void BeginIndentedSection(GUIContent sectionTitle)
		{
		EditorGUILayout.LabelField(sectionTitle, CFEditorStyles.Inst.boldText, GUILayout.MinWidth(30), GUILayout.ExpandWidth(true));

		CFGUI.BeginIndentedVertical(CFEditorStyles.Inst.transpSunkenBG);
		}

	static public bool BeginIndentedSection(GUIContent sectionTitle, ref bool foldedOut)
		{
		if (!CFGUI.BoldFoldout(sectionTitle, ref foldedOut))
			return false;

		CFGUI.BeginIndentedVertical(CFEditorStyles.Inst.transpSunkenBG);

		return true;
		}

	static public bool BeginIndentedCheckboxSection(GUIContent sectionTitle, ref bool foldedOut)
		{
		if (!CFGUI.BoldCheckbox(sectionTitle, ref foldedOut))
			return false;

		CFGUI.BeginIndentedVertical(CFEditorStyles.Inst.transpSunkenBG);

		return true;
		}




	// -----------
	static public void EndIndentedSection()
		{
		CFGUI.EndIndentedVertical();
		}


		
	// --------------
	static public void DrawErrorBox(string errMsg)
		{
		EditorGUILayout.HelpBox(errMsg, MessageType.Error);
		}


	
	}
}

#endif
