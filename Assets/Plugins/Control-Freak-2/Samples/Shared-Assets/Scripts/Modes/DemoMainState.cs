// -------------------------------------------
// Control Freak 2
// Copyright (C) 2013-2020 Dan's Game Tools
// http://DansGameTools.blogspot.com
// -------------------------------------------

using UnityEngine;

namespace ControlFreak2.Demos
{

public class DemoMainState : ControlFreak2.GameState
	{
	public MultiDemoManager 
		multiDemoManager;

		
	public HelpBoxState
		helpBox; 

	public KeyCode		
		helpKey = KeyCode.Escape;



	// --------------------
	protected override void OnStartState(ControlFreak2.GameState parentState)
		{
		base.OnStartState(parentState);

		this.gameObject.SetActive(true);
		}


	// --------------------
	protected override void OnExitState()
		{
		base.OnExitState();

		this.gameObject.SetActive(false);
		}



	// -------------------
	protected override void OnUpdateState()
		{
		if ((this.helpBox != null) && !this.helpBox.IsRunning() && (this.helpKey != KeyCode.None) && CF2Input.GetKeyDown(this.helpKey))
			this.ShowHelpBox();				

		base.OnUpdateState();
		}


	// -------------------
	public void ExitToMainMenu()
		{
		if (this.multiDemoManager != null)
			this.multiDemoManager.EnterMainMenu();
		else
			Application.Quit();
		}


	// --------------------
	public void ShowHelpBox()
		{
		if (this.helpBox != null)
			this.helpBox.ShowHelpBox(this);
		}
	}
}
