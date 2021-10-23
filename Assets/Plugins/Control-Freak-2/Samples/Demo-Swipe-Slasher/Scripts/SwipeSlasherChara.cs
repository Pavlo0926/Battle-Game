// -------------------------------------------
// Control Freak 2
// Copyright (C) 2013-2020 Dan's Game Tools
// http://DansGameTools.blogspot.com
// -------------------------------------------

using UnityEngine;
using UnityEngine.UI;

namespace ControlFreak2.Demos.SwipeSlasher
{
public class SwipeSlasherChara : MonoBehaviour 
	{

	// ---------------------
	public enum ActionType
		{
		LEFT_STAB,
		LEFT_SLASH_U,
		LEFT_SLASH_R,
		LEFT_SLASH_D,
		LEFT_SLASH_L,

		RIGHT_STAB,
		RIGHT_SLASH_U,
		RIGHT_SLASH_R,
		RIGHT_SLASH_D,
		RIGHT_SLASH_L,

		DODGE_LEFT,
		DODGE_RIGHT
		}	

	public AudioClip
		soundDodge,
		soundSlash,
		soundStab;


	public UnityEngine.UI.Graphic
		graphicLeftStab,
		graphicLeftSlashU,
		graphicLeftSlashR,
		graphicLeftSlashD,
		graphicLeftSlashL,

		graphicRightStab,
		graphicRightSlashU,
		graphicRightSlashR,
		graphicRightSlashD,
		graphicRightSlashL,

		graphicDodgeLeft,		
		graphicDodgeRight;

	public float 
		fadeDur = 0.5f;

	private Animator
		animator;

	public string
		animLeftStab	= "Left-Stab",
		animLeftSlashU	= "Left-Slash-U",
		animLeftSlashR	= "Left-Slash-R",
		animLeftSlashD	= "Left-Slash-D",
		animLeftSlashL	= "Left-Slash-L",

		animRightStab	= "Right-Stab",
		animRightSlashU	= "Right-Slash-U",
		animRightSlashR	= "Right-Slash-R",
		animRightSlashD	= "Right-Slash-D",
		animRightSlashL	= "Right-Slash-L",

		animDodgeLeft	= "Dodge-Left",
		animDodgeRight	= "Dodge-Right";


	// -------------------
	void OnEnable()
		{
		this.animator = this.GetComponent<Animator>();
		}

	// ------------------
	void Start()	
		{
		// Zero out action graphics' alpha...

		for (ActionType a = ActionType.LEFT_STAB; a <= ActionType.DODGE_RIGHT; ++a)
			{
			Graphic g = this.GetActionGraphic(a);
			if (g != null)
				{
				Color c = g.color;		
				c.a = 0;
				g.color = c;
				}
			}
	
		}


	// -----------------
	private UnityEngine.UI.Graphic GetActionGraphic(ActionType a)
		{	
		switch (a)
			{
			case ActionType.DODGE_LEFT		: return this.graphicDodgeLeft;
			case ActionType.DODGE_RIGHT		: return  this.graphicDodgeRight; 

			case ActionType.LEFT_STAB		: return this.graphicLeftStab; 
			case ActionType.LEFT_SLASH_U	: return this.graphicLeftSlashU; 
			case ActionType.LEFT_SLASH_D	: return this.graphicLeftSlashD; 
			case ActionType.LEFT_SLASH_R	: return this.graphicLeftSlashR;
			case ActionType.LEFT_SLASH_L	: return this.graphicLeftSlashL;

			case ActionType.RIGHT_STAB		: return this.graphicRightStab;
			case ActionType.RIGHT_SLASH_U	: return this.graphicRightSlashU; 
			case ActionType.RIGHT_SLASH_D	: return this.graphicRightSlashD; 
			case ActionType.RIGHT_SLASH_R	: return this.graphicRightSlashR; 
			case ActionType.RIGHT_SLASH_L	: return this.graphicRightSlashL;
			}
		return null;		
		}



	// ------------------
	private void SetActionTriggerState(ActionType action)
		{
		if (this.animator == null)
			return;
	
		string animName = null;
	
		switch (action)
			{
			case ActionType.LEFT_STAB		: animName = this.animLeftStab; break;
			case ActionType.LEFT_SLASH_U	: animName = this.animLeftSlashU; break;
			case ActionType.LEFT_SLASH_R	: animName = this.animLeftSlashR; break;
			case ActionType.LEFT_SLASH_D	: animName = this.animLeftSlashD; break;
			case ActionType.LEFT_SLASH_L	: animName = this.animLeftSlashL; break;

			case ActionType.RIGHT_STAB		: animName = this.animRightStab; break;
			case ActionType.RIGHT_SLASH_U	: animName = this.animRightSlashU; break;
			case ActionType.RIGHT_SLASH_R	: animName = this.animRightSlashR; break;
			case ActionType.RIGHT_SLASH_D	: animName = this.animRightSlashD; break;
			case ActionType.RIGHT_SLASH_L	: animName = this.animRightSlashL; break;

			case ActionType.DODGE_LEFT : animName = this.animDodgeLeft; break;
			case ActionType.DODGE_RIGHT : animName = this.animDodgeRight; break;
			}

		if (!string.IsNullOrEmpty(animName))
			this.animator.SetTrigger(animName);

		}


	// -------------------
	public void ExecuteAction(ActionType action)
		{
		UnityEngine.UI.Graphic 
			targetGr = this.GetActionGraphic(action);


		// Play a sound effect...
		
		if ((action == ActionType.LEFT_STAB) || (action == ActionType.RIGHT_STAB))
			this.PlaySound(this.soundStab);

		else if ((action == ActionType.DODGE_LEFT) || (action == ActionType.DODGE_RIGHT))
			this.PlaySound(this.soundDodge);

		else 
			this.PlaySound(this.soundSlash);

		// Play animation...
		
		if (this.animator != null)
			{
			this.SetActionTriggerState(action);
			}


		// Show and fade out graphic...

		if (targetGr != null)
			{
			Color c = targetGr.color;
			c.a = 1.0f;
			targetGr.color = c;

			targetGr.CrossFadeAlpha(1, 0, true);						
			targetGr.CrossFadeAlpha(0, this.fadeDur, true);						
			}
		}



	// ----------------------
	private void PlaySound(AudioClip clip)
		{
		if (clip != null)
			AudioSource.PlayClipAtPoint(clip, Vector3.zero);
		}


	

	}
}
