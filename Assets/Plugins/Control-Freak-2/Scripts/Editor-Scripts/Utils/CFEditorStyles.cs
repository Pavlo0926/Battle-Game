// -------------------------------------------
// Control Freak 2
// Copyright (C) 2013-2020 Dan's Game Tools
// http://DansGameTools.blogspot.com
// -------------------------------------------

#if UNITY_EDITOR

using UnityEngine;
using UnityEditor;

namespace ControlFreak2Editor
{
public class CFEditorStyles 
	{
	public GUIStyle
		iconFolder,
		iconScriptCS,
		iconScriptJS,
		iconOk,
		iconOkWarning,
		iconError,
		iconQuestionMark,
	
		foldout,
		boldFoldout,
		//checkbox,

		treeViewElemLabel,
		treeViewElemTranspLabel,
		treeViewBG,
		treeViewElemBG,
		treeViewElemSelBG,

		whiteBG,

		whiteSunkenBG,
		transpSunkenBG,

		whiteBevelBG,
		transpBevelBG,
			
		scriptFragInfoBG,

		featureBoxStyle,
		installerButton,

		toolbarBG,
			
		centeredTextSunkenBG,
		centeredTextTranspBG,

		headerScriptConverter,
		headerScriptBackup,
		headerInstaller,
		headerAssistant,
		headerRig,
		headerButton,
		headerJoystick,
		headerTouchZone,
		headerTrackPad,
		headerWheel,
		headerTouchPanel,
		headerGamepadManager,
		headerDynamicRegion,
		headerTouchSplitter,

		boldText,
		
		iconButtonStyle,
		buttonStyle,

		toggleButtonStyle,

		hintStyle,

		emptyStyle
		;

	public Texture2D
		magnifiyingGlassTex,
		texMoveUp,
		texMoveDown,
		folderTex,
		trashCanTex,
		createNewTex,
	
		sepAxesOnTex,
		sepAxesOffTex,			

		texPlusSign,
		texMinusSign,

		texFinger,
		texSettings,
		texWrench,

		texTwistHint,
		texPinchHint,
			

		checkboxCheckedTex,
		checkboxUncheckedTex,
		checkboxMixedTex, 
				
		greenDotTex,
		recordTex,
		stopTex,
		pauseTex,

		arrowUpTex,
		arrowDownTex,
		arrowRightTex,
		arrowLeftTex,
		arrowUpRightTex,
		arrowDownRightTex,
		arrowDownLeftTex,
		arrowUpLeftTex,

		texOk,
		texWarning,
		texError,
		texIgnore
	
		;

	
	// --------------------
	static private CFEditorStyles mInst;

	static public CFEditorStyles Inst
		{ get { return ((mInst == null) ? (mInst = new CFEditorStyles()) : mInst); } } 
		
	
	private const string ICONS_FOLDER = "Assets/Plugins/Control-Freak-2/Editor-Assets/GUI-Icons/";
		
	private const string WHITE_TEX		= "WHITE";


