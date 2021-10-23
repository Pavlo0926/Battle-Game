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
	
[CustomEditor(typeof(ControlFreak2.TouchSteeringWheelSpriteAnimator))]
public class TouchSteeringWheelSpriteAnimatorInspector : UnityEditor.Editor
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

		TouchSteeringWheelSpriteAnimator target = this.target as TouchSteeringWheelSpriteAnimator;
		if ((target == null))
			return;

		if (!TouchControlSpriteAnimatorInspector.DrawSourceGUI(target))
			return;

		TouchSteeringWheel wheel = target.sourceControl as TouchSteeringWheel; 

	

		InspectorUtils.BeginIndentedSection(new GUIContent("Sprite Settings"));

			this.spriteNeutral.Draw(target.spriteNeutral, target, true, false);

			EditorGUILayout.Space();
			this.spritePressed.Draw(target.spritePressed, target, target.IsIllegallyAttachedToSource());
				

		InspectorUtils.EndIndentedSection();
			

		// Scaling GUI...

		float	
			rotationRange	= target.rotationRange,
			rotationSmoothingTime = target.rotationSmoothingTime;


		InspectorUtils.BeginIndentedSection(new GUIContent("Transform Animation Settings"));


			if ((wheel != null) && (wheel.wheelMode == TouchSteeringWheel.WheelMode.Turn))
				{
				GUI.enabled = false;
				CFGUI.FloatField(new GUIContent("Rot. range", "Wheel's Rotation Range - in TURN wheel mode it's taken from Wheel."), 
					wheel.maxTurnAngle, 0, 100000, 65);
				GUI.enabled = true;
				}
			else
				{
				rotationRange = CFGUI.Slider(new GUIContent("Rot. range", "Wheel's Rotation Range"), rotationRange, 0, 180, 65);
				}

			
			rotationSmoothingTime = CFGUI.FloatFieldEx(new GUIContent("Smooting Time (ms)", "Wheel Rotation Smooting Time in milliseconds"), 
				rotationSmoothingTime, 0, 10, 1000, true, 120);

			EditorGUILayout.Space();

//			TouchControlSpriteAnimatorInspector.DrawScaleGUI(target);
	
		InspectorUtils.EndIndentedSection();


		

		if ((rotationRange 			!= target.rotationRange) ||
			(rotationSmoothingTime 	!= target.rotationSmoothingTime))
			{
			CFGUI.CreateUndo("Sprite Animator Trsnaform modification", target);

			target.rotationRange			= rotationRange;
			target.rotationSmoothingTime	= rotationSmoothingTime;
				
			CFGUI.EndUndo(target);
			}


		
		}

	
	
	}

		
}
#endif
