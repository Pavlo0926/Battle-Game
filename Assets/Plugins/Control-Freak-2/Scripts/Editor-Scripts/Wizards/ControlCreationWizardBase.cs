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

using System.Collections.Generic;

using ControlFreak2;
using ControlFreak2.Internal;
using ControlFreak2Editor.Internal;

namespace ControlFreak2Editor.Inspectors
{




// ----------------------
public enum ControlMode
	{
	Static,
	DynamicWithRegion
	}

// --------------------	
public enum RegionRectPreset
	{
	FullScreen,

	LeftHalf,
	RightHalf,

	LeftThird,
	MidThird,
	RightThird,

	TopLeftQuarter,
	TopRightQuarter,
	BottomLeftQuarter,
	BottomRightQuarter,
	}

// ----------------------
public enum ControlAnchorPoint
	{
	BottomRight,
	BottomLeft,
	TopRight,
	TopLeft
	}
	
		

// -----------------------	
abstract public class ControlCreationWizardBase : EditorWindow
	{
	public const float MAX_LABEL_WIDTH = 100;				


	public 		TouchControlPanel panel;
	public		Canvas 			canvas;
	protected	string			
		controlName,
		defaultControlName;
		
	protected System.Action 
		onCreationCallback;
	
	protected Sprite 
		defaultSprite;

	protected ControlMode 
		dynamicMode;

	protected TouchControl.Shape
		controlShape;

	protected RegionRectPreset
		regionRect;
	
	protected ControlAnchorPoint
		staticAnchor;	
	
	protected float		
		controlSize,
		controlDepth,
		regionDepth;

	static protected bool
		mStoredParamsSaved;
	static protected ControlAnchorPoint
		mStoredStaticAnchor;
	static protected TouchControl.Shape
		mStoredControlShape;
	static protected float
		mStoredControlSize,
		mStoredControlDepth,
		mStoredRegionDepth;

	private Vector2 scrollPos;

	protected bool 
		emulateTouchPressure;




	public const float
		DEPTH_MIN = 0,
		DEPTH_MAX = 100;

	// -------------------	
	abstract protected void OnCreatePressed(bool selectAfterwards);
	//abstract protected GUIContent GetTitle();
	


	// --------------------
	public void Init(TouchControlPanel panel, System.Action onCreationCallback)
		{
		this.panel = panel;
		this.onCreationCallback = onCreationCallback;

		this.emulateTouchPressure = true;

		this.canvas = (Canvas)CFUtils.GetComponentHereOrInParent(this.panel, typeof(Canvas));

		this.controlName = "NewControl";

		this.dynamicMode	= ControlMode.Static;
		this.regionRect		= RegionRectPreset.LeftHalf;
		this.controlShape	= TouchControl.Shape.Circle;

		this.staticAnchor	= ControlAnchorPoint.BottomRight;
		this.controlSize	= 0.15f;
	
		this.controlDepth	= Mathf.Lerp(DEPTH_MIN, DEPTH_MAX, 0.25f);
		this.regionDepth	= Mathf.Lerp(DEPTH_MIN, DEPTH_MAX, 0.70f);
		
		// Restore params...

		if (mStoredParamsSaved)
			{
			this.controlSize	= mStoredControlSize;
			this.staticAnchor	= mStoredStaticAnchor;	
			this.controlDepth	= mStoredControlDepth;
			this.regionDepth	= mStoredRegionDepth;
			this.controlShape	= mStoredControlShape;
			}
		}



	
	// -----------------
	private void StoreParameters()
		{
		mStoredParamsSaved 	= true;
		mStoredControlSize 	= this.controlSize;
		mStoredStaticAnchor = this.staticAnchor;
		mStoredRegionDepth 	= this.regionDepth;
		mStoredControlDepth = this.controlDepth;
		mStoredControlShape	= this.controlShape;
		}

	// ----------------
	public ControlCreationWizardBase() : base()
		{
		this.minSize = new Vector2(350, 320);
		//this.maxSize = new Vector2(350, 500);
		}
	

