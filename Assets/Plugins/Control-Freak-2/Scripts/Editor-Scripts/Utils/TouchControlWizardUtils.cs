// -------------------------------------------
// Control Freak 2
// Copyright (C) 2013-2020 Dan's Game Tools
// http://DansGameTools.blogspot.com
// -------------------------------------------

#if UNITY_EDITOR 



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


using UnityEngine;
using UnityEditor;
using ControlFreak2;
using ControlFreak2.Internal;
using System.Collections.Generic;

using ControlFreak2Editor.Inspectors;


namespace ControlFreak2Editor
{
static public class TouchControlWizardUtils
	{
	const string 
		NOTIFIER_PREFAB_PATH = "Assets/Plugins/Control-Freak-2/Prefabs/Other/CF2-Gamepad-Notifier.prefab",		
		CREATION_MENU_PATH = "GameObject/Control Freak 2/";
	const int
		CREATION_MENU_PRIO	= 10;
	

	// ---------------
	public enum CreationMode
		{
		Always,
		OnlyIfNotPresent,
		AskIfPresent
		}


	// -------------------
	[MenuItem(CREATION_MENU_PATH + "InputRig with Panel", false, CREATION_MENU_PRIO + 1)]
	static public void MenuCreateInputRigWithPanel()
		{
		InputRig rig = TouchControlWizardUtils.CreateRig("CF2-Rig");
			
		Canvas canvas = TouchControlWizardUtils.CreateCanvas(rig, "CF2-Canvas");

		TouchControlWizardUtils.CreatePanel(rig, canvas, "CF2-Panel", new Rect(0, 0, 1, 1)); 

		Undo.RegisterCreatedObjectUndo(rig.gameObject, "Create CF2 Rig with Panel");

		Selection.activeObject = rig;

		UnityInputManagerToRigDialog.AskToShowDialog(rig);
		}



	// -------------------
	[MenuItem(CREATION_MENU_PATH + "Gamepad Manager", false, CREATION_MENU_PRIO + 1)]
	static public void MenuCreateGamepadManager()
		{
		GamepadManager gm = CreateGamepadManager(false, CreationMode.AskIfPresent, "Create CF2 Gamepad Manager");

		Selection.activeObject = gm;
		}


	// -------------------
	[MenuItem(CREATION_MENU_PATH + "Gamepad Manager with Notifier", false, CREATION_MENU_PRIO + 1)]
	static public void MenuCreateGamepadManagerWithNotifier()
		{
		GamepadManager gm = CreateGamepadManager(true, CreationMode.AskIfPresent, "Create CF2 Gamepad Manager with Notifier");
		Selection.activeObject = gm;
		}


	// -------------------
	[MenuItem(CREATION_MENU_PATH + "Event System", false, CREATION_MENU_PRIO + 1)]
	static public void MenuCreateEventSystem()
		{
		Selection.activeObject = CreateEventSystem("CF2-Event-System", CreationMode.AskIfPresent, "Create CF2 Event System");
		}
		


	// -------------------
	[MenuItem(CREATION_MENU_PATH + "Total Package (Event System + Rig + Gamepad Manager)", false, CREATION_MENU_PRIO)]
	static public void MenuCreateTotalPackage()
		{
		UnityEngine.EventSystems.EventSystem 
			sys = CreateEventSystem("CF2-Event-System", CreationMode.AskIfPresent, null);

		InputRig 
			rig = TouchControlWizardUtils.CreateRig("CF2-Rig");
			
		Canvas
			canvas = TouchControlWizardUtils.CreateCanvas(rig, "CF2-Canvas");

		TouchControlWizardUtils.CreatePanel(rig, canvas, "CF2-Panel", new Rect(0, 0, 1, 1)); 

		GamepadManager	
			gm = CreateGamepadManager(true, CreationMode.AskIfPresent, null);


		string undoLabel = "Create CF2 Total Package";

		if (sys != null) 
			Undo.RegisterCreatedObjectUndo(sys.gameObject, undoLabel);
		if (gm != null)
			Undo.RegisterCreatedObjectUndo(gm.gameObject, undoLabel);
		
		Undo.RegisterCreatedObjectUndo(rig.gameObject, undoLabel);

		Selection.activeObject = rig;

		UnityInputManagerToRigDialog.AskToShowDialog(rig);
		}


