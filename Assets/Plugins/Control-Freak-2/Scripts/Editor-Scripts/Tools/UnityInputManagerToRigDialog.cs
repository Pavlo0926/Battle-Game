// -------------------------------------------
// Control Freak 2
// Copyright (C) 2013-2020 Dan's Game Tools
// http://DansGameTools.blogspot.com
// -------------------------------------------

#if UNITY_EDITOR

using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

using ControlFreak2;

namespace ControlFreak2Editor
{

public class UnityInputManagerToRigDialog : EditorWindow 
	{
	private Vector2 
		scrollPos;
	private List<AxisDescription>
		axisList;
 	private InputRig
		rig;
		

	const string DIALOG_TITLE = "CF2 - Transfer Input Manager Axes to Input Rig";

		
	// --------------------
	public UnityInputManagerToRigDialog() : base()
		{
		this.minSize = new Vector2(500, 400);
		CFEditorUtils.SetWindowTitle(this, new GUIContent(DIALOG_TITLE));
		
		this.axisList = new List<AxisDescription>(32);
		}


	// ---------------------
	public static void AskToShowDialog(InputRig rig)
		{
		if (EditorUtility.DisplayDialog("Control Freak 2", "Do you wish to transfer axis configuration from Unity Input Manager to the newly created rig?", "Yes", "No"))
			ShowDialog(rig);
		}

	// --------------------
	public static void ShowDialog(InputRig rig)
		{
		UnityInputManagerToRigDialog dialog = ScriptableObject.CreateInstance<UnityInputManagerToRigDialog>();

		dialog.InitDialog(rig);
	
		dialog.ShowUtility();
		}
		

	// -------------------
	public void InitDialog(InputRig rig)
		{
		this.rig = rig;

		// Add axes...

		UnityInputManagerUtils.InputAxisList inputAxisList = UnityInputManagerUtils.LoadInputManagerAxes();

		for (int i = 0; i < inputAxisList.Count; ++i)
			{
			if (inputAxisList[i].name.StartsWith("cf"))	
				continue;

			UnityInputManagerUtils.InputAxis inputAxis = inputAxisList[i];

			AxisDescription axisDesc = this.FindDescription(inputAxis);

			if (axisDesc == null)
				{
				axisDesc = new AxisDescription(inputAxis);
					
				int axisId = 0;

				axisDesc.enabled 			= true; 
				axisDesc.wasAlreadyDefined	= rig.IsAxisDefined(inputAxis.name, ref axisId);

				if (inputAxis.name.Equals("Submit", System.StringComparison.OrdinalIgnoreCase) ||
					inputAxis.name.Equals("Cancel", System.StringComparison.OrdinalIgnoreCase))
					axisDesc.enabled = false;
			

				this.axisList.Add(axisDesc);	
					
				}
			else
				{
				axisDesc.inputAxes.Add(inputAxis);
				}
			}
		}


				
	// ---------------
	private AxisDescription FindDescription(UnityInputManagerUtils.InputAxis inputAxis)
		{
		return (this.axisList.Find(x => (x.name.Equals(inputAxis.name))));
		}
		


	

