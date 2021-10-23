// -------------------------------------------
// Control Freak 2
// Copyright (C) 2013-2020 Dan's Game Tools
// http://DansGameTools.blogspot.com
// -------------------------------------------

using UnityEngine;

namespace ControlFreak2.Demos.RPG
{

abstract public class InteractiveObjectBase : MonoBehaviour 
	{
	public float 
		radius = 2;

	abstract public void OnCharacterAction(CharacterAction chara);
	


	// ---------------------
	virtual public bool IsNear(CharacterAction chara)
		{
		return ((chara.transform.position - this.transform.position).sqrMagnitude < (this.radius * this.radius));
		}



	// --------------
	void OnDrawGizmos()
		{
		this.DrawDefaultGizmo();
		}


	// ---------------
	protected void DrawDefaultGizmo()
		{
		Gizmos.matrix = Matrix4x4.identity;
		Gizmos.DrawWireSphere(this.transform.position, this.radius);
		}

	

	}
}
