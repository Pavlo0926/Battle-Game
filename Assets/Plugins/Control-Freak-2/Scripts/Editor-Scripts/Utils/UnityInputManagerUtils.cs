// -------------------------------------------
// Control Freak 2
// Copyright (C) 2013-2020 Dan's Game Tools
// http://DansGameTools.blogspot.com
// -------------------------------------------

#if UNITY_EDITOR

// ------------------------------------------------------
// Some parts of the code come from : 
//	http://www.plyoung.com/blog/manipulating-input-manager-in-script.html
// ------------------------------------------------------



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
using System.Collections.Generic;

using ControlFreak2;

namespace ControlFreak2Editor
{
static public class UnityInputManagerUtils
	{
	const string INPUT_ASSET_PATH = "ProjectSettings/InputManager.asset";

	public const int 
		MOUSE_X_AXIS_ID				= 1,
		MOUSE_Y_AXIS_ID				= 2,
		SCROLL_PRIMARY_AXIS_ID		= 3,
		SCROLL_SECONDARY_AXIS_ID	= 4; 

	

	// ------------------
	static public bool IsJoystickKeyCode(KeyCode keyCode)
		{
#if UNITY_PRE_5_0
		const KeyCode maxJoyKeycode = KeyCode.Joystick4Button19;
#else
		const KeyCode maxJoyKeycode = KeyCode.Joystick8Button19;
#endif

		return (
			((keyCode >= KeyCode.Joystick1Button0) && (keyCode <= maxJoyKeycode)) ||
			((keyCode >= KeyCode.JoystickButton0) && (keyCode <= KeyCode.JoystickButton19)) );
		}
		

	// ----------------------
	static private void Clear()
		{
		SerializedObject serializedObject = new SerializedObject(AssetDatabase.LoadAllAssetsAtPath(INPUT_ASSET_PATH)[0]);
		SerializedProperty axesProperty = serializedObject.FindProperty("m_Axes");
		axesProperty.ClearArray();
		serializedObject.ApplyModifiedProperties();
		}


	// ---------------------	
	private static SerializedProperty GetChildProperty(SerializedProperty parent, string name)
		{
		SerializedProperty child = parent.Copy();
		child.Next(true);
		do	{
			if (child.name == name) 
				return child;
			}
			while (child.Next(false));
		
		return null;
		}
		

	// --------------------
	public enum AxisType
		{
		KeyOrMouseButton	= 0,
		MouseMovement		= 1,
		JoystickAxis		= 2
		};

	public class InputAxis
		{
		public string name;
		public string descriptiveName;
		public string descriptiveNegativeName;
		public string negativeButton;
		public string positiveButton;
		public string altNegativeButton;
		public string altPositiveButton;
	
		public float gravity;
		public float dead;
		public float sensitivity;
	
		public bool snap = false;
		public bool invert = false;
	
		public AxisType type;
	
		public int axis;
		public int joyNum;
		}
			


	// ---------------------
	public class InputAxisList : List<InputAxis>
		{
		// ---------------------
		public InputAxisList(int capacity) : base(capacity)
			{
			}

			
		// --------------------
		public bool Contains(string name)
			{
			return (this.Find(x => (x.name == name)) != null);
			}
			
		// -------------------	
		public void AddAxisList(InputAxisList list)
			{
			for (int i = 0; i < list.Count; ++i)
				this.AddAxis(list[i]);
			}

		// --------------------
		public void RemoveAxisList(InputAxisList list)
			{
			for (int i = 0; i < list.Count; ++i)
				this.RemoveAxis(list[i]);
			}

		// --------------------
		public void AddAxis(InputAxis axis)
			{
			int index = this.FindIndex(x => (x.name == axis.name));
			if (index < 0)
				this.Add(axis);
			else
				this[index] = axis;
			}

		// --------------------
		public void RemoveAxis(InputAxis axis)
			{
			int index = this.FindIndex(x => (x.name == axis.name));
			if (index >= 0)
				this.RemoveAt(index);
			}
		}


