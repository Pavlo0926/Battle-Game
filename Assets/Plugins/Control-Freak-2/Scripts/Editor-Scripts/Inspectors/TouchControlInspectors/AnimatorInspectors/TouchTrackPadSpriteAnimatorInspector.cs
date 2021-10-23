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
	
[CustomEditor(typeof(ControlFreak2.TouchTrackPadSpriteAnimator))]
public class TouchTrackPadSpriteAnimatorInspector : UnityEditor.Editor
	{
	private SpriteConfigInspector
		spriteNeutral,
		spritePressed;
	
	


	// ---------------------
	void OnEnable()
		{
		this.spriteNeutral 				= new SpriteConfigInspector(new GUIContent("Neutral State", "Neutral Sprite and Color"));
		this.spritePressed 				= new SpriteConfigInspector(new GUIContent("Pressed State", "Pressed Sprite and Color"));
		
		}

		
	// ---------------
	public override void OnInspectorGUI()
		{

		TouchTrackPadSpriteAnimator target = this.target as TouchTrackPadSpriteAnimator;
		if ((target == null))
			return;
			
		if (!TouchControlSpriteAnimatorInspector.DrawSourceGUI(target))
			return;

//		TouchControlSpriteAnimatorInspector.DrawTimingGUI(target);
			
		

		InspectorUtils.BeginIndentedSection(new GUIContent("Sprite Settings"));

			this.spriteNeutral.Draw(target.spriteNeutral, target, true, false);

			EditorGUILayout.Space();
			this.spritePressed.Draw(target.spritePressed, target, target.IsIllegallyAttachedToSource());
				

		InspectorUtils.EndIndentedSection();
			
//
//		TouchControlSpriteAnimatorInspector.DrawDefaultTransformGUI(target);
		
		}

	
	
	}

		
}
#endif
