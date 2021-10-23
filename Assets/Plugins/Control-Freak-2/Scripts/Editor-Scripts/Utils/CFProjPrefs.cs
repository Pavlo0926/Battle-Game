// -------------------------------------------
// Control Freak 2
// Copyright (C) 2013-2020 Dan's Game Tools
// http://DansGameTools.blogspot.com
// -------------------------------------------

#if UNITY_EDITOR

using UnityEngine;
using UnityEditor;
using ControlFreak2Editor.Internal;


namespace  ControlFreak2Editor
{

[System.Serializable]
public class CFProjPrefs 
	{
	private const string SETTINGS_PATH_REL_TO_PROJ = "CF2-Data/Settings/CF2.config";

	public string	
		projectPath;
	public bool		
		isIsntalled,
		wasShown;
	public int		
		installedVer;



	// -------------------
	void OnEnable()
		{
//Debug.Log("Prefs OnEnable");
		}	


	// --------------------
	static private CFProjPrefs mInst;
	
	static public CFProjPrefs Inst 
		{
		get {
			if (mInst == null)
				{
				mInst = Load(); 
				}
			
			return mInst;
			}
		}

	
	// ------------------
	static public string GetFullPath()
		{
		return CFEditorUtils.GetProjectPath() + SETTINGS_PATH_REL_TO_PROJ;
		}
	
	// -----------------
	public static CFProjPrefs Load() 
		{
		string fullPath = GetFullPath(); 

		CFEditorUtils.EnsureDirectoryExists(fullPath);

		CFProjPrefs o = CFEditorUtils.LoadObjectFromXml(fullPath, typeof(CFProjPrefs)) as CFProjPrefs; //LoadXml<CFProjPrefs>(fullPath);
		return ((o == null) ? (new CFProjPrefs()) : o);
		}


	// -----------------
	public void Save()
		{
		CFEditorUtils.SaveObjectAsXml(GetFullPath(), this, this.GetType());
		}

	}
}

#endif