	// -------------------
	public CFEditorStyles()
		{
		this.iconFolder			= CreateIcon("Icon-Folder.png"); 
		this.iconScriptCS		= CreateIcon("Icon-Script-CS.png");
		this.iconScriptJS		= CreateIcon("Icon-Script-JS.png");

		this.iconOk 			= CreateIcon("Icon-OK.png");
		this.iconOkWarning		= CreateIcon("Icon-OK-warning.png");
		this.iconError			= CreateIcon("Icon-Error.png");
		this.iconQuestionMark	= CreateIcon("Icon-Question-Mark.png");
			

		this.foldout = CreateFoldoutStyle("Icon-Foldout-Open.png", "Icon-Foldout-Closed.png");

		this.boldFoldout = new GUIStyle(this.foldout);
		this.boldFoldout.fontStyle = FontStyle.Bold;


		//this.foldout			= CreateIcon("Icon-Foldout-Closed.png", "Icon-Foldout-Open.png");
		//this.foldout.contentOffset = new Vector2(16, 0);
		this.foldout.imagePosition = ImagePosition.ImageLeft;
		//this.foldout.


		//this.checkbox			= new GUIStyle(EditorStyles.toggle);
		//this.checkbox.fixedWidth = 16;
		//this.checkbox.fixedHeight = 16;

		Texture2D bgButtonNormal = LoadTex("BG-Button-Released.png");
		Texture2D bgButtonPressed = LoadTex("BG-Button-Pressed.png");
		Texture2D bgButtonToggled = LoadTex("BG-Button-Toggled.png");

		this.buttonStyle				= CreatePushBG(bgButtonNormal, bgButtonPressed, bgButtonToggled, 4, 2, 0);
		this.buttonStyle.contentOffset	= Vector2.zero;
		this.buttonStyle.alignment		= TextAnchor.MiddleCenter;
		this.buttonStyle.imagePosition = ImagePosition.ImageLeft;

		this.iconButtonStyle				= new GUIStyle(this.buttonStyle);
		this.iconButtonStyle.fixedWidth		= 16;
		this.iconButtonStyle.fixedHeight	= 16;
	
		this.checkboxCheckedTex		= LoadTex("Icon-CheckBox-On.png");
		this.checkboxUncheckedTex	= LoadTex("Icon-CheckBox-Off.png");
		this.checkboxMixedTex		= LoadTex("Icon-CheckBox-Mixed.png");

			
		this.boldText	= new GUIStyle();
		this.boldText.fontStyle = FontStyle.Bold;
		this.boldText.padding = new RectOffset(4, 4, 2, 2);

		
		this.scriptFragInfoBG	= CreateBG("BG-sunken.png", 4, 4, 2);

		this.whiteSunkenBG		= CreateBG("BG-sunken.png", 4, 4, 4);
		this.transpSunkenBG		= CreateBG("BG-sunken-transp.png", 4, 4, 4);
		this.whiteBevelBG		= CreateBG("BG-bevel.png", 4,4,0);
		this.transpBevelBG		= CreateBG("BG-bevel-transp.png", 4,4,0);

		this.featureBoxStyle	= CreateBG("BG-bevel-transp.png", 4,8,2);
	
		this.installerButton	= new GUIStyle(this.buttonStyle);
		this.installerButton.fontSize = 12;
		this.installerButton.padding = new RectOffset(10, 10, 8, 8);
		this.installerButton.margin = new RectOffset(10, 10, 2, 2);

			
		this.whiteBG			= CreateBG(WHITE_TEX, 0,0,0);
			
		this.treeViewElemBG		= CreateBG(WHITE_TEX, 4,0,0);
		this.treeViewElemSelBG	= CreateBG("BG-bevel.png", 4,0,0);

		this.treeViewElemLabel			= CreateBG("BG-bevel.png", 4, 2, 0);
		this.treeViewElemTranspLabel	= CreateBG("BG-bevel-transp.png", 4, 2, 0);
			
		this.treeViewBG			= this.transpSunkenBG;
		this.toolbarBG			= CreateBG("BG-bevel-transp.png", 4, 8, 4);
			
		this.headerScriptConverter	= CreateHeaderStyle("BG-Header-Script-Converter.png");
		this.headerScriptBackup		= CreateHeaderStyle("BG-Header-Script-Backup.png");
		this.headerInstaller		= CreateHeaderStyle("BG-Header-Installer.png");
		this.headerAssistant		= CreateHeaderStyle("BG-Header-Assistant.png");
		this.headerRig				= CreateHeaderStyle("BG-Header-Rig.png");
		this.headerButton			= CreateHeaderStyle("BG-Header-Button.png");
		this.headerJoystick			= CreateHeaderStyle("BG-Header-Joystick.png");
		this.headerTouchZone		= CreateHeaderStyle("BG-Header-TouchZone.png");
		this.headerTrackPad			= CreateHeaderStyle("BG-Header-TrackPad.png");
		this.headerWheel			= CreateHeaderStyle("BG-Header-Wheel.png");
		this.headerTouchPanel		= CreateHeaderStyle("BG-Header-TouchControlPanel.png");
		this.headerDynamicRegion	= CreateHeaderStyle("BG-Header-DynamicRegion.png");
		this.headerGamepadManager	= CreateHeaderStyle("BG-Header-GamepadManager.png");
		this.headerTouchSplitter	= CreateHeaderStyle("BG-Header-TouchSplitter.png");

		
		

		this.magnifiyingGlassTex	= LoadTex("Icon-Magnifying-Glass.png");			
		this.texMoveUp				= LoadTex("Icon-Move-Up.png");
		this.texMoveDown			= LoadTex("Icon-Move-Down.png"); 
		this.folderTex				= LoadTex("Icon-Folder.png");	
		this.trashCanTex			= LoadTex("Icon-Trash-Can.png");
		this.createNewTex			= LoadTex("Icon-Create-New.png");
			
		this.texFinger				= LoadTex("Icon-Finger.png");
		this.texSettings			= LoadTex("Icon-Settings.png");
		this.texWrench				= LoadTex("Icon-Wrench.png");

		this.texTwistHint			= LoadTex("Icon-Twist-Hint.png");
		this.texPinchHint			= LoadTex("Icon-Pinch-Hint.png");


		this.stopTex				= LoadTex("Icon-Stop.png"); 
		this.pauseTex				= LoadTex("Icon-Pause.png"); 
		this.recordTex				= LoadTex("Icon-Record.png"); 
		this.greenDotTex			= LoadTex("Icon-Green-Dot.png"); 


		this.sepAxesOnTex			= LoadTex("Icon-Sep-Axes-On.png");
		this.sepAxesOffTex			= LoadTex("Icon-Sep-Axes-Off.png");
			
		this.arrowUpTex				= LoadTex("Icon-Arrow-U.png");
		this.arrowDownTex			= LoadTex("Icon-Arrow-D.png");
		this.arrowRightTex			= LoadTex("Icon-Arrow-R.png");
		this.arrowLeftTex			= LoadTex("Icon-Arrow-L.png");
		this.arrowUpRightTex		= LoadTex("Icon-Arrow-UR.png");
		this.arrowDownRightTex		= LoadTex("Icon-Arrow-DR.png");
		this.arrowDownLeftTex		= LoadTex("Icon-Arrow-DL.png");
		this.arrowUpLeftTex			= LoadTex("Icon-Arrow-UL.png");
		

		this.texOk		= LoadTex("Icon-OK.png");			
		this.texWarning	= LoadTex("Icon-OK-warning.png");			
		this.texError	= LoadTex("Icon-Error.png");			
		this.texIgnore	= LoadTex("Icon-Ignore.png");			
		
		this.texPlusSign	= LoadTex("Icon-Plus-Sign.png");			
		this.texMinusSign	= LoadTex("Icon-Minus-Sign.png");			


		this.centeredTextSunkenBG	= CreateBG("BG-sunken-transp.png", 4, 8, 0);
		this.centeredTextTranspBG	= CreateBG("", 0, 4, 0);

		this.centeredTextSunkenBG.alignment = TextAnchor.MiddleCenter;
		this.centeredTextTranspBG.alignment = TextAnchor.MiddleCenter;
		this.centeredTextSunkenBG.richText = true;
		this.centeredTextTranspBG.richText = true;

		this.hintStyle				= new GUIStyle(this.transpSunkenBG);
		this.hintStyle.padding		= new RectOffset(6, 6, 4, 4);
		this.hintStyle.margin 		= new RectOffset(6, 6, 2, 4);	
		this.hintStyle.richText		= true;
		this.hintStyle.alignment	= TextAnchor.UpperLeft;
		this.hintStyle.wordWrap		= true;
		this.hintStyle.fontStyle 	= FontStyle.Italic;

		this.emptyStyle = GUIStyle.none;

		//this.toggleButtonStyle = this.miniButtonStyle;

			


		
		}

	