	// ----------------------
	protected TouchControl CreateDynamicTouchControl(System.Type controlType)
		{
		TouchControl c = null;
		if (this.dynamicMode == ControlMode.Static)
			{
			c = TouchControlWizardUtils.CreateStaticControl(controlType, this.panel, this.panel.transform, this.controlName, 
				this.GetAnchorPos(), this.GetControlOffset(), this.GetControlSize(), this.GetControlDepth());
			}
		else
			{
			c = TouchControlWizardUtils.CreateDynamicControlWithRegion(controlType, this.panel, this.controlName, 
				this.GetControlSize(), this.GetRegionRect(), this.GetRegionDepth(), this.GetControlDepth());
			}
		
		c.shape = this.controlShape;

		return c;
		}

	// ---------------------
	protected Vector2 GetAnchorPos()
		{
		switch (this.staticAnchor)
			{
			case ControlAnchorPoint.BottomLeft		: return (new Vector2(0, 0));
			case ControlAnchorPoint.BottomRight	: return (new Vector2(1, 0));
			case ControlAnchorPoint.TopLeft		: return (new Vector2(0, 1));
			case ControlAnchorPoint.TopRight		: return (new Vector2(1, 1));
			}

		return Vector2.zero;
		}
		
	// ----------------------
	protected Vector2 BottomLeftToAnchorRelativePos(Vector2 pos)
		{
		switch (this.staticAnchor)
			{
			case ControlAnchorPoint.BottomLeft :
				break;

			case ControlAnchorPoint.BottomRight :
				pos.x = -pos.x;
				break;

			case ControlAnchorPoint.TopLeft :
				pos.y = -pos.y;
				break;

			case ControlAnchorPoint.TopRight :
				pos.x = -pos.x;
				pos.y = -pos.y;
				break;
			}

		return pos;
		}



	// ----------------------
	protected Vector2 BottomLeftToAnchorRelativeLocal(Vector2 pos)
		{
		RectTransform tr = (RectTransform)this.panel.transform;
		Rect panelRect = tr.rect;

		switch (this.staticAnchor)
			{
			case ControlAnchorPoint.BottomLeft :
				pos.x += panelRect.xMin;
				pos.y += panelRect.yMin;
				break;

			case ControlAnchorPoint.BottomRight :
				pos.x = panelRect.xMax - pos.x;
				pos.y += panelRect.yMin;
				break;

			case ControlAnchorPoint.TopLeft :
				pos.x += panelRect.xMin;
				pos.y = panelRect.yMax - pos.y;
				break;

			case ControlAnchorPoint.TopRight :
				pos.x = panelRect.xMax - pos.x;
				pos.y = panelRect.yMax - pos.y;
				break;
			}
		
		return pos;
		}

	// ----------------------
	protected Vector2 BottomLeftToAnchorRelativeWorldPos(Vector2 pos)
		{
		RectTransform tr = (RectTransform)this.panel.transform;
		Rect panelRect = tr.rect;

		switch (this.staticAnchor)
			{
			case ControlAnchorPoint.BottomLeft :
				pos.x += panelRect.xMin;
				pos.y += panelRect.yMin;
				break;

			case ControlAnchorPoint.BottomRight :
				pos.x = panelRect.xMax - pos.x;
				pos.y += panelRect.yMin;
				break;

			case ControlAnchorPoint.TopLeft :
				pos.x += panelRect.xMin;
				pos.y = panelRect.yMax - pos.y;
				break;

			case ControlAnchorPoint.TopRight :
				pos.x = panelRect.xMax - pos.x;
				pos.y = panelRect.yMax - pos.y;
				break;
			}
		

		return tr.localToWorldMatrix.MultiplyPoint3x4((Vector3)pos);
		}