	// ----------------
	private void AddAxesToRig()
		{
			
		// Check for already present axes...

		int enabledAxisNum			= 0;
		int alreadyPresentAxisNum	= 0;
		string presentAxisNames		= "";			

		for (int i = 0; i < this.axisList.Count; ++i)
			{
			if (!this.axisList[i].enabled)
				continue;

			enabledAxisNum++;
				
			int axisId = 0;
			if (this.rig.IsAxisDefined(this.axisList[i].name, ref axisId))
				{
				++alreadyPresentAxisNum;

				if (alreadyPresentAxisNum <= 10)
					presentAxisNames += (((alreadyPresentAxisNum == 10) ? "..." : this.axisList[i].name) + "\n");  
				}	

			}
			
		bool overwriteAll = false;
		bool igonrePresentAxes = false;
		
		if (alreadyPresentAxisNum > 0)
			{
			int overwriteMethod = EditorUtility.DisplayDialogComplex(DIALOG_TITLE, "" + alreadyPresentAxisNum + " out of " + enabledAxisNum + 
				" selected axes are already present in selected Input Rig.\n\n" +	
				presentAxisNames + "\n" + "What do you want to do with them?", "Overwrite All", "Ignore All", "Choose one by one");

				
			if (overwriteMethod == 1)
				igonrePresentAxes = true;

			else if (overwriteMethod == 0)
				overwriteAll = true;	
			}


		// Apply...

		CFGUI.CreateUndo("Transfer axes from Input Manager to Input Rig", this.rig);
	
		for (int i = 0; i < this.axisList.Count; ++i)
			{
			AxisDescription axisDesc = this.axisList[i];
			if (!axisDesc.enabled)
				continue;

			
			
			InputRig.AxisConfig axisConfig = this.rig.GetAxisConfig(axisDesc.name);

			if (axisConfig != null)
				{
				if (igonrePresentAxes)
					continue;

				if (!overwriteAll && !EditorUtility.DisplayDialog(DIALOG_TITLE, "Transfer and overwrite [" + axisDesc.name + "] axis?", "Transfer", "Skip"))
					continue;
			
				axisConfig.axisType = axisDesc.targetAxisType;
				}	
			else
				{
				axisConfig = this.rig.axes.Add(axisDesc.name, axisDesc.targetAxisType, false);
				}	

				
			axisConfig.keyboardNegative		= KeyCode.None;
			axisConfig.keyboardNegativeAlt0	= KeyCode.None;
			axisConfig.keyboardPositive		= KeyCode.None;	
			axisConfig.keyboardPositiveAlt0	= KeyCode.None;	

			axisConfig.scale = 1;	
			axisConfig.digitalToAnalogDecelTime = 0;
			axisConfig.digitalToAnalogAccelTime = 0;
			axisConfig.smoothingTime = 0;
			axisConfig.rawSmoothingTime = 0;
			axisConfig.snap = false;				

			for (int ai = 0; ai < axisDesc.inputAxes.Count; ++ai)	
				{
				UnityInputManagerUtils.InputAxis inputAxis = axisDesc.inputAxes[ai];
					
				switch (inputAxis.type)
					{
					case UnityInputManagerUtils.AxisType.KeyOrMouseButton :
						KeyCode 	
							positiveCode 		= InputRig.NameToKeyCode(!inputAxis.invert ? inputAxis.positiveButton : inputAxis.negativeButton),
							positiveAltCode	= InputRig.NameToKeyCode(!inputAxis.invert ? inputAxis.altPositiveButton : inputAxis.altNegativeButton),
							negativeCode 		= InputRig.NameToKeyCode(!inputAxis.invert ? inputAxis.negativeButton : inputAxis.positiveButton),
							negativeAltCode	= InputRig.NameToKeyCode(!inputAxis.invert ? inputAxis.altNegativeButton : inputAxis.altPositiveButton);
		
						if ((positiveCode != KeyCode.None) && !UnityInputManagerUtils.IsJoystickKeyCode(positiveCode))
							{
							axisConfig.keyboardPositive = positiveCode;
							axisConfig.affectedKeyPositive = positiveCode;
							}
						if ((positiveAltCode != KeyCode.None) && !UnityInputManagerUtils.IsJoystickKeyCode(positiveAltCode))
							axisConfig.keyboardPositiveAlt0 = positiveAltCode;

						if ((negativeCode != KeyCode.None) && !UnityInputManagerUtils.IsJoystickKeyCode(negativeCode))
							{
							axisConfig.keyboardNegative = negativeCode;
							axisConfig.affectedKeyNegative = negativeCode;
							}
						if ((negativeAltCode != KeyCode.None) && !UnityInputManagerUtils.IsJoystickKeyCode(negativeAltCode))
							axisConfig.keyboardNegativeAlt0 = negativeAltCode;


						if (inputAxis.snap)
							axisConfig.snap = true;

						break;


					case UnityInputManagerUtils.AxisType.JoystickAxis :
						break;


					case UnityInputManagerUtils.AxisType.MouseMovement :
						{
						// Mouse Delta...

						if ((inputAxis.axis == UnityInputManagerUtils.MOUSE_X_AXIS_ID) ||
							(inputAxis.axis == UnityInputManagerUtils.MOUSE_Y_AXIS_ID))
							{
							ControlFreak2.Internal.AxisBinding mouseDeltaBinding = (inputAxis.axis == UnityInputManagerUtils.MOUSE_X_AXIS_ID) ? 
								this.rig.mouseConfig.horzDeltaBinding : this.rig.mouseConfig.vertDeltaBinding;

							mouseDeltaBinding.Clear();
							mouseDeltaBinding.Enable(); //.enabled = true;
							mouseDeltaBinding.AddTarget().SetSingleAxis(axisDesc.name, inputAxis.invert); //, axisDesc.inputAxis.invert);
			
							//mouseDeltaBinding.separateAxes = false;
							//mouseDeltaBinding.singleAxis = axisDesc.name;

							axisConfig.scale = inputAxis.sensitivity;
							}

						// Scroll wheel...

						else if ((inputAxis.axis == UnityInputManagerUtils.SCROLL_PRIMARY_AXIS_ID) ||
								 (inputAxis.axis == UnityInputManagerUtils.SCROLL_SECONDARY_AXIS_ID))
							{
							ControlFreak2.Internal.AxisBinding scrollBinding = (inputAxis.axis == UnityInputManagerUtils.SCROLL_PRIMARY_AXIS_ID) ? 
								this.rig.scrollWheel.vertScrollDeltaBinding.deltaBinding : this.rig.scrollWheel.horzScrollDeltaBinding.deltaBinding;
							
							scrollBinding.Clear();
							scrollBinding.AddTarget().SetSingleAxis(axisDesc.name, inputAxis.invert);

							//scrollBinding.enabled = true;
							//scrollBinding.separateAxes = false;
							//scrollBinding.singleAxis = axisDesc.name;
							}
						}
						break;
					}
				
				
				}

			// Set mouse delta scaling...

			if (axisDesc.targetAxisType == InputRig.AxisType.Delta)
				{
				axisConfig.deltaMode = InputRig.DeltaTransformMode.EmulateMouse;
				axisConfig.scale = axisDesc.inputAxis.sensitivity;
				}					

			// Convert gravity to smoothing time...

			else if ((axisDesc.targetAxisType == InputRig.AxisType.SignedAnalog) || (axisDesc.targetAxisType == InputRig.AxisType.UnsignedAnalog))
				{
				float 
					gravity = 0,
					sensitivity = 0;
					
				// Find biggest gravity and sensitivity...

				for (int di = 0; di < axisDesc.inputAxes.Count; ++di)
					{
					if (axisDesc.inputAxes[di].type != UnityInputManagerUtils.AxisType.KeyOrMouseButton)
						continue;

					gravity		= Mathf.Max(gravity,		axisDesc.inputAxes[di].gravity);
					sensitivity	= Mathf.Max(sensitivity,	axisDesc.inputAxes[di].sensitivity);
					}
					
				// Convert graivty and sensitivity to digiAccel/Decel times...

				axisConfig.digitalToAnalogDecelTime = ((gravity < 0.001f) 		? 0.2f : (1.0f / gravity));
				axisConfig.digitalToAnalogAccelTime = ((sensitivity < 0.001f)	? 0.2f : (1.0f / sensitivity));
		
				axisConfig.smoothingTime = 0;
				axisConfig.rawSmoothingTime = 0;

				//if (axisDesc.inputAxis.gravity > 0.1f)		
				//	axisConfig.smoothingTime = Mathf.Min(1.0f, (1.0f / axisDesc.inputAxis.gravity));
				}						
				
		
			//if (axisDesc.inputAxis.invert)
			//	axisConfig.scale = -axisConfig.scale;

			}

		CFGUI.EndUndo(this.rig);
		}

	
	// -------------------	
	public void OnGUI()
		{
	
		this.scrollPos = EditorGUILayout.BeginScrollView(this.scrollPos, CFEditorStyles.Inst.transpSunkenBG);
			
			for (int i = 0; i < this.axisList.Count; ++i)
				{
				this.axisList[i].DrawElementGUI();
				}

		EditorGUILayout.EndScrollView();
			

		// Creation buttons...

		EditorGUILayout.BeginHorizontal();
			EditorGUILayout.Space();
			if (GUILayout.Button(new GUIContent("Transfer Axes", "Add selected axes to Input Rig."), GUILayout.Height(30)))
				{
				this.AddAxesToRig();
				this.Close();
				}
		EditorGUILayout.Space();

		EditorGUILayout.EndHorizontal();
		EditorGUILayout.Space();

		}
		


	
	
		
	// ----------------------
	// Axis Description Class
	// ----------------------
	public class AxisDescription
		{
		public string 
			name;
		public bool	
			enabled,
			wasAlreadyDefined;
		public InputRig.AxisType
			targetAxisType;
			
