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
// Digital Binding Inspector.
// ----------------------
public class DigitalBindingInspector
	{	
	private GUIContent			labelContent;
	private Object				undoObject;
		
	List<AxisElemInspector>	
		axisElemInspList;


	// ------------------
	public DigitalBindingInspector(/*Editor editor, */ Object undoObject, GUIContent labelContent)
		{
		this.labelContent	= labelContent;
		this.undoObject	= undoObject;
		this.axisElemInspList = new List<AxisElemInspector>(2);
		}


	// ---------------------
	private void PrepareAxisElemInspList(int count)
		{
		if ((this.axisElemInspList != null) && (this.axisElemInspList.Count == count))
			return;

		this.axisElemInspList.Clear();
		for (int i = 0; i < count; ++i)
			this.axisElemInspList.Add(new AxisElemInspector(this));
		}



	// ------------------
	public void Draw(DigitalBinding bind, InputRig rig)
		{
		bool	bindingEnabled	= bind.enabled;

		EditorGUILayout.BeginVertical();

		if (bindingEnabled = EditorGUILayout.ToggleLeft(this.labelContent, bindingEnabled, GUILayout.MinWidth(30)))
			{

			CFGUI.BeginIndentedVertical(CFEditorStyles.Inst.transpSunkenBG);
					

				EditorGUILayout.BeginVertical(CFEditorStyles.Inst.transpSunkenBG);
			
					EditorGUILayout.BeginHorizontal();
			
					EditorGUILayout.LabelField(new GUIContent("Axis Targets"), GUILayout.MinWidth(30), GUILayout.ExpandWidth(true));

						{
						
						if (GUILayout.Button(new GUIContent(CFEditorStyles.Inst.texPlusSign, "Add Axis Target"), CFEditorStyles.Inst.iconButtonStyle)) //, GUILayout.Width(20), GUILayout.Height(20)))
							{
							CFGUI.CreateUndo("Add new axis to Digital Binding.", this.undoObject);					
							bind.AddAxis();
							CFGUI.EndUndo(this.undoObject);
							}
						
						if (GUILayout.Button(new GUIContent(CFEditorStyles.Inst.texMinusSign, "Remove Axis Target"), CFEditorStyles.Inst.iconButtonStyle)) //, GUILayout.Width(20), GUILayout.Height(20)))
							{
							CFGUI.CreateUndo("Remove axis from Digital Binding.", this.undoObject);					
							bind.RemoveLastAxis();
							CFGUI.EndUndo(this.undoObject);
							}						
						}

					EditorGUILayout.EndHorizontal();

				
						{ 

						this.PrepareAxisElemInspList(bind.axisList.Count);

						if (bind.axisList.Count == 0)
							{
							EditorGUILayout.LabelField("No axis targets defined...", CFEditorStyles.Inst.centeredTextTranspBG);
							}
						else
							{
							for (int i = 0; i < bind.axisList.Count; ++i)
								{
								this.axisElemInspList[i].DrawGUI(bind.axisList[i], rig);							
								}
							}
						}
			
				EditorGUILayout.EndVertical();


				// Key targets...


				EditorGUILayout.BeginVertical(CFEditorStyles.Inst.transpSunkenBG);
					EditorGUILayout.BeginHorizontal();

					EditorGUILayout.LabelField(new GUIContent("Key Targets"), GUILayout.MinWidth(30), GUILayout.ExpandWidth(true));
						
						{
						
						if (GUILayout.Button(new GUIContent(CFEditorStyles.Inst.texPlusSign, "Add Key Target"), CFEditorStyles.Inst.iconButtonStyle)) //, GUILayout.Width(20), GUILayout.Height(20)))
							{
							CFGUI.CreateUndo("Add new key to Digital Binding.", this.undoObject);					
							bind.AddKey(KeyCode.None);
							CFGUI.EndUndo(this.undoObject);
							}
						
						if (GUILayout.Button(new GUIContent(CFEditorStyles.Inst.texMinusSign, "Remove Key Target"), CFEditorStyles.Inst.iconButtonStyle)) //GUILayout.Width(20), GUILayout.Height(20)))
							{
							CFGUI.CreateUndo("Remove key from Digital Binding.", this.undoObject);					
							bind.RemoveLastKey();
							CFGUI.EndUndo(this.undoObject);
							}						
						
						}

					EditorGUILayout.EndHorizontal();

						{ 
						if (bind.keyList.Count == 0)
							{
							EditorGUILayout.LabelField("No key targets defined...", CFEditorStyles.Inst.centeredTextTranspBG);
							}
						else
							{
							EditorGUILayout.BeginVertical(CFEditorStyles.Inst.transpSunkenBG);

							for (int i = 0; i < bind.keyList.Count; ++i)
								{
								KeyCode
									keyOriginal = bind.keyList[i],
									key			= keyOriginal;
	
								key = (KeyCode)EditorGUILayout.EnumPopup("" /*"KeyCode"*/, key, GUILayout.MinWidth(30), GUILayout.ExpandWidth(false));
	
								if (key != keyOriginal)	
									{
									CFGUI.CreateUndo("Digital Binding Key modification.", this.undoObject);
									bind.keyList[i] = key;
									CFGUI.EndUndo(this.undoObject);
									}
								}

							EditorGUILayout.EndVertical();
							}
						}
			
					
				EditorGUILayout.EndVertical();
			
						
			
				CFGUI.EndIndentedVertical();
			}

		EditorGUILayout.EndVertical();


		if ((bindingEnabled	!= bind.enabled) )
			{
			CFGUI.CreateUndo("Digital Binding modification.", this.undoObject);
			
			bind.enabled		= bindingEnabled;
			
			CFGUI.EndUndo(this.undoObject);
			}
		}



	// --------------------
	private class AxisElemInspector 
		{
		private DigitalBindingInspector
			parent;
		private RigAxisNameDrawer
			axisField;
			

		// --------------------
		public AxisElemInspector(DigitalBindingInspector parent)
			{
			this.parent = parent;
			this.axisField = new RigAxisNameDrawer(InputRig.InputSource.Digital);
			}
			

		// --------------------
		public void DrawGUI(DigitalBinding.AxisElem target, InputRig rig)
			{
			string	axisName		= target.axisName;
			bool	axisPositiveSide= target.axisPositiveSide;
	
			EditorGUILayout.BeginHorizontal(CFEditorStyles.Inst.transpSunkenBG);
	
				axisPositiveSide = CFGUI.PushButton(new GUIContent("", CFEditorStyles.Inst.texPlusSign, "Bind to positive side of target axis."),
					new GUIContent("", CFEditorStyles.Inst.texMinusSign, "Bind to negative side of target axis"), axisPositiveSide, CFEditorStyles.Inst.iconButtonStyle, GUILayout.Width(16));
	
				axisName = this.axisField.Draw("", axisName, rig);
						
			EditorGUILayout.EndHorizontal();
	
	
			if ((axisName			!= target.axisName) ||
				(axisPositiveSide	!= target.axisPositiveSide) )
				{
				CFGUI.CreateUndo("Digital Binding Axis modification.", this.parent.undoObject);
				
				target.axisName		= axisName;
				target.axisPositiveSide = axisPositiveSide;
				
				CFGUI.EndUndo(this.parent.undoObject);
				}
			}
		
		}
	} 
	
	



		
}
#endif
