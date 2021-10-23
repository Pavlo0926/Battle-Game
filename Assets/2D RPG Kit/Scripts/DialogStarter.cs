using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

//Adds a BoxCollider2D component automatically to the game object
[RequireComponent(typeof(BoxCollider2D))]
public class DialogStarter : MonoBehaviour {

    [Header("Dialog Lines")]
    //The lines the npcs say when the player talks to them
    public string[] lines;
    public string[] sayGoodBye;

    //Check wheather the player is in range to talk to npc
    private bool canActivate;

    [Header("Activation")]
    //For different activation methods
    public bool activateOnAwake;
    public bool activateOnButtonPress;
    public bool activateOnEnter;
    public bool waitBeforeActivatingDialog;
    public float waitTime;

    [Header("NPC Settings")]
    //Check if the player talks to a person for displaying a name tag
    public bool displayName = true;

    //If npc should join your party
    public bool addToPartyCharacter2 = false;
    public bool addToPartyCharacter3 = false;

    [Header("Inn Settings")]
    //If npc should be an inn keeper
    public bool isInn;
    public int innPrice;

    [Header("Shop Settings")]
    //If npc should be a shop keeper
    public bool isShop;
    public string[] ItemsForSale = new string[40];

    [Header("Quest Settings")]
    //For completing quests after dialog
    //public bool shouldActivateQuest;
    public string questToMark;
    public bool markComplete;

    [Header("Event Settings")]
    //For completing quests after dialog
    //public bool shouldActivateQuest;
    public string eventToMark;
    public bool markEventComplete;


    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        //Check if dialog should be activated on awake or enter
        if (activateOnAwake || activateOnEnter)
        {
            //Check if player is in reach and if no other dialog is currently active
            if (canActivate && !DialogManager.instance.dialogBox.activeInHierarchy && !Inn.instance.innMenu.activeInHierarchy && !GameMenu.instance.menu.activeInHierarchy)
            {
                PlayerController.instance.canMove = false;
                //Set this to false to prevent activating dialog endlessly
                activateOnEnter = false;

                if (!DialogManager.instance.dontOpenDialogAgain)
                {
                    if (waitBeforeActivatingDialog)
                    {
                        //Disable player movement
                        PlayerController.instance.canMove = false;
                        StartCoroutine(waitCo());
                    }
                    else
                    {
                        activateOnAwake = false;

                        //Hide mobile controller during dialogs
                        GameMenu.instance.touchController.SetActive(false);

                        //Add new member to party
                        if (addToPartyCharacter2)
                        {
                            DialogManager.instance.addToPartyCharacter2 = addToPartyCharacter2;
                            addToPartyCharacter2 = false; //prevents from adding multiple times to party by talking to npc again
                        }

                        //Add new member to party
                        if (addToPartyCharacter3)
                        {
                            DialogManager.instance.addToPartyCharacter3 = addToPartyCharacter3;
                            addToPartyCharacter3 = false; //prevents from adding multiple times to party by talking to npc again
                        }

                        //Show inn menu
                        if (isInn)
                        {
                            DialogManager.instance.isInn = isInn;
                            DialogManager.instance.innPrice = innPrice;
                            Inn.instance.sayGoodBye = sayGoodBye;
                        }

                        //Show shop menu
                        if (isShop)
                        {
                            DialogManager.instance.isShop = isShop;
                            Shop.instance.itemsForSale = ItemsForSale;
                            Shop.instance.sayGoodBye = sayGoodBye;
                        }
                        
                        DialogManager.instance.ShowDialogAuto(lines, displayName);
                        
                        DialogManager.instance.ShouldActivateQuestAtEnd(questToMark, markComplete);
                        if (markEventComplete)
                        {
                            DialogManager.instance.ActivateEventAtEnd(eventToMark, markEventComplete);
                        }
                        
                    }
                }
            }
        }

        //Check for button input
        if (Input.GetButtonDown("RPGConfirmPC") || Input.GetButtonDown("RPGConfirmJoy") || CrossPlatformInputManager.GetButtonDown("RPGConfirmTouch") && !DialogManager.instance.dialogBox.activeInHierarchy)
        {
            
            if (canActivate && !DialogManager.instance.dialogBox.activeInHierarchy && !Inn.instance.innMenu.activeInHierarchy && !GameMenu.instance.menu.activeInHierarchy && !GameManager.instance.battleActive)
            {
                PlayerController.instance.canMove = false;
                //activateOnEnterConfirm = false;
                if (!DialogManager.instance.dontOpenDialogAgain)
                {
                    if (waitBeforeActivatingDialog)
                    {
                        //Disable player movement
                        PlayerController.instance.canMove = false;
                        StartCoroutine(waitCo());
                    }else
                    {
                        activateOnAwake = false;
                        GameMenu.instance.touchController.SetActive(false);

                        //Add new member to party
                        if (addToPartyCharacter2)
                        {
                            DialogManager.instance.addToPartyCharacter2 = addToPartyCharacter2;
                            addToPartyCharacter2 = false; //prevents from adding multiple times to party by talking to npc again
                        }

                        //Add new member to party
                        if (addToPartyCharacter3)
                        {
                            DialogManager.instance.addToPartyCharacter3 = addToPartyCharacter3;
                            addToPartyCharacter3 = false; //prevents from adding multiple times to party by talking to npc again
                        }

                        if (isInn)
                        {
                            DialogManager.instance.isInn = isInn;
                            DialogManager.instance.innPrice = innPrice;
                            Inn.instance.sayGoodBye = sayGoodBye;
                        }

                        if(isShop)
                        {
                            DialogManager.instance.isShop = isShop;
                            Shop.instance.itemsForSale = ItemsForSale;
                            Shop.instance.sayGoodBye = sayGoodBye;
                        }

                        
                            DialogManager.instance.ShowDialog(lines, displayName);
                        
                        //DialogManager.instance.SayGoodBye(sayGoodBye, isPerson);
                        DialogManager.instance.ShouldActivateQuestAtEnd(questToMark, markComplete);
                        if (markEventComplete)
                        {
                            DialogManager.instance.ActivateEventAtEnd(eventToMark, markEventComplete);
                        }
                        
                    }                    
                }
            }
        }
	}

    //Check if player enters trigger zone
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            canActivate = true;
            //DialogManager.instance.dontOpenDialogAgain = false;
            
        }
    }

    //Check if player exits trigger zone
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            canActivate = false;

            if (!activateOnButtonPress)
            {
                activateOnEnter = true;
            }
        }
    }

    //Put in a slight delay between activating the dialog and showing the dialog
    IEnumerator waitCo()
    {

        yield return new WaitForSeconds(waitTime);
        waitBeforeActivatingDialog = false;
        
    }
}