	// -------------------
	[MenuItem(CREATION_MENU_PATH + "Event System and Gamepad Manager (if missing)", false, CREATION_MENU_PRIO)]
	static public void MenuCreateEventSystemAndGamepadManager()
		{
		string undoLabel = "Create CF2 Event System and Gamepad Manager";

		UnityEngine.EventSystems.EventSystem 
			sys = CreateEventSystem("CF2-Event-System", CreationMode.OnlyIfNotPresent, null);

		GamepadManager	
			gm = CreateGamepadManager(true, CreationMode.OnlyIfNotPresent, null);
	
		if (sys != null) 
			Undo.RegisterCreatedObjectUndo(sys.gameObject, undoLabel);
		if (gm != null)
			Undo.RegisterCreatedObjectUndo(gm.gameObject, undoLabel);
		
		if ((gm != null) || (sys != null))
			Selection.activeObject = ((gm != null) ? (Object)gm : (Object)sys);
		}




	// ----------------------
	static public ControlFreak2.GamepadManager CreateGamepadManager(
		bool				withNotifier, 
		CreationMode	creationMode, 
		string			undoLabel = null)
		{
		int gmPresence = IsThereGamepadManagerInTheScene();

		if ((gmPresence != 0) && (creationMode == CreationMode.OnlyIfNotPresent))
			return null;

		if ((gmPresence != 0) && (creationMode == CreationMode.AskIfPresent))
			{
			string msg = null;
			msg = "There's a CF2 Gamepad Manager in the scene already. Do you want to create a new one anyway?";
			if (!EditorUtility.DisplayDialog("Control Freak 2 - Create Gamepad Manager", msg, "Yes", "No"))
				return null;
			}
				
		GamepadManager gm = null;

		gm = (GamepadManager)TouchControlWizardUtils.CreateObjectWithComponent("CF2-Gamepad-Manager", typeof(ControlFreak2.GamepadManager));
		if (gm == null)
			return null;

		if (withNotifier)
			{
			ControlFreak2.GamepadNotifier gn = CreateGamepadNotifer("CF2-Gamepad-Notifier", null);
			if (gn != null)
				gn.transform.SetParent(gm.transform, false);
			}

		if (undoLabel != null)
			Undo.RegisterCreatedObjectUndo(gm.gameObject, undoLabel);

		return gm;
		}


	// ---------------------
	static public ControlFreak2.GamepadNotifier CreateGamepadNotifer(string name, string undoLabel = null)
		{
		ControlFreak2.GamepadNotifier
			gn = (ControlFreak2.GamepadNotifier)UnityEditor.AssetDatabase.LoadAssetAtPath(NOTIFIER_PREFAB_PATH, typeof(ControlFreak2.GamepadNotifier));

		if (gn == null)
			{
			Debug.LogError("Can't load default Gamepad Notifier Prefab from [" + NOTIFIER_PREFAB_PATH + "]! Reimport Control Freak 2 package to fix the issue.");
			return null;
			}


		//gn = (ControlFreak2.GamepadNotifier)ControlFreak2.GamepadNotifier.Instantiate(gn);
		gn = (ControlFreak2.GamepadNotifier)PrefabUtility.InstantiatePrefab(gn);

		gn.name = name;
		
		if (undoLabel != null)
			Undo.RegisterCreatedObjectUndo(gn.gameObject, undoLabel);
		
		return gn;
		}





	// -------------------
	static public int IsThereEventSystemInTheScene()
		{
		UnityEngine.Component sysGo = (UnityEngine.Component)(GameObject.FindObjectOfType(typeof(UnityEngine.EventSystems.EventSystem)));
		if (sysGo == null)
			return 0;
		return (sysGo.GetComponent(typeof(ControlFreak2.GamepadInputModule)) ? 1 : -1);
		}


	// -------------------
	static public int IsThereGamepadManagerInTheScene()
		{
		UnityEngine.Component gmGo = (UnityEngine.Component)(GameObject.FindObjectOfType(typeof(ControlFreak2.GamepadManager)));
		if (gmGo == null)
			return 0;
		return (gmGo.GetComponentInChildren(typeof(ControlFreak2.GamepadNotifier)) ? 1 : -1);
		}