	// ----------------------
	static public InputAxisList LoadInputManagerAxes()
		{
		try 
			{
			SerializedObject serializedObject = new SerializedObject(AssetDatabase.LoadAllAssetsAtPath(INPUT_ASSET_PATH)[0]);
			SerializedProperty axesProperty = serializedObject.FindProperty("m_Axes");
		
			int axisCount = axesProperty.arraySize;
			InputAxisList axes = new InputAxisList(axisCount); // InputAxis[axisCount];
	
			for (int i = 0; i < axisCount; ++i)
				{
				SerializedProperty axisProperty = axesProperty.GetArrayElementAtIndex(i);
	
				InputAxis axis = new InputAxis();
				//axes[i] = axis;
				axes.Add(axis);

				axis.name 						= GetChildProperty(axisProperty, "m_Name").stringValue;
				axis.descriptiveName 			= GetChildProperty(axisProperty, "descriptiveName").stringValue;	
				axis.descriptiveNegativeName	= GetChildProperty(axisProperty, "descriptiveNegativeName").stringValue;
				axis.negativeButton				= GetChildProperty(axisProperty, "negativeButton").stringValue;
				axis.positiveButton				= GetChildProperty(axisProperty, "positiveButton").stringValue;
				axis.altNegativeButton			= GetChildProperty(axisProperty, "altNegativeButton").stringValue;
				axis.altPositiveButton			= GetChildProperty(axisProperty, "altPositiveButton").stringValue;
				axis.gravity					= GetChildProperty(axisProperty, "gravity").floatValue;
				axis.dead						= GetChildProperty(axisProperty, "dead").floatValue;
				axis.sensitivity				= GetChildProperty(axisProperty, "sensitivity").floatValue;
				axis.snap						= GetChildProperty(axisProperty, "snap").boolValue;
				axis.invert						= GetChildProperty(axisProperty, "invert").boolValue;
				axis.type 						= (AxisType)GetChildProperty(axisProperty, "type").intValue;
				axis.axis 						= GetChildProperty(axisProperty, "axis").intValue +1; //??
				axis.joyNum 					= GetChildProperty(axisProperty, "joyNum").intValue;

				}
			
			return axes;
			}
		catch (System.Exception )
			{
			}

		return null;		
		}



	// -----------------------
	static private void SaveAxesToInputManager(InputAxisList list)
		{
		try 
			{	
			SerializedObject serializedObject = new SerializedObject(AssetDatabase.LoadAllAssetsAtPath(INPUT_ASSET_PATH)[0]);
			SerializedProperty axesProperty = serializedObject.FindProperty("m_Axes");
		
			axesProperty.arraySize = list.Count;
			serializedObject.ApplyModifiedProperties();
		
			for (int i = 0; i < list.Count; ++i)
				{
				InputAxis axis = list[i];
		
				SerializedProperty axisProperty = axesProperty.GetArrayElementAtIndex(i);
			
				GetChildProperty(axisProperty, "m_Name").stringValue = axis.name;
				GetChildProperty(axisProperty, "descriptiveName").stringValue = axis.descriptiveName;
				GetChildProperty(axisProperty, "descriptiveNegativeName").stringValue = axis.descriptiveNegativeName;
				GetChildProperty(axisProperty, "negativeButton").stringValue = axis.negativeButton;
				GetChildProperty(axisProperty, "positiveButton").stringValue = axis.positiveButton;
				GetChildProperty(axisProperty, "altNegativeButton").stringValue = axis.altNegativeButton;
				GetChildProperty(axisProperty, "altPositiveButton").stringValue = axis.altPositiveButton;
				GetChildProperty(axisProperty, "gravity").floatValue = axis.gravity;
				GetChildProperty(axisProperty, "dead").floatValue = axis.dead;
				GetChildProperty(axisProperty, "sensitivity").floatValue = axis.sensitivity;
				GetChildProperty(axisProperty, "snap").boolValue = axis.snap;
				GetChildProperty(axisProperty, "invert").boolValue = axis.invert;
				GetChildProperty(axisProperty, "type").intValue = (int)axis.type;
				GetChildProperty(axisProperty, "axis").intValue = axis.axis - 1;
				GetChildProperty(axisProperty, "joyNum").intValue = axis.joyNum;			
				}

			serializedObject.ApplyModifiedProperties();
		
			}
		catch (System.Exception )
			{
			}

		}


