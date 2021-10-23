// -------------------------------------------
// Control Freak 2
// Copyright (C) 2013-2020 Dan's Game Tools
// http://DansGameTools.blogspot.com
// -------------------------------------------

#if UNITY_EDITOR

using UnityEngine;
using UnityEditor;
using System.Collections.Generic;


namespace ControlFreak2Editor.Internal
{
	
public class TreeView : TreeViewElem
	{
	public float indentInc;

	public TreeViewElem selectedElem;

	public Vector2 scrollPos;

	public int		treeElemDrawCount;


	// ---------------
	public TreeView() : base(null)
		{
		this.indentInc = 16;
		this.view = this;
		}
	

	// ------------------
	public bool IsEmpty()
		{ return (this.children.Count == 0); }
		
	// ----------------
	public int GetElemDrawCount() 
		{ return this.treeElemDrawCount; }
		

	// ----------------------
	public void DrawTreeGUI()
		{
		this.scrollPos = EditorGUILayout.BeginScrollView(this.scrollPos, GUILayout.ExpandWidth(true));
					
		this.treeElemDrawCount = 0;

		this.DrawGUI(0);

		EditorGUILayout.EndScrollView();
		}
	
	// ---------------------
	public void Select(TreeViewElem elem)
		{
		this.selectedElem = elem;
		}

	}



// ------------------
public class TreeViewElem
	{
	public TreeView 			view;
	public string				name;
	public TreeViewElem			parent;
	public List<TreeViewElem>	children;
	public bool					isFoldedOut;
	protected bool				dirtyFlag;
		
	// -------------------
	public TreeViewElem(TreeView view)
		{
		this.isFoldedOut = true;
		this.view = view;
		this.children = new List<TreeViewElem>(8);
		}
		

	public enum TriState
		{
		True,
		False,
		Mixed
		}
	

	// ------------------
	virtual protected void	OnSetState(int stateId, int state)		{ }
	virtual protected int	OnGetState(int stateId, int prevState)	{ return prevState; }
	virtual protected void	OnDrawGUI(float indent)					{ }
	virtual protected void	OnInvalidate()							{ }
	virtual protected bool	IsVisible()								{ return true; }
		

	// ------------------
	public TreeViewElem Find(string path)
		{ 
		int separatorPos		= path.IndexOf("/");
		string curLevelName		= ((separatorPos < 0) ? path : path.Substring(0, separatorPos));

		foreach (TreeViewElem c in this.children)
			{
			if (c.name.Equals(curLevelName, System.StringComparison.OrdinalIgnoreCase))
				return ((separatorPos < 0) ? c : c.Find(path.Substring(separatorPos))); 
			}

		return null;
		}
	
		

	// -------------------
	public void AddChild(TreeViewElem elem)
		{
		if ((elem.parent != null) && (elem.parent != this))
			elem.parent.RemoveChild(elem);
 
		this.children.Add(elem);
		elem.parent = this;
		}
		

	// -----------------
	public void RemoveChild(TreeViewElem elem)
		{
		this.children.Remove(elem);
		}


	// ----------------		
	public void SetState(int stateId, int state)
		{
		this.SetStateRecursively(stateId, state);

		this.InvalidateDownwards(false);
		}

	// --------------------
	private void SetStateRecursively(int stateId, int state)
		{
		this.OnSetState(stateId, state);

		for (int i = 0; i < this.children.Count; ++i)
			this.children[i].SetState(stateId, state);
		}
		

	// -------------------
	public int GetState(int stateId, int undefinedValue, int defaultValue)
		{
		int v = this.GetStateRecursively(stateId, undefinedValue);
		return ((v == undefinedValue) ? defaultValue : v);
		}

	// -------------------
	private int GetStateRecursively(int stateId, int curValue) //undefinedValue, int defaultValue)
		{
//		int curValue = undefinedValue;

		if (this.children.Count == 0)
			return this.OnGetState(stateId, curValue); // ? TriState.TRUE : TriState.FALSE);
			
		//int state = curValue; //TriState.FALSE;
			
		for (int i = 0; i < this.children.Count; ++i)
			{
			curValue = this.children[i].GetStateRecursively(stateId, curValue); //, undefinedValue);

			}
			
	//	if (curValue == undefinedValue)
	//		return defaultValue;
		
		return curValue; //state;
		}




	// ------------------
	public void SetDirtyFlag()
		{
		for (TreeViewElem elem = this; elem != null; elem = elem.parent)
			elem.dirtyFlag = true;
		}

	

	
	// ------------------
	protected void DrawGUI(float indent)
		{
		if (this.IsVisible())
			{
			this.OnDrawGUI(indent);
			++this.view.treeElemDrawCount;
			}				

		if (!this.isFoldedOut)
			return;

		if (this != this.view)
			indent += this.view.indentInc;
		
		for (int i = 0; i < this.children.Count; ++i)
			this.children[i].DrawGUI(indent);
		}
		

	// --------------------
	public void InvalidateUpwards(bool forced)
		{
		if (!forced && !this.dirtyFlag)
			return;

		this.OnInvalidate();
		this.dirtyFlag = false;

		for (int i = 0; i < this.children.Count; ++i)
			this.children[i].InvalidateUpwards(forced);
		}

	// ------------------
	public void InvalidateDownwards(bool forced)
		{
		if (!forced && !this.dirtyFlag)
			return;

		this.OnInvalidate();
		this.dirtyFlag = false;

		if (this.parent != null)
			this.parent.InvalidateDownwards(forced);
		}


	// ------------------
	public delegate TreeViewElem ElemConstructor(TreeViewElem root, string elemPath, string elemName);

	// -------------------
	static public TreeViewElem CreateDirectoryStructure(TreeViewElem root, string fullPath, 
		ElemConstructor folderConstructor)
		{
		if (fullPath.IndexOf('\\') >= 0)
			fullPath = fullPath.Replace('\\', '/');
			

		int startPos = 0;
		int sepPos = 0;
			
		TreeViewElem parent = root;

		for (; (startPos < fullPath.Length); startPos = (sepPos + 1))
			{
			sepPos = fullPath.IndexOf("/", startPos);
			if (sepPos == startPos)
				continue;
				
			if (sepPos < 0) 
				sepPos = fullPath.Length;

			string folderName = fullPath.Substring(startPos, (sepPos - startPos));

//Debug.Log("\tCreate sub folder [" + folderName + "] of [" + path + "]");

			TreeViewElem folderElem = parent.Find(folderName); // as FolderElem;
			if (folderElem == null)
				{
//Debug.Log("\tNOT found...");
				folderElem = folderConstructor(root, fullPath.Substring(0, sepPos), folderName);
			
				folderElem.name = folderName;
				folderElem.parent = parent;

				parent.AddChild(folderElem);
				}	
			else
				{
//Debug.Log("\tFOUND!");

				}
				

			parent = folderElem;
			}
			
		return parent;
		}

		
	}

}



#endif