	// ----------------------------------
	static public UnityEngine.EventSystems.EventSystem CreateEventSystem(
		string			name,
		CreationMode	creationMode,
		string			undoName = null)
		{
		int eventSysPresence = IsThereEventSystemInTheScene();
		
		if ((eventSysPresence != 0) && (creationMode == CreationMode.OnlyIfNotPresent))
			return null;

		if ((eventSysPresence != 0) && (creationMode == CreationMode.AskIfPresent))
			{
			if (!EditorUtility.DisplayDialog("Control Freak 2 - Create Event System", (eventSysPresence == 1) ?
				"There's a CF2 Event System in the scene already. Do you want to create a new one anyway?" :
				"There's an Event System in the scene, but it isn't using CF2 Input Module. Do you want to create a new one anyway?", "Yes", "No"))
					return null;
			}

		UnityEngine.EventSystems.EventSystem eventSys = 
			(UnityEngine.EventSystems.EventSystem)TouchControlWizardUtils.CreateObjectWithComponent(name, typeof(UnityEngine.EventSystems.EventSystem));
			
		eventSys.gameObject.AddComponent(typeof(ControlFreak2.GamepadInputModule));
		//eventSys.gameObject.AddComponent(typeof(ControlFreak2.MouseInputModule));

#if UNITY_PRE_5_3
		eventSys.gameObject.AddComponent(typeof(UnityEngine.EventSystems.TouchInputModule));
#endif

		UnityEngine.EventSystems.StandaloneInputModule standaloneModule = 
			(UnityEngine.EventSystems.StandaloneInputModule)eventSys.gameObject.AddComponent(typeof(UnityEngine.EventSystems.StandaloneInputModule));
			
		standaloneModule.horizontalAxis	= InputRig.CF_EMPTY_AXIS;
		standaloneModule.verticalAxis	= InputRig.CF_EMPTY_AXIS;
		standaloneModule.submitButton	= InputRig.CF_EMPTY_AXIS;
		standaloneModule.cancelButton	= InputRig.CF_EMPTY_AXIS;


		if (undoName != null)
			Undo.RegisterCreatedObjectUndo(eventSys.gameObject, undoName);
 
		return eventSys;
		}




	// -------------------
	static public InputRig CreateRig(
		string name,
		string undoName = null)
		{
		return (InputRig)CreateObjectWithComponent(name, typeof(InputRig), undoName);
		
		}


		
	// -------------------
	static private Component CreateObjectWithComponent(
		string 		name,
		System.Type	componentType,
		string 		undoName = null)
		{
		GameObject obj = new GameObject(name, componentType);			

		Component comp = obj.GetComponent(componentType);

		obj.transform.localPosition = Vector3.zero;
		obj.transform.localRotation = Quaternion.identity;
		obj.transform.localScale = Vector3.one;
			
		if (undoName != null)
			Undo.RegisterCreatedObjectUndo(obj, undoName);

		return comp;
		}


		

	// -----------------
	static public Component GetComponentFromSelection(System.Type compType)
		{
		Transform[] sel = Selection.transforms;
		for (int i = 0; i < sel.Length; ++i)
			{
			Component c = CFUtils.GetComponentHereOrInParent(sel[i], compType);
			if (c != null)
				return c;	
			}

		return null;
		}
	

	// -------------------	
	static public UnityEngine.Component GetComponentFromSelectionEx(System.Type objType, UnityEngine.Component prevSel, bool searchForAnythingIfNothingIsSelected = false) 
		{ 
		UnityEngine.Component  newSel = GetComponentFromSelection(objType);
		if (newSel != null)
			return newSel;
		if (prevSel != null)
			return prevSel;

		return (UnityEngine.Component)GameObject.FindObjectOfType(objType);
		}

	
	// -----------------
	static public InputRig GetRigFromSelection(InputRig prevSel, bool searchForAnythingIfNothingIsSelected) 
		{ 
		return (InputRig)GetComponentFromSelectionEx(typeof(InputRig), prevSel, searchForAnythingIfNothingIsSelected);
		}

		
	// ------------------
	static public TouchControlPanel GetRigPanel(InputRig rig)
		{
		if (rig == null)
			return null;

		TouchControlPanel panel = rig.GetComponentInChildren<TouchControlPanel>();
		return panel;	
		}
		