	// --------------------
	static public float GetArcAngle(float orbitRad, float smallRad)
		{
		if (smallRad >= (orbitRad * 2.0f))
			return (Mathf.PI);
		
		//float arcAngle = Mathf.Acos((bigRad - smallRad) / (bigRad ));
		
		float c = (smallRad * 0.5f);
		float a = Mathf.Sqrt((orbitRad * orbitRad) - (c * c));
		//float t = orbitRad - a;
		
		float halfAngle = Mathf.Acos(((a * a) + (orbitRad * orbitRad) - (c * c)) / (2.0f * a * orbitRad));
		return halfAngle * 4;
		}		


	// -----------------------
	protected Vector2 GetControlOffset()
		{
		if (this.canvas == null)
			return Vector2.zero;

	


		List<TouchControl> controlList = this.panel.rig.GetTouchControls();

		//Rect canvasRect = ((RectTransform)this.canvas.transform).rect;			
		Rect panelRect = ((RectTransform)this.panel.transform).rect;			
		float controlRad = this.GetControlSize() / 2.0f;
		
		float separation = Mathf.Min(panelRect.width, panelRect.height) * 0.01f; 

		Vector2 anchorPos = Vector2.one * (controlRad + separation);
	

		Bounds worldBounds = CFUtils.GetWorldAABB(this.canvas.transform.localToWorldMatrix, BottomLeftToAnchorRelativeLocal(anchorPos), new Vector3(controlRad * 2, controlRad * 2, 0));

		float worldControlRad = Mathf.Max(worldBounds.size.x, worldBounds.size.y) * 0.5f;					


		Vector2 bestPos			= anchorPos;		
		float	bestDist		= 100;		// closest distance to other controls
		bool	bestPosFound	= false;


		int ringCount = Mathf.Max(1, 
			Mathf.CeilToInt((panelRect.width - separation) / ((controlRad * 2.0f) + separation)), //Factor)), 
			Mathf.CeilToInt((panelRect.height - separation) / ((controlRad * 2.0f) + separation))); //Factor)));			
		
		for (int ri = 0; ri < ringCount; ++ri)
			{
			float ringRad = separation + ((float)ri * ((controlRad * 2.0f) + separation)) ; //Factor);

		
		float controlArcAngle = GetArcAngle(ringRad, controlRad) * Mathf.Rad2Deg;
		int stepCount = (ringRad < 0.0001f) ? 1 : Mathf.Max(1, Mathf.CeilToInt(90 / controlArcAngle));
			
		//float angleInc = controlArcAngle;
		//float angleOfs = ((90 - ((float)stepCount * angleInc)) * 0.5f) + (angleInc * 0.5f);			


		if (stepCount <= 1)
			{
		//	angleInc =0;
		//	angleOfs = 0;
	
			}

		
		for (int si = 0; si < stepCount; ++si)
			{
			//float orbitAngle = (angleOfs + ((float)si * angleInc)) ;
			float orbitAngle = (stepCount < 2) ? 45 : (((float)(si)+0) * (90.0f / (float)(stepCount - 1))); 

			Vector2 pos = anchorPos + ((new Vector2(Mathf.Cos(orbitAngle * Mathf.Deg2Rad), Mathf.Sin(orbitAngle * Mathf.Deg2Rad))) * ringRad) ;//* separationFactor;

			if ((pos.x > panelRect.width) || (pos.y > panelRect.height)) 
				continue;



			Vector2 worldPos = this.BottomLeftToAnchorRelativeWorldPos(pos);	// this.canvas.transform.localToWorldMatrix.MultiplyPoint3x4((Vector3)pos);
				
				if (controlList.Count == 0)
					break;
				
				bool controlTested = false;
				float closestControlDist = -100000;	
				Vector2 closestControlPos = Vector2.zero;	

				for (int ci = 0; ci < controlList.Count; ++ci)
					{
					TouchControl c = controlList[ci];
					if ((c is DynamicRegion) || 
						CFUtils.IsStretchyRectTransform(c.transform))
						continue;

					Bounds controlBounds = c.GetWorldSpaceAABB();					

					Vector2 testControlCen = controlBounds.center;
					Vector2 testControlSize = controlBounds.size;
					float testControlRad = Mathf.Max(testControlSize.x, testControlSize.y) / 2.0f; 

					float curDist = (testControlCen - worldPos).magnitude - (testControlRad);


					if (!controlTested || ((curDist < closestControlDist)))
						{
						controlTested = true;
						closestControlDist = curDist;
						closestControlPos = pos;
						}				
					}


				if (!controlTested)
					{
					}
				else
					{	
					if (closestControlDist >= worldControlRad)
						{
						return this.BottomLeftToAnchorRelativePos(closestControlPos);
						}

					if (closestControlDist > 0) //-(worldControlRad * 0.5f))
						{
						return this.BottomLeftToAnchorRelativePos(closestControlPos);
						}


					if (!bestPosFound || (closestControlDist > bestDist ))
						{
						bestPosFound	= true;
						bestDist		= closestControlDist;
						bestPos			= closestControlPos;
						}
					}
				}
			}
		
		// If the control is almost fully covered by other controls, place it at default position...

		if (bestDist < -(worldControlRad * 0.9f))
			{
			bestPos = anchorPos;
			}	
		else
			{		
//Debug.Log("GetOffs : returning best IMPERFECTn pos : dist : " + bestDist + "  pos: " + bestPos +"  rad: " + controlRad + " worldRad: " + worldControlRad + " ctrlNear: " + ((bestControlNear==null)? "NULL" : bestControlNear.name) + "ctrlRad:" + bestControlRad);
			}

		
		return this.BottomLeftToAnchorRelativePos(bestPos);
		}



