// -------------------------------------------
// Control Freak 2
// Copyright (C) 2013-2020 Dan's Game Tools
// http://DansGameTools.blogspot.com
// -------------------------------------------
/*
#if UNITY_4_6 || UNITY_4_7 || UNITY_4_8 || UNITY_4_9 || UNITY_4
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
*/


#if UNITY_4_6 || UNITY_4_7 || UNITY_4_8 || UNITY_4_9 || UNITY_4
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

#if UNITY_PRE_5_5 || UNITY_5_5 
	#define UNITY_PRE_5_6
#endif

#if UNITY_PRE_5_6 || UNITY_5_6 
	#define UNITY_PRE_5_7
#endif

#if UNITY_PRE_5_7 || UNITY_5_7
	#define UNITY_PRE_2017
#endif

#if UNITY_PRE_2017 || UNITY_2017
	#define UNITY_PRE_2018_0
#endif

#if UNITY_PRE_2018_0 || UNITY_2018_0
	#define UNITY_PRE_2018_1
#endif






#if UNITY_EDITOR

using System.Xml.Serialization;
using System.IO;


using UnityEngine;
using UnityEditor;


namespace ControlFreak2Editor
{
public static class CFEditorUtils
	{
	public const string
		INTEGRATIONS_MENU_PATH = "Control Freak 2/Integrations/";

	public const int
		INTEGRATIONS_MENU_PRIO	= 99;


#if UNITY_PRE_2018_1	
// -------------------
	static public void AddOnHierarchyChange	(EditorApplication.CallbackFunction c)	{ EditorApplication.hierarchyWindowChanged	+= c; }
	static public void RemoveOnHierarchyChange(EditorApplication.CallbackFunction c)	{ EditorApplication.hierarchyWindowChanged	-= c; }
#else
	static public void AddOnHierarchyChange	(System.Action c)	{ EditorApplication.hierarchyChanged			+= c; }
	static public void RemoveOnHierarchyChange(System.Action c)	{ EditorApplication.hierarchyChanged			-= c; }
#endif




	// ------------------
	static public void SetWindowTitle(EditorWindow w, GUIContent title)
		{
#if UNITY_PRE_5_1
		w.title = title.text;
#else
		w.titleContent = title;
#endif
		}


		
	// ------------------
	static public string GetAssetsPath()
		{
		string s = Application.dataPath;
		if (s.Length == 0)
			return s;

		char c = s[s.Length - 1];
		return (((c != '\\') && (c != '/')) ? (s + "/") : s).Replace('\\', '/');
		}

	// ------------------
	static public string GetProjectPath()
		{
		string s = Application.dataPath;
		if (s.Length == 0)
			return s;

		char c = s[s.Length - 1];

		// Eat 'Assets/' at the end...

		return s.Substring(0, s.Length - (6 + (((c == '\\') || (c == '/')) ? 1 : 0))).Replace('\\', '/');
		}
	
		
	// -------------------
	static public string GetProjectRelativePath(string fullPath)
		{
		fullPath = fullPath.Replace('\\', '/');
		string projPath = GetProjectPath();

		if (fullPath.StartsWith(projPath))
			return fullPath.Substring(projPath.Length);
		
		return fullPath;		
		}


	// -------------------
	static public string GetAssetsRelativePath(string fullPath)
		{
		fullPath = fullPath.Replace('\\', '/');
		string assetsPath = GetAssetsPath();

		if (fullPath.StartsWith(assetsPath))
			return fullPath.Substring(assetsPath.Length);
		
		return fullPath;		
		}


	// ----------------
	static public bool OpenScriptInIDE(string fullPath, int line)
		{
		string relPath = fullPath.Replace('\\', '/').Replace(Application.dataPath.Replace('\\', '/'), "");
		relPath = ((relPath[0] == '/') ? "Assets" : "Assets/") + relPath;
			
		Object assetRef = AssetDatabase.LoadMainAssetAtPath(relPath);
		if (assetRef == null)
			{
			return false;
			}

		return AssetDatabase.OpenAsset(assetRef, line);
		}
		

	// -------------------
	static public Object LoadSubAsset(string path, string subAssetName, System.Type assetType) 
			{ return LoadSubAsset(path, subAssetName, assetType, System.StringComparison.OrdinalIgnoreCase); }

	static public Object LoadSubAsset(string path, string subAssetName, System.Type assetType, 
		System.StringComparison strCmpMode)
		{
		Object[] subAssets = AssetDatabase.LoadAllAssetsAtPath(path);	
		if (subAssets == null)
			return null;
			
		for (int i = 0; i < subAssets.Length; ++i)
			{
			Object o = subAssets[i];
			if (o.GetType() != assetType)
				continue;
	

			if ((subAssetName == null) || subAssetName.Equals(o.name, strCmpMode))
				return o; 
			}

		return null; 
		}



	// --------------------
	static public void EnsureDirectoryExists(string fullPath)
		{
		string dirPath = System.IO.Path.GetDirectoryName(fullPath);
		if (string.IsNullOrEmpty(dirPath))
			return;

		if (!System.IO.Directory.Exists(dirPath))
			System.IO.Directory.CreateDirectory(dirPath);
		}

		
	

	// ---------------------
	public static object LoadObjectFromXml(string fileName, System.Type objType)
		{
		try {			
			using (FileStream stream = File.OpenRead(fileName))
				{
				object result = new XmlSerializer(objType).Deserialize(stream);
				return result;
				}
			}
		catch (System.Exception)
			{
			}

        return null;
    	}


	// ---------------------

	static public void SaveObjectAsXml(string fileName, object obj, System.Type objType)
		{
		using (FileStream stream = new FileStream(fileName, FileMode.Create))
			{
			new XmlSerializer(objType).Serialize(stream, obj);
			}
		}

	


	// ----------------
	static public System.Type FindClass(string className)
		{
		System.Reflection.Assembly[] assemblies = 
			System.AppDomain.CurrentDomain.GetAssemblies();
			
		if (assemblies == null)
			return null;

		for (int i = 0; i < assemblies.Length; ++i)
			{
			System.Type t = assemblies[i].GetType(className);
			if (t != null)
				return t;
			}
		return null;
		}	


	}
}

#endif
