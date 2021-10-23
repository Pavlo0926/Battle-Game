// -------------------------------------------
// Control Freak 2
// Copyright (C) 2013-2020 Dan's Game Tools
// http://DansGameTools.blogspot.com
// -------------------------------------------

using UnityEngine;

namespace ControlFreak2.Demos.Helpers
{

public class AudioIgnoreListenerPauseAndVolume : MonoBehaviour 
	{
	public AudioSource[]
		targetSources;

	public bool ignoreListenerVolume = true;
	public bool ignoreListenerPause = true;


	// -----------------------	
	void OnEnable()
		{
		if ((this.targetSources == null) || (this.targetSources.Length == 0))
			this.targetSources = this.GetComponents<AudioSource>();

		for (int i = 0; i < this.targetSources.Length; ++i)
			{
			if (this.targetSources[i] != null)
				{
				this.targetSources[i].ignoreListenerPause = this.ignoreListenerPause;
				this.targetSources[i].ignoreListenerVolume = this.ignoreListenerVolume;
				}
			}
		}


	}
}