	// -----------------
	static public string GetUniqueButtonName	(InputRig rig)	{	return GetUniqueControlName(rig, typeof(ControlFreak2.TouchButton), "Button-"); }
	static public string GetUniqueJoystickName	(InputRig rig)	{	return GetUniqueControlName(rig, typeof(ControlFreak2.TouchJoystick), "Joystick-"); }
	static public string GetUniqueTrackPadName	(InputRig rig)	{	return GetUniqueControlName(rig, typeof(ControlFreak2.TouchTrackPad), "TrackPad-"); }
	static public string GetUniqueWheelName		(InputRig rig)	{	return GetUniqueControlName(rig, typeof(ControlFreak2.TouchSteeringWheel), "Wheel-"); }
	static public string GetUniqueTouchZoneName	(InputRig rig)	{	return GetUniqueControlName(rig, typeof(ControlFreak2.SuperTouchZone), "TouchZone-"); }
	static public string GetUniqueTouchSplitterName	(InputRig rig)	{	return GetUniqueControlName(rig, typeof(ControlFreak2.TouchSplitter), "TouchSplitter-"); }


	// ----------------
	static private string GetUniqueControlName(InputRig rig, System.Type controlType, string prefix)
		{
		if (rig == null)
			return "";

		List<TouchControl> controls = rig.GetTouchControls();
			

		for (int ci = 0; ci < 100; ++ci)
			{
			string nameToTest = prefix + ci.ToString();

			for (int i = 0; i < controls.Count; ++i)
				{
				TouchControl c = controls[i];
				if ((c.GetType() == controlType) && (c.name == nameToTest))
					{
					nameToTest = null;
					break;
					}
				}

			if (nameToTest != null)
				return nameToTest;
			}

		return (prefix + controls.Count.ToString());
		}



		
	// --------------------
	static public TouchControlPanel CreatePanelOnCanvas(
		InputRig	rig,
		string		namePrefix,
		string		undoName = null)
		{
		if (rig == null)
			return null;

		Canvas canvas = CreateCanvas(rig, namePrefix + "Canvas");
		TouchControlPanel panel = CreatePanel(rig, canvas, namePrefix + "Panel", new Rect(0, 0, 1, 1));

		if (undoName != null)
			Undo.RegisterCreatedObjectUndo(canvas.gameObject, undoName);

		return panel;
		}

		
	// ---------------------
	static public Canvas CreateCanvas(
		InputRig rig,
		//Camera cam,
		string name,
		string undoName = null)
		{
		GameObject canvasObj = new GameObject(name, typeof(Canvas), typeof(UnityEngine.UI.CanvasScaler), typeof(UnityEngine.UI.GraphicRaycaster));

		Canvas canvas = canvasObj.GetComponent<Canvas>();

		canvas.renderMode = RenderMode.ScreenSpaceOverlay;
		//canvas.worldCamera = cam;
		canvas.overridePixelPerfect = true;
		canvas.pixelPerfect = false;
		canvas.sortingOrder	= 100;

		canvas.transform.SetParent(rig.transform);


		UnityEngine.UI.CanvasScaler canvasScaler = canvasObj.GetComponent<UnityEngine.UI.CanvasScaler>();
	
		canvasScaler.matchWidthOrHeight = 0;
		canvasScaler.screenMatchMode = UnityEngine.UI.CanvasScaler.ScreenMatchMode.Expand;
		canvasScaler.uiScaleMode = UnityEngine.UI.CanvasScaler.ScaleMode.ScaleWithScreenSize;
		canvasScaler.referenceResolution = new Vector2(800, 480);

			
		if (undoName != null)
			Undo.RegisterCreatedObjectUndo(canvas.gameObject, undoName);

		return canvas;
		}


	// --------------------
	static public TouchControlPanel CreatePanel(
		InputRig	rig,
		Canvas		canvas,
		string		name,
		Rect		rect,
		string		undoName = null)
		{

		TouchControlPanel panel = (TouchControlPanel)CreateStretchyRectTr(typeof(TouchControlPanel), rig.transform, name, rect, 0);	
			
		// Add default Unity UI add-on...

		panel.gameObject.AddComponent<TouchControlPanelUnityUiAddOn>();

		// Setup hierarchy...

		panel.transform.SetParent(canvas.transform, false);
		panel.InvalidateHierarchy();		

			
		if (undoName != null)
			Undo.RegisterCreatedObjectUndo(panel.gameObject, undoName);


		return panel;
		}
		
	
	// -----------------
	static private Component CreateConstSizeRectTr(
		System.Type		componentType,
		Transform		root,			
		string			name,
		Vector2			anchorPos,
		Vector2			offset,
		Vector2			size,
		float			localDepth)
		{
		GameObject o = new GameObject(name, typeof(RectTransform), componentType);
			
		RectTransform tr = o.GetComponent<RectTransform>();
			
		tr.SetParent(root, false);
		tr.localPosition	= new Vector3(0, 0, localDepth);
		tr.localScale		= Vector3.one;
		tr.localRotation	= Quaternion.identity;

			
		tr.pivot = new Vector2(0.5f, 0.5f);

		tr.anchorMax = anchorPos;
		tr.anchorMin = anchorPos;

		tr.offsetMin = offset - (size * 0.5f);
		tr.offsetMax = offset + (size * 0.5f);

		Component c = o.GetComponent(componentType);
			
		return c; 
		}

	
	// ---------------------

