// -------------------------------------------
// Control Freak 2
// Copyright (C) 2013-2020 Dan's Game Tools
// http://DansGameTools.blogspot.com
// -------------------------------------------

#if UNITY_EDITOR

using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;

using ControlFreak2;
using ControlFreak2Editor.Internal;

namespace ControlFreak2Editor
{

public class Assistant : EditorWindow
	{
	static Assistant mInst;
	static bool mPrefsChecked;
	static bool mAutoOpen;
	public bool recordingOn;
	public bool propBoxOn;
	public InputRig selectedRig;

	const string AUTO_OPEN_PREF_KEY = "CF2AutoOpenAssistant";


	private List<LogEntry> logEntries;
		


	private LogEntry selectedEntry;


	// ------------------
	public enum CallType
		{
		Axis,
		Button,
		Key,
		Touch,
		ScrollWheel,
		MousePosition,
		CursorLock
		}		


	// -----------------
	public Assistant()
		{
		this.logEntries = new List<LogEntry>(128);
		this.minSize = new Vector2(200, 200);
		}
		

	// ---------------------
	[MenuItem("Control Freak 2/CF2 Assistant")]
	static void ShowListenerMenuItem()
		{ ShowListener(true); }

	// ---------------------
	static private Assistant ShowListener(bool focus)
		{ 
		Assistant listener = (GetWindow(typeof(Assistant), true, "Control Freak 2 Assistant", focus) as Assistant);
		if (listener == null)
			return null;

		return listener;
		}

		
	// ----------------------
	static private bool IsAutoOpenEnabled()
		{
		if (!mPrefsChecked)
			{
			mPrefsChecked = true;
			mAutoOpen = EditorPrefs.GetBool(AUTO_OPEN_PREF_KEY);				
			}
		
		return mAutoOpen;
		}


	// ---------------------
	static public void CaptureKey		(KeyCode keyParam)	{ CaptureEx(CallType.Key,			"",			keyParam); }
	static public void CaptureButton	(string buttonName)	{ CaptureEx(CallType.Button,	 	buttonName,	KeyCode.None); }
	static public void CaptureAxis		(string axisName)	{ CaptureEx(CallType.Axis,			axisName,	KeyCode.None); }
	static public void CaptureMousePos	()					{ CaptureEx(CallType.MousePosition,"",			KeyCode.None); }
	static public void CaptureScrollWheel()					{ CaptureEx(CallType.ScrollWheel,	"",			KeyCode.None); }
	static public void CaptureTouch		()					{ CaptureEx(CallType.Touch,			"",			KeyCode.None); }
	static public void CaptureCursorLock()					{ CaptureEx(CallType.CursorLock,	"",			KeyCode.None); }


	// ---------------------
	static private void CaptureEx(CallType callType, string strParam, KeyCode keyParam)
		{
		if (mInst == null)
			{
			if (IsAutoOpenEnabled())
				mInst = ShowListener(false);
			}

		if (mInst == null)
			return;

		mInst.CaptureCall(callType, strParam, keyParam);
		}


		
	// -------------------
	private InputRig GetCurrentlySelectedRig()
		{
		InputRig rig = TouchControlWizardUtils.GetRigFromSelection(this.selectedRig, true);
		if (rig == null)
			rig = CF2Input.activeRig;
		
		return rig;

		//if (Selection.activeTransform == null)
		//	return null;

		//return Selection.activeTransform.GetComponent<InputRig>();
		}


		
	// ------------------
	private void OnSelectionChange()
		{
		this.selectedRig = this.GetCurrentlySelectedRig();
			
		this.RefreshRigState();

		this.Repaint();
		}


	// -------------------
	public void ClearEntries()
		{
		this.logEntries.Clear();
		this.selectedEntry = null;
		}

	// ------------------
	public void RefreshRigState()
		{
		if (this.selectedRig == null)
			this.selectedRig = this.GetCurrentlySelectedRig();

		for (int i = 0; i < this.logEntries.Count; ++i)
			{	
			this.logEntries[i].CheckRigState(this.selectedRig);
			}

		//this.Repaint();
		}


