// -------------------------------------------
// Control Freak 2
// Copyright (C) 2013-2020 Dan's Game Tools
// http://DansGameTools.blogspot.com
// -------------------------------------------

using UnityEngine;

namespace ControlFreak2.Demos.RPG
{

public class CharacterAction : MonoBehaviour 
	{
	public LevelData
		levelData;

	public string 
		actionButonName = "Action";

	public string
		actionAnimatorTrigger = "Action";


	private Animator
		animator;



	// ------------------
	void OnEnable()
		{
		this.animator = this.GetComponent<Animator>();
		}

	// ------------------
	void Update()
		{
		if (!string.IsNullOrEmpty(this.actionButonName) && CF2Input.GetButtonDown(this.actionButonName))
			this.PerformAction();
		
		}
	
	
	// --------------------
	public void PerformAction()	
		{
		if (this.levelData == null)
			return;

		InteractiveObjectBase 
			obj = this.levelData.FindInteractiveObjectFor(this);
		
		if (obj != null)
			{
			if (!string.IsNullOrEmpty(this.actionAnimatorTrigger) && (this.animator != null)) 
				this.animator.SetTrigger(this.actionAnimatorTrigger);

			obj.OnCharacterAction(this);
			}
		}


	}
}