	// -----------------
	static private Component CreateStretchyRectTr(
		System.Type		componentType,
		Transform		root,			
		string			name,
		Rect 			normalizedRect,
		float 			localDepth)
		{
		GameObject o = new GameObject(name, typeof(RectTransform), componentType);
			
		RectTransform tr = o.GetComponent<RectTransform>();
			
		tr.SetParent(root, false);
		tr.localPosition	= new Vector3(0, 0, localDepth);
		tr.localScale		= Vector3.one;
		tr.localRotation	= Quaternion.identity;
			
		tr.pivot = new Vector2(0.5f, 0.5f);

		tr.anchorMax = normalizedRect.max;
		tr.anchorMin = normalizedRect.min;

		tr.anchoredPosition = Vector2.zero;

		tr.offsetMin = Vector2.zero;
		tr.offsetMax = Vector2.zero;

		Component c = o.GetComponent(componentType);
			
		return c; 
		}	

	// ------------------------
	static private RectTransform CreateSubRectTr(
		Transform	parentControl,
		string		name,
		float		relativeSize	
		)
		{
#if UNITY_PRE_5_0
		GameObject o = new GameObject((name + parentControl.childCount), typeof(RectTransform));
#else
		GameObject o = new GameObject(GameObjectUtility.GetUniqueNameForSibling(parentControl, name), typeof(RectTransform));
#endif
		RectTransform tr = o.GetComponent<RectTransform>();
			
		tr.SetParent(parentControl, false);

		tr.pivot = new Vector2(0.5f, 0.5f);


		tr.anchorMin = Vector2.one * (1.0f - relativeSize) * 0.5f;
		tr.anchorMax = Vector2.one * (1.0f + ((relativeSize - 1.0f) * 0.5f));

		tr.offsetMax = Vector2.zero;
		tr.offsetMin = Vector2.zero;
		
		return tr;
		}
		

		
	// -----------------------
	static private void SetImagePreserveAspectRatio(GameObject go, bool preserveAspectRatio)
		{
		if (go == null) return;
		UnityEngine.UI.Image img = go.GetComponent<UnityEngine.UI.Image>();
		if (img != null)
			img.preserveAspect = preserveAspectRatio;
		}

	// -------------------
	static public TouchButtonSpriteAnimator CreateButtonAnimator(
		TouchButton 		target,
		string 				nameSuffix,
		Sprite				sprite,
		float					scale,
		string 				undoLabel = null)
		{
		RectTransform subObj = CreateSubRectTr(target.transform, target.name + nameSuffix, scale);
			
		// Create Sprite animator...

		TouchButtonSpriteAnimator sprAnimator = subObj.gameObject.AddComponent<TouchButtonSpriteAnimator>();

		sprAnimator.autoConnectToSource = true;
		sprAnimator.SetSourceControl(target);

		sprAnimator.SetStateSprite(TouchButtonSpriteAnimator.ControlState.Neutral, sprite);

		SetImagePreserveAspectRatio(subObj.gameObject, true);

			

		if (undoLabel != null)
			Undo.RegisterCreatedObjectUndo(subObj.gameObject, undoLabel);

		return sprAnimator;
		}		
	



	// -------------------
	static public TouchSteeringWheelSpriteAnimator CreateWheelAnimator(
		TouchSteeringWheel	target,
		string 		nameSuffix,
		Sprite		sprite,
		float			scale,
		string		undoLabel = null)
		{
		RectTransform subObj = CreateSubRectTr(target.transform, target.name + nameSuffix, scale);
			
		// Create Sprite animator...

		TouchSteeringWheelSpriteAnimator sprAnimator = subObj.gameObject.AddComponent<TouchSteeringWheelSpriteAnimator>();

		sprAnimator.autoConnectToSource = true;
		sprAnimator.SetSourceControl(target);

		sprAnimator.SetStateSprite(TouchSteeringWheelSpriteAnimator.ControlState.Neutral, sprite);

		SetImagePreserveAspectRatio(subObj.gameObject, true);

			


		if (undoLabel != null)
			Undo.RegisterCreatedObjectUndo(subObj.gameObject, undoLabel);

		return sprAnimator;
		}		

		



