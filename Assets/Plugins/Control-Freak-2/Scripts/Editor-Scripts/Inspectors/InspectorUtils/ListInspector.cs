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
	
abstract public class ListInspector
	{
	public const float BUTTON_WIDTH = 18;
		
	protected IList
		targetList;
	protected List<ElemInspector> 
		elemInspList;

	public Object	undoObject;

	public bool		isFoldedOut;
	public bool		isFoldable;
	
	// ------------------
	public ListInspector(Object undoObject, IList targetList)
		{
		this.elemInspList	= new List<ElemInspector>(16);
		this.targetList		= targetList;
		this.undoObject		= undoObject;
		this.isFoldable		= true;
		}



	// -----------------
	abstract public string GetUndoPrefix();
	abstract public GUIContent GetListTitleContent();
	abstract protected void OnNewElemClicked();
	abstract protected ElemInspector CreateElemInspector(object obj);
		
	
	// --------------------------
	public IList GetTargetList()	{ return this.targetList; }

	// ---------------
	protected void Rebuild()
		{
		this.elemInspList.Clear();

		for (int i = 0; i < this.targetList.Count; ++i)
			{
			this.elemInspList.Add(this.CreateElemInspector(this.targetList[i]));
			this.elemInspList[i].SetId(i);
			}
		}

	// ------------------
	public int GetElemCount()
		{
		return this.targetList.Count;
		}
		

	// ----------------
	private void ResetElemIds()
		{
		for (int i = 0; i < this.elemInspList.Count; ++i)
			this.elemInspList[i].SetId(i);
		}



	// -------------------	
	protected void AddNewElem(object targetObject)
		{
		//if (targetObject == null)
		//	return;
			
		CFGUI.CreateUndo(this.GetUndoPrefix() + "Add new elem.", this.undoObject);

		this.targetList.Insert(0, targetObject);
		this.elemInspList.Insert(0, this.CreateElemInspector(targetObject));

		this.ResetElemIds();

		CFGUI.EndUndo(this.undoObject);
		}


	// ----------------
	public void MoveElem(int elemId, bool moveUp)
		{
		if ((elemId < 0) || (elemId >= this.targetList.Count))
			{
			return;
			}			
	
		int targetId = elemId + (moveUp ? -1 : 1);
		if ((targetId < 0) || (targetId >= this.targetList.Count))	
			return;

		CFGUI.CreateUndo(this.GetUndoPrefix() + "Reorder ", this.undoObject);
			
		object elem = this.targetList[elemId];
		this.targetList.RemoveAt(elemId);

		ElemInspector elemInsp = this.elemInspList[elemId];
		this.elemInspList.RemoveAt(elemId);

		this.targetList.Insert(targetId, elem);
		this.elemInspList.Insert(targetId, elemInsp);

		this.ResetElemIds();	

		CFGUI.EndUndo(this.undoObject);	
	
		}		


	// ----------------
	public void DeleteElem(int elemId)
		{
		if ((elemId < 0) || (elemId >= this.targetList.Count))
			{
//Debug.LogError("Delete Elem ListInsp - out of range! " + elemId + "/" + this.targetList.Count); 
			return;
			}			
	

		CFGUI.CreateUndo(this.GetUndoPrefix() + "Delete elem", this.undoObject);

		this.targetList.RemoveAt(elemId);
		this.elemInspList.RemoveAt(elemId);
			
		this.ResetElemIds();	

		CFGUI.EndUndo(this.undoObject);
		}



	// ------------------
	public void DrawGUI()
		{
		if (this.elemInspList.Count != this.targetList.Count)
			{
//Debug.Log("[" + Time.frameCount + "] Rebuilding list onGUI (" + this.GetType() + ")!");
			this.Rebuild();
			}				

		EditorGUILayout.BeginVertical();
			
		// Draw header...

		Color initialBgColor = GUI.backgroundColor;
		
		GUI.backgroundColor = InspectorUtils.SectionHeaderColor;
		EditorGUILayout.BeginHorizontal(InspectorUtils.SectionHeaderStyle);				
		GUI.backgroundColor = initialBgColor;
					
			//this.isFoldedOut = EditorGUILayout.Foldout(this.isFoldedOut, this.GetListTitleContent(), CFEditorStyles.Inst.foldout, );
			if (this.isFoldable)
				this.isFoldedOut = GUILayout.Toggle(this.isFoldedOut, this.GetListTitleContent(), CFEditorStyles.Inst.foldout, GUILayout.ExpandWidth(true), GUILayout.MinWidth(30));
			else
				GUILayout.Label(this.GetListTitleContent(), GUILayout.ExpandWidth(true), GUILayout.MinWidth(30));

			EditorGUILayout.Space();

			if (GUILayout.Button(new GUIContent("", CFEditorStyles.Inst.createNewTex, "Add new element..."), CFEditorStyles.Inst.iconButtonStyle, GUILayout.Width(BUTTON_WIDTH)))
				this.OnNewElemClicked();
			
		EditorGUILayout.EndHorizontal();

		// Draw elements...
			
		if (this.isFoldedOut)
			{
	
			if (this.elemInspList.Count == 0)
				{
				CFGUI.BeginIndentedVertical(CFEditorStyles.Inst.transpSunkenBG);
					EditorGUILayout.LabelField("List is empty.", CFEditorStyles.Inst.centeredTextTranspBG);
				CFGUI.EndIndentedVertical();
				}
			else
				{
				CFGUI.BeginIndentedVertical();
		
				for (int i = 0; i < this.elemInspList.Count; ++i)
					{
					if (!this.elemInspList[i].DrawGUI())
						break;
					}
					
				CFGUI.EndIndentedVertical();
				}
			}

		EditorGUILayout.EndVertical();	
		}
		




	// -----------------
	// Base element Inspector class.
	// -----------------
	abstract public class ElemInspector
		{
		protected ListInspector 
			listInsp; 
		protected int 
			elemId;
		protected bool
			isFoldedOut;
			

		// -------------------	
		public ElemInspector(ListInspector listInsp)
			{
			this.listInsp = listInsp;
			}

		// -----------------
		abstract protected GUIContent GetElemTitleContent(); //object targetObj);
		abstract protected void DrawGUIContent(); //object targetObj);

		// ----------------
		virtual public bool IsFoldable()
			{
			return true;
			}
 
		// ------------------
		public void SetId(int id)
			{
			this.elemId = id;
			}
		
	
		// ---------------------
		protected virtual void DrawExtraHeaderGUI()
			{
			}

			
		// ---------------------
		protected bool DrawDefaultButtons() 
			{ return DrawDefaultButtons(true, true); } 

		protected bool DrawDefaultButtons(bool moveButtons, bool deleteButton)
			{
			bool retVal = true;
				
			if (moveButtons)
				{
				GUI.enabled = (this.elemId > 0);
				if (GUILayout.Button(new GUIContent(CFEditorStyles.Inst.texMoveUp, "Move element up"), CFEditorStyles.Inst.iconButtonStyle, GUILayout.Width(BUTTON_WIDTH)) && retVal)
					{
					retVal = false;
					this.listInsp.MoveElem(this.elemId, true);
					}
	
				GUI.enabled = (this.elemId < (this.listInsp.GetElemCount() - 1));
				if (GUILayout.Button(new GUIContent(CFEditorStyles.Inst.texMoveDown, "Move element down"), CFEditorStyles.Inst.iconButtonStyle, GUILayout.Width(BUTTON_WIDTH)) && retVal)
					{
					retVal = false;
					this.listInsp.MoveElem(this.elemId, false);
					}
				}

			if (deleteButton)
				{
				GUI.enabled = true;	
				if (GUILayout.Button(new GUIContent(CFEditorStyles.Inst.trashCanTex, "Delete"), CFEditorStyles.Inst.iconButtonStyle, GUILayout.Width(BUTTON_WIDTH)) && retVal)
					{
					retVal = false;
					this.listInsp.DeleteElem(this.elemId);
					}
				}

			GUI.enabled = true;

			return retVal;
			}


		// ------------------	
		public void DrawDefaultLabel()
			{
			if (this.IsFoldable())
				this.isFoldedOut = GUILayout.Toggle(this.isFoldedOut, this.GetElemTitleContent(), CFEditorStyles.Inst.foldout, GUILayout.MinWidth(30), GUILayout.ExpandWidth(true));
			else
				{
				this.isFoldedOut = true;
				
				EditorGUILayout.LabelField(this.GetElemTitleContent(), GUILayout.MinWidth(30), GUILayout.ExpandWidth(true));
				}
			}

	
		// --------------------
		virtual public bool DrawGUI()
			{
			bool retVal = true;

			EditorGUILayout.BeginHorizontal(CFEditorStyles.Inst.transpBevelBG);

			this.DrawDefaultLabel();

			this.DrawExtraHeaderGUI();

			retVal = this.DrawDefaultButtons();
	
			EditorGUILayout.EndHorizontal();

			if (this.isFoldedOut && retVal)	
				{
				CFGUI.BeginIndentedVertical(CFEditorStyles.Inst.transpSunkenBG);
					this.DrawGUIContent();
				CFGUI.EndIndentedVertical();
				}					

			return retVal;
			}
		}
	}

		
}
#endif