	// ------------------
	static private GUIStyle CreateHeaderStyle(string texName)
		{	
		GUIStyle s = CreateBG(texName, new RectOffset(340, 6, 6, 6));
		Texture2D t = s.normal.background;

		s.border = new RectOffset(t.width - 2, 2, 0, 0);
 
		s.fixedHeight = t.height;
		return s;
		}


	// ----------------------
	static private Texture2D LoadTex(string iconName)
		{
		if (iconName == WHITE_TEX)
			return EditorGUIUtility.whiteTexture;

		Texture2D tex = AssetDatabase.LoadAssetAtPath(ICONS_FOLDER + iconName, typeof(Texture2D)) as Texture2D;

		if (tex == null)
			{
			Debug.LogError("Can't load texture : " + iconName);
			return EditorGUIUtility.whiteTexture;	
			}

		return tex;	
		}


	// ---------------------
	static private GUIStyle CreateIcon(string iconName) { return CreateIcon(iconName, ""); }
	static private GUIStyle CreateIcon(string iconName, string iconOnName)
		{
		Texture2D iconTex	= LoadTex(iconName);
		Texture2D iconOnTex = (iconOnName.Length == 0) ? iconTex : LoadTex(iconOnName);

		GUIStyle s = new GUIStyle();
		
		s.fixedWidth = iconTex.width;
		s.fixedHeight = iconTex.height;

		s.normal.background = iconTex;

		s.onNormal.background = iconOnTex;

		UnifyStyleStates(s, true);

		return s;	
		}
		

	
	// ----------------------
	

