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

#if UNITY_PRE_5_4 || UNITY_5_4 
	#define UNITY_PRE_5_5
#endif

#if UNITY_EDITOR 
 
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using ControlFreak2Editor.Internal;
 
namespace ControlFreak2Editor
{
[InitializeOnLoad()]
public class Installer : EditorWindow
	{		
	const string 
		DIALOG_TITLE		= "Control Freak 2 Installer",	
		DIALOG_SHORT_TITLE	= "CF2 Installer",

		ONLINE_DOCS_URL		= "http://dgtdocs.000webhostapp.com/",
		WEBSITE_URL				= "http://dansgametools.blogspot.com/",
		VIDEO_TUTORIALS_URL 	= "https://www.youtube.com/playlist?list=PLrXZsI52MMml0io_4nWs47RDZvudZyMmt",

		PLAYMAKER_ADD_ON_PATH 		= "Assets/Plugins/Control-Freak-2/Add-Ons/CF2-Playmaker.unitypackage",
		UFE_ADD_ON_PATH 				= "Assets/Plugins/Control-Freak-2/Add-Ons/CF2-UFE.unitypackage",
		UFE_HACKY_ADD_ON_PATH 		= "Assets/Plugins/Control-Freak-2/Add-Ons/CF2-UFE-Hack.unitypackage",
		UFPS_ADD_ON_PATH 				= "Assets/Plugins/Control-Freak-2/Add-Ons/CF2-UFPS.unitypackage",
		UNITZ_UNET_ADD_ON_PATH		= "Assets/Plugins/Control-Freak-2/Add-Ons/CF2-UnitZ-UNet.unitypackage",
		//OPSIVE_TPC_ADD_ON_PATH 		= "Assets/Plugins/Control-Freak-2/Add-Ons/CF2-Opsive-TPC.unitypackage",	
		OPSIVE_TPC_1_DL_URL				= "http://opsive.com/assets/ThirdPersonController/integrations.php",

		OPSIVE_TPC_2_DL_URL				= "https://opsive.com/downloads/?pid=926",
		OPSIVE_GENERAL_DL_URL				= "https://opsive.com/solutions/character-solution/",

		CF1_MIGRATION_ADD_ON_PATH	= "Assets/Plugins/Control-Freak-2/Add-Ons/CF2-CF1-Migration-Tools.unitypackage";

	private const bool 
		DONT_SHOW_ADD_ON_PACKAGE_IMPORT_WINDOW = true;
	

	const int 
		CF2_INSTALLER_VERSION = 1;


	private bool 
		//defineSymbolsPresent,
		inputAxesPresent,

		newUfePresent,
		oldUfePresent,
		opsiveTpcPresent,
		playmakerPresent,
		ufpsPresent,
		unitzUnetPresent,
		controlFreak1Present;
		//evpPresent;

		
	public PlatformOptionList
		platformOptionList;


	// ---------------------
	public Installer() : base()
		{
		this.minSize = new Vector2(400, 450); 
		this.platformOptionList = new PlatformOptionList();
		}



	// ---------------------
	void OnEnable()
		{	
		this.Repaint();

		this.inputAxesPresent = UnityInputManagerUtils.AreControlFreakAxesPresent();
		//this.defineSymbolsPresent = ConfigManager.AreControlFreakSymbolsDefined();

		this.playmakerPresent = 
			((CFEditorUtils.FindClass("HutongGames.PlayMaker.FsmStateAction") != null));

		this.ufpsPresent = 
			((CFEditorUtils.FindClass("vp_FPInput") != null));

		this.controlFreak1Present = 
			((CFEditorUtils.FindClass("TouchController") != null) && 
			 (CFEditorUtils.FindClass("TouchStick") != null) &&
			 (CFEditorUtils.FindClass("TouchZone") != null) && 
			 (CFEditorUtils.FindClass("CFInput") != null));
	
		//this.evpPresent	=
		//	((CFEditorUtils.FindClass("EVP.VehicleController") != null));

		this.opsiveTpcPresent =
			(CFEditorUtils.FindClass("Opsive.ThirdPersonController.Wrappers.RigidbodyCharacterController") != null) ||
			(CFEditorUtils.FindClass("Opsive.ThirdPersonController.RigidbodyCharacterController") != null);

		this.newUfePresent = 
			((CFEditorUtils.FindClass("UFEController") != null) && 
			 (CFEditorUtils.FindClass("InputTouchControllerBridge") != null));

		this.oldUfePresent = 
			((CFEditorUtils.FindClass("UFEController") != null) && 
			 (CFEditorUtils.FindClass("InputTouchControllerBridge") == null));


		this.unitzUnetPresent = 
			((CFEditorUtils.FindClass("UnitZManager") != null)); 
		


		this.platformOptionList.Refresh();
		}


	
	// --------------------- 
	void OnDisable()
		{
		}

//#if !UNITY_PRE_5_0

//	// --------------------
//	[InitializeOnLoadMethod]
//	static private void OnProjectLoad()
//		{
//		if (EditorApplication.isPlaying || EditorApplication.isPlayingOrWillChangePlaymode)
//			return;
 
//		CheckInstallation();
//		}

//#endif

