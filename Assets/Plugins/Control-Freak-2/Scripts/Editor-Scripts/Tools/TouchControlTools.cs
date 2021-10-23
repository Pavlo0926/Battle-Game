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


namespace ControlFreak2Editor
{

static public class TouchControlTools
	{
	// ---------------
	[MenuItem("GameObject/CF2 Transform Tools/Unify selection's depth (Move to front)")]
	static public void UnifySelectionDepthFront()
		{ ModifyTransforms(Selection.transforms, ControlTransformMode.UnifyDepthFront); }	

	// ---------------
	[MenuItem("GameObject/CF2 Transform Tools/Unify selection's depth (Move to center)")]
	static public void UnifySelectionDepthCenter()
		{ ModifyTransforms(Selection.transforms, ControlTransformMode.UnifyDepthCenter); }	

	// ---------------
	[MenuItem("GameObject/CF2 Transform Tools/Unify selection's depth (Move to back)")]
	static public void UnifySelectionDepthBack()
		{ ModifyTransforms(Selection.transforms, ControlTransformMode.UnifyDepthBack); }	

	// ---------------
	[MenuItem("GameObject/CF2 Transform Tools/Round Depth to nearest integer")]
	static public void SnapSelectionDepth()
		{ ModifyTransforms(Selection.transforms, ControlTransformMode.SnapDepth); }	



	// -------------------
	public enum ControlTransformMode	
		{
		SnapDepth,

		UnifyDepthFront,
		UnifyDepthCenter,
		UnifyDepthBack
		}



	// ------------------------
	static public void ModifyTransforms(Transform[] trList, ControlTransformMode mode)
		{
		if (trList.Length <= 0)
			{
			Debug.Log("No touch controls selected...");
			return; 
			}					

	
		Bounds bb = new Bounds();
		
		for (int i = 0; i < trList.Length; ++i)
			{
			Transform t = trList[i];
	
			if (i == 0)
				bb.SetMinMax(t.position, t.position);
			else
				bb.Encapsulate(t.position);
			}



		CFGUI.CreateUndoMulti("Modify Transforms : " + mode, trList);

		for (int i = 0; i < trList.Length; ++i)
			{
			Transform t = trList[i];	
			Vector3 pos = t.position;

			switch (mode)
				{
				case ControlTransformMode.SnapDepth :
					pos.z = Mathf.Round(pos.z);
					break;

				case ControlTransformMode.UnifyDepthBack : 	
					pos.z = bb.min.z;
					break;

				case ControlTransformMode.UnifyDepthCenter : 	
					pos.z = bb.center.z;
					break;

				case ControlTransformMode.UnifyDepthFront :
					pos.z = bb.max.z;
					break;	
				
				}
	
			t.position = pos;
			}
		
		CFGUI.EndUndoMulti(trList);
		}


	}
}

#endif