		public UnityInputManagerUtils.InputAxis
			inputAxis;

		public List<UnityInputManagerUtils.InputAxis>
			inputAxes;

		
		// -------------------
		public AxisDescription(UnityInputManagerUtils.InputAxis	inputAxis)
			{
			this.name		= inputAxis.name;	
			this.inputAxis	= inputAxis;

			this.inputAxes	= new List<UnityInputManagerUtils.InputAxis>(4);

			this.inputAxes.Add(inputAxis);

			if (inputAxis.type == UnityInputManagerUtils.AxisType.JoystickAxis)
				{
				this.targetAxisType = InputRig.AxisType.SignedAnalog;
				}					

			else if (inputAxis.type == UnityInputManagerUtils.AxisType.MouseMovement)
				{
				this.targetAxisType = (((inputAxis.axis == UnityInputManagerUtils.SCROLL_PRIMARY_AXIS_ID) || 
					(inputAxis.axis == UnityInputManagerUtils.SCROLL_SECONDARY_AXIS_ID)) ?
						InputRig.AxisType.ScrollWheel : InputRig.AxisType.Delta);
				}

			else 
				{
				if ((inputAxis.negativeButton.Length > 0) || (inputAxis.altNegativeButton.Length > 0))
					this.targetAxisType = InputRig.AxisType.SignedAnalog;
				else
					this.targetAxisType = InputRig.AxisType.Digital;
				}
			}
			

		// ------------------
		public void DrawElementGUI()
			{
			EditorGUILayout.BeginHorizontal(CFEditorStyles.Inst.transpBevelBG);
			
				this.enabled = EditorGUILayout.ToggleLeft(new GUIContent(this.name, "Transfer this axis to Input Rig."), this.enabled, GUILayout.ExpandWidth(true));
	
				this.targetAxisType = (InputRig.AxisType)CFGUI.EnumPopup(new GUIContent("Add as:", "Select target axis type."), this.targetAxisType, 50, GUILayout.Width(180));
				

			EditorGUILayout.EndHorizontal();
			}
			
		}
	}
}


#endif
