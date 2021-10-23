// -------------------------------------------
// Control Freak 2
// Copyright (C) 2013-2020 Dan's Game Tools
// http://DansGameTools.blogspot.com
// -------------------------------------------


#if UNITY_4_6 || UNITY_4_7 || UNITY_4_8 || UNITY_4_9 
	#define UNITY_PRE_5_0
#endif

#if UNITY_PRE_5_0 || UNITY_5_0 
	#define UNITY_PRE_5_1
#endif

#if UNITY_PRE_5_1 || UNITY_5_1 
	#define UNITY_PRE_5_2
#endif

#if UNITY_PRE_5_2 || UNITY_5_2 
	#define UNITY_PRE_5_3
#endif

#if UNITY_PRE_5_3 || UNITY_5_3 
	#define UNITY_PRE_5_4
#endif


#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

using ControlFreak2Editor;
using ControlFreak2;
using ControlFreak2.Internal;
using ControlFreak2Editor.Internal;

namespace ControlFreak2Editor.Inspectors
{
[System.Serializable]
public class SpriteConfigInspector
	{
	private GUIContent labelContent;		
	private Sprite lastSprite;
	private Texture2D cachedPreview;
		

	// -----------------
	public SpriteConfigInspector(GUIContent labelContent)
		{
		this.labelContent = labelContent;
		}

