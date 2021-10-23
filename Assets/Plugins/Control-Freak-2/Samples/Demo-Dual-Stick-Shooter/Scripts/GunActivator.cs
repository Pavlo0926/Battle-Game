// -------------------------------------------
// Control Freak 2
// Copyright (C) 2013-2020 Dan's Game Tools
// http://DansGameTools.blogspot.com
// -------------------------------------------

using UnityEngine;

namespace ControlFreak2.Demos.Guns
{

public class GunActivator : MonoBehaviour 
	{
	public Gun[]
		gunList;

	public string 
		buttonName = "Fire1";
	public KeyCode
		key			= KeyCode.None;

		

	// -----------------
	void Update()
		{
		bool triggerState =
			((this.key != KeyCode.None) ? CF2Input.GetKey(this.key) : false) ||
			(!string.IsNullOrEmpty(this.buttonName) ? CF2Input.GetButton(this.buttonName) : false);

		for (int i = 0; i < this.gunList.Length; ++i)
			{
			Gun g = this.gunList[i];
			if (g != null)
				g.SetTriggerState(triggerState);
			}
		}
	}
}