	// --------------------
	static private GUIStyle CreateBG(string texName, RectOffset border, RectOffset padding, RectOffset margin)
		{
		GUIStyle s = new GUIStyle();
			
		s.normal.background = ((texName.Length == 0) ? null : LoadTex(texName));
		UnifyStyleStates(s, false);

		s.border	= border;
		s.margin	= margin;
		s.padding	= padding;

		return s;
		}

	// --------------------
	static private GUIStyle CreateBG(string texName, string texOnName, RectOffset border, RectOffset padding, RectOffset margin)
		{
		GUIStyle s = new GUIStyle();
			
		s.normal.background = ((texName.Length == 0) ? null : LoadTex(texName));
		s.onNormal.background = ((texOnName.Length == 0) ? null : LoadTex(texOnName));
		UnifyStyleStates(s, true);

		s.border	= border;
		s.margin	= margin;
		s.padding	= padding;

		return s;
		}



	// -------------------------
	static private GUIStyle CreateBG(string texName, RectOffset border)
		{
		return CreateBG(texName, border, new RectOffset(), new RectOffset());
		}
		


	// --------------------------------------
	static private GUIStyle CreatePushBG(Texture2D texNormal, Texture2D texPressed, Texture2D texToggled, int border, int padding, int margin)
		{ return CreatePushBG(texNormal, texPressed, texToggled, new RectOffset(border, border, border, border), new RectOffset(padding, padding, padding, padding), new RectOffset(margin, margin, margin, margin)); }

	static private GUIStyle CreatePushBG(Texture2D texNormal, Texture2D texPressed, Texture2D texToggled, RectOffset border, RectOffset padding, RectOffset margin)
		{
		GUIStyle s = new GUIStyle();
			
		s.normal.background = texNormal;
		s.active.background = texPressed;
		s.onNormal.background	= texToggled;

		s.onNormal = s.onHover = s.onFocused = s.onNormal;
		s.onActive = s.active;
		s.hover = s.focused = s.normal;

		s.border	= border;
		s.margin	= margin;
		s.padding	= padding;

		return s;
		}


	// --------------------
	static private GUIStyle CreateBG(string texName, int border, int padding, int margin)
		{
		GUIStyle s = new GUIStyle();
			
		s.normal.background = ((texName.Length == 0) ? null : LoadTex(texName));
		UnifyStyleStates(s, false);

		s.border	= new RectOffset(border, border, border, border);
		s.margin	= new RectOffset(margin, margin, margin, margin);
		s.padding	= new RectOffset(padding, padding, padding, padding);
			
		s.fixedWidth = 0;
		s.fixedHeight = 0;
			 
		return s;
		}




	// --------------------
	static private GUIStyle CreateFoldoutStyle(string texOpen, string texClosed)
		{
		GUIStyle s = new GUIStyle();
			
		s.normal.background = ((texClosed.Length == 0) ? EditorGUIUtility.whiteTexture : LoadTex(texClosed));
		s.onNormal.background = ((texOpen.Length == 0) ? EditorGUIUtility.whiteTexture : LoadTex(texOpen));

		UnifyStyleStates(s, true);
				
		Texture2D t = s.normal.background;

		s.border	= new RectOffset(t.width-1, 1, t.height-1, 1);
		//s.border	= new RectOffset(0, 0, 0, 0); //t.width, 0, t.height, 0);
		//s.margin	= new RectOffset(margin, margin, margin, margin);
		s.padding	= new RectOffset(t.width + 2, 0, 0, 0);
			
		s.fixedWidth = 0;
		s.fixedHeight = 0;
			 
		return s;
		}


	// ---------------------
	static private void UnifyStyleStates(GUIStyle s, bool separateOnStates)
		{
		s.active	= s.hover	= s.focused		= s.normal;
		s.onActive	= s.onHover	= s.onFocused	= (separateOnStates ? s.onNormal : s.normal);
		}

	}
}
#endif