	// ------------------
	private void CaptureCall(CallType callType, string strParam, KeyCode keyParam)
		{
		if (!this.recordingOn)
			return;
	
		if (string.IsNullOrEmpty(strParam) && (keyParam == KeyCode.None))	
			return;
	
		System.Diagnostics.StackTrace trace = new System.Diagnostics.StackTrace(1, true);
			
		string stackTrace = trace.ToString();

		if (this.logEntries.Find(x => x.IsDuplicateOf(callType, strParam, keyParam, stackTrace)) == null)
			{
			this.logEntries.Add(new LogEntry(callType, strParam, keyParam, trace));

			this.RefreshRigState();

			this.Repaint();
			}

		
		}

		

#if UNITY_2017_3_OR_NEWER
	private void OnPlayModeStateChanged(PlayModeStateChange ch)	{ this.RefreshRigState(); }
#endif	


		
	// -------------------
	void OnEnable()
		{
		//Debug.Log("ENABLE LISTENER : " + ((this.logEntries == null) ? "NULL" : this.logEntries.Count.ToString()) + 
		//	" MSGS: " + ((this.logEntries == null) ? "NULL" : this.msgs.Count.ToString()) + " INST: " + ((mInst == null) ? "NULL" : "NOT NULL"));
		mInst = this;


#if UNITY_2017_3_OR_NEWER
		EditorApplication.playModeStateChanged += this.OnPlayModeStateChanged;
#else
		EditorApplication.playmodeStateChanged += this.RefreshRigState;
#endif
 
		//Selection.selectionChanged += this.OnSelChange;
		
		this.selectedEntry = null;
		this.propBoxOn = true;
	
		this.recordingOn = true;

		this.OnSelectionChange();

		this.Repaint();
		}

	// ------------------
	void OnDisable()
		{
#if UNITY_2017_3_OR_NEWER
		EditorApplication.playModeStateChanged -= this.OnPlayModeStateChanged;
#else
		EditorApplication.playmodeStateChanged -= this.RefreshRigState;
#endif
		//Selection.selectionChanged -= this.OnSelChange;
		}
			

	// -----------------
	void OnFocus()
		{
		this.RefreshRigState();
		}

		
	private Vector2
		scrollPos,
		propBoxScroll;



	// ------------------
	public void SelectEntry(LogEntry entry)
		{
		this.selectedEntry = entry;
		}


	// -----------------
	public bool IsEntrySelected(LogEntry entry)
		{
		return (entry == this.selectedEntry);
		}

