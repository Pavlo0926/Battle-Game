// -------------------------------------------
// Control Freak 2
// Copyright (C) 2013-2020 Dan's Game Tools
// http://DansGameTools.blogspot.com
// -------------------------------------------

#if UNITY_EDITOR

using UnityEngine;
using UnityEditor;

using System.Collections;

using ControlFreak2;
using ControlFreak2.Internal;

using ControlFreak2Editor;
using ControlFreak2Editor.Internal;

namespace ControlFreak2Editor.Inspectors
{

// -----------------------
// Object List Inspector.
// ------------------------

public class ObjectListInspector : ListInspector
	{
	private GUIContent
		titleContent;

	private System.Type
		objType;

		
	// -----------------
	virtual protected Object HandleObjectChange(Object origianlObj, Object newObj)
		{
		return newObj;
		}

	// -------------------
	public ObjectListInspector(GUIContent titleContent, Object undoObject, System.Type objType, IList targetList) : base(undoObject, targetList)
		{			
		this.titleContent = titleContent;

		this.objType = objType;

		this.isFoldedOut	= true;
		this.isFoldable		= false;
			
		this.Rebuild();
		}

	// -------------------
	override public string GetUndoPrefix()								{	return (this.titleContent.text + " - "); }
	override public GUIContent GetListTitleContent()					{	return (this.titleContent); }

	override protected ElemInspector CreateElemInspector(object obj)	{	return new ObjectElemInspector(this); }

	// -------------------
	override protected void OnNewElemClicked()
		{
		this.AddNewElem(null);
		}


		
	// ---------------------
	// Object Elem Inspector.		
	// ----------------------	
	private class ObjectElemInspector : ListInspector.ElemInspector
		{
		// -------------------
		public ObjectElemInspector(ObjectListInspector listInsp) : base(listInsp)
			{
			}

		// --------------
		override protected GUIContent GetElemTitleContent()
			{
			return (GUIContent.none);
			}

		// ---------------
		override public bool IsFoldable()
			{
			return false;
			}
			
		// ----------------
		override protected void DrawGUIContent()
			{
			}

		// --------------
		override public bool DrawGUI() //object targetObj)
			{
			bool retVal = true;

			Object originalObj = (Object)this.listInsp.GetTargetList()[this.elemId];
			Object obj = originalObj;				

			EditorGUILayout.BeginHorizontal(CFEditorStyles.Inst.transpBevelBG);
				
				obj = EditorGUILayout.ObjectField(obj, ((ObjectListInspector)this.listInsp).objType, true, GUILayout.ExpandWidth(true));
				
			if (!this.DrawDefaultButtons(false, true))
				retVal = false;
				

			EditorGUILayout.EndHorizontal();

				
			if (retVal)
				{
				if (obj != originalObj)
					obj = ((ObjectListInspector)this.listInsp).HandleObjectChange(originalObj, obj);

				if ((obj != originalObj) )
					{
					CFGUI.CreateUndo(this.listInsp.GetUndoPrefix() + "Modification", this.listInsp.undoObject);

					this.listInsp.GetTargetList()[this.elemId] = obj;

					CFGUI.EndUndo(this.listInsp.undoObject);
					}
				}

			return retVal;
			}

		} 
	}
	

}

#endif

