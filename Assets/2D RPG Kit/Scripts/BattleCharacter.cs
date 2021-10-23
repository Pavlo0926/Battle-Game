using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleCharacter : MonoBehaviour {

    public Animator anim;

    [Header("Initialization")]
    //Game objects used by this code
    public SpriteRenderer spriteRenderer;
    public Sprite defeatedSprite;
    public Sprite aliveSprite;
    public Sprite portrait;

    [Header("Character Settings")]
    //For checking if this script is attached to a player game object. Else would be enemy game object
    public bool character;
    //Fill in the avaiilable skills for this character
    public string[] skills;

    public string characterName;
    public int currentHp, maxHP, currentSP, maxSP, strength, defense, weaponStrength, armorStrength;
    public bool defeated;

    

    private bool fadeOut;
    public float fadeOutSpeed = 1f;
    private bool activeBattlerIndicator;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		if(fadeOut)
        {
            spriteRenderer.color = new Color(Mathf.MoveTowards(spriteRenderer.color.r, 1f, fadeOutSpeed * Time.deltaTime), Mathf.MoveTowards(spriteRenderer.color.g, 0f, fadeOutSpeed * Time.deltaTime), Mathf.MoveTowards(spriteRenderer.color.b, 0f, fadeOutSpeed * Time.deltaTime), Mathf.MoveTowards(spriteRenderer.color.a, 0f, fadeOutSpeed * Time.deltaTime));
            if(spriteRenderer.color.a == 0)
            {
                gameObject.SetActive(false);
            }
        }
	}

    public void EnemyFade()
    {
        fadeOut = true;
    }
}