	// -----------------------
	void OnGUI()
		{
		const float BUTTON_HEIGHT = 30;
		const float TOOLBAR_HEIGHT = 34;
		const float PROP_BOX_BUTTON_HEIGHT = 20;
		const float MAX_PROP_BOX_HEIGHT = 300;
		const float ENTRY_LIST_REL_HEIGHT = 0.55f;

		
		float entryListHeight = (this.position.height - TOOLBAR_HEIGHT) - PROP_BOX_BUTTON_HEIGHT - 100;
		float propBoxHeight = Mathf.Min((MAX_PROP_BOX_HEIGHT), (entryListHeight * (1.0f - ENTRY_LIST_REL_HEIGHT)));
		
		
		GUILayout.Box(GUIContent.none, CFEditorStyles.Inst.headerAssistant, GUILayout.ExpandWidth(true));


		EditorGUILayout.BeginHorizontal(CFEditorStyles.Inst.toolbarBG);

			this.recordingOn = CFGUI.PushButton(
				new GUIContent("Capture in progress!", CFEditorStyles.Inst.greenDotTex, "Editor must be in Play mode to catch Input calls!"), 
				new GUIContent("Stopped. Press to begin capture", CFEditorStyles.Inst.pauseTex, "Editor must be in Play mode to catch Input calls!"),
				this.recordingOn, CFEditorStyles.Inst.buttonStyle, GUILayout.Height(BUTTON_HEIGHT), GUILayout.ExpandWidth(true));

			GUI.backgroundColor = (Color.white);

			if (GUILayout.Button("Clear", GUILayout.Height(BUTTON_HEIGHT), GUILayout.Width(70)))
				this.ClearEntries();				

		EditorGUILayout.EndHorizontal();

			
		if (this.selectedRig == null)
			EditorGUILayout.HelpBox("No rig selected!! Select one to be able to easily create controls or bind commands!", MessageType.Warning);
		else
			EditorGUILayout.HelpBox("Rig: " + this.selectedRig.name, MessageType.None);
			
		if (!CFUtils.editorStopped && (this.logEntries.Count > 0))
			EditorGUILayout.HelpBox("Remember to stop Editor before binding or creating controls!", MessageType.Warning);


		this.scrollPos = EditorGUILayout.BeginScrollView(this.scrollPos, CFEditorStyles.Inst.transpSunkenBG, GUILayout.ExpandHeight(true)); //GUILayout.Height(entryListHeight));
			
			if (this.logEntries.Count == 0)
				{
				GUILayout.Box("Nothing captured yet!" + (CFUtils.editorStopped ? "\nTo capture anything hit the PLAY button!" : ""),
					CFEditorStyles.Inst.centeredTextTranspBG);
				}
			else
				{
	
				for (int i = this.logEntries.Count - 1; i >= 0; --i)
					{
					//GUILayout.Label("[" + i + "] : " + this.logEntries[i].ToString());
					this.logEntries[i].DrawListElemGUI(this);
					}	
				}	

		EditorGUILayout.EndScrollView();	
			
		if (this.selectedEntry != null)
			{
	
			this.propBoxOn = CFGUI.PushButton(
				new GUIContent("Hide Properties", CFEditorStyles.Inst.texMoveDown),
				new GUIContent("Show Properties", CFEditorStyles.Inst.texMoveUp),
				this.propBoxOn, CFEditorStyles.Inst.buttonStyle);
	
			if (this.propBoxOn)
				{
				this.propBoxScroll = EditorGUILayout.BeginScrollView(this.propBoxScroll, CFEditorStyles.Inst.transpSunkenBG, GUILayout.Height(propBoxHeight));
		
					this.selectedEntry.DrawPropertiesGUI();
	
				EditorGUILayout.EndScrollView();
				}
			}

		} 




	// ---------------------	
	[System.Serializable]
	public class LogEntry
		{
		private List<Frame> frames;
		private CallType	callType;
		private string		strParam;
		private KeyCode		keyParam;
		//private string		sceneName;
		private string		stackTrace;

		private int 		rigAxisId;		

		private bool
			availableInRig,	
			availableOnMobile;	


		const string PATH_TO_IGNORE = "Assets/Plugins/Control-Freak-2/";


		// ------------------
		public enum SourceAvailablity
			{
			Not,
			MobileOnly,
			NotOnMobile		
			}



		// ----------------
		public LogEntry(CallType callType, string strParam, KeyCode keyParam, System.Diagnostics.StackTrace trace)
			{
			this.callType	= callType;
			this.keyParam	= keyParam;
			this.strParam	= strParam;

	

			

			this.stackTrace	= trace.ToString();

			int frameCount = trace.FrameCount;

			this.frames = new List<Frame>(frameCount);
				
			for (int i = 0; i < frameCount; ++i)
				{
				System.Diagnostics.StackFrame fr = trace.GetFrame(i);

				if (IsFileInternal(fr.GetFileName()))
					{
					continue;
					}
					
				//outsideMethodFound = true;
				this.frames.Add(new Frame(fr));


				}
			}
				

