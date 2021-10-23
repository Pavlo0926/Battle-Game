using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    //Make instance of this script to be able reference from other scripts!
    public static GameManager instance;

    [Header("Initialization")]
    //Game objects used by this code
    public GameObject character0;
    public GameObject character1;
    public GameObject character2;

    //Initialize a list of all character stats
    public CharacterStatus[] characterStatus;

    [Header("Currently active menus")]
    //Bools for checking if one of these menus is currently active
    public bool cutSceneActive;
    public bool gameMenuOpen;
    public bool dialogActive;
    public bool fadingBetweenAreas;
    public bool shopActive;
    public bool battleActive;
    public bool saveMenuActive;
    public bool innActive;
    public bool itemCharChoiceMenu;
    public bool loadPromt;
    public bool quitPromt;
    public bool itemMenu;
    public bool equipMenu;
    public bool statsMenu;
    public bool skillsMenu;

    [Header("Character Bools")]
    //For checking if the player can move
    public bool confirmCanMove;

    [Header("Existing Game Items")]
    //Put items that the game uses here!
    public Item[] existingItems;

    [Header("Currently Owned Items")]
    //View items that are in your inventory Can also be used to give the player some items to start the game with
    public string[] itemsHeld;
    //public int[] numberOfItems;
    public string[] equipItemsHeld;    
    //public int[] numberOfEquipItems;
    
    [Header("Gold Settings")]
    //The amount of gold currently owned by the player. Can also be used to give the player some gold to start the game with
    public int currentGold;

	// Use this for initialization
	void Start () {
        instance = this;

        DontDestroyOnLoad(gameObject);

        SortItems();
	}
	
	// Update is called once per frame
	void Update () {

        //Check if any meu is currently open and prevent the player from moving
        if (gameMenuOpen || dialogActive || fadingBetweenAreas || shopActive || battleActive || innActive || itemCharChoiceMenu || loadPromt || quitPromt || itemMenu || equipMenu ||statsMenu)
        {
            PlayerController.instance.canMove = false;
            confirmCanMove = PlayerController.instance.canMove;
        } else
        {
            PlayerController.instance.canMove = true;
            confirmCanMove = PlayerController.instance.canMove;
        }
    }

    //Returns the details of a list of items
    public Item GetItemDetails(string itemToGrab)
    {

        for(int i = 0; i < existingItems.Length; i++)
        {
            if(existingItems[i].itemName == itemToGrab)
            {
                return existingItems[i];
            }
        }
        
        return null;
    }

    //An algorithm to sort items in a list to avoid empty spaces in the inventory
    public void SortItems()
    {
        bool itemAfterSpace = true;

        while (itemAfterSpace)
        {
            itemAfterSpace = false;
            for (int i = 0; i < itemsHeld.Length - 1; i++)
            {
                if (itemsHeld[i] == "")
                {
                    itemsHeld[i] = itemsHeld[i + 1];
                    itemsHeld[i + 1] = "";

                    //numberOfItems[i] = numberOfItems[i + 1];
                    //numberOfItems[i + 1] = 0;

                    if(itemsHeld[i] != "")
                    {
                        itemAfterSpace = true;
                    }
                }
            }
        }
    }

    //An algorithm to sort items in a list to avoid empty spaces in the inventory
    public void SortEquipItems()
    {
        bool itemAFterSpace = true;

        while (itemAFterSpace)
        {
            itemAFterSpace = false;
            for (int i = 0; i < equipItemsHeld.Length - 1; i++)
            {
                if (equipItemsHeld[i] == "")
                {
                    equipItemsHeld[i] = equipItemsHeld[i + 1];
                    equipItemsHeld[i + 1] = "";

                    //numberOfEquipItems[i] = numberOfEquipItems[i + 1];
                    //numberOfEquipItems[i + 1] = 0;

                    if (equipItemsHeld[i] != "")
                    {
                        itemAFterSpace = true;
                    }
                }
            }
        }
    }

    //A method to add items to the inventory
    public void AddItem(string itemToAdd)
    {
        int newItemPosition = 0;
        bool foundSpace = false;
        
        if (Shop.instance.selectedItem.item)
        {
            for (int i = 0; i < itemsHeld.Length; i++)
            {
                if (itemsHeld[i] == "" )//|| itemsHeld[i] == itemToAdd)
                {
                    newItemPosition = i;
                    i = itemsHeld.Length;
                    foundSpace = true;
                }
            }

            if (foundSpace)
            {
                bool itemExists = false;
                for (int i = 0; i < existingItems.Length; i++)
                {
                    if (existingItems[i].itemName == itemToAdd)
                    {
                        itemExists = true;

                        i = existingItems.Length;
                    }
                }

                if (itemExists)
                {
                    itemsHeld[newItemPosition] = itemToAdd;
                    //numberOfItems[newItemPosition]++;
                }
                else
                {
                    Debug.LogError(itemToAdd + " Does Not Exist!!");
                }
            }
        }

        if (Shop.instance.selectedItem.offense || Shop.instance.selectedItem.defense)
        {
            for (int i = 0; i < equipItemsHeld.Length; i++)
            {
                if (equipItemsHeld[i] == "" )//|| equipItemsHeld[i] == itemToAdd)
                {
                    newItemPosition = i;
                    i = equipItemsHeld.Length;
                    foundSpace = true;
                }
            }

            if (foundSpace)
            {
                bool itemExists = false;
                for (int i = 0; i < existingItems.Length; i++)
                {
                    if (existingItems[i].itemName == itemToAdd)
                    {
                        itemExists = true;

                        i = existingItems.Length;
                    }
                }

                if (itemExists)
                {
                    equipItemsHeld[newItemPosition] = itemToAdd;
                    //numberOfEquipItems[newItemPosition]++;
                }
                else
                {
                    Debug.LogError(itemToAdd + " Does Not Exist!!");
                }
            }
        }

        GameMenu.instance.ShowItems();

        GameMenu.instance.buyItemButton.interactable = true;
    }

    //A method for equipping items
    public void EquipItem(string itemToAdd)
    {
        int newItemPosition = 0;
        bool foundSpace = false;
        
            for (int i = 0; i < equipItemsHeld.Length; i++)
            {
                if (equipItemsHeld[i] == "")// || equipItemsHeld[i] == itemToAdd)
                {
                    newItemPosition = i;
                    i = equipItemsHeld.Length;
                    foundSpace = true;
                }
            }

            if (foundSpace)
            {
                bool itemExists = false;
                for (int i = 0; i < existingItems.Length; i++)
                {
                    if (existingItems[i].itemName == itemToAdd)
                    {
                        itemExists = true;

                        i = existingItems.Length;
                    }
                }

                if (itemExists)
                {
                    equipItemsHeld[newItemPosition] = itemToAdd;
                    //numberOfEquipItems[newItemPosition]++;
                }
                else
                {
                    Debug.LogError(itemToAdd + " Does Not Exist!!");
                }
            }

        GameMenu.instance.ShowItems();
        
    }

    //A method to add reward items after battle
    public void AddRewardItem(string itemToAdd)
    {
        int newItemPosition = 0;
        bool foundSpace = false;
        
            for (int i = 0; i < itemsHeld.Length; i++)
            {
                if (itemsHeld[i] == "")// || itemsHeld[i] == itemToAdd)
                {
                    newItemPosition = i;
                    i = itemsHeld.Length;
                    foundSpace = true;
                }
            }

            if (foundSpace)
            {
                bool itemExists = false;
                for (int i = 0; i < existingItems.Length; i++)
                {
                    if (existingItems[i].itemName == itemToAdd)
                    {
                        itemExists = true;

                        i = existingItems.Length;
                    }
                }

                if (itemExists)
                {
                    itemsHeld[newItemPosition] = itemToAdd;
                    //numberOfItems[newItemPosition]++;
                }
                else
                {
                    Debug.LogError(itemToAdd + " Does Not Exist!!");
                }
            }        
        
        GameMenu.instance.ShowItems();
    }

    //A method to add reward equip items after battle
    public void AddRewardEquipItem(string itemToAdd)
    {
        int newItemPosition = 0;
        bool foundSpace = false;

        for (int i = 0; i < equipItemsHeld.Length; i++)
        {
            if (equipItemsHeld[i] == "") //|| equipItemsHeld[i] == itemToAdd)
            {
                newItemPosition = i;
                i = equipItemsHeld.Length;
                foundSpace = true;
            }
        }

        if (foundSpace)
        {
            bool itemExists = false;
            for (int i = 0; i < existingItems.Length; i++)
            {
                if (existingItems[i].itemName == itemToAdd)
                {
                    itemExists = true;

                    i = existingItems.Length;
                }
            }

            if (itemExists)
            {
                equipItemsHeld[newItemPosition] = itemToAdd;
                //numberOfEquipItems[newItemPosition]++;
            }
            else
            {
                Debug.LogError(itemToAdd + " Does Not Exist!!");
            }
        }

        GameMenu.instance.ShowItems();
    }

    //A method for removing items after usage
    public void RemoveItem(string itemToRemove)
    {
        bool foundItem = false;
        int itemPosition = 0;

        if (GameMenu.instance.activeItem.item)
        {
            for (int i = 0; i < itemsHeld.Length; i++)
            {
                if (itemsHeld[i] == itemToRemove)
                {
                    foundItem = true;
                    itemPosition = i;

                    i = itemsHeld.Length;
                }
            }

            if (foundItem)
            {
                //numberOfItems[itemPosition]--;
                itemsHeld[itemPosition] = "";
                //if (numberOfItems[itemPosition] <= 0)
                //{
                    //itemsHeld[itemPosition] = "";
                //}

                GameMenu.instance.ShowItems();
                GameMenu.instance.ShowEquipItems();
            }
            else
            {

                Debug.LogError("Couldn't find " + itemToRemove);
            }
        }

        if (GameMenu.instance.activeItem.defense || GameMenu.instance.activeItem.offense)
        {
            for (int i = 0; i < equipItemsHeld.Length; i++)
            {
                if (equipItemsHeld[i] == itemToRemove)
                {
                    foundItem = true;
                    itemPosition = i;

                    i = equipItemsHeld.Length;
                }
            }

            if (foundItem)
            {
                //numberOfEquipItems[itemPosition]--;
                equipItemsHeld[itemPosition] = "";
                //if (numberOfEquipItems[itemPosition] <= 0)
                //{
                //equipItemsHeld[itemPosition] = "";
                //}

                GameMenu.instance.ShowItems();
                GameMenu.instance.ShowEquipItems();
            }
            else
            {

                Debug.LogError("Couldn't find " + itemToRemove);
            }
        }
    }

    //A method for removing a sold item
    public void RemoveSoldItem(string itemToRemove)
    {
        bool foundItem = false;
        int itemPosition = 0;

        if (Shop.instance.selectedItem.item)
        {
            for (int i = 0; i < itemsHeld.Length; i++)
            {
                if (itemsHeld[i] == itemToRemove)
                {
                    foundItem = true;
                    itemPosition = i;

                    i = itemsHeld.Length;
                }
            }

            if (foundItem)
            {
                //numberOfItems[itemPosition]--;

                //if (numberOfItems[itemPosition] <= 0)
                //{
                    itemsHeld[itemPosition] = "";
                //}

                GameMenu.instance.ShowItems();
                GameMenu.instance.ShowEquipItems();
            }
            else
            {

                Debug.LogError("Couldn't find " + itemToRemove);
            }
        }

        if (Shop.instance.selectedItem.offense || Shop.instance.selectedItem.defense)
        {
            for (int i = 0; i < equipItemsHeld.Length; i++)
            {
                if (equipItemsHeld[i] == itemToRemove)
                {
                    foundItem = true;
                    itemPosition = i;

                    i = equipItemsHeld.Length;
                }
            }

            if (foundItem)
            {
                //numberOfEquipItems[itemPosition]--;

                //if (numberOfEquipItems[itemPosition] <= 0)
                //{
                    equipItemsHeld[itemPosition] = "";
                //}

                GameMenu.instance.ShowItems();
                GameMenu.instance.ShowEquipItems();
            }
            else
            {

                Debug.LogError("Couldn't find " + itemToRemove);
            }
        }
    }

    //A system to save player status data and inventory
    public void SaveData()
    {
        //Saves current scene + player position
        PlayerPrefs.SetString("Current_Scene", SceneManager.GetActiveScene().name);
        PlayerPrefs.SetFloat("Player_Position_x", PlayerController.instance.transform.position.x);
        PlayerPrefs.SetFloat("Player_Position_y", PlayerController.instance.transform.position.y);
        PlayerPrefs.SetFloat("Player_Position_z", PlayerController.instance.transform.position.z);

        //Saves character status
        for(int i = 0; i < characterStatus.Length; i++)
        {
            if(characterStatus[i].gameObject.activeInHierarchy)
            {
                PlayerPrefs.SetInt("Player_" + characterStatus[i].characterName + "_active", 1);
            } else
            {
                PlayerPrefs.SetInt("Player_" + characterStatus[i].characterName + "_active", 0);
            }
            
            PlayerPrefs.SetInt("Player_" + characterStatus[i].characterName + "_Level", characterStatus[i].level);
            PlayerPrefs.SetInt("Player_" + characterStatus[i].characterName + "_CurrentExp", characterStatus[i].currentEXP);
            PlayerPrefs.SetInt("Player_" + characterStatus[i].characterName + "_CurrentHP", characterStatus[i].currentHP);
            PlayerPrefs.SetInt("Player_" + characterStatus[i].characterName + "_MaxHP", characterStatus[i].maxHP);
            PlayerPrefs.SetInt("Player_" + characterStatus[i].characterName + "_CurrentMP", characterStatus[i].currentSP);
            PlayerPrefs.SetInt("Player_" + characterStatus[i].characterName + "_MaxMP", characterStatus[i].maxSP);
            PlayerPrefs.SetInt("Player_" + characterStatus[i].characterName + "_Strength", characterStatus[i].strength);
            PlayerPrefs.SetInt("Player_" + characterStatus[i].characterName + "_Defence", characterStatus[i].defence);
            PlayerPrefs.SetInt("Player_" + characterStatus[i].characterName + "_WpnPwr", characterStatus[i].offenseStrength);
            PlayerPrefs.SetInt("Player_" + characterStatus[i].characterName + "_ArmrPwr", characterStatus[i].defenseStrength);
            PlayerPrefs.SetString("Player_" + characterStatus[i].characterName + "_EquippedWpn", characterStatus[i].equippedOffenseItem);
            PlayerPrefs.SetString("Player_" + characterStatus[i].characterName + "_EquippedArmr", characterStatus[i].equippedDefenseItem);
        }

        //Saves inventory
        for(int i = 0; i < itemsHeld.Length; i++)
        {
            PlayerPrefs.SetString("ItemInInventory_" + i, itemsHeld[i]);
            PlayerPrefs.SetString("EquipItemInInventory_" + i, equipItemsHeld[i]);
            //PlayerPrefs.SetInt("ItemAmount_" + i, numberOfItems[i]);
        }
    }

    //A system to load saved data
    public void LoadData()
    {
        //Load player position
        PlayerController.instance.transform.position = new Vector3(PlayerPrefs.GetFloat("Player_Position_x"), PlayerPrefs.GetFloat("Player_Position_y"), PlayerPrefs.GetFloat("Player_Position_z"));

        //Load character status
        for(int i = 0; i < characterStatus.Length; i++)
        {
            if(PlayerPrefs.GetInt("Player_" + characterStatus[i].characterName + "_active") == 0)
            {
                characterStatus[i].gameObject.SetActive(false);
            } else
            {
                characterStatus[i].gameObject.SetActive(true);
            }

            characterStatus[i].level = PlayerPrefs.GetInt("Player_" + characterStatus[i].characterName + "_Level");
            characterStatus[i].currentEXP = PlayerPrefs.GetInt("Player_" + characterStatus[i].characterName + "_CurrentExp");
            characterStatus[i].currentHP = PlayerPrefs.GetInt("Player_" + characterStatus[i].characterName + "_CurrentHP");
            characterStatus[i].maxHP = PlayerPrefs.GetInt("Player_" + characterStatus[i].characterName + "_MaxHP");
            characterStatus[i].currentSP = PlayerPrefs.GetInt("Player_" + characterStatus[i].characterName + "_CurrentMP");
            characterStatus[i].maxSP = PlayerPrefs.GetInt("Player_" + characterStatus[i].characterName + "_MaxMP");
            characterStatus[i].strength = PlayerPrefs.GetInt("Player_" + characterStatus[i].characterName + "_Strength");
            characterStatus[i].defence = PlayerPrefs.GetInt("Player_" + characterStatus[i].characterName + "_Defence");
            characterStatus[i].offenseStrength = PlayerPrefs.GetInt("Player_" + characterStatus[i].characterName + "_WpnPwr");
            characterStatus[i].defenseStrength = PlayerPrefs.GetInt("Player_" + characterStatus[i].characterName + "_ArmrPwr");
            characterStatus[i].equippedOffenseItem = PlayerPrefs.GetString("Player_" + characterStatus[i].characterName + "_EquippedWpn");
            characterStatus[i].equippedDefenseItem = PlayerPrefs.GetString("Player_" + characterStatus[i].characterName + "_EquippedArmr");
        }

        //Load inventory
        for(int i = 0; i < itemsHeld.Length; i++)
        {
            itemsHeld[i] = PlayerPrefs.GetString("ItemInInventory_" + i);
            equipItemsHeld[i] = PlayerPrefs.GetString("EquipItemInInventory_" + i);
            //numberOfItems[i] = PlayerPrefs.GetInt("ItemAmount_" + i);
        }
    }

    //Check if inventory contains a specific item
    public bool HasItem(string searchItem)
    {
        for (int i = 0; i < itemsHeld.Length - 1; i++)
        {
            if (itemsHeld[i] == searchItem)
            {
                return true;
            }
        }

        for (int i = 0; i < equipItemsHeld.Length - 1; i++)
        {
            if (equipItemsHeld[i] == searchItem)
            {
                return true;
            }
        }
 
        return false;

    }
}