	// ------------------
	static Installer()
		{
		EditorApplication.update -= CheckInstallation;
		EditorApplication.update += CheckInstallation;
		}


	// --------------------
	[MenuItem("Control Freak 2/CF2 Installer")]
	static public void ShowInstallerWindow()
		{
		GetWindow<ControlFreak2Editor.Installer>(true, DIALOG_SHORT_TITLE, true);
		}
	
	// ---------------------
	static private void CheckInstallation()
		{
		EditorApplication.update -= CheckInstallation;

		if (EditorApplication.isPlaying || EditorApplication.isPlayingOrWillChangePlaymode)
			{
			return;
			}

		CFProjPrefs prefs = CFProjPrefs.Inst;

		string curProjPath = Application.dataPath;

		if (prefs.isIsntalled && (prefs.projectPath != curProjPath))
			{
			EditorUtility.DisplayDialog(DIALOG_TITLE, "Looks like CF2 was originally installed in different location!\nPlease run the CF2 Installer.", "OK");

			//prefs.wasShown = false;
			}

		if (prefs.isIsntalled && (prefs.installedVer != CF2_INSTALLER_VERSION))
		 	{
			// TODO : upgrade...
			}



		prefs.projectPath = curProjPath;
		prefs.Save();


		if (!prefs.wasShown || (prefs.installedVer != CF2_INSTALLER_VERSION))
			{
			ShowInstallerWindow();
			}

		}
	


	// -------------------
	void OnGUI()
		{
		GUILayout.Box(GUIContent.none, CFEditorStyles.Inst.headerInstaller, GUILayout.ExpandWidth(true));

		EditorGUILayout.Space();

		GUILayout.Box("Welcome to <b>Control Freak 2</b>!",  CFEditorStyles.Inst.centeredTextTranspBG, 
			GUILayout.ExpandWidth(true)); //, GUILayout.ExpandHeight(true));


		EditorGUILayout.Space();

		CFGUI.BeginIndentedVertical(CFEditorStyles.Inst.transpSunkenBG);

			DrawFeatureBox(this.inputAxesPresent, "Input Axes Present!", "Input Axes are missing or are incomplete! (Click INSTALL)");
			//DrawFeatureBox(this.defineSymbolsPresent, "Scripting Symbols Defined!", "Scripting Symbols are not defined for all platforms! (Click INSTALL)");

			if (CFProjPrefs.Inst.isIsntalled)
				this.platformOptionList.DrawForceMobileModeButton();

		CFGUI.EndIndentedVertical();
			
		GUILayout.FlexibleSpace();

			
		if (GUILayout.Button(new GUIContent("Install", "Clicking this button will:\n\n" +
			"* Add default CF2 scripting define symbols.\n\n" +
			"* Setup \'Forced Mobile Mode\' on all mobile platforms.\n\n" +	
			"* Add CF2 Input Axes to Unity Input Manager (without removing existing axes).\n" ), 
			CFEditorStyles.Inst.installerButton))
			{
			this.Install();
			this.Close();
			}

		if (GUILayout.Button(new GUIContent("Uninstall", "Clicking this button will:\n\n" +
			"* Remove all CF2-related define symbols.\n\n" +
			"* Remove CF2 Axes from Unity Input Manager.\n" ), 	
			CFEditorStyles.Inst.installerButton))
			{
			this.Uninstall();
			this.Close();
			}

		GUILayout.Space(20);

		if (GUILayout.Button(new GUIContent("Install Add-Ons..."), CFEditorStyles.Inst.installerButton))
			this.ShowAddOnMenu();



		GUILayout.Space(20);

		if (GUILayout.Button("View Online Documentation.", CFEditorStyles.Inst.installerButton))
			Application.OpenURL(ONLINE_DOCS_URL);
		if (GUILayout.Button("View Video Tutorials on YouTube.", CFEditorStyles.Inst.installerButton))
			Application.OpenURL(VIDEO_TUTORIALS_URL);
		if (GUILayout.Button("View Our Website.", CFEditorStyles.Inst.installerButton))
			Application.OpenURL(WEBSITE_URL);


		}