	// -----------------------
	protected float GetControlSize()
		{
		if ((this.canvas == null))
			return (400 * this.controlSize);

		Rect canvasRect = ((RectTransform)this.canvas.transform).rect;

		return (Mathf.Min(canvasRect.width, canvasRect.height) * this.controlSize);
		}

	// --------------------
	protected Rect GetRegionRect()
		{
		switch (this.regionRect)
			{
			case RegionRectPreset.FullScreen	: return (new Rect(0.0f, 0.0f, 1.0f, 1.0f)); 

			case RegionRectPreset.LeftHalf	: return (new Rect(0.0f, 0.0f, 0.5f, 1.0f));  
			case RegionRectPreset.RightHalf	: return (new Rect(0.5f, 0.0f, 0.5f, 1.0f)); 

			case RegionRectPreset.LeftThird	: return (new Rect(0.0f,  0.0f, 0.33f, 1.0f));
			case RegionRectPreset.MidThird		: return (new Rect(0.33f, 0.0f, 0.33f, 1.0f));
			case RegionRectPreset.RightThird	: return (new Rect(0.66f, 0.0f, 0.34f, 1.0f));

			case RegionRectPreset.BottomLeftQuarter	: return (new Rect(0.0f, 0.0f, 0.5f, 0.5f));
			case RegionRectPreset.BottomRightQuarter	: return (new Rect(0.5f, 0.0f, 0.5f, 0.5f));
			case RegionRectPreset.TopLeftQuarter		: return (new Rect(0.0f, 0.5f, 0.5f, 0.5f));
			case RegionRectPreset.TopRightQuarter		: return (new Rect(0.5f, 0.5f, 0.5f, 0.5f));
			}
		
	
		return new UnityEngine.Rect(0, 0, 1, 1);
		}
	

	// -------------------
	protected float GetControlDepth() { return this.controlDepth; }
	protected float GetRegionDepth() { return this.regionDepth; }


	// -------------------
	virtual protected void DrawGUI()
		{	
		this.DrawHeader();

		this.scrollPos = EditorGUILayout.BeginScrollView(this.scrollPos, CFEditorStyles.Inst.transpSunkenBG, 
			GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));

			this.DrawNameGUI();
			EditorGUILayout.Space();

			this.DrawPositionAndModeGUI();
			EditorGUILayout.Space();

			this.DrawPresentationGUI();
			EditorGUILayout.Space();
	
			this.DrawBindingGUI();

		EditorGUILayout.EndScrollView();

		EditorGUILayout.Space();

		this.DrawCreationButtons();
		}
	
		
		