		// --------------------
		public void CheckRigState(InputRig rig)
			{
			this.availableInRig = false;
			this.availableOnMobile = false;
				
			if (rig == null)
				return;

			switch (this.callType)
				{
				case CallType.Button :
				case CallType.Axis :
					this.availableInRig 	= rig.IsAxisDefined(this.strParam, ref this.rigAxisId);
					this.availableOnMobile 	= rig.IsAxisAvailableOnMobile(this.strParam);
					break;
	
				case CallType.Key :
					this.availableInRig 	= true;
					this.availableOnMobile	= rig.IsKeyAvailableOnMobile(this.keyParam);
					break;

				case CallType.Touch :
					this.availableInRig		= 
					this.availableOnMobile	= rig.IsTouchEmulatedOnMobile();
					break;
	
				case CallType.MousePosition :
					this.availableInRig 	= 
					this.availableOnMobile 	= rig.IsMousePositionEmulatedOnMobile();
					break;

				case CallType.ScrollWheel :
					this.availableInRig		= true;
					this.availableOnMobile	= rig.IsScrollWheelEmulatedOnMobile();
					break;
				}
			}


		// -----------------
		public bool IsDuplicateOf(CallType call, string strParam, KeyCode keyParam, string stackTrace)
			{
			return ((this.callType == call) && (this.strParam == strParam) && (this.keyParam == keyParam) && (this.stackTrace == stackTrace));
			}


		// -----------------
		static private bool IsFileInternal(string path)
			{
			return ((path == null) || (path.Length == 0) || (path.Replace('\\', '/').IndexOf(PATH_TO_IGNORE) >= 0));
			}


		// -----------------
		public override string ToString()
			{
			string s = this.callType.ToString() + "  (frames: " + this.frames.Count + ")\n";
			foreach (Frame frame in this.frames)
				s += "\t" + frame.ToString() + "\n";
		
			return s;
			}
			