	// ---------------------
	private void DrawFeatureBox(bool ok, string okMsg, string errMsg)
		{	
		GUILayout.Box(ok ? 
			(new GUIContent(okMsg, CFEditorStyles.Inst.texOk)) : 
			(new GUIContent(errMsg, CFEditorStyles.Inst.texError)), CFEditorStyles.Inst.featureBoxStyle, GUILayout.ExpandWidth(true));

		GUI.color = Color.white;
		}




		
	// --------------------
	bool Install()
		{
		try 
			{
			EditorUtility.DisplayProgressBar(DIALOG_TITLE, "Adding CF2 Input Manager axes...", 0.1f);			
			UnityInputManagerUtils.AddControlFreakAxes();

			EditorUtility.DisplayProgressBar(DIALOG_TITLE, "Adding Define Symbols...", 0.6f);	
			ConfigManager.AddDefaultControlFreakSymbols();
		

			// Save config...

			EditorUtility.DisplayProgressBar(DIALOG_TITLE, "Saving...", 0.8f);	
			
			CFProjPrefs prefs = CFProjPrefs.Inst;
		
			prefs.wasShown		= true;
			prefs.installedVer	= CF2_INSTALLER_VERSION;
			prefs.projectPath	= Application.dataPath;
			prefs.isIsntalled	= true;
			
			prefs.Save();	

			//Debug.Log("Saved!");
			prefs = CFProjPrefs.Inst;
			//Debug.Log("reloaded : " + prefs.isIsntalled + " ver:" + prefs.installedVer);		
				
				
			// Recompile scripts...

			AssetDatabase.Refresh();


			EditorUtility.ClearProgressBar();
			

			}
		catch (System.Exception e)
			{
			EditorUtility.ClearProgressBar();
			EditorUtility.DisplayDialog(DIALOG_TITLE, "Something went wrong during installation!\n" + e.Message, "OK");	
			return false;
			}	

		return true;
		}

	
	// -------------------
	public bool Uninstall()
		{
		try 
			{
			EditorUtility.DisplayProgressBar(DIALOG_TITLE, "Removing CF2 Axes from Input Manager...", 0.1f);			
			UnityInputManagerUtils.RemoveControlFreakAxes();

			EditorUtility.DisplayProgressBar(DIALOG_TITLE, "Remove Define Symbols...", 0.6f);	
			ConfigManager.RemoveAllControlFreakSymbols();
			

			// Save config...

			EditorUtility.DisplayProgressBar(DIALOG_TITLE, "Saving...", 0.8f);	
			
			CFProjPrefs prefs = CFProjPrefs.Inst;
		
			prefs.installedVer= CF2_INSTALLER_VERSION;
			prefs.projectPath	= Application.dataPath;
			prefs.isIsntalled	= false;
			prefs.wasShown		= true;			

			prefs.Save();	

			//Debug.Log("Saved!");
			prefs = CFProjPrefs.Inst;
			//Debug.Log("reloaded : " + prefs.isIsntalled + " ver:" + prefs.installedVer);		
				
				
			// Recompile scripts...

			AssetDatabase.Refresh();


			EditorUtility.ClearProgressBar();
			

			}
		catch (System.Exception e)
			{
			EditorUtility.DisplayDialog(DIALOG_TITLE, "Something went wrong during uninstallation!\n" + e.Message, "OK");	
			return false;
			}	
		return true;
		}


	
	
	
	// ---------------------
	private void ShowAddOnMenu()
		{
		GenericMenu menu = new GenericMenu();

		AddMenuItem(menu, new GUIContent("Install \'Control Freak 1.x Migration Tools\'..."), this.InstallMigrationTools, this.controlFreak1Present);
		menu.AddSeparator("");

		AddMenuItem(menu, new GUIContent("Install \'Playmaker\' Add-On..."), this.InstallPlaymakerAddOn, this.playmakerPresent);
		//AddMenuItem(menu, new GUIContent("Install \'Edy's Vehicle Physics\' Add-On..."), this.InstallEvpAddOn, this.evpPresent);

		AddMenuItem(menu, new GUIContent("Download \'Opsive Ultimate Character Controller\' add-ons..."), this.InstallOpsiveV2AddOn);

		AddMenuItem(menu, new GUIContent("Install \'UFPS - Ultimate FPS\' 1.x Add-On..."), this.InstallUfpsAddOn, this.ufpsPresent);
		AddMenuItem(menu, new GUIContent("Download \'Opsive Third Person Controller\' 1.x Add-On..."), this.InstallOpsiveTpcAddOn, this.opsiveTpcPresent);

		if (this.newUfePresent)
			AddMenuItem(menu, new GUIContent("Install \'Universal Fighting Engine\' Official Add-On..."), this.InstallUfeAddOn, this.newUfePresent);
		else 
			AddMenuItem(menu, new GUIContent("Install \'Universal Fighting Engine\' Hack Add-On..."), this.InstallHackyUfeAddOn, this.oldUfePresent);

		AddMenuItem(menu, new GUIContent("Install \'UnitZ UNET\' Add-On... (Unity 5.5+ only)"), this.InstallUnitZAddOn, this.unitzUnetPresent);


		menu.ShowAsContext();
		}



