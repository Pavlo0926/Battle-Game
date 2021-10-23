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


using UnityEngine;
using System.Collections;

public class TouchGestureConfigInspector {
		
	private GUIContent titleContent;
	private Object undoObject;


	// ----------------
	public TouchGestureConfigInspector(Object undoObject, GUIContent titleContent)
		{
		this.undoObject		= undoObject;
		this.titleContent	= titleContent;
		}

	const float LABEL_WIDTH = 120;

	// ---------------
	public void DrawGUI(TouchGestureConfig config)
		{
		int 
			maxTapCount						= config.maxTapCount;
		bool		
			cleanTapsOnly					= config.cleanTapsOnly,	
			detectLongTap					= config.detectLongTap,
			detectLongPress					= config.detectLongPress,
			endLongPressWhenMoved			= config.endLongPressWhenMoved,
			endLongPressWhenSwiped			= config.endLongPressWhenSwiped;
		TouchGestureConfig.DirMode
			dirMode							= config.dirMode;
		TouchGestureConfig.DirConstraint
			swipeConstraint					= config.swipeConstraint,
			swipeDirConstraint				= config.swipeDirConstraint,
			scrollConstraint				= config.scrollConstraint;				
	DirectionState.OriginalDirResetMode
			swipeOriginalDirResetMode		= config.swipeOriginalDirResetMode;
			
	
			
		// GUI... 

		
		InspectorUtils.BeginIndentedSection(this.titleContent);

		maxTapCount = CFGUI.IntSlider(new GUIContent("Max Number of Taps", "Maximal number of consecutive taps to detect.\nWhen \'Report All Taps\' option is turned off, system will wait for potential follow-up taps so they may be reported with slight delay."),
			maxTapCount, 0, 5, LABEL_WIDTH);

		dirMode = (TouchGestureConfig.DirMode)CFGUI.EnumPopup(new GUIContent("Swipe Dir. Mode", "Swipe Segmwnt Direction Mode"), dirMode, LABEL_WIDTH);

		swipeOriginalDirResetMode = (DirectionState.OriginalDirResetMode)CFGUI.EnumPopup(new GUIContent("Swipe Dir. Reset Mode", "Swipe Segment's Original Direction Reset Mode - choose when original direction is reset. This option is used by Direction Binding in ORIGINAL group modes..."),
			swipeOriginalDirResetMode, LABEL_WIDTH);

		swipeDirConstraint = (TouchGestureConfig.DirConstraint)CFGUI.EnumPopup(new GUIContent("Scroll Dir Constraint", "Scroll Direction Constraint Mode"), swipeDirConstraint, LABEL_WIDTH);
		swipeConstraint = (TouchGestureConfig.DirConstraint)CFGUI.EnumPopup(new GUIContent("Swipe Constraint", "Swipe Constraint Mode"), swipeConstraint, LABEL_WIDTH);
		scrollConstraint = (TouchGestureConfig.DirConstraint)CFGUI.EnumPopup(new GUIContent("Scroll Constraint", "Scroll Constraint Mode"), scrollConstraint, LABEL_WIDTH);

		cleanTapsOnly = EditorGUILayout.ToggleLeft(new GUIContent("Clean Taps Only", "When turned on, only clean taps will be reported."),
			cleanTapsOnly);			

		detectLongPress = EditorGUILayout.ToggleLeft(new GUIContent("Detect Long Press", 
			"Long Press is a static touch (tap threshold) pressed for some time (Long Press Min. Duration)."),
			detectLongPress);			

		if (detectLongPress)
			{
			detectLongTap = EditorGUILayout.ToggleLeft(new GUIContent("Detect Long Tap", ""),
				detectLongTap);			
	
	
			endLongPressWhenMoved = EditorGUILayout.ToggleLeft(new GUIContent("End Long Press When Moved", "End Long Press (and start Normal Press) when touch moved past the Tap Threshold."),
				endLongPressWhenMoved);			
			endLongPressWhenSwiped = EditorGUILayout.ToggleLeft(new GUIContent("End Long Press When Swiped", "End Long Press (and start Normal Press) when touch moved past the Swipe Threshold."),
				endLongPressWhenSwiped);			
			}

		InspectorUtils.EndIndentedSection();

			
		// Register Undo...

		if ((maxTapCount					!= config.maxTapCount) ||
			(cleanTapsOnly					!= config.cleanTapsOnly) ||
			(detectLongTap					!= config.detectLongTap) ||
			(detectLongPress				!= config.detectLongPress) ||
			(dirMode						!= config.dirMode) ||
			(swipeConstraint				!= config.swipeConstraint) ||
			(swipeDirConstraint				!= config.swipeDirConstraint) ||
			(scrollConstraint				!= config.scrollConstraint) ||
			(swipeOriginalDirResetMode		!= config.swipeOriginalDirResetMode) ||
			(endLongPressWhenSwiped			!= config.endLongPressWhenSwiped) ||
			(endLongPressWhenMoved			!= config.endLongPressWhenMoved))
			{
			CFGUI.CreateUndo("Gesture Config modification", this.undoObject);
				
			config.dirMode						= dirMode;
			config.swipeConstraint				= swipeConstraint;
			config.swipeDirConstraint			= swipeDirConstraint;
			config.scrollConstraint				= scrollConstraint;

			config.maxTapCount					= maxTapCount;		
			config.cleanTapsOnly				= cleanTapsOnly;		
			config.detectLongPress				= detectLongPress;		
			config.detectLongTap				= detectLongTap;		
			config.endLongPressWhenMoved		= endLongPressWhenMoved;
			config.endLongPressWhenSwiped		= endLongPressWhenSwiped;
			config.swipeOriginalDirResetMode	= swipeOriginalDirResetMode;


			CFGUI.EndUndo(this.undoObject);
			}


		}

	}
}

#endif

