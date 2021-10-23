using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RewardScreen : MonoBehaviour {

    public int numberOfItemsHeld;
    public int numberOfEquipItemsHeld;

    //Make instance of this script to be able reference from other scripts!
    public static RewardScreen instance;

    [Header("Initialization")]
    //Game objects used by this code
    public Text earnedText;
    public Text itemText;
    public GameObject rewardScreen;
    public string[] rewardItems;
    public string[] rewardEquipItems;
    public int xpEarned;
    public int goldEarned;
    //For UI button higlighting
    public Button closeButton;

    [Header("Reward Settings")]
    public bool markQuestComplete;
    public string questToMark;

    

    // Use this for initialization
    void Start () {
        instance = this;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OpenRewardScreen(int xp, int gold, string[] rewards, string[] rewardsequip)
    {
        GameManager.instance.battleActive = true;

        xpEarned = xp;
        goldEarned = gold;
        rewardItems = rewards;
        rewardEquipItems = rewardsequip;

        earnedText.text = xpEarned + " EXP" + "\n" + goldEarned + " Gold";
        itemText.text = "";

        if (rewardItems.Length == 0 && rewardEquipItems.Length == 0)
        {
            itemText.text += "None";
        }

        //Check if the reward is only item
        if (rewardItems.Length > 0 && rewardEquipItems.Length == 0)
        {
            for (int i = 0; i < rewardItems.Length; i++)
            {
                itemText.text += rewards[i] + "\n";
            }
        }

        //Check if reward is only equipment
        if (rewardItems.Length == 0 && rewardEquipItems.Length > 0)
        {
            for (int i = 0; i < rewardEquipItems.Length; i++)
            {
                itemText.text += rewardsequip[i] + "\n";
            }
        }

        //Check if reward is both item and equipment
        if (rewardItems.Length > 0 && rewardEquipItems.Length > 0)
        {
            for (int i = 0; i < rewardItems.Length; i++)
            {
                itemText.text += rewards[i] + "\n";

                
            }
            for (int j = 0; j < rewardEquipItems.Length; j++)
            {
                itemText.text += rewardsequip[j] + "\n";
            }
        }
        

        rewardScreen.SetActive(true);
        

        if (ControlManager.instance.mobile == false)
        {
            GameMenu.instance.btn = closeButton;
            GameMenu.instance.SelectFirstButton();
        }
        
        GameManager.instance.gameMenuOpen = true;
    }

    public void CloseRewardScreen()
    {
        
        //Calculate the amount of items / equipment held in inventory to prevent adding more items if inventory is full
        numberOfItemsHeld = 0;
        numberOfEquipItemsHeld = 0;

        for (int i = 0; i < GameManager.instance.itemsHeld.Length; i++)
        {
            if (GameManager.instance.itemsHeld[i] != "")
            {
                numberOfItemsHeld++;
            }
        }

        for (int i = 0; i < GameManager.instance.equipItemsHeld.Length; i++)
        {
            if (GameManager.instance.equipItemsHeld[i] != "")
            {
                numberOfEquipItemsHeld++;
            }
        }


        for (int i = 0; i < GameManager.instance.characterStatus.Length; i++)
        {
            if(GameManager.instance.characterStatus[i].gameObject.activeInHierarchy && GameManager.instance.characterStatus[i].currentHP > 0)
            {
                GameManager.instance.characterStatus[i].AddExp(xpEarned);
            }
        }

        GameManager.instance.currentGold += goldEarned;


        if (numberOfItemsHeld < GameManager.instance.itemsHeld.Length && rewardItems.Length > 0)
        {

            for (int i = 0; i < rewardItems.Length; i++)
            {
                GameManager.instance.AddRewardItem(rewardItems[i]);
            }
        }
        if (numberOfItemsHeld + rewardItems.Length > GameManager.instance.itemsHeld.Length && rewardItems.Length > 0)
        {
            Shop.instance.promptText.text = "Your item bag is full!";
            StartCoroutine(Shop.instance.PromptCo());
        }


        if (numberOfEquipItemsHeld < GameManager.instance.equipItemsHeld.Length && rewardEquipItems.Length > 0)
        {
            for (int i = 0; i < rewardEquipItems.Length; i++)
            {
                GameManager.instance.AddRewardEquipItem(rewardEquipItems[i]);
            }
        }
        if (numberOfEquipItemsHeld + rewardEquipItems.Length > GameManager.instance.equipItemsHeld.Length && rewardEquipItems.Length > 0)
        {
            Shop.instance.promptText.text = "Your equipment bag is full!";
            StartCoroutine(Shop.instance.PromptCo());
        }

        if (numberOfItemsHeld + rewardItems.Length > GameManager.instance.itemsHeld.Length && rewardItems.Length > 0 && numberOfEquipItemsHeld + rewardEquipItems.Length > GameManager.instance.equipItemsHeld.Length && rewardEquipItems.Length > 0)
        {
            Shop.instance.promptText.text = "Your item and equipment bag is full!";
            StartCoroutine(Shop.instance.PromptCo());
        }

        rewardScreen.SetActive(false);
        StartCoroutine(closeRewardScreen());
        GameManager.instance.gameMenuOpen = false;

        if(markQuestComplete)
        {
            QuestManager.instance.MarkQuestComplete(questToMark);
        }

        if (ControlManager.instance.mobile == true)
        {
            GameMenu.instance.touchMenuButton.SetActive(true);
            GameMenu.instance.touchController.SetActive(true);
            GameMenu.instance.touchConfirmButton.SetActive(true);
        }

        AudioManager.instance.PlayBGM(FindObjectOfType<CameraController>().musicToPlay);

    }

    public IEnumerator closeRewardScreen()
    {
        yield return new WaitForSeconds(.1f);
        GameManager.instance.battleActive = false;
    }
}