	// ---------------
	public void Draw(SpriteConfig target, Object undoObject, bool skipTransforms, bool canBeDisabled = true)
		{	
		bool
			enabled	= target.enabled;
		Color
			color	= target.color;
		Sprite
			sprite	= target.sprite;
		float
			rotation	= target.rotation,
			scale		= target.scale;
		Vector2
			offset		= target.offset;
		bool
			resetOffset		= target.resetOffset,
			resetScale		= target.resetScale,
			resetRotation	= target.resetRotation;

		float 
			baseTransitionTime			= target.baseTransitionTime,
			colorTransitionFactor		= target.colorTransitionFactor,
			offsetTransitionFactor		= target.offsetTransitionFactor,
			rotationTransitionFactor	= target.rotationTransitionFactor,
			scaleTransitionFactor		= target.scaleTransitionFactor,
			duration					= target.duration;


		if (!canBeDisabled)
			enabled = true;
			
		Color initialGuiColor = GUI.color;			


		EditorGUILayout.BeginVertical(CFEditorStyles.Inst.transpBevelBG);

		EditorGUILayout.BeginVertical();


			GUI.enabled = canBeDisabled;
			enabled = EditorGUILayout.ToggleLeft(labelContent, enabled, CFEditorStyles.Inst.boldText, GUILayout.MinWidth(30), GUILayout.ExpandWidth(true));
			GUI.enabled = true;


			if (enabled)
				{
#if UNITY_PRE_5_2
				color = EditorGUILayout.ColorField(color,  GUILayout.ExpandWidth(true));		
				sprite = (Sprite)EditorGUILayout.ObjectField(sprite, typeof(Sprite), false);

#else
				EditorGUILayout.BeginHorizontal();
					color = EditorGUILayout.ColorField(color,  GUILayout.ExpandWidth(true));		
					sprite = (Sprite)EditorGUILayout.ObjectField(sprite, typeof(Sprite), false, GUILayout.Width(64), GUILayout.Height(64));
				EditorGUILayout.EndHorizontal();
#endif
				}



//			EditorGUILayout.BeginHorizontal();
//				//GUILayout.Label(labelContent, CFEditorStyles.Inst.boldText, GUILayout.MinWidth(30), GUILayout.ExpandWidth(true));

//				if (enabled)
//					color = EditorGUILayout.ColorField(color,  GUILayout.ExpandWidth(true));		

//			if (enabled)
//				{
//#if UNITY_PRE_5_2
//				sprite = (Sprite)EditorGUILayout.ObjectField(sprite, typeof(Sprite), false);
//#else
//				sprite = (Sprite)EditorGUILayout.ObjectField(sprite, typeof(Sprite), false, GUILayout.Width(64), GUILayout.Height(64));
//#endif
//				}
//			EditorGUILayout.EndHorizontal();

			EditorGUILayout.EndVertical();
			
			if (enabled)
				{
				if (target.oneShotState)
					duration = CFGUI.FloatFieldEx(new GUIContent("Duration", "Duration if this one-shot state in milliseconds."),
						duration, 0, 100, 1000, true, 120);
	
				baseTransitionTime = CFGUI.FloatFieldEx(new GUIContent("Base transition time", "Base transition time in milliseconds to all transtions below..."),
					baseTransitionTime, 0, 100, 1000, true, 120);
	
				colorTransitionFactor = CFGUI.Slider(new GUIContent("Color Transition", "Color Transition as a fraction on the Base Transition Time."),
					colorTransitionFactor, 0, 1, 120);
				offsetTransitionFactor = CFGUI.Slider(new GUIContent("Offset Transition", "Offset Transition as a fraction on the Base Transition Time."),
					offsetTransitionFactor, 0, 1, 120);
				rotationTransitionFactor = CFGUI.Slider(new GUIContent("Rotation Transition", "Rotation Transition as a fraction on the Base Transition Time."),
					rotationTransitionFactor, 0, 1, 120);
				scaleTransitionFactor = CFGUI.Slider(new GUIContent("Scale Transition", "Scale Transition as a fraction on the Base Transition Time."),
					scaleTransitionFactor, 0, 1, 120);


				if (!skipTransforms)
					{
					EditorGUILayout.Space();
	
					resetScale = EditorGUILayout.ToggleLeft(new GUIContent("Reset Scale", "Reset scale on start."),
						resetScale, GUILayout.ExpandWidth(true), GUILayout.MinWidth(30));
					scale = CFGUI.FloatField(new GUIContent("Scale", "State specific scale"), 
						scale, 0, 100, 80);	
			
					EditorGUILayout.Space();
	
					resetRotation = EditorGUILayout.ToggleLeft(new GUIContent("Reset Rotation", "Reset rotation on start."),
						resetRotation, GUILayout.ExpandWidth(true), GUILayout.MinWidth(30));
					rotation = CFGUI.FloatField(new GUIContent("Rotation", "State specific rotation"), 
						rotation, -10000, 10000, 80);
					EditorGUILayout.Space();
	
					resetOffset = EditorGUILayout.ToggleLeft(new GUIContent("Reset Offset", "Reset Offset on start."),
						resetOffset, GUILayout.ExpandWidth(true), GUILayout.MinWidth(30));

					offset.x = CFGUI.FloatField(new GUIContent("Offset X", "State specific offset X"), 
						offset.x, -1000000, 100000, 80);
					offset.y = CFGUI.FloatField(new GUIContent("Offset Y", "State specific offset X"), 
						offset.y, -1000000, 100000, 80);
	
										
					}
	
				}		


			EditorGUILayout.EndVertical();




		
		GUI.color = initialGuiColor;


		// Record undo...

		if ((enabled	!= target.enabled) ||
			(color 		!= target.color) || 
			(rotation	!= target.rotation) || 
			(scale		!= target.scale) || 
			(offset		!= target.offset) || 
			(sprite		!= target.sprite) ||
			(resetOffset	!= target.resetOffset) ||
			(resetScale		!= target.resetScale) ||
			(resetRotation	!= target.resetRotation) ||
			(baseTransitionTime			!= target.baseTransitionTime) ||
			(colorTransitionFactor		!= target.colorTransitionFactor) ||
			(offsetTransitionFactor		!= target.offsetTransitionFactor) ||
			(rotationTransitionFactor	!= target.rotationTransitionFactor) ||
			(scaleTransitionFactor		!= target.scaleTransitionFactor) ||
			(duration					!= target.duration) )
			{
			CFGUI.CreateUndo("Modify sprite config for " + undoObject.name, undoObject);
				
			target.enabled	= enabled;
			target.color	= color;
			target.sprite	= sprite;
			target.scale	= scale;
			target.rotation	= rotation;
			target.offset	= offset;

			target.resetOffset		= resetOffset;
			target.resetScale		= resetScale;
			target.resetRotation	= resetRotation;


			target.baseTransitionTime		= baseTransitionTime;
			target.colorTransitionFactor	= colorTransitionFactor;
			target.offsetTransitionFactor	= offsetTransitionFactor;
			target.rotationTransitionFactor	= rotationTransitionFactor;
			target.scaleTransitionFactor	= scaleTransitionFactor;
			target.duration					= duration;


			CFGUI.EndUndo(undoObject);
			}
		
		}

	}
}

#endif
