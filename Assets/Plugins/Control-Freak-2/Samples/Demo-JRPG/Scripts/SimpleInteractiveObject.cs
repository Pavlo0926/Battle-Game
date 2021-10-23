// -------------------------------------------
// Control Freak 2
// Copyright (C) 2013-2020 Dan's Game Tools
// http://DansGameTools.blogspot.com
// -------------------------------------------

using UnityEngine;

namespace ControlFreak2.Demos.RPG
{
public class SimpleInteractiveObject : InteractiveObjectBase 
	{
	public Color 
		activatedColor = Color.green;	

	public Renderer
		targetRenderer;

	public AudioClip
		soundEffect;

	private bool 
		isActivated = false;

	// ------------------
	override public void OnCharacterAction(CharacterAction chara)
		{
		this.isActivated = !this.isActivated;
		
		if (this.targetRenderer != null)
			this.targetRenderer.material.color = (this.isActivated ? this.activatedColor : Color.white);

		if (this.soundEffect != null)
			AudioSource.PlayClipAtPoint(this.soundEffect, this.transform.position);
		}


	}
}