	// -------------------
	static public TouchTrackPadSpriteAnimator CreateTouchTrackPadAnimator(
		TouchTrackPad	target,
		string 			nameSuffix,
		Sprite			sprite,
		float				scale,
		string			undoLabel = null)
		{
		RectTransform subObj = CreateSubRectTr(target.transform, target.name + nameSuffix, scale);
		
	
		// Create Sprite animator...

		TouchTrackPadSpriteAnimator sprAnimator = subObj.gameObject.AddComponent<TouchTrackPadSpriteAnimator>();

		sprAnimator.autoConnectToSource = true;
		sprAnimator.SetSourceControl(target);

		sprAnimator.SetStateSprite(TouchTrackPadSpriteAnimator.ControlState.Neutral, sprite);
			

		if (undoLabel != null)
			Undo.RegisterCreatedObjectUndo(subObj.gameObject, undoLabel);

		return sprAnimator;
		}		


	

	// -------------------
	static public SuperTouchZoneSpriteAnimator CreateSuperTouchZoneAnimator(
		ControlFreak2.SuperTouchZone	target,
		string 		nameSuffix,
		Sprite		sprite,
		float			scale,
		string		undoLabel = null)
		{
		RectTransform subObj = CreateSubRectTr(target.transform, target.name + nameSuffix, scale);
		
	
		// Create Sprite animator...

		SuperTouchZoneSpriteAnimator sprAnimator = subObj.gameObject.AddComponent<SuperTouchZoneSpriteAnimator>();

		sprAnimator.autoConnectToSource = true;
		sprAnimator.SetSourceControl(target);

		sprAnimator.SetStateSprite(SuperTouchZoneSpriteAnimator.ControlState.Neutral, sprite);
			

		if (undoLabel != null)
			Undo.RegisterCreatedObjectUndo(subObj.gameObject, undoLabel);

		return sprAnimator;
		}		

			

	// -------------------
	static public void CreateTouchJoystickAnalogAnimators(
		TouchJoystick 	joystick,
		Sprite		baseSprite,
		Sprite		hatSprite,
		float 		hatScale)
		{
		CreateTouchJoystickSimpleAnimator(joystick, "-Base", baseSprite, 1, 0);
		CreateTouchJoystickSimpleAnimator(joystick, "-Hat", hatSprite, hatScale, 0.5f);		
		}
		

	// -------------------
	static public TouchJoystickSpriteAnimator CreateTouchJoystickSimpleAnimator(
		TouchJoystick 	target,
		string 		nameSuffix,
		Sprite		sprite,
		float			scale,
		float			moveScale,
		string		undoLabel = null)
		{
		RectTransform subObj = CreateSubRectTr(target.transform, target.name + nameSuffix, scale);
			
		// Setup sprite animator...

		TouchJoystickSpriteAnimator sprAnimator = subObj.gameObject.AddComponent<TouchJoystickSpriteAnimator>();	

		sprAnimator.autoConnectToSource = true;
		sprAnimator.SetSourceControl(target);

		sprAnimator.animateTransl = (moveScale > 0.0001f);
		sprAnimator.moveScale = Vector2.one * moveScale;

		sprAnimator.spriteNeutralPressed.enabled = true;

		sprAnimator.SetStateSprite(TouchJoystickSpriteAnimator.ControlState.Neutral, sprite);

		SetImagePreserveAspectRatio(subObj.gameObject, true);



		if (undoLabel != null)
			Undo.RegisterCreatedObjectUndo(subObj.gameObject, undoLabel);

		return sprAnimator;

		}		
	
		
	// -----------------
	static public TouchControl CreateStaticControl(
		System.Type			controlType,
		TouchControlPanel	panel, 
		Transform			root,
		string				name,
		Vector2				anchorPos,
		Vector2				offset,
		float 				size,
		float				localDepth)
		{
		TouchControl c = (TouchControl)CreateConstSizeRectTr(controlType, panel.transform, name, anchorPos, offset, Vector2.one * size, localDepth);
		
		c.InvalidateHierarchy();
 
	
		return c;
		}
		