	// -------------------
	virtual protected void DrawHeader()
		{ 
		}

	// -----------------
	virtual protected void DrawNameGUI()
		{
		InspectorUtils.BeginIndentedSection(); //this.GetTitle());

		this.controlName = CFGUI.TextField(new GUIContent("Name"), this.controlName, MAX_LABEL_WIDTH);

		InspectorUtils.EndIndentedSection();
		}

	// -------------------
	virtual protected void DrawPositionAndModeGUI()
		{
		InspectorUtils.BeginIndentedSection(new GUIContent("Positioning"));

		this.dynamicMode  = (ControlMode) CFGUI.EnumPopup(new GUIContent("Mode", "Positioning Mode - Static or Dynamic?"), 
				this.dynamicMode, MAX_LABEL_WIDTH); 

			//EditorGUILayout.Toggle(new GUIContent("Dynamic Mode", "Create this control with it's own DynamicRegion."), 
			//this.dynamicWithRegion, GUILayout.ExpandWidth(true));

		if (this.dynamicMode == ControlMode.DynamicWithRegion)
			{
			this.regionRect = (RegionRectPreset)CFGUI.EnumPopup(new GUIContent("Region", "Select Dynamic Region's screen position."), 
				this.regionRect, MAX_LABEL_WIDTH); 
			}
		else
			{
			this.staticAnchor = (ControlAnchorPoint)CFGUI.EnumPopup(new GUIContent("Anchor", "New control will be automatically placed near selected anchor point."), 
				this.staticAnchor, MAX_LABEL_WIDTH); 
			}
		
		this.controlShape = (TouchControl.Shape)CFGUI.EnumPopup(new GUIContent("Shape", "Control's shape"), this.controlShape, MAX_LABEL_WIDTH);

		this.controlSize = CFGUI.Slider(new GUIContent("Size", "Control's size relative to the shorter dimension of parent canvas."), 
			this.controlSize, 0.05f, 0.5f, MAX_LABEL_WIDTH);


		this.controlDepth = (float)DrawDepthSlider(new GUIContent("Control Depth", "Depth of the control - how far to push the control into the screen.\n\nControls of depth closer to screen (smaller values) are picked first!"),
			this.controlDepth,  MAX_LABEL_WIDTH);

		if (this.dynamicMode == ControlMode.DynamicWithRegion)
			{
			this.regionDepth = (float)DrawDepthSlider(new GUIContent("Region Depth", "Depth of control's dynamic region. It's recommended to use higher depth for region than for the control itself.\n\nControls of depth closer to screen (smaller values) are picked first!"),
				this.regionDepth,  MAX_LABEL_WIDTH);
			}


		InspectorUtils.EndIndentedSection();
		}


	// -------------------
	static protected float DrawDepthSlider(GUIContent content, float val, float labelWidth)
		{
		val = (float)CFGUI.IntSlider(content, (int)val, (int)DEPTH_MIN, (int)DEPTH_MAX, labelWidth);

		string hintText = "";
		
		if (val <= 50)
			hintText = "(0-50 - range typically used by buttons and other small controls.)";
		else if (val < 75) 
			hintText = "(50-75 - range used by Dynamic Regions and other large controls.)";
		else
			hintText = "(75-100 - background range, the best place to put full-screen Touch Zones, etc.)";

		EditorGUILayout.LabelField(hintText, CFEditorStyles.Inst.hintStyle, GUILayout.MinWidth(30), GUILayout.ExpandWidth(true));

		return val;
		}

	// ----------------
	virtual protected void DrawPresentationGUI()
		{
		InspectorUtils.BeginIndentedSection(new GUIContent("Presentation"));
		
			EditorGUILayout.BeginHorizontal(GUILayout.ExpandWidth(true));
			EditorGUILayout.Space();
			this.defaultSprite = DrawSpriteBox(new GUIContent("Sprite", "Simple single sprite for new control."), this.defaultSprite);			
			EditorGUILayout.Space();
			EditorGUILayout.EndHorizontal();

		InspectorUtils.EndIndentedSection();
		}
		