		// ----------------------
		public void DrawListElemGUI(Assistant listener)
			{
			//const float AXIS_BUTTON_WIDTH = 30;
			const float HEIGHT = 20;
				
			if (listener.IsEntrySelected(this))
				GUI.backgroundColor = new Color(0.7f, 0.7f, 0.9f, 1.0f);
			else
				GUI.backgroundColor = new Color(0.7f, 0.7f, 0.7f, 1.0f);

			EditorGUILayout.BeginHorizontal(CFEditorStyles.Inst.whiteBevelBG);

				GUI.backgroundColor = Color.white;

				
				GUIContent stateContent = GUIContent.none;
				
				GUIContent nameContent = GUIContent.none;


				if (listener.selectedRig == null)
					stateContent = new GUIContent(CFEditorStyles.Inst.texError, "No rig selected!");

				switch (this.callType)
					{
					case CallType.Axis :
					case CallType.Button :
						nameContent = new GUIContent(this.strParam, ((this.callType == Assistant.CallType.Axis) ? "Axis Name" : "Button Name"));

						if (listener.selectedRig != null)
							{
							if (!this.availableInRig)
								stateContent = new GUIContent(CFEditorStyles.Inst.texError, "Axis not available in [" + listener.selectedRig.name + "] rig!");
						
							else if (!this.availableOnMobile)
								stateContent = new GUIContent(CFEditorStyles.Inst.texWarning, "Axis not available in [" + listener.selectedRig.name + "] rig in Mobile Mode!");
			
							else 
								stateContent = new GUIContent(CFEditorStyles.Inst.texOk, "Axis available in desktop and mobile modes!");
							}		
						break;
					

					case CallType.Key :
						nameContent = new GUIContent(this.keyParam.ToString(), "Key code");

						if (listener.selectedRig != null)
							{
							if (!this.availableInRig)
								stateContent = new GUIContent(CFEditorStyles.Inst.texError, "Key not available in [" + listener.selectedRig.name + "] rig!");
						
							else if (!this.availableOnMobile)
								stateContent = new GUIContent(CFEditorStyles.Inst.texWarning, "Key not available in [" + listener.selectedRig.name + "] rig in Mobile Mode!");
			
							else 
								stateContent = new GUIContent(CFEditorStyles.Inst.texOk, "Key available in desktop and mobile modes!");
							}
						break;


					case CallType.ScrollWheel :
						if (listener.selectedRig != null)
							{
							if (!this.availableInRig)
								stateContent = new GUIContent(CFEditorStyles.Inst.texError, "Scroll Wheel not available in [" + listener.selectedRig.name + "] rig! Create an axis of SCROLL type and bind it as one of scroll wheel axes in Input Rig's Scroll Wheel Settings!");
						
							else if (!this.availableOnMobile)
								stateContent = new GUIContent(CFEditorStyles.Inst.texWarning, "Scroll Wheel not available in [" + listener.selectedRig.name + "] rig in Mobile Mode! Create an axis of SCROLL type and bind it as one of scroll wheel axes in Input Rig's Scroll Wheel Settings!");
			
							else 
								stateContent = new GUIContent(CFEditorStyles.Inst.texOk, "Scroll Wheel available in desktop and mobile modes!");
							}
						break;

					case CallType.MousePosition :
						if (listener.selectedRig != null)
							{
							if (!this.availableInRig)
								stateContent = new GUIContent(CFEditorStyles.Inst.texError, "Mouse Position is not available in [" + listener.selectedRig.name + "] rig! Create an Touch Zone and bind mouse position to it.");
						
							else if (!this.availableOnMobile)
								stateContent = new GUIContent(CFEditorStyles.Inst.texError, "Mouse Position is not available in [" + listener.selectedRig.name + "] rig! Create an Touch Zone and bind mouse position to it.");
			
							else 
								stateContent = new GUIContent(CFEditorStyles.Inst.texOk, "Mouse Position available in desktop and mobile modes!");
							}
						break;

					case CallType.Touch :
						if (listener.selectedRig != null)
							{
							if (!this.availableInRig)
								stateContent = new GUIContent(CFEditorStyles.Inst.texError, "Emulated Touches are not available in [" + listener.selectedRig.name + "] rig! Create an Touch Zone and bind emulated touches to it.");
						
							else if (!this.availableOnMobile)
								stateContent = new GUIContent(CFEditorStyles.Inst.texError, "Emulated Touches are not available in [" + listener.selectedRig.name + "] rig! Create an Touch Zone and bind emulated touches to it.");
			
							else 
								stateContent = new GUIContent(CFEditorStyles.Inst.texOk, "Emulated Touches are available in desktop and mobile modes!");
							}
						break;

					case CallType.CursorLock:
						stateContent = GUIContent.none;
						break;

					}
				

				EditorGUILayout.LabelField(stateContent, GUILayout.Width(20), GUILayout.Height(HEIGHT));

				if ((listener.selectedRig != null)) //&& ((this.callType == Assistant.CallType.Axis) || (this.callType == CallType.Button)) && !this.availableInRig)
					{
					switch (this.callType)
						{
						case CallType.Axis : 
							if (GUILayout.Button(new GUIContent((this.availableInRig ? CFEditorStyles.Inst.texWrench : CFEditorStyles.Inst.createNewTex), 
								"Add or bind this axis..."), GUILayout.Width(20), GUILayout.Height(HEIGHT)))
								{
								WizardMenuUtils.CreateContextMenuForAxisBinding(listener.selectedRig, this.strParam, listener.RefreshRigState);
								}
							break;

						case CallType.Button :
							if (GUILayout.Button(new GUIContent((this.availableInRig ? CFEditorStyles.Inst.texWrench : CFEditorStyles.Inst.createNewTex), 
								"Add or bind this button..."), GUILayout.Width(20), GUILayout.Height(HEIGHT)))
								{
								WizardMenuUtils.CreateContextMenuForAxisBinding(listener.selectedRig, this.strParam, listener.RefreshRigState);
								}
							break;

						case CallType.Key :
							if (GUILayout.Button(new GUIContent(CFEditorStyles.Inst.texWrench,	
								"Bind this key..."), GUILayout.Width(20), GUILayout.Height(HEIGHT)))
								{
								WizardMenuUtils.CreateContextMenuForKeyBinding(listener.selectedRig, this.keyParam, listener.RefreshRigState);
								}
							break;

						case CallType.MousePosition :
							if (GUILayout.Button(new GUIContent(CFEditorStyles.Inst.texWrench,	
								"Bind mouse position..."), GUILayout.Width(20), GUILayout.Height(HEIGHT)))
								{
								WizardMenuUtils.CreateContextMenuForMousePositionBinding(listener.selectedRig, listener.RefreshRigState);
								}
							break;
	
						case CallType.Touch :
							if (GUILayout.Button(new GUIContent(CFEditorStyles.Inst.texWrench,	
								"Bind emu. touch..."), GUILayout.Width(20), GUILayout.Height(HEIGHT)))
								{
								WizardMenuUtils.CreateContextMenuForEmuTouchBinding(listener.selectedRig, listener.RefreshRigState);
								}
							break;

						}
	
					}


				bool showPropertiesClicked = false;

				if (GUILayout.Button(new GUIContent(this.callType.ToString()), CFEditorStyles.Inst.transpBevelBG, GUILayout.Width(100), GUILayout.Height(HEIGHT)))
					showPropertiesClicked = true;
				
				GUI.backgroundColor = new Color(1, 1, 1, 0.3f);
				if (GUILayout.Button(nameContent, CFEditorStyles.Inst.whiteBevelBG, GUILayout.ExpandWidth(true), GUILayout.Height(HEIGHT)))
					showPropertiesClicked = true;

				GUI.backgroundColor = Color.white;


				if (showPropertiesClicked)
					listener.SelectEntry(this);

			EditorGUILayout.EndHorizontal();
			}

			


