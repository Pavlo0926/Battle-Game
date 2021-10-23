// -------------------------------------------
// Control Freak 2
// Copyright (C) 2013-2020 Dan's Game Tools
// http://DansGameTools.blogspot.com
// -------------------------------------------

using UnityEngine;

using ControlFreak2;

namespace ControlFreak2.Demos
{

public class MultiDemoManager : ControlFreak2.GameState 
	{	
	const string 
		MAIN_MENU_SCENE_NAME = "CF2-Multi-Demo-Manager";


	public DemoMainState
		mainMenuState;


	// ----------------
	public void EnterMainMenu()
		{
		this.StartSubState(this.mainMenuState);

		//Application.LoadLevel(MAIN_MENU_SCENE_NAME);
		}

	// ------------------	
	private void Start()
		{
		this.OnStartState(null);
		}


	// --------------------
	void Update()
		{	
		if (this.IsRunning())
			this.OnUpdateState();
		}


	// ------------------
	protected override void OnStartState(GameState parentState)
		{		
		base.OnStartState(parentState);

		this.gameObject.SetActive(true);

		this.EnterMainMenu();
		}


	// -----------------
	protected override void OnExitState()
		{
		base.OnExitState();

		this.gameObject.SetActive(false);
		}	

	
	}
}
