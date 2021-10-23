// -------------------------------------------
// Control Freak 2
// Copyright (C) 2013-2020 Dan's Game Tools
// http://DansGameTools.blogspot.com
// -------------------------------------------

#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

using ControlFreak2Editor;
using ControlFreak2;
using ControlFreak2.Internal;
using ControlFreak2Editor.Internal;

namespace ControlFreak2Editor.Inspectors
{


// ----------------------
// Analog Axis Binding Inspector.
// ----------------------
public class AxisBindingInspector
	{	
	public GUIContent			labelContent;
	public Object				undoObject;
	//private Editor				editor;
	public bool					allowAxisSeparation;
	private System.Action		customExtraGUI;

	public InputRig.InputSource 
		inputSource;

	private List<TargetElemInspector>
		targetElemInspList;

	// ------------------
	public AxisBindingInspector(/*Editor editor, */Object undoObject, GUIContent labelContent, bool allowAxisSeparation, InputRig.InputSource inputSource, 
		System.Action customExtraGUI = null)
		{
		this.labelContent			= labelContent;
		this.allowAxisSeparation	= allowAxisSeparation;
		this.undoObject				= undoObject;
		this.inputSource			= inputSource;

		this.customExtraGUI = customExtraGUI;
		}

		
	// ------------------	
	public void ChangeInputSource(InputRig.InputSource source)
		{
		if (this.targetElemInspList != null)
			{
			for (int i = 0; i < this.targetElemInspList.Count; ++i)
				this.targetElemInspList[i].SetInputSource(source);
			}

		this.inputSource = source;
		}
		
		


	// ------------------------
	private void PrepareTargetElemListInspectors(int count)
		{	
		if ((this.targetElemInspList != null) && (this.targetElemInspList.Count == count))
			return;

		if (this.targetElemInspList == null)
			this.targetElemInspList = new List<TargetElemInspector>(Mathf.Max(2, count));
	
		this.targetElemInspList.Clear();

		for (int i = 0; i < count; ++i)
			{
			this.targetElemInspList.Add(new TargetElemInspector(this));
			}		

		this.ChangeInputSource(this.inputSource);
		}


	// ------------------
	public void Draw(AxisBinding bind, InputRig rig)
		{
		bool		bindingEnabled	= bind.enabled;

		EditorGUILayout.BeginVertical();
			EditorGUILayout.BeginHorizontal();
			
			bindingEnabled = EditorGUILayout.ToggleLeft(this.labelContent, bindingEnabled, GUILayout.MinWidth(30));
			

			if (bindingEnabled)
				{
				if (GUILayout.Button(new GUIContent(CFEditorStyles.Inst.texPlusSign, "Add target"), CFEditorStyles.Inst.iconButtonStyle)) //, GUILayout.Width(20), GUILayout.Height(20)))
					{
					CFGUI.CreateUndo("Add new target to Analog Binding.", this.undoObject);					
					bind.AddTarget();
					CFGUI.EndUndo(this.undoObject);
					}
				
				if (GUILayout.Button(new GUIContent(CFEditorStyles.Inst.texMinusSign, "Remove target"), CFEditorStyles.Inst.iconButtonStyle)) //, GUILayout.Width(20), GUILayout.Height(20)))
					{
					CFGUI.CreateUndo("Remove target from Analog Binding.", this.undoObject);					
					bind.RemoveLastTarget();
					CFGUI.EndUndo(this.undoObject);
					}

				}


			EditorGUILayout.EndHorizontal();

		if (bindingEnabled)
			{	
			CFGUI.BeginIndentedVertical(CFEditorStyles.Inst.transpSunkenBG);


			if (this.customExtraGUI != null)
				this.customExtraGUI();

				
			this.PrepareTargetElemListInspectors(bind.targetList.Count);
	
			if (bind.targetList.Count == 0)
				{
				EditorGUILayout.LabelField("No targets defined...", CFEditorStyles.Inst.centeredTextTranspBG);
				}
			else
				{
				for (int i = 0; i < bind.targetList.Count; ++i)
					{
					this.targetElemInspList[i].DrawGUI(bind.targetList[i], rig,  (i == (bind.targetList.Count - 1)));
					}
				}
				
			CFGUI.EndIndentedVertical();
			}
		EditorGUILayout.EndVertical();
			
		
		if ((bindingEnabled	!=	bind.enabled) 
			)
			{		
			CFGUI.CreateUndo("Analog Axis Binding modification.", this.undoObject);
			
			bind.enabled			= bindingEnabled;
					
			CFGUI.EndUndo(this.undoObject);
			}
		}



	//-------------------------
	// Target Element Inspector.
	// -------------------------
	private class TargetElemInspector
		{
		private AxisBindingInspector
			parent;
		private RigAxisNameDrawer
			singleAxisField,
			positiveAxisField,
			negativeAxisField;
	
			
		// ------------------
		public TargetElemInspector(AxisBindingInspector parent)
			{
			this.parent		= parent;
			this.singleAxisField	= new RigAxisNameDrawer(parent.inputSource);
			this.positiveAxisField	= new RigAxisNameDrawer(parent.inputSource);
			this.negativeAxisField	= new RigAxisNameDrawer(parent.inputSource);
			}