	// -------------------------
	private void InstallMigrationTools()		{ 	AssetDatabase.ImportPackage(CF1_MIGRATION_ADD_ON_PATH,DONT_SHOW_ADD_ON_PACKAGE_IMPORT_WINDOW); }
	private void InstallPlaymakerAddOn()		{ 	AssetDatabase.ImportPackage(PLAYMAKER_ADD_ON_PATH,		DONT_SHOW_ADD_ON_PACKAGE_IMPORT_WINDOW); }
	private void InstallUfpsAddOn()				{ 	AssetDatabase.ImportPackage(UFPS_ADD_ON_PATH,			DONT_SHOW_ADD_ON_PACKAGE_IMPORT_WINDOW); }
	private void InstallUfeAddOn()				{ 	AssetDatabase.ImportPackage(UFE_ADD_ON_PATH,				DONT_SHOW_ADD_ON_PACKAGE_IMPORT_WINDOW); }
	private void InstallUnitZAddOn()				{ 	AssetDatabase.ImportPackage(UNITZ_UNET_ADD_ON_PATH,	DONT_SHOW_ADD_ON_PACKAGE_IMPORT_WINDOW); }
	private void InstallOpsiveTpcAddOn()		{ 	Application.OpenURL(OPSIVE_TPC_1_DL_URL); } //AssetDatabase.ImportPackage(OPSIVE_TPC_ADD_ON_PATH, DONT_SHOW_ADD_ON_PACKAGE_IMPORT_WINDOW); }


	// --------------------
	private void InstallHackyUfeAddOn()			
		{ 	
		if (this.controlFreak1Present)
			{
			EditorUtility.DisplayDialog("UFE Hack Add-On", 
				"Control Freak 1.x detected!\n" + "Before installing this add-on, all Control Freak 1.x scripts must be deleted!", "OK");	
			return;
			}

		AssetDatabase.ImportPackage(UFE_HACKY_ADD_ON_PATH, DONT_SHOW_ADD_ON_PACKAGE_IMPORT_WINDOW); 
		}



	// ------------------
	private void InstallOpsiveV2AddOn()
		{
		if (EditorUtility.DisplayDialog("Opsive Integration Add-on", 
			"You will be taken to Opsive website where you can download a CF2 Integration add-on for any of the Opsive Character Solutions, including:\n\n" +
			"* Ultimate Character Controller\n" +
			"* Ultimate First Person Controller\n" +
			"* Third Person Controller\n" +
			"\n" +
			"Pick your solution and the go to the \'Downloads\' section.\n" 
			, "OK", "Cancel"))
			{
			Application.OpenURL(OPSIVE_GENERAL_DL_URL);
			}	
		}


	// --------------------
	static private void AddMenuItem(GenericMenu menu, GUIContent label, GenericMenu.MenuFunction func, bool enabled = true, bool checkedItem = false)
		{
		if (enabled)
			menu.AddItem(label, checkedItem, func);
		else
			menu.AddDisabledItem(label);
		}

		


	// ------------------
	public class PlatformOptionList
		{
		public List<PlatformState> 
			platformList;
			
		private string 
			mobileModeButtonLabel = "";

			
		// ------------------
		public PlatformOptionList()
			{
			this.platformList = new List<PlatformState>(16);
			}

