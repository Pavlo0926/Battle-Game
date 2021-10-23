// -------------------------------------------
// Control Freak 2
// Copyright (C) 2013-2020 Dan's Game Tools
// http://DansGameTools.blogspot.com
// -------------------------------------------

using UnityEngine;
using System.Collections.Generic;

namespace ControlFreak2.Demos.RPG
{

public class LevelData : MonoBehaviour 
	{
	public List<InteractiveObjectBase> 
		interactiveObjectList;


	// --------------------
	public LevelData() : base()
		{
		this.interactiveObjectList = new List<InteractiveObjectBase>();
		}


	// -------------------
	public InteractiveObjectBase FindInteractiveObjectFor(CharacterAction chara)
		{
		InteractiveObjectBase 
			nearestObj = null;
		float	
			nearestObjDistSq = 0;



		for (int i = 0; i < this.interactiveObjectList.Count; ++i)
			{
			InteractiveObjectBase o = this.interactiveObjectList[i];
			if ((o == null) || !o.IsNear(chara))
				continue;

			float distSq = (chara.transform.position - o.transform.position).sqrMagnitude;

			if ((nearestObj == null) || (distSq < nearestObjDistSq))
				{
				nearestObj = o;
				nearestObjDistSq = distSq;
				}
			}		

		return nearestObj;
		}
	}
}
