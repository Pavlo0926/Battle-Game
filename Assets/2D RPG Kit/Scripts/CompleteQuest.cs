using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

//Adds a BoxCollider2D component automatically to the game object
[RequireComponent(typeof(BoxCollider2D))]
public class CompleteQuest : MonoBehaviour {

    public string questToMark;
    public bool markComplete;

    public bool markOnButtonPress;
    public bool markOnEnter;
    public bool markOnExit;
    private bool canMark;

    public bool deactivateOnMarking;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        //if(canMark)
        //{
        // canMark = false;
        //MarkQuest();
        //}
        //Check for button input
        if (Input.GetButtonDown("RPGConfirmPC") || Input.GetButtonDown("RPGConfirmJoy") || CrossPlatformInputManager.GetButtonDown("RPGConfirmTouch") && !DialogManager.instance.dialogBox.activeInHierarchy)
        {
            if (canMark && markOnButtonPress && !GameManager.instance.battleActive && !GameManager.instance.gameMenuOpen)
            {
                MarkQuest();
            }
        }
    }

    public void MarkQuest()
    {
        if(markComplete)
        {
            QuestManager.instance.MarkQuestComplete(questToMark);
        } else
        {
            QuestManager.instance.MarkQuestIncomplete(questToMark);
        }

        gameObject.SetActive(!deactivateOnMarking);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {

        if (other.tag == "Player")
        {
            if (markOnEnter)
            {
                MarkQuest();
            }
            else
            {
                canMark = true;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            if (markOnExit)
            {
                MarkQuest();
            }
            else
            {
                canMark = false;
            }
        }
    }
}