	// --------------------
	static protected Sprite DrawSpriteBox(GUIContent labelContent, Sprite sprite)
		{
		EditorGUILayout.BeginVertical(GUILayout.Width(100));				
			EditorGUILayout.LabelField(labelContent, GUILayout.MinWidth(60));

#if UNITY_PRE_5_2
			sprite = (Sprite)EditorGUILayout.ObjectField(sprite, typeof(Sprite), false);
#else
			sprite = (Sprite)EditorGUILayout.ObjectField(sprite, typeof(Sprite), false, GUILayout.Width(64), GUILayout.MinHeight(64));
#endif
		EditorGUILayout.EndVertical();

		return sprite;
		}
	
	// ------------------
	virtual protected void DrawBindingGUI()
		{
		}



	// ----------------
	protected void DrawPressureBindingExtraGUI()
		{
		this.emulateTouchPressure = EditorGUILayout.ToggleLeft(new GUIContent("Emulate Touch Pressure", "When enabled and controlling touch is not pressure sensitive, digital state will be applied to analog targets."),
			this.emulateTouchPressure, GUILayout.ExpandWidth(true), GUILayout.MinWidth(30));
		}


	// ------------------
	virtual protected void DrawCreationButtons()
		{
		EditorGUILayout.Space();

		EditorGUILayout.BeginHorizontal();

		//if (GUILayout.Button(new GUIContent("Cancel"), GUILayout.Height(32)))
		//	this.Close();

		if (GUILayout.Button(new GUIContent("Create",  CFEditorStyles.Inst.texOk), GUILayout.Height(32)))
			{
			this.OnCreateButtonPressed(false);
			}

		if (GUILayout.Button(new GUIContent("Create And Select", CFEditorStyles.Inst.texOk), GUILayout.Height(32)))
			{
			this.OnCreateButtonPressed(true);
			}

		EditorGUILayout.EndHorizontal();
		}
	

	// -------------------
	private void OnCreateButtonPressed(bool sel)
		{
		if (!this.CheckNameBeforeCreation())
			return;

		this.StoreParameters();
		this.OnCreatePressed(sel);
		this.Close();

		if (this.onCreationCallback != null)
			this.onCreationCallback();
		}


	// ----------------
	protected bool CheckNameBeforeCreation()
		{
		if (this.controlName.Length == 0)
			{
			EditorUtility.DisplayDialog("Control Freak 2", "Please enter control's name before continuing...", "OK");
			return false;
			}

		if (this.panel.rig.FindTouchControl(this.controlName) != null)
			{
			EditorUtility.DisplayDialog("Control Freak 2", "There's already a touch control sharing the same name!\n\nPlease choose a different name...", "OK");
			return false;
			}

		if (this.controlName == this.defaultControlName)
			{
			if (!EditorUtility.DisplayDialog("Using a meaningful name is alwyas a better idea!\n\n" +
				"Control Freak 2", "Are you sure you want to use a generic name for this control?"
				, "I don't care! Create my control!", "I want to go back...")) 	  
				return false;
			}
		return true;
		}

	// -------------------
	void OnGUI()
		{
		this.DrawGUI();
		}	



	// --------------------
	static protected void SetupDigitalBinding(DigitalBinding binding, string axisName, KeyCode key)
		{	
		binding.Clear();

		if (!string.IsNullOrEmpty(axisName))
			{
			binding.Enable();	
			binding.AddAxis().SetAxis(axisName, true);
			}

		if (key != KeyCode.None)
			{
			binding.Enable();
			binding.AddKey(key);
			}
		}

	// ----------------------------
	static protected void SetupAxisBinding(AxisBinding binding, string axisName)
		{
		binding.Clear();

		if (!string.IsNullOrEmpty(axisName))
			{
			binding.Enable();
			binding.AddTarget().SetSingleAxis(axisName, false);
			}
		}

	}
	



	
	
