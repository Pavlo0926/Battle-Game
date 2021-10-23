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
	
[CustomEditor(typeof(ControlFreak2.SuperTouchZoneSpriteAnimator))]
public class SuperTouchZoneSpriteAnimatorInspector : UnityEditor.Editor
	{
	private SpriteConfigInspector[]
		spriteConfigInspArray;

	
	


	// ---------------------
	void OnEnable()
		{
		this.spriteConfigInspArray = new SpriteConfigInspector[(int)SuperTouchZoneSpriteAnimator.ControlStateCount];

		this.spriteConfigInspArray[(int)SuperTouchZoneSpriteAnimator.ControlState.Neutral] = new SpriteConfigInspector(new GUIContent("Neutral State"));
		this.spriteConfigInspArray[(int)SuperTouchZoneSpriteAnimator.ControlState.RawPress] = new SpriteConfigInspector(new GUIContent("Raw Press State"));
		this.spriteConfigInspArray[(int)SuperTouchZoneSpriteAnimator.ControlState.NormalPress] = new SpriteConfigInspector(new GUIContent("Normal Press State"));
		this.spriteConfigInspArray[(int)SuperTouchZoneSpriteAnimator.ControlState.LongPress] = new SpriteConfigInspector(new GUIContent("Long Press State"));
		this.spriteConfigInspArray[(int)SuperTouchZoneSpriteAnimator.ControlState.Tap] = new SpriteConfigInspector(new GUIContent("Tap State"));
		this.spriteConfigInspArray[(int)SuperTouchZoneSpriteAnimator.ControlState.DoubleTap] = new SpriteConfigInspector(new GUIContent("Double Tap State"));
		this.spriteConfigInspArray[(int)SuperTouchZoneSpriteAnimator.ControlState.LongTap] = new SpriteConfigInspector(new GUIContent("Long Tap State"));
		this.spriteConfigInspArray[(int)SuperTouchZoneSpriteAnimator.ControlState.NormalScrollU] = new SpriteConfigInspector(new GUIContent("Normal Press Scroll UP State"));
		this.spriteConfigInspArray[(int)SuperTouchZoneSpriteAnimator.ControlState.NormalScrollR] = new SpriteConfigInspector(new GUIContent("Normal Press Scroll RIGHT State"));
		this.spriteConfigInspArray[(int)SuperTouchZoneSpriteAnimator.ControlState.NormalScrollD] = new SpriteConfigInspector(new GUIContent("Normal Press Scroll DOWN State"));
		this.spriteConfigInspArray[(int)SuperTouchZoneSpriteAnimator.ControlState.NormalScrollL] = new SpriteConfigInspector(new GUIContent("Normal Press Scroll LEFT State"));

		this.spriteConfigInspArray[(int)SuperTouchZoneSpriteAnimator.ControlState.LongScrollU] = new SpriteConfigInspector(new GUIContent("Long Press Scroll UP State"));
		this.spriteConfigInspArray[(int)SuperTouchZoneSpriteAnimator.ControlState.LongScrollR] = new SpriteConfigInspector(new GUIContent("Long Press Scroll RIGHT State"));
		this.spriteConfigInspArray[(int)SuperTouchZoneSpriteAnimator.ControlState.LongScrollD] = new SpriteConfigInspector(new GUIContent("Long Press Scroll DOWN State"));
		this.spriteConfigInspArray[(int)SuperTouchZoneSpriteAnimator.ControlState.LongScrollL] = new SpriteConfigInspector(new GUIContent("Long Press Scroll LEFT State"));

		}

		
	// ---------------
	public override void OnInspectorGUI()
		{

		SuperTouchZoneSpriteAnimator target = this.target as SuperTouchZoneSpriteAnimator;
		if ((target == null))
			return;
			
		if (!TouchControlSpriteAnimatorInspector.DrawSourceGUI(target))
			return;

		//TouchControlSpriteAnimatorInspector.DrawTimingGUI(target);
			
		

		InspectorUtils.BeginIndentedSection(new GUIContent("Sprite Settings"));

			if (this.spriteConfigInspArray != null)
				{
				for (SuperTouchZoneSpriteAnimator.ControlState i = SuperTouchZoneSpriteAnimator.ControlStateFirst; 
					i < SuperTouchZoneSpriteAnimator.ControlStateCount; ++i)
					{
					if (this.spriteConfigInspArray[(int)i] == null)
						EditorGUILayout.LabelField("ERROR");
					else
						this.spriteConfigInspArray[(int)i].Draw(target.GetStateSpriteConfig(i), target, ((int)i == 0), ((int)i != 0));
					}
				}
//
//			this.spriteNeutral.Draw(target.spriteNeutral, target, true);
//
//			EditorGUILayout.Space();
//			this.spritePressed.Draw(target.spritePressed, target, target.IsIllegallyAttachedToSource());
				

		InspectorUtils.EndIndentedSection();
			

		//TouchControlSpriteAnimatorInspector.DrawDefaultTransformGUI(target);
		
		}

	
	
	}

		
}
#endif