		// ---------------	
		private PlatformState FindPlatform(BuildTargetGroup platform)
			{
			return this.platformList.Find(x => (x.platform == platform));
			}


		// -------------------
		public void Refresh()
			{
			this.platformList.Clear();

			BuildTargetGroup[] platformIds = (BuildTargetGroup[])System.Enum.GetValues(typeof(BuildTargetGroup));
			for (int i = 0; i < platformIds.Length; ++i)	
				{
				if (!ConfigManager.IsBuildTargetGroupSupported(platformIds[i]))
					continue;
				if (this.FindPlatform(platformIds[i]) != null)
					continue;

				ConfigManager.SymbolState symbolState = ConfigManager.IsSymbolDefined(ConfigManager.CF_FORCE_MOBILE_MODE, platformIds[i]);
				if (symbolState == ConfigManager.SymbolState.ERROR)
					continue;

				PlatformState s = new PlatformState(this);

				s.platform 				= platformIds[i];
				s.forceMobileMode 	= (symbolState == ConfigManager.SymbolState.ON); 

				this.platformList.Add(s);
				}
				
			this.OnOptionChange();	
			}

		
			
		// ----------------
		public void CreateContextMenu()
			{
			GenericMenu menu = new GenericMenu();
				

			menu.AddItem(new GUIContent("In Editor"), !(ConfigManager.IsSymbolDefinedForAll(ConfigManager.CF_DONT_FORCE_MOBILE_MODE_IN_EDITOR) > 0), 
				this.ToggleForceMobileModeInEditor);
			menu.AddSeparator("");

			for (int i = 0; i < this.platformList.Count; ++i)
				{
				PlatformState ps = this.platformList[i];
					menu.AddItem(new GUIContent(ps.platform.ToString()), 
						ps.forceMobileMode, ps.ToggleMobileMode);
				}

			menu.ShowAsContext();
			}
	

		// ---------------
		private void ToggleForceMobileModeInEditor()
			{
			ConfigManager.SetSymbolForAll(ConfigManager.CF_DONT_FORCE_MOBILE_MODE_IN_EDITOR, 
				!(ConfigManager.IsSymbolDefinedForAll(ConfigManager.CF_DONT_FORCE_MOBILE_MODE_IN_EDITOR) > 0));
			}
		

			
		// --------------------
		public void OnOptionChange()
			{
			int mobileModeCount = 0;

			for (int i = 0; i < this.platformList.Count; ++i)
				{
				PlatformState ps = this.platformList[i];
				if (ps.forceMobileMode)
					mobileModeCount++;
				}
				
			this.mobileModeButtonLabel =
				(mobileModeCount == 0) ? "None" :
				(mobileModeCount == this.platformList.Count) ? "All" : (mobileModeCount.ToString() + " of " + this.platformList.Count.ToString());
			}


		// -----------------
		public void DrawForceMobileModeButton()
			{
				EditorGUILayout.BeginHorizontal(CFEditorStyles.Inst.featureBoxStyle);
				EditorGUILayout.LabelField("Force Mobile Mode on follwing platforms:", GUILayout.ExpandWidth(true)); //GUILayout.Width(200));
				if (GUILayout.Button(new GUIContent(this.mobileModeButtonLabel, 
				//if (GUILayout.Button(new GUIContent("Forced Mobile Mode is set for " + this.mobileModeButtonLabel + " platforms. (Click for more info)", 
					"Set Forced Mobile Mode for each platform...\n\nClick for more details."), CFEditorStyles.Inst.buttonStyle, GUILayout.Width(70))) //.featureBoxStyle))		
					{
					this.CreateContextMenu();
					}

			EditorGUILayout.EndHorizontal();
			}


		// ---------------
		public class PlatformState
			{
			public BuildTargetGroup 
				platform;
			public bool
				forceMobileMode; 

			public PlatformOptionList
				platformList;

			// ---------------
			public PlatformState(PlatformOptionList platformList)
				{
				this.platformList = platformList;
				}

			// ----------------
			public void ToggleMobileMode()
				{
				if (!this.forceMobileMode)
					ConfigManager.AddSymbol(ConfigManager.CF_FORCE_MOBILE_MODE, this.platform);
				else
					ConfigManager.RemoveSymbol(ConfigManager.CF_FORCE_MOBILE_MODE, this.platform);

				this.forceMobileMode = !this.forceMobileMode;		

				this.platformList.OnOptionChange();		
				}
			}
		}



	}
}

#endif