	// -----------------
	static public TouchControl CreateStretchyControl(
		System.Type			controlType,
		TouchControlPanel	panel, 
		Transform			root,
		string				name,
		Rect				regionRect,
		float				localDepth)
		{
		TouchControl c = (TouchControl)CreateStretchyRectTr(controlType, panel.transform, name, regionRect, localDepth);
		
		c.InvalidateHierarchy();
 
		return c;
		}
		


	// ----------------------
	static public DynamicTouchControl CreateDynamicControlWithRegion(
		System.Type			controlType,
		TouchControlPanel	panel, 
		string				name,
		float 				size,
		//Sprite				sprite,
		Rect				regionRect,
		float 				regionDepth,
		float				controlDepth)
		{	
		DynamicRegion region = (DynamicRegion)CreateStretchyRectTr(typeof(DynamicRegion), panel.transform, name + "-Region", regionRect, regionDepth);
		region.InvalidateHierarchy();

		DynamicTouchControl c = (DynamicTouchControl)CreateConstSizeRectTr(controlType, region.transform, name, new Vector2(0.5f, 0.5f), 
			Vector2.zero, Vector2.one * size, (controlDepth - regionDepth));
		c.InvalidateHierarchy();
			
		c.SetTargetDynamicRegion(region);
		region.SetTargetControl(c);

		return c;
		}
		


	// --------------------
	// Default Sprites for newly created controls.
	// --------------------

	const string 	
		DEFAULT_CONTROL_SPRITE_FOLDER = "Assets/Plugins/Control-Freak-2/Sprites/";

	// ---------------------
	static private Sprite LoadDefaultSprite(string atlasName, string spriteName)
		{
		return (Sprite)CFEditorUtils.LoadSubAsset(DEFAULT_CONTROL_SPRITE_FOLDER + atlasName, spriteName, typeof(UnityEngine.Sprite));
		}
		

	// -------------------
	static public Sprite GetDefaultTrackPadSprite		(string suffix = null)	{ return LoadDefaultSprite("CF-Rect.png", null); }
	static public Sprite GetDefaultSuperTouchZoneSprite	(string suffix = null)	{ return LoadDefaultSprite("CF-Rect.png", null); }
	static public Sprite GetDefaultWheelSprite			(string suffix = null)	{ return LoadDefaultSprite("CF-Steering-Wheel.png", null); }
	static public Sprite GetDefaultAnalogJoyBaseSprite	(string suffix = null)	{ return LoadDefaultSprite("CF-Default-Joy-Base.png", null); }
	static public Sprite GetDefaultAnalogJoyHatSprite	(string suffix = null)	{ return LoadDefaultSprite("CF-Default-Joy-Hat.png", null); }
	static public Sprite GetDefaultDpadSprite			(string suffix = null)	{ return LoadDefaultSprite("CF-Dpad-8-way.png", "CF-Dpad-8-way_N"); }

