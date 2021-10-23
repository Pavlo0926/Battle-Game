// -------------------------------------------
// Control Freak 2
// Copyright (C) 2013-2020 Dan's Game Tools
// http://DansGameTools.blogspot.com
// -------------------------------------------

#if UNITY_EDITOR

using UnityEngine;
using UnityEditor;

using System.Collections.Generic;

using ControlFreak2.Internal;
using ControlFreak2;

namespace ControlFreak2Editor
{

public class InputRigSpriteOptimizer : ISpriteOptimizer
	{

	// ----------------------
	static public void OptimizeHierarchy(GameObject root)
		{
		

		// if all sprites belong to the same texure dont pack!

		

		}	
		

	// --------------
	private List<SpriteElem>	sprites;

	
	// ------------------
	private InputRigSpriteOptimizer()
		{
		this.sprites = new List<SpriteElem>(32);		
		}



	// ---------------------
	void ISpriteOptimizer.AddSprite(Sprite sprite)
		{
		SpriteElem elem = this.FindElem(sprite);
		if (elem != null)
			return;	
			
		elem = new SpriteElem(sprite);
		this.sprites.Add(elem);
		}

	// ------------------
	Sprite ISpriteOptimizer.GetOptimizedSprite(Sprite sprite)
		{
		SpriteElem elem = this.FindElem(sprite);
		if (elem != null)
			return sprite;
			
		return elem.GetNewSprite();
		}
		




	// --------------------
	private SpriteElem FindElem(Sprite srcSprite)
		{
		return this.sprites.Find(elem => (elem.srcSprite == srcSprite));	
		}		


	// ----------------------
	private class SpriteElem
		{
		public Sprite srcSprite;	
		public Sprite newSprite;


		// ---------------
		public SpriteElem(Sprite srcSprite)
			{
			this.srcSprite = srcSprite;
			this.newSprite = srcSprite;
			}

		// ----------------
		public Sprite GetNewSprite()
			{
			return ((this.newSprite == null) ? this.srcSprite : this.newSprite);
			}
		}
	
	}
}

#endif