// ---------------------
// Non-dynamic Control Creation Wizard Base
// ---------------------
public abstract class NonDynamicControlWizardBase : ControlCreationWizardBase
	{
	protected enum PositionMode
		{
		ConstantSize,
		Stretch	
		}

	protected PositionMode
		positionMode;


	protected bool		
		createSpriteAnimator;

	
	
	// --------------------
	public NonDynamicControlWizardBase() : base()
		{
		
		}

	// --------------------------
	protected TouchControl CreateNonDynamicTouchControl(System.Type controlType)
		{
		TouchControl c = null;

		if (this.positionMode == NonDynamicControlWizardBase.PositionMode.ConstantSize)
			{
			c = TouchControlWizardUtils.CreateStaticControl(controlType, this.panel, this.panel.transform, this.controlName, 
				this.GetAnchorPos(), this.GetControlOffset(), this.GetControlSize(), this.GetControlDepth());
			}
		else
			{
			c = TouchControlWizardUtils.CreateStretchyControl(controlType, this.panel, this.panel.transform, this.controlName, 
				this.GetRegionRect(), this.GetRegionDepth()); //this.GetControlDepth());
			}	
		
		c.shape = this.controlShape;

		return c;	
		}



	// ---------------------
	override protected void DrawPositionAndModeGUI()
		{
		InspectorUtils.BeginIndentedSection(new GUIContent("Positioning"));

		this.positionMode  = (PositionMode) CFGUI.EnumPopup(new GUIContent("Mode", "Positioning Mode - Const Size or Stretchy Screen Region?"), 
				this.positionMode, MAX_LABEL_WIDTH); 

		if (this.positionMode == PositionMode.Stretch)
			{
			this.regionRect = (RegionRectPreset)CFGUI.EnumPopup(new GUIContent("Region", "Select Dynamic Region's screen position."), 
				this.regionRect, MAX_LABEL_WIDTH); 
			}
		else
			{
			this.staticAnchor = (ControlAnchorPoint)CFGUI.EnumPopup(new GUIContent("Anchor", "New control will be automatically placed near selected anchor point."), 
				this.staticAnchor, MAX_LABEL_WIDTH); 

			this.controlSize = CFGUI.Slider(new GUIContent("Size", "Control's size relative to the shorter dimension of parent canvas."), 
				this.controlSize, 0.05f, 0.5f, MAX_LABEL_WIDTH);

			}

		this.controlShape = (TouchControl.Shape)CFGUI.EnumPopup(new GUIContent("Shape", "Control's shape"), this.controlShape, MAX_LABEL_WIDTH);


		if (this.positionMode == PositionMode.Stretch)
			{
			this.regionDepth = (float)DrawDepthSlider(new GUIContent("Control Depth", "Depth of the control - how far to push the control into the screen.\n\nControls of depth closer to screen (smaller values) are picked first!"),
				this.regionDepth,  MAX_LABEL_WIDTH);
			}
		else
			{
			this.controlDepth = (float)DrawDepthSlider(new GUIContent("Control Depth", "Depth of the control - how far to push the control into the screen.\n\nControls of depth closer to screen (smaller values) are picked first!"),
				this.controlDepth,  MAX_LABEL_WIDTH);
			}



		InspectorUtils.EndIndentedSection();
		}


	// ----------------
	override protected void DrawPresentationGUI()
		{
		InspectorUtils.BeginIndentedSection(new GUIContent("Presentation"));
		
			this.createSpriteAnimator = EditorGUILayout.ToggleLeft(new GUIContent("Create Sprite Animator"), this.createSpriteAnimator);

			if (this.createSpriteAnimator)
				{
				EditorGUILayout.BeginHorizontal(GUILayout.ExpandWidth(true));
				EditorGUILayout.Space();
				this.defaultSprite = DrawSpriteBox(new GUIContent("Sprite", "Simple single sprite for new control."), this.defaultSprite);			
				EditorGUILayout.Space();
				EditorGUILayout.EndHorizontal();
				}

		InspectorUtils.EndIndentedSection();
		}
	
	}


}

#endif