	static public Sprite GetDefaultButtonSprite			(string suffix = null)	
		{ 
		string filename = "CF-Default-Button.png";

		if (suffix.IndexOf("Mirror", System.StringComparison.OrdinalIgnoreCase) >= 0)
			filename = "CF-Button-Mirror.png";
	
		else if (suffix.IndexOf("Reload", System.StringComparison.OrdinalIgnoreCase) >= 0)
			filename = "CF-Button-Reload.png";

		else if (suffix.IndexOf("Fire", System.StringComparison.OrdinalIgnoreCase) >= 0)
			filename = "CF-Button-Fire.png";

		else if (suffix.IndexOf("Pause", System.StringComparison.OrdinalIgnoreCase) >= 0)
			filename = "CF-Button-Pause.png";
	
		else if (suffix.IndexOf("Jump", System.StringComparison.OrdinalIgnoreCase) >= 0)
			filename = "CF-Button-Jump.png";
			
		else if (suffix.IndexOf("Dash", System.StringComparison.OrdinalIgnoreCase) >= 0)
			filename = "CF-Button-Dash.png";
			
		else if (suffix.IndexOf("Punch", System.StringComparison.OrdinalIgnoreCase) >= 0)
			filename = "CF-Button-Punch.png";
			
		else if (suffix.IndexOf("Kick", System.StringComparison.OrdinalIgnoreCase) >= 0)
			filename = "CF-Button-Kick.png";
			
		else if (suffix.IndexOf("Crouch", System.StringComparison.OrdinalIgnoreCase) >= 0)
			filename = "CF-Button-Crouch.png";
			
		else if ((suffix.IndexOf("Block", System.StringComparison.OrdinalIgnoreCase) >= 0) ||
				 (suffix.IndexOf("Guard", System.StringComparison.OrdinalIgnoreCase) >= 0))
			filename = "CF-Button-Guard.png";

		else if ((suffix.IndexOf("Attack", System.StringComparison.OrdinalIgnoreCase) >= 0) )
			filename = "CF-Button-Attack.png";
			
		else if ((suffix.IndexOf("Melee", System.StringComparison.OrdinalIgnoreCase) >= 0))
			filename = "CF-Button-Melee.png";

		else if ((suffix.IndexOf("Cam", System.StringComparison.OrdinalIgnoreCase) >= 0))
			filename = "CF-Button-Camera.png";
			
		else if (suffix.IndexOf("Zoom", System.StringComparison.OrdinalIgnoreCase) >= 0)
			filename = "CF-Button-Zoom.png";
	
		else if (suffix.IndexOf("Jetpack", System.StringComparison.OrdinalIgnoreCase) >= 0)
			filename = "CF-Button-Jetpack.png";
	
		else if ((suffix.IndexOf("Gas", System.StringComparison.OrdinalIgnoreCase) >= 0) ||
				 (suffix.IndexOf("Accel", System.StringComparison.OrdinalIgnoreCase) >= 0))
			filename = "CF-Button-Gas-Pedal.png";

		else if ((suffix.IndexOf("Brake", System.StringComparison.OrdinalIgnoreCase) >= 0))
			filename = "CF-Button-Brake-Pedal.png";

		else if ((suffix.IndexOf("Run", System.StringComparison.OrdinalIgnoreCase) >= 0))
			filename = "CF-Button-Run.png";

		else if ((suffix.IndexOf("Sprint", System.StringComparison.OrdinalIgnoreCase) >= 0))
			filename = "CF-Button-Sprint.png";

		else if ((suffix.IndexOf("Grenade", System.StringComparison.OrdinalIgnoreCase) >= 0))
			filename = "CF-Button-Grenade.png";

		else if (
			(suffix.IndexOf("Flash light", System.StringComparison.OrdinalIgnoreCase) >= 0) ||
			(suffix.IndexOf("Flashlight", System.StringComparison.OrdinalIgnoreCase) >= 0) ||
			(suffix.IndexOf("Flash-light", System.StringComparison.OrdinalIgnoreCase) >= 0) )
			filename = "CF-Button-Flashlight.png";

		else if (
			(suffix.IndexOf("Bullet time", System.StringComparison.OrdinalIgnoreCase) >= 0) ||
			(suffix.IndexOf("Bullet-time", System.StringComparison.OrdinalIgnoreCase) >= 0) ||
			(suffix.IndexOf("BulletTime", System.StringComparison.OrdinalIgnoreCase) >= 0) )
			filename = "CF-Button-Bullet-Time.png";


		else if ((suffix.IndexOf("Grab", System.StringComparison.OrdinalIgnoreCase) >= 0) ||
				 (suffix.IndexOf("Catch", System.StringComparison.OrdinalIgnoreCase) >= 0))
			filename = "CF-Button-Grab.png";

		else if ((suffix.IndexOf("Pass", System.StringComparison.OrdinalIgnoreCase) >= 0))
			filename = "CF-Button-Pass.png";

		else if ((suffix.IndexOf("Shoot", System.StringComparison.OrdinalIgnoreCase) >= 0))
			filename = "CF-Button-Shoot.png";

		else if ((suffix.IndexOf("Use", System.StringComparison.OrdinalIgnoreCase) >= 0))
			filename = "CF-Button-Use.png";

		else if ((suffix.IndexOf("Throw", System.StringComparison.OrdinalIgnoreCase) >= 0))
			filename = "CF-Button-Throw.png";
			
		else if ((suffix.IndexOf("Action", System.StringComparison.OrdinalIgnoreCase) >= 0))
			filename = "CF-Button-Action.png";


		return LoadDefaultSprite(filename, null); 
		}

	// -------------------
		


	}


}

#endif
