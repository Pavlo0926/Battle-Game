// -------------------------------------------
// Control Freak 2
// Copyright (C) 2013-2020 Dan's Game Tools
// http://DansGameTools.blogspot.com
// -------------------------------------------

using UnityEngine;

namespace ControlFreak2.Demos
{

public class HelpBoxState : ControlFreak2.GameState
	{
	protected DemoMainState
		parentDemoState;


	protected override void OnStartState(ControlFreak2.GameState parentState)
		{
		base.OnStartState(parentState);

		this.gameObject.SetActive(true);
		}


	// ----------------------
	protected override void OnExitState()
		{
		base.OnExitState();

		this.gameObject.SetActive(false);
		}

	// -------------------
	public void ShowHelpBox(DemoMainState parentDemoState)
		{
		this.parentDemoState = parentDemoState;
		this.parentDemoState.StartSubState(this);	
		}


	// -----------------
	public void ExitToMainMenu()
		{
		if (this.parentDemoState != null)
			this.parentDemoState.ExitToMainMenu();
		}
	
	}
}