	// -----------------
	private static InputAxisList GetDefaultAxes()
		{
		InputAxisList axes = new InputAxisList(30);

		axes.Add(new InputAxis()	{	name	= "Horizontal",	descriptiveName	= "",	descriptiveNegativeName	= "",	negativeButton	= "left",	positiveButton	= "right",	altNegativeButton	= "a",	altPositiveButton	= "d",	gravity	= 3f,	dead	= 0.001f,	sensitivity	= 3f,	snap	= true,	invert	= false,	type	= AxisType.KeyOrMouseButton,	axis	= 0,	joyNum	= 0	});
		axes.Add(new InputAxis()	{	name	= "Vertical",	descriptiveName	= "",	descriptiveNegativeName	= "",	negativeButton	= "down",	positiveButton	= "up",	altNegativeButton	= "s",	altPositiveButton	= "w",	gravity	= 3f,	dead	= 0.001f,	sensitivity	= 3f,	snap	= true,	invert	= false,	type	= AxisType.KeyOrMouseButton,	axis	= 0,	joyNum	= 0	});
		axes.Add(new InputAxis()	{	name	= "Fire1",	descriptiveName	= "",	descriptiveNegativeName	= "",	negativeButton	= "",	positiveButton	= "left ctrl",	altNegativeButton	= "",	altPositiveButton	= "mouse 0",	gravity	= 1000f,	dead	= 0.001f,	sensitivity	= 1000f,	snap	= false,	invert	= false,	type	= AxisType.KeyOrMouseButton,	axis	= 0,	joyNum	= 0	});
		axes.Add(new InputAxis()	{	name	= "Fire2",	descriptiveName	= "",	descriptiveNegativeName	= "",	negativeButton	= "",	positiveButton	= "left alt",	altNegativeButton	= "",	altPositiveButton	= "mouse 1",	gravity	= 1000f,	dead	= 0.001f,	sensitivity	= 1000f,	snap	= false,	invert	= false,	type	= AxisType.KeyOrMouseButton,	axis	= 0,	joyNum	= 0	});
		axes.Add(new InputAxis()	{	name	= "Fire3",	descriptiveName	= "",	descriptiveNegativeName	= "",	negativeButton	= "",	positiveButton	= "left cmd",	altNegativeButton	= "",	altPositiveButton	= "mouse 2",	gravity	= 1000f,	dead	= 0.001f,	sensitivity	= 1000f,	snap	= false,	invert	= false,	type	= AxisType.KeyOrMouseButton,	axis	= 0,	joyNum	= 0	});
		axes.Add(new InputAxis()	{	name	= "Jump",	descriptiveName	= "",	descriptiveNegativeName	= "",	negativeButton	= "",	positiveButton	= "space",	altNegativeButton	= "",	altPositiveButton	= "",	gravity	= 1000f,	dead	= 0.001f,	sensitivity	= 1000f,	snap	= false,	invert	= false,	type	= AxisType.KeyOrMouseButton,	axis	= 0,	joyNum	= 0	});
		axes.Add(new InputAxis()	{	name	= "Mouse X",	descriptiveName	= "",	descriptiveNegativeName	= "",	negativeButton	= "",	positiveButton	= "",	altNegativeButton	= "",	altPositiveButton	= "",	gravity	= 0f,	dead	= 0f,	sensitivity	= 0.1f,	snap	= false,	invert	= false,	type	= AxisType.MouseMovement,	axis	= 1,	joyNum	= 0	});
		axes.Add(new InputAxis()	{	name	= "Mouse Y",	descriptiveName	= "",	descriptiveNegativeName	= "",	negativeButton	= "",	positiveButton	= "",	altNegativeButton	= "",	altPositiveButton	= "",	gravity	= 0f,	dead	= 0f,	sensitivity	= 0.1f,	snap	= false,	invert	= false,	type	= AxisType.MouseMovement,	axis	= 2,	joyNum	= 0	});
		axes.Add(new InputAxis()	{	name	= "Mouse ScrollWheel",	descriptiveName	= "",	descriptiveNegativeName	= "",	negativeButton	= "",	positiveButton	= "",	altNegativeButton	= "",	altPositiveButton	= "",	gravity	= 0f,	dead	= 0f,	sensitivity	= 0.1f,	snap	= false,	invert	= false,	type	= AxisType.MouseMovement,	axis	= 3,	joyNum	= 0	});
		axes.Add(new InputAxis()	{	name	= "Horizontal",	descriptiveName	= "",	descriptiveNegativeName	= "",	negativeButton	= "",	positiveButton	= "",	altNegativeButton	= "",	altPositiveButton	= "",	gravity	= 0f,	dead	= 0.19f,	sensitivity	= 1f,	snap	= false,	invert	= false,	type	= AxisType.JoystickAxis,	axis	= 1,	joyNum	= 0	});
		axes.Add(new InputAxis()	{	name	= "Vertical",	descriptiveName	= "",	descriptiveNegativeName	= "",	negativeButton	= "",	positiveButton	= "",	altNegativeButton	= "",	altPositiveButton	= "",	gravity	= 0f,	dead	= 0.19f,	sensitivity	= 1f,	snap	= false,	invert	= true,	type	= AxisType.JoystickAxis,	axis	= 2,	joyNum	= 0	});
		axes.Add(new InputAxis()	{	name	= "Fire1",	descriptiveName	= "",	descriptiveNegativeName	= "",	negativeButton	= "",	positiveButton	= "joystick button 0",	altNegativeButton	= "",	altPositiveButton	= "",	gravity	= 1000f,	dead	= 0.001f,	sensitivity	= 1000f,	snap	= false,	invert	= false,	type	= AxisType.KeyOrMouseButton,	axis	= 0,	joyNum	= 0	});
		axes.Add(new InputAxis()	{	name	= "Fire2",	descriptiveName	= "",	descriptiveNegativeName	= "",	negativeButton	= "",	positiveButton	= "joystick button 1",	altNegativeButton	= "",	altPositiveButton	= "",	gravity	= 1000f,	dead	= 0.001f,	sensitivity	= 1000f,	snap	= false,	invert	= false,	type	= AxisType.KeyOrMouseButton,	axis	= 0,	joyNum	= 0	});
		axes.Add(new InputAxis()	{	name	= "Fire3",	descriptiveName	= "",	descriptiveNegativeName	= "",	negativeButton	= "",	positiveButton	= "joystick button 2",	altNegativeButton	= "",	altPositiveButton	= "",	gravity	= 1000f,	dead	= 0.001f,	sensitivity	= 1000f,	snap	= false,	invert	= false,	type	= AxisType.KeyOrMouseButton,	axis	= 0,	joyNum	= 0	});
		axes.Add(new InputAxis()	{	name	= "Jump",	descriptiveName	= "",	descriptiveNegativeName	= "",	negativeButton	= "",	positiveButton	= "joystick button 3",	altNegativeButton	= "",	altPositiveButton	= "",	gravity	= 1000f,	dead	= 0.001f,	sensitivity	= 1000f,	snap	= false,	invert	= false,	type	= AxisType.KeyOrMouseButton,	axis	= 0,	joyNum	= 0	});
		axes.Add(new InputAxis()	{	name	= "Submit",	descriptiveName	= "",	descriptiveNegativeName	= "",	negativeButton	= "",	positiveButton	= "return",	altNegativeButton	= "",	altPositiveButton	= "joystick button 0",	gravity	= 1000f,	dead	= 0.001f,	sensitivity	= 1000f,	snap	= false,	invert	= false,	type	= AxisType.KeyOrMouseButton,	axis	= 0,	joyNum	= 0	});
		axes.Add(new InputAxis()	{	name	= "Submit",	descriptiveName	= "",	descriptiveNegativeName	= "",	negativeButton	= "",	positiveButton	= "enter",	altNegativeButton	= "",	altPositiveButton	= "space",	gravity	= 1000f,	dead	= 0.001f,	sensitivity	= 1000f,	snap	= false,	invert	= false,	type	= AxisType.KeyOrMouseButton,	axis	= 0,	joyNum	= 0	});
		axes.Add(new InputAxis()	{	name	= "Cancel",	descriptiveName	= "",	descriptiveNegativeName	= "",	negativeButton	= "",	positiveButton	= "escape",	altNegativeButton	= "",	altPositiveButton	= "joystick button 1",	gravity	= 1000f,	dead	= 0.001f,	sensitivity	= 1000f,	snap	= false,	invert	= false,	type	= AxisType.KeyOrMouseButton,	axis	= 0,	joyNum	= 0	});

		return axes;		
		}
		
	