		// ---------------------
		public void DrawPropertiesGUI()
			{
			string propStr = this.callType.ToString();

			if ((this.callType == Assistant.CallType.Axis) || 
				(this.callType == Assistant.CallType.Button))
				propStr += " : " + this.strParam;

			else if (this.callType == Assistant.CallType.Key)
				propStr += " : " + this.keyParam;
	
		

			GUILayout.Box(propStr, CFEditorStyles.Inst.transpBevelBG);
				

			EditorGUILayout.Space();


			EditorGUILayout.BeginVertical(CFEditorStyles.Inst.transpSunkenBG);

			EditorGUILayout.LabelField("Call stack trace:");
				
			for (int i = 0; i < this.frames.Count; ++i)
				{
				Frame fr = this.frames[i];;

				if (GUILayout.Button(new GUIContent(//"Method: " + fr.method + "\n" + "Class: + " + fr.classname + "\n" +
					" Line: " + fr.line + " in " + fr.relFilename, CFEditorStyles.Inst.magnifiyingGlassTex, "Open this line in IDE.")))
					{
					fr.ShowInIDE();
					}
				}

			EditorGUILayout.EndVertical();
			}
			



		// ---------------------
		[System.Serializable]
		private class Frame
			{
			public string
				filename,
				relFilename,
				classname,
				method;
			public int
				line;
				
			// -----------------
			public Frame(System.Diagnostics.StackFrame frame)
				{
				this.filename	= frame.GetFileName();
				this.filename = ((this.filename == null) ? "" : this.filename.Replace('\\', '/'));
	
				this.relFilename = filename.Replace(Application.dataPath.Replace('\\', '/'), "");

				this.classname	= frame.GetType().ToString();
				this.method		= frame.GetMethod().ToString();
				this.line		= frame.GetFileLineNumber();	
				}

			// ------------------
			public void ShowInIDE()
				{
				CFEditorUtils.OpenScriptInIDE(this.filename, this.line);
				}

			// ------------------
			override public string ToString()
				{
				return (this.method + "\t at line:" + this.line + " in [" + this.relFilename + "]");
				}
			}
		}

	}



}

#endif


