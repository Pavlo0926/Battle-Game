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
	
[CustomEditor(typeof(ControlFreak2.TouchButtonSpriteAnimator))]
public class TouchButtonSpriteAnimatorInspector : UnityEditor.Editor
	{
	private SpriteConfigInspector
		spriteNeutral,
		spritePressed,
		spriteToggled,
		spriteToggledAndPressed;
	
	


	// ---------------------
	void OnEnable()
		{
		this.spriteNeutral 				= new SpriteConfigInspector(new GUIContent("Neutral State", "Neutral Sprite and Color"));
		this.spritePressed 				= new SpriteConfigInspector(new GUIContent("Pressed State", "Pressed Sprite and Color"));
		this.spriteToggled 				= new SpriteConfigInspector(new GUIContent("Toggled State", "Toggled Sprite and Color"));
		this.spriteToggledAndPressed	= new SpriteConfigInspector(new GUIContent("Toggled And Pressed State", "Toggled And Pressed Sprite and Color"));
		
		}

		
	// ---------------
	public override void OnInspectorGUI()
		{

		TouchButtonSpriteAnimator target = this.target as TouchButtonSpriteAnimator;
		if ((target == null))
			return;
			
		


		if (!TouchControlSpriteAnimatorInspector.DrawSourceGUI(target))
			return;



		if (target.sourceControl == null)
			{ } //InspectorUtils.DrawErrorBox("Source Button is not connected!");
		else
			{	
			InspectorUtils.BeginIndentedSection(new GUIContent("Sprite Settings"));
				

			

			this.spriteNeutral.Draw(target.spriteNeutral, target, true, false);

				{
				EditorGUILayout.Space();
				this.spritePressed.Draw(target.spritePressed, target, target.IsIllegallyAttachedToSource());
					
				if (((TouchButton)target.sourceControl).toggle)
					{
					EditorGUILayout.Space();
					this.spriteToggled.Draw(target.spriteToggled, target, target.IsIllegallyAttachedToSource());
	
					EditorGUILayout.Space();
					this.spriteToggledAndPressed.Draw(target.spriteToggledAndPressed, target, target.IsIllegallyAttachedToSource());
					}
				}

			InspectorUtils.EndIndentedSection();
			}	
			
		
		}

	
	
	}

		
}
#endif