	// -------------------
	static private InputAxisList GetControlFreakAxes()
		{
		InputAxisList axes = new InputAxisList(5 + (GamepadManager.MAX_JOYSTICKS * GamepadManager.MAX_INTERNAL_AXES));

		// Add mouse definit
		axes.Add(new InputAxis() { name = InputRig.CF_EMPTY_AXIS,				sensitivity = 1f, type = AxisType.KeyOrMouseButton });
		axes.Add(new InputAxis() { name = InputRig.CF_MOUSE_DELTA_X_AXIS,		sensitivity = 1f, type = AxisType.MouseMovement, axis = 1 });
		axes.Add(new InputAxis() { name = InputRig.CF_MOUSE_DELTA_Y_AXIS,		sensitivity = 1f, type = AxisType.MouseMovement, axis = 2 });
		axes.Add(new InputAxis() { name = InputRig.CF_SCROLL_WHEEL_X_AXIS,		sensitivity = 1f, type = AxisType.MouseMovement, axis = 3 });
		axes.Add(new InputAxis() { name = InputRig.CF_SCROLL_WHEEL_Y_AXIS,		sensitivity = 1f, type = AxisType.MouseMovement, axis = 4 });
	

		// Add gamepad definitions...

		for (int i = 0; i < GamepadManager.MAX_JOYSTICKS; i++)
			{
			for (int j = 0; j < GamepadManager.MAX_INTERNAL_AXES; j++)
				{
				axes.Add(new InputAxis() 
					{ 
					name 		= GamepadManager.GetJoyAxisName(i, j), 
					dead 		= 0.2f,
					sensitivity = 1f,
					type		= AxisType.JoystickAxis,
					axis		= (j + 1),
					joyNum		= (i + 1)
					});
				}
			}	

		return axes;
		}



	// ----------------------
	static public bool AreControlFreakAxesPresent()
		{
		InputAxisList cfAxes = GetControlFreakAxes();

		InputAxisList unityAxes = LoadInputManagerAxes();


		for (int i = 0; i < cfAxes.Count; ++i)
			{
			if (!unityAxes.Contains(cfAxes[i].name))
				{
//Debug.Log("Axis [" + cfAxes[i].name + "] is not defined!");
				return false;
				}
			}
			
		return true;
		}

		

	// -------------------
	static public void AddControlFreakAxes()
		{
		InputAxisList axes = LoadInputManagerAxes();

		axes.AddAxisList(GetControlFreakAxes());

		SaveAxesToInputManager(axes);
		}


	// ----------------
	static public void RemoveControlFreakAxes()
		{
		InputAxisList axes = LoadInputManagerAxes();

		axes.RemoveAxisList(GetControlFreakAxes());

		SaveAxesToInputManager(axes);
		}
		
	}
}


#endif