		// ---------------------
		public void SetInputSource(InputRig.InputSource	source)
			{
			this.singleAxisField.ChangeInputSource(source);
			this.positiveAxisField.ChangeInputSource(source);
			this.negativeAxisField.ChangeInputSource(source);
			}

		// ---------------------
		public void DrawGUI(AxisBinding.TargetElem target, InputRig rig, bool isLast)
			{
			bool		separateAxes		= target.separateAxes;
			string		singleAxis			= target.singleAxis;
			bool		reverseSingleAxis	= target.reverseSingleAxis;
	
			string		positiveAxis		= target.positiveAxis;
			string		negativeAxis		= target.negativeAxis;
			bool		positiveAsPositive  = target.positiveAxisAsPositive;
			bool		negativeAsPositive  = target.negativeAxisAsPositive;
	
			if (this.parent.allowAxisSeparation)
				EditorGUILayout.BeginHorizontal(CFEditorStyles.Inst.transpSunkenBG);
			else
				EditorGUILayout.BeginHorizontal();
				
					{
					if (!this.parent.allowAxisSeparation)
						separateAxes = false;
					else
						{
						EditorGUILayout.BeginVertical(GUILayout.Width(20));
						
							GUILayout.Space(separateAxes ? 12 : 3);						

							separateAxes = CFGUI.PushButton(new GUIContent("", CFEditorStyles.Inst.sepAxesOnTex, "Separate axes for positive and negative values."),
								new GUIContent("", CFEditorStyles.Inst.sepAxesOffTex, "Single target axis."), separateAxes, CFEditorStyles.Inst.buttonStyle, //CFEditorStyles.Inst.iconButtonStyle, 
								GUILayout.Width(18), GUILayout.Height(18)); //.ExpandHeight(true), GUILayout.ExpandWidth(true));
												

						EditorGUILayout.EndVertical();
						}
					}
	
				{	
				EditorGUILayout.BeginVertical(CFEditorStyles.Inst.transpSunkenBG);
	
		
				if (!separateAxes)
					{
					EditorGUILayout.BeginHorizontal();
		
						reverseSingleAxis = CFGUI.PushButton(new GUIContent("", CFEditorStyles.Inst.texMinusSign, "Reverse source value."),
							new GUIContent("", CFEditorStyles.Inst.texPlusSign, "Don't reverse source value."), reverseSingleAxis, CFEditorStyles.Inst.iconButtonStyle);
	
						singleAxis = this.singleAxisField.Draw("", singleAxis, rig);
					EditorGUILayout.EndHorizontal();
					}
				else
					{
						EditorGUILayout.BeginHorizontal();
						
							EditorGUILayout.LabelField(new GUIContent("Axis +", "Posiitive value axis"), GUILayout.Width(40));

							positiveAsPositive = CFGUI.PushButton(new GUIContent("", CFEditorStyles.Inst.texPlusSign, "Send positive values as positive to target axis."),
								new GUIContent("", CFEditorStyles.Inst.texMinusSign, "Send positive values as negative."), positiveAsPositive, CFEditorStyles.Inst.iconButtonStyle);
		
							positiveAxis = this.positiveAxisField.Draw("", positiveAxis, rig);
						EditorGUILayout.EndHorizontal();
	
	
	
						EditorGUILayout.BeginHorizontal();
						
							EditorGUILayout.LabelField(new GUIContent("Axis -", "Negative value axis"), GUILayout.Width(40));
		
							negativeAsPositive = CFGUI.PushButton(new GUIContent("", CFEditorStyles.Inst.texPlusSign, "Send negative values as positive to target axis."),
								new GUIContent("", CFEditorStyles.Inst.texMinusSign, "Send negative values as negative."), negativeAsPositive, CFEditorStyles.Inst.iconButtonStyle);
								//EditorGUILayout.ToggleLeft(new GUIContent("-", "Activate the negative side of the axis."), axisNegSide, 
		
							negativeAxis = this.negativeAxisField.Draw("", negativeAxis, rig);
						EditorGUILayout.EndHorizontal();
						
					}
				
					
				EditorGUILayout.EndVertical();	
				}

			EditorGUILayout.EndHorizontal();
				
			
			if (//(bindingEnabled	!=	bind.enabled) ||
				(separateAxes		!= target.separateAxes)	||
				(singleAxis			!= target.singleAxis)	||
				(reverseSingleAxis	!= target.reverseSingleAxis) ||
				(positiveAxis		!= target.positiveAxis)	||
				(negativeAxis		!= target.negativeAxis)	||
				(positiveAsPositive	!= target.positiveAxisAsPositive)	||
				(negativeAsPositive	!= target.negativeAxisAsPositive)	)
				{		
				CFGUI.CreateUndo("Analog Axis Binding modification.", this.parent.undoObject);
				
				//bind.enabled			= bindingEnabled;
				target.separateAxes				= separateAxes;
				target.singleAxis				= singleAxis;
				target.positiveAxis				= positiveAxis;
				target.negativeAxis				= negativeAxis;
				target.positiveAxisAsPositive	= positiveAsPositive;
				target.negativeAxisAsPositive	= negativeAsPositive;
				target.reverseSingleAxis		= reverseSingleAxis;
						
				CFGUI.EndUndo(this.parent.undoObject);
				}
			
			}
		}
	} 


		
}
#endif
