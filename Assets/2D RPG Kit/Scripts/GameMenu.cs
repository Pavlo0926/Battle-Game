using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class GameMenu : MonoBehaviour {

    //Make instance of this script to be able reference from other scripts!
    public static GameMenu instance;

    Navigation customNav = new Navigation();

    [Header("Initialization")]
    //Game objects used by this code
    public Image itemSprite;
    public Image equipItemSprite;
    public bool itemSelected;
    public GameObject touchConfirmButton;
    public GameObject touchMenuButton;
    public GameObject touchBackButton;
    public GameObject touchController;
    public GameObject menu;
    public GameObject gotItemMessage;
    public Text gotItemMessageText;
    public GameObject[] windows;
    public GameObject loadPrompt;
    public GameObject quitPrompt;
    public GameObject discardPrompt;
    public Text discardItemText;
    public GameObject itemWindow;
    public GameObject skillsWindow; 
    public GameObject skillsMenu;
    public SelectSkill[] skillButtons;
    public GameObject equipWindow;
    public GameObject statusWindow;
    public GameObject[] menuCharacterSlots;
    public GameObject[] statusMenuCharacterSlots;
    public GameObject[] itemMenuCharacterSlots;
    public GameObject[] equipMenuCharacterSlots;
    private CharacterStatus[] playerStats;
    public Text[] nameText, hpText, spText, lvlText, expText;
    public Text[] nameTextItem, hpTextItem, spTextItem, lvlTextItem; //, expTextItem;
    public Text[] nameTextStatus, hpTextStatus, spTextStatus, lvlTextStatus, expTextStatus;
    public Slider[] expSlider, expSliderItem, expSliderStatus, HPSlider, SPSlider, HPSliderItem, SPSliderItem, HPSliderStatus, SPSliderStatus;
    public Image[] charImage, charImageItem, charImageStatus;
    public GameObject[] charSkillsButtons;
    public Text[] statusStr, statusDef, statusWpnEqpd, statusWpnPwr, statusArmrEqpd, statusArmrPwr;
    public Text[] equipName, equipLvl, equipWpnEqpd, equipWpnPwr, equipArmrEqpd, equipArmrPwr;
    public ItemButton[] itemButtons;
    public ReadHilightedButton[] hilightedItemButtons;
    public ItemButton[] equipItemButtons;
    public ReadHilightedButton[] hilightedEquipItemButtons;
    public Item activeItem;
    public Text itemName, itemDescription, useButtonText, equipItemName, equipItemDescription, equipUseButtonText;
    public GameObject itemCharChoiceMenu;
    public GameObject equipCharChoiceMenu;
    public Text[] itemCharChoiceNames;
    public Text[] equipCharChoiceNames;
    public GameObject gold;
    public Text goldText;
    public GameObject loadButton;
    public GameObject itemCharChoiceButton1;
    public GameObject itemCharChoiceButton2;
    public GameObject itemCharChoiceButton3;
    public GameObject equipCharChoiceButton1;
    public GameObject equipCharChoiceButton2;
    public GameObject equipCharChoiceButton3;
    public int buttonValue;

    //Event sytsem
    public EventSystem es;

    //Game menu
    public Button btn;
    public Button item;
    public Button equip;
    public Button skills;
    public Button status;
    public Button load;
    public Button close;
    public Button quit;
    public Button useItemButton;
    public Button useEquipItemButton;
    public Button discardItemButton;
    public Button discardEquipItemButton;
    public Button itemMenuItem0;
    public Button[] itemButtonsB;
    public Button[] equipItemButtonsB;
    public Button itemUse;
    public Button equipMenuItem0;
    public Button equipUse;
    public Button selectP1;
    public Button selectEquipP1;
    public Button selectP1Skill;
    public Button no;
    public Button quitNo;
    public Button discardNo;

    //Save point
    public Button saveButton;
    public Button closeButtonSave;
    
    //Inn
    public Button stayButton;
    public Button exitInnButton;
    public Button noInnButton;

    //Shop
    public Button buyButton;
    public Button[] buyItemButtons;
    public Button buyItemButton;
    public Button sellItemButton;
    public Button[] sellItemButtons;
    public Button sellEquipItemButton;
    public Button[] sellEquipItemButtons;
    public Button exitShopButton;
    public Button buyMenuItem0;
    public Button yesButtonBuy;
    public Button noButtonBuy;
    public Button sellMenuItem0;
    public Button sellMenuEquipItem0;
    public Button yesButtonSell;
    public Button yesButtonSellEqipItem;

    [Header("Item Selection")]
    public bool itemConfirmed = false;
    public string selectedItem;
    
    [Header("Linked Scenes")]
    public string mainMenuScene;
    public string loadGameScene;

    [Header("Sound")]
    public int openMenuButtonSound;
    public int cancelButtonSound;

    // Use this for initialization
    void Start () {

        instance = this;
        
    }
	
	// Update is called once per frame
	void Update () {

        //Open game menu
        if (Input.GetButtonDown("RPGMenuPC") || Input.GetButtonDown("RPGMenuJoy"))
        {
            if (!menu.activeInHierarchy)
            {
                AudioManager.instance.PlaySFX(openMenuButtonSound);
            }
            //Check if game menu can be opened. For example the game menu should not open during dialog or battle
            if (ScreenFade.instance.fading == false && !GameManager.instance.battleActive && !GameManager.instance.dialogActive && !GameManager.instance.shopActive && !GameManager.instance.innActive && !GameManager.instance.saveMenuActive && !GameManager.instance.cutSceneActive)
            {
                //Prevents higlighted button from going back to btn = btn when pressing RPGmenuPC or RPGMenuJoy button during open game menu 
                if (!menu.activeInHierarchy)
                {
                    if (ControlManager.instance.mobile == false)
                    {
                        btn = item;
                        SelectFirstButton();
                        
                    }

                    if (ControlManager.instance.mobile == true)
                    {
                        EventSystem.current.SetSelectedGameObject(null);
                    }
                }
                    menu.SetActive(true);
                    UpdateMainStats();
                    GameManager.instance.gameMenuOpen = true;

                if (PlayerPrefs.HasKey("Current_Scene"))
                {
                    load.interactable = true;

                }
                else
                {
                    load.interactable = false;
                }

            }
        }
        //Close game menu
        if (Input.GetButtonDown("RPGCanclePC") || Input.GetButtonDown("RPGCancleJoy"))
        {
            if (GameManager.instance.gameMenuOpen)
            {
                AudioManager.instance.PlaySFX(cancelButtonSound);
            }

            //Close menu if now other window is active
            if(!itemWindow.activeInHierarchy && !equipWindow.activeInHierarchy && !skillsWindow.activeInHierarchy && !statusWindow.activeInHierarchy && !loadPrompt.activeInHierarchy && !quitPrompt.activeInHierarchy)
            {
                if (!Save.instance.saveMenu.activeInHierarchy)
                {
                    CloseMenu();
                }
                
            }

            //Quit prompt
            if (quitPrompt.activeInHierarchy)
            {
                quitPrompt.SetActive(false);

                //Set disabled buttons back to interactable
                item.interactable = true;
                equip.interactable = true;
                skills.interactable = true;
                status.interactable = true;
                load.interactable = true;
                close.interactable = true;
                quit.interactable = true;

                if (ControlManager.instance.mobile == false)
                {
                    btn = item;
                    SelectFirstButton();

                }

                if (ControlManager.instance.mobile == true)
                {
                    EventSystem.current.SetSelectedGameObject(null);
                }
            }

            //Load prompt
            if (loadPrompt.activeInHierarchy)
            {
                loadPrompt.SetActive(false);

                //Set disabled buttons back to interactable
                item.interactable = true;
                equip.interactable = true;
                skills.interactable = true;
                status.interactable = true;
                load.interactable = true;
                close.interactable = true;
                quit.interactable = true;

                if (ControlManager.instance.mobile == false)
                {
                    btn = item;
                    SelectFirstButton();

                }

                if (ControlManager.instance.mobile == true)
                {
                    EventSystem.current.SetSelectedGameObject(null);
                }
            }

            //Status menu
            if (statusWindow.activeInHierarchy)
            {
                statusWindow.SetActive(false);

                if (ControlManager.instance.mobile == false)
                {
                    btn = item;
                    SelectFirstButton();

                }

                if (ControlManager.instance.mobile == true)
                {
                    EventSystem.current.SetSelectedGameObject(null);
                }
            }

            //Skill menu
            if (skillsWindow.activeInHierarchy)
            {
                skillsMenu.SetActive(false);
                skillsWindow.SetActive(false);

                if (ControlManager.instance.mobile == false)
                {
                    btn = item;
                    SelectFirstButton();

                }

                if (ControlManager.instance.mobile == true)
                {
                    EventSystem.current.SetSelectedGameObject(null);
                }
            }

            //Item window
            if (itemWindow.activeInHierarchy && !useItemButton.interactable && !itemCharChoiceMenu.activeInHierarchy)
            {
                itemWindow.SetActive(false);

                //Set disabled buttons back to interactable
                item.interactable = true;
                equip.interactable = true;
                skills.interactable = true;
                status.interactable = true;
                load.interactable = true;
                close.interactable = true;
                quit.interactable = true;

                if (ControlManager.instance.mobile == false)
                {
                    btn = item;
                    SelectFirstButton();

                }

                if (ControlManager.instance.mobile == true)
                {
                    EventSystem.current.SetSelectedGameObject(null);
                }
            }

            //Equip window
            if (equipWindow.activeInHierarchy && !useEquipItemButton.interactable && !equipCharChoiceMenu.activeInHierarchy)
            {
                equipWindow.SetActive(false);

                //Set disabled buttons back to interactable
                item.interactable = true;
                equip.interactable = true;
                skills.interactable = true;
                status.interactable = true;
                load.interactable = true;
                close.interactable = true;
                quit.interactable = true;

                if (ControlManager.instance.mobile == false)
                {
                    btn = item;
                    SelectFirstButton();

                }

                if (ControlManager.instance.mobile == true)
                {
                    EventSystem.current.SetSelectedGameObject(null);
                }
            }

            //Close item character choice menu
            if (itemCharChoiceMenu.activeInHierarchy)
            {
                CloseItemCharChoice();

                for (int i = 0; i < itemButtonsB.Length; i++)
                {
                    itemButtonsB[i].interactable = true;
                }

                for (int i = 0; i < itemButtonsB.Length; i++)
                {
                    //Set navigation mode of non-disabled buttons to automatic in order to avoid navigating into disabled buttons
                    if (GameManager.instance.itemsHeld[i] != "")
                    {
                        itemButtonsB[i].navigation = customNav;

                    }

                    //Make only those item buttons interactable which actually hold items 
                    if (GameManager.instance.itemsHeld[i] == "")
                    {
                        itemButtonsB[i].interactable = false;
                    }
                }

                OpenItemWindow();
                
            }

            //Close item action buttons
            if (itemWindow.activeInHierarchy && useItemButton.interactable)
            {
                for (int i = 0; i < itemButtonsB.Length; i++)
                {
                    itemButtonsB[i].interactable = true;
                }

                for (int i = 0; i < itemButtonsB.Length; i++)
                {
                    //Set navigation mode of non-disabled buttons to automatic in order to avoid navigating into disabled buttons
                    if (GameManager.instance.itemsHeld[i] != "")
                    {
                        itemButtonsB[i].navigation = customNav;

                    }

                    //Make only those item buttons interactable which actually hold items 
                    if (GameManager.instance.itemsHeld[i] == "")
                    {
                        itemButtonsB[i].interactable = false;
                    }
                }

                OpenItemWindow();
            }

            //Close equip action buttons
            if (equipWindow.activeInHierarchy && useEquipItemButton.interactable)
            {
                for (int i = 0; i < equipItemButtonsB.Length; i++)
                {
                    equipItemButtonsB[i].interactable = true;
                }

                for (int i = 0; i < equipItemButtonsB.Length; i++)
                {
                    //Set navigation mode of non-disabled buttons to automatic in order to avoid navigating into disabled buttons
                    if (GameManager.instance.equipItemsHeld[i] != "")
                    {
                        equipItemButtonsB[i].navigation = customNav;

                    }

                    //Make only those item buttons interactable which actually hold items 
                    if (GameManager.instance.equipItemsHeld[i] == "")
                    {
                        equipItemButtonsB[i].interactable = false;
                    }
                }

                OpenEquipWindow();
            }

            //Close equip character choice menu
            if (equipCharChoiceMenu.activeInHierarchy)
            {
                CloseEquipCharChoice();

                for (int i = 0; i < equipItemButtonsB.Length; i++)
                {
                    equipItemButtonsB[i].interactable = true;
                }

                for (int i = 0; i < itemButtonsB.Length; i++)
                {
                    //Set navigation mode of non-disabled buttons to automatic in order to avoid navigating into disabled buttons
                    if (GameManager.instance.equipItemsHeld[i] != "")
                    {
                        equipItemButtonsB[i].navigation = customNav;

                    }

                    //Make only those item buttons interactable which actually hold items 
                    if (GameManager.instance.equipItemsHeld[i] == "")
                    {
                        equipItemButtonsB[i].interactable = false;
                    }
                }

                OpenEquipWindow();
            }
            
            //Close discard prompt
            if (discardPrompt.activeInHierarchy)
            {
                if (itemWindow.activeInHierarchy)
                {
                    CloseDiscardPrompt();
                    OpenItemWindow();
                }

                if (equipWindow.activeInHierarchy)
                {
                    CloseDiscardPrompt();
                    OpenEquipWindow();
                }
            }
            
        }
	}

    IEnumerator WaitFrameCo()
    {
        yield return new WaitForSeconds(2);
    }

    public void OpenGameMenu()
    {
        if (ControlManager.instance.mobile == true)
        {
            EventSystem.current.SetSelectedGameObject(null);
        }

        touchBackButton.SetActive(true);

        menu.SetActive(true);
        UpdateMainStats();
        GameManager.instance.gameMenuOpen = true;
    }

    public void BackButton()
    {
        //Close menu if now other window is active
        if (!itemWindow.activeInHierarchy && !equipWindow.activeInHierarchy && !skillsWindow.activeInHierarchy && !statusWindow.activeInHierarchy && !loadPrompt.activeInHierarchy && !quitPrompt.activeInHierarchy)
        {
            CloseMenu();
        }

        //Quit prompt
        if (quitPrompt.activeInHierarchy)
        {
            quitPrompt.SetActive(false);

            //Set disabled buttons back to interactable
            item.interactable = true;
            equip.interactable = true;
            skills.interactable = true;
            status.interactable = true;
            load.interactable = true;
            close.interactable = true;
            quit.interactable = true;

            if (ControlManager.instance.mobile == false)
            {
                btn = item;
                SelectFirstButton();

            }

            if (ControlManager.instance.mobile == true)
            {
                EventSystem.current.SetSelectedGameObject(null);
            }
        }

        //Load prompt
        if (loadPrompt.activeInHierarchy)
        {
            loadPrompt.SetActive(false);

            //Set disabled buttons back to interactable
            item.interactable = true;
            equip.interactable = true;
            skills.interactable = true;
            status.interactable = true;
            load.interactable = true;
            close.interactable = true;
            quit.interactable = true;

            if (ControlManager.instance.mobile == false)
            {
                btn = item;
                SelectFirstButton();

            }

            if (ControlManager.instance.mobile == true)
            {
                EventSystem.current.SetSelectedGameObject(null);
            }
        }

        //Status menu
        if (statusWindow.activeInHierarchy)
        {
            statusWindow.SetActive(false);

            if (ControlManager.instance.mobile == false)
            {
                btn = item;
                SelectFirstButton();

            }

            if (ControlManager.instance.mobile == true)
            {
                EventSystem.current.SetSelectedGameObject(null);
            }
        }

        //Skill menu
        if (skillsWindow.activeInHierarchy)
        {
            skillsMenu.SetActive(false);
            skillsWindow.SetActive(false);

            if (ControlManager.instance.mobile == false)
            {
                btn = item;
                SelectFirstButton();

            }

            if (ControlManager.instance.mobile == true)
            {
                EventSystem.current.SetSelectedGameObject(null);
            }
        }

        //Item window
        if (itemWindow.activeInHierarchy && !useItemButton.interactable && !itemCharChoiceMenu.activeInHierarchy)
        {
            itemWindow.SetActive(false);

            //Set disabled buttons back to interactable
            item.interactable = true;
            equip.interactable = true;
            skills.interactable = true;
            status.interactable = true;
            load.interactable = true;
            close.interactable = true;
            quit.interactable = true;

            if (ControlManager.instance.mobile == false)
            {
                btn = item;
                SelectFirstButton();

            }

            if (ControlManager.instance.mobile == true)
            {
                EventSystem.current.SetSelectedGameObject(null);
            }
        }

        //Equip window
        if (equipWindow.activeInHierarchy && !useEquipItemButton.interactable && !equipCharChoiceMenu.activeInHierarchy)
        {
            equipWindow.SetActive(false);

            //Set disabled buttons back to interactable
            item.interactable = true;
            equip.interactable = true;
            skills.interactable = true;
            status.interactable = true;
            load.interactable = true;
            close.interactable = true;
            quit.interactable = true;

            if (ControlManager.instance.mobile == false)
            {
                btn = item;
                SelectFirstButton();

            }

            if (ControlManager.instance.mobile == true)
            {
                EventSystem.current.SetSelectedGameObject(null);
            }
        }

        //Close item character choice menu
        if (itemCharChoiceMenu.activeInHierarchy)
        {
            CloseItemCharChoice();

            for (int i = 0; i < itemButtonsB.Length; i++)
            {
                itemButtonsB[i].interactable = true;
            }

            for (int i = 0; i < itemButtonsB.Length; i++)
            {
                //Set navigation mode of non-disabled buttons to automatic in order to avoid navigating into disabled buttons
                if (GameManager.instance.itemsHeld[i] != "")
                {
                    itemButtonsB[i].navigation = customNav;

                }

                //Make only those item buttons interactable which actually hold items 
                if (GameManager.instance.itemsHeld[i] == "")
                {
                    itemButtonsB[i].interactable = false;
                }
            }

            OpenItemWindow();

        }

        //Close item action buttons
        if (itemWindow.activeInHierarchy && useItemButton.interactable)
        {
            itemWindow.SetActive(false);

            //Set disabled buttons back to interactable
            item.interactable = true;
            equip.interactable = true;
            skills.interactable = true;
            status.interactable = true;
            load.interactable = true;
            close.interactable = true;
            quit.interactable = true;

            if (ControlManager.instance.mobile == false)
            {
                btn = item;
                SelectFirstButton();

            }

            if (ControlManager.instance.mobile == true)
            {
                EventSystem.current.SetSelectedGameObject(null);
            }
        }

        //Close equip action buttons
        if (equipWindow.activeInHierarchy && useEquipItemButton.interactable)
        {
            equipWindow.SetActive(false);

            //Set disabled buttons back to interactable
            item.interactable = true;
            equip.interactable = true;
            skills.interactable = true;
            status.interactable = true;
            load.interactable = true;
            close.interactable = true;
            quit.interactable = true;

            if (ControlManager.instance.mobile == false)
            {
                btn = item;
                SelectFirstButton();

            }

            if (ControlManager.instance.mobile == true)
            {
                EventSystem.current.SetSelectedGameObject(null);
            }
        }

        //Close equip character choice menu
        if (equipCharChoiceMenu.activeInHierarchy)
        {
            CloseEquipCharChoice();

            for (int i = 0; i < equipItemButtonsB.Length; i++)
            {
                equipItemButtonsB[i].interactable = true;
            }

            for (int i = 0; i < itemButtonsB.Length; i++)
            {
                //Set navigation mode of non-disabled buttons to automatic in order to avoid navigating into disabled buttons
                if (GameManager.instance.equipItemsHeld[i] != "")
                {
                    equipItemButtonsB[i].navigation = customNav;

                }

                //Make only those item buttons interactable which actually hold items 
                if (GameManager.instance.equipItemsHeld[i] == "")
                {
                    equipItemButtonsB[i].interactable = false;
                }
            }

            OpenEquipWindow();
        }

        //Close discard prompt
        if (discardPrompt.activeInHierarchy)
        {
            if (itemWindow.activeInHierarchy)
            {
                CloseDiscardPrompt();
                OpenItemWindow();
            }

            if (equipWindow.activeInHierarchy)
            {
                CloseDiscardPrompt();
                OpenEquipWindow();
            }
        }
    }

    public void ItemConfirmed(bool callItemCondirmed)
    {
           itemConfirmed = callItemCondirmed;   
    }

    public void SelectFirstButton()
    {
        
            es.SetSelectedGameObject(btn.gameObject);
            // Select the button
            btn.Select();
            // Highlight the button
            btn.OnSelect(null);
        
    }

    public void OpenItemWindow()
    {
        buttonValue = 0;

        //Set button navigation mode to automatic
        customNav.mode = Navigation.Mode.Automatic;

        item.interactable = false;
        equip.interactable = false;
        skills.interactable = false;
        status.interactable = false;
        load.interactable = false;
        close.interactable = false;
        quit.interactable = false;

        if (!ControlManager.instance.mobile)
        {
            useItemButton.interactable = false;
            discardItemButton.interactable = false;
        }
        

        for (int i = 0; i < itemButtonsB.Length; i++)
        {
            itemButtonsB[i].interactable = true;
        }

        for (int i = 0; i < itemButtonsB.Length; i++)
        {
            //Set navigation mode of non-disabled buttons to automatic in order to avoid navigating into disabled buttons
            if (GameManager.instance.itemsHeld[i] != "")
            {
                itemButtonsB[i].navigation = customNav;

            }

            //Make only those item buttons interactable which actually hold items 
            if (GameManager.instance.itemsHeld[i] == "")
            {
                itemButtonsB[i].interactable = false;
            }
        }

        itemWindow.SetActive(true);
        GameManager.instance.itemMenu = true;
        ShowItems();

        EventSystem.current.SetSelectedGameObject(null);
            btn = itemMenuItem0;
            SelectFirstButton();
        
    }

    public void UpdateMainStats()
    {
        playerStats = GameManager.instance.characterStatus;

        for(int i = 0; i < playerStats.Length; i++)
        {
            if(playerStats[i].gameObject.activeInHierarchy)
            {
                menuCharacterSlots[i].SetActive(true);
                itemMenuCharacterSlots[i].SetActive(true);
                equipMenuCharacterSlots[i].SetActive(true);

                //Update main menu info
                nameText[i].text = playerStats[i].characterName;
                hpText[i].text = playerStats[i].currentHP + " / " + playerStats[i].maxHP;
                HPSlider[i].maxValue = playerStats[i].maxHP;
                HPSlider[i].value = playerStats[i].currentHP;
                spText[i].text = playerStats[i].currentSP + " / " + playerStats[i].maxSP;
                SPSlider[i].maxValue = playerStats[i].maxSP;
                SPSlider[i].value = playerStats[i].currentSP;
                lvlText[i].text = "" + playerStats[i].level;
                expText[i].text = "" + playerStats[i].currentEXP + " / " + playerStats[i].eXPToNextLevel[playerStats[i].level];
                expSlider[i].maxValue = playerStats[i].eXPToNextLevel[playerStats[i].level];
                expSlider[i].value = playerStats[i].currentEXP;
                charImage[i].sprite = playerStats[i].characterIamge;

                //Update item menu info
                nameTextItem[i].text = playerStats[i].characterName;
                hpTextItem[i].text = playerStats[i].currentHP + " / " + playerStats[i].maxHP;
                HPSliderItem[i].maxValue = playerStats[i].maxHP;
                HPSliderItem[i].value = playerStats[i].currentHP;
                spTextItem[i].text = playerStats[i].currentSP + " / " + playerStats[i].maxSP;
                SPSliderItem[i].maxValue = playerStats[i].maxSP;
                SPSliderItem[i].value = playerStats[i].currentSP;
                lvlTextItem[i].text = "" + playerStats[i].level;

                //expTextItem[i].text = "" + playerStats[i].currentEXP + " / " + playerStats[i].expToNextLevel[playerStats[i].level];
                expSliderItem[i].maxValue = playerStats[i].eXPToNextLevel[playerStats[i].level];
                expSliderItem[i].value = playerStats[i].currentEXP;
                charImageItem[i].sprite = playerStats[i].characterIamge;

                //Update status menu info
                nameTextStatus[i].text = playerStats[i].characterName;
                hpTextStatus[i].text = playerStats[i].currentHP + " / " + playerStats[i].maxHP;
                HPSliderStatus[i].maxValue = playerStats[i].maxHP;
                HPSliderStatus[i].value = playerStats[i].currentHP;
                spTextStatus[i].text = playerStats[i].currentSP + " / " + playerStats[i].maxSP;
                SPSliderStatus[i].maxValue = playerStats[i].maxSP;
                SPSliderStatus[i].value = playerStats[i].currentSP;
                lvlTextStatus[i].text = "" + playerStats[i].level;
                expTextStatus[i].text = "EXP: " + playerStats[i].currentEXP + "  Next: " + playerStats[i].eXPToNextLevel[playerStats[i].level];
                expSliderStatus[i].maxValue = playerStats[i].eXPToNextLevel[playerStats[i].level];
                expSliderStatus[i].value = playerStats[i].currentEXP;
                charImageStatus[i].sprite = playerStats[i].characterIamge;

                equipName[i].text = playerStats[i].characterName;
                equipLvl[i].text = "" + playerStats[i].level;

            } else
            {
                menuCharacterSlots[i].SetActive(false);
            }
        }

        goldText.text = GameManager.instance.currentGold.ToString() + "G";
    }

    public void CloseMenu()
    {
        itemWindow.SetActive(false);
        equipWindow.SetActive(false);
        statusWindow.SetActive(false);
        loadPrompt.SetActive(false);
        quitPrompt.SetActive(false);
        menu.SetActive(false);
        itemCharChoiceMenu.SetActive(false);

        GameManager.instance.itemCharChoiceMenu = false;
        GameManager.instance.loadPromt = false;
        GameManager.instance.quitPromt = false;
        GameManager.instance.gameMenuOpen = false;
        GameManager.instance.itemMenu = false;
        GameManager.instance.equipMenu = false;
        GameManager.instance.statsMenu = false;

        touchBackButton.SetActive(false);
    }

    public void OpenSkills()
    {
        skillsWindow.SetActive(true);
        GameManager.instance.skillsMenu = true;
        UpdateMainStats();

        if (ControlManager.instance.mobile == true)
        {
            EventSystem.current.SetSelectedGameObject(null);
        }else
        {
            btn = selectP1Skill;
            SelectFirstButton();
        }
                             
        //update the information that is shown
        StatusChar();

        for (int i = 0; i < charSkillsButtons.Length; i++)
        {
            charSkillsButtons[i].SetActive(playerStats[i].gameObject.activeInHierarchy);
            charSkillsButtons[i].GetComponentInChildren<Text>().text = playerStats[i].characterName;
        }
    }

    public void ShowSkills(int charNumber)
    {
        skillsMenu.SetActive(true);

        for (int i = 0; i < skillButtons.Length; i++)
        {
            if (playerStats[charNumber].skills.Length > i)
            {
                skillButtons[i].gameObject.SetActive(true);

                skillButtons[i].skill = playerStats[charNumber].skills[i];
                skillButtons[i].nameText.text = skillButtons[i].skill;

                for (int j = 0; j < BattleManager.instance.skillList.Length; j++)
                {
                    if (BattleManager.instance.skillList[j].skillName == skillButtons[i].skill)
                    {
                        skillButtons[i].skillCost = BattleManager.instance.skillList[j].skillCost;
                        skillButtons[i].costText.text = skillButtons[i].skillCost.ToString();
                    }
                }
            }
            else
            {
                skillButtons[i].gameObject.SetActive(false);
            }
        }
    }

    public void OpenStatus()
    {
        statusWindow.SetActive(true);
        GameManager.instance.statsMenu = true;
        UpdateMainStats();
        
        //update the information that is shown
        StatusChar();
        
    }

    public void StatusChar()
    {
        playerStats = GameManager.instance.characterStatus;

        for (int i = 0; i < playerStats.Length; i++)
        {
            if (playerStats[i].gameObject.activeInHierarchy)
            {
                statusMenuCharacterSlots[i].SetActive(true);
                
                statusStr[i].text = playerStats[i].strength.ToString();
                statusDef[i].text = playerStats[i].defence.ToString();
                     if(playerStats[i].equippedOffenseItem != "")
                     {
                        statusWpnEqpd[i].text = playerStats[i].equippedOffenseItem;
                     }
                statusWpnPwr[i].text = playerStats[i].offenseStrength.ToString();
                     if (playerStats[i].equippedDefenseItem != "")
                     {
                        statusArmrEqpd[i].text = playerStats[i].equippedDefenseItem;
                     }
                 statusArmrPwr[i].text = playerStats[i].defenseStrength.ToString();
            }
            else
            {
                statusMenuCharacterSlots[i].SetActive(false);
            }
        }

    }

    public void ShowItems()
    {
        GameManager.instance.SortItems();

        for(int i = 0; i < itemButtons.Length; i++)
        {            
            itemButtons[i].buttonValue = i;
            hilightedItemButtons[i].buttonValue = i;

            if(GameManager.instance.itemsHeld[i] != "")
            {
                //itemButtons[i].buttonImage.gameObject.SetActive(true);
                //itemButtons[i].buttonImage.sprite = GameManager.instance.GetItemDetails(GameManager.instance.itemsHeld[i]).itemSprite;
                //itemButtons[i].amountText.text = GameManager.instance.numberOfItems[i].ToString();
                itemButtons[i].amountText.text = GameManager.instance.GetItemDetails(GameManager.instance.itemsHeld[i]).itemName;
            } else
            {
                //itemButtons[i].buttonImage.gameObject.SetActive(false);
                itemButtons[i].amountText.text = "";
            }
        }
    }

    public void UpdateItemDetailsOnHilighted(Item newItem)
    {
        activeItem = newItem;
        itemName.text = activeItem.itemName;
        itemDescription.text = activeItem.description;
        itemSprite.sprite = activeItem.itemSprite;
    }

    public void UpdateNoHilightedButton()
    {
        activeItem = null;
        SelectItem(activeItem);
        
    }

    public void SelectItem(Item newItem)
    {
        if (newItem != null)
        { //If any item was selected, show proper description and item sprite
            itemSelected = true;
            activeItem = newItem;
            if (activeItem.item)
            {
                useButtonText.text = "Use";
            }
            if (activeItem.offense || activeItem.defense)
            {
                useButtonText.text = "Equip";
            }

            itemName.text = activeItem.itemName;
            itemDescription.text = activeItem.description;
            //Set item sprite to visible
            itemSprite.color = new Color(1, 1, 1, 1);
            itemSprite.sprite = activeItem.itemSprite;
        }
        else
        { //If no item is selected/item inventory empty don't show anything in description panel
            itemSelected = false;
            activeItem = null;
            itemName.text = "";
            itemDescription.text = "";
            //Set item sprite to invisible
            itemSprite.color = new Color(1, 1, 1, 0);
        }
    }

    public void DiscardItem()
    {
        for (int i = 0; i < itemButtonsB.Length; i++)
        {
            itemButtonsB[i].interactable = true;
        }

        for (int i = 0; i < equipItemButtonsB.Length; i++)
        {
            equipItemButtonsB[i].interactable = true;
        }

        buttonValue = 0;

        //Set button navigation mode to automatic
        customNav.mode = Navigation.Mode.Automatic;

        item.interactable = false;
        equip.interactable = false;
        skills.interactable = false;
        status.interactable = false;
        load.interactable = false;
        close.interactable = false;
        quit.interactable = false;

        if (!ControlManager.instance.mobile)
        {
            useItemButton.interactable = false;
            discardItemButton.interactable = false;
        }
        

        if (itemWindow.activeInHierarchy)
        {
            for (int i = 0; i < itemButtonsB.Length; i++)
            {
                //Set navigation mode of non-disabled buttons to automatic in order to avoid navigating into disabled buttons
                if (GameManager.instance.itemsHeld[i] != "")
                {
                    itemButtonsB[i].navigation = customNav;

                }

                //Make only those item buttons interactable which actually hold items 
                if (GameManager.instance.itemsHeld[i] == "")
                {
                    itemButtonsB[i].interactable = false;
                }
            }
        }
        
        if (equipWindow.activeInHierarchy)
        {
            for (int i = 0; i < equipItemButtonsB.Length; i++)
            {
                //Set navigation mode of non-disabled buttons to automatic in order to avoid navigating into disabled buttons
                if (GameManager.instance.equipItemsHeld[i] != "")
                {
                    equipItemButtonsB[i].navigation = customNav;

                }

                //Make only those item buttons interactable which actually hold items 
                if (GameManager.instance.equipItemsHeld[i] == "")
                {
                    equipItemButtonsB[i].interactable = false;

                }
            }
        }
        
        discardPrompt.SetActive(false);
        
        if (activeItem != null)
        {
            GameManager.instance.RemoveItem(activeItem.itemName);
            if (itemWindow.activeInHierarchy)
            {
                    btn = itemMenuItem0;
                    SelectFirstButton();
                
            }
            if (equipWindow.activeInHierarchy)
            {
                    btn = equipMenuItem0;
                    SelectFirstButton();
                
            }

            if (!GameManager.instance.HasItem(activeItem.itemName))
            {
                SelectItem(null);
            }            
        }
        if (itemWindow.activeInHierarchy)
        {
            OpenItemWindow();
        }

        if (equipWindow.activeInHierarchy)
        {
            OpenEquipWindow();
        }
        
    }

    public void OpenItemCharChoice()
    {
        if (activeItem != null)
        {
            //disable every item button except for selected item button
            for (int i = 0; i < itemButtonsB.Length; i++)
            {
                if (i != buttonValue)
                {
                    itemButtonsB[i].interactable = false;
                }
                    
            }

            for (int i = 0; i < equipItemButtonsB.Length; i++)
            {
                equipItemButtonsB[i].interactable = false;
            }
        }


        if (!ControlManager.instance.mobile)
        {
            useItemButton.interactable = false;
            discardItemButton.interactable = false;
        }
        

        if (activeItem != null)
        {
            itemCharChoiceMenu.SetActive(true);
            GameManager.instance.itemCharChoiceMenu = true;

            if (ControlManager.instance.mobile == false)
            {
                btn = selectP1;
                SelectFirstButton();
            }

            if (ControlManager.instance.mobile == true)
            {
                EventSystem.current.SetSelectedGameObject(null);
            }

            for (int i = 0; i < itemCharChoiceNames.Length; i++)
            {
                itemCharChoiceNames[i].text = GameManager.instance.characterStatus[i].characterName;
                itemCharChoiceNames[i].transform.parent.gameObject.SetActive(GameManager.instance.characterStatus[i].gameObject.activeInHierarchy);
            }

            itemConfirmed = false;
        }
        
    }

    public void OpenEquipCharChoice()
    {
        if (!ControlManager.instance.mobile)
        {
            useEquipItemButton.interactable = false;
            discardEquipItemButton.interactable = false;
        }
        

        if (activeItem != null)
        {
            for (int i = 0; i < itemButtonsB.Length; i++)
            {
                itemButtonsB[i].interactable = false;
            }

            //disable every item button except for selected item button
            for (int i = 0; i < equipItemButtonsB.Length; i++)
            {
                if (i != buttonValue)
                {
                    equipItemButtonsB[i].interactable = false;
                }

            }
            
            
        }

        if (activeItem != null)
        {
            equipCharChoiceMenu.SetActive(true);
            GameManager.instance.itemCharChoiceMenu = true;

            if (ControlManager.instance.mobile == false)
            {
                btn = selectEquipP1;
                SelectFirstButton();
            }

            if (ControlManager.instance.mobile == true)
            {
                EventSystem.current.SetSelectedGameObject(null);
            }

            for (int i = 0; i < itemCharChoiceNames.Length; i++)
            {
                equipCharChoiceNames[i].text = GameManager.instance.characterStatus[i].characterName;
                equipCharChoiceNames[i].transform.parent.gameObject.SetActive(GameManager.instance.characterStatus[i].gameObject.activeInHierarchy);
            }

            itemConfirmed = false;
        }

    }

    public void CloseItemCharChoice()
    {
        itemCharChoiceMenu.SetActive(false);
        itemCharChoiceButton1.SetActive(false);
        itemCharChoiceButton2.SetActive(false);
        itemCharChoiceButton3.SetActive(false);
    }

    public void CloseEquipCharChoice()
    {
        equipCharChoiceMenu.SetActive(false);
        equipCharChoiceButton1.SetActive(false);
        equipCharChoiceButton2.SetActive(false);
        equipCharChoiceButton3.SetActive(false);

        btn = equipMenuItem0;
        SelectFirstButton();
    }

    public void UseItem(int selectChar)
    {
        
        
        
        activeItem.Use(selectChar);
        

    }

    public void CompleteUseItem()
    {
        for (int i = 0; i < itemButtonsB.Length; i++)
        {
            itemButtonsB[i].interactable = true;
        }
        for (int i = 0; i < equipItemButtonsB.Length; i++)
        {
            equipItemButtonsB[i].interactable = true;
        }

        CloseItemCharChoice();
        CloseEquipCharChoice();

        UpdateMainStats();
        EquipStatsChar();
        itemConfirmed = false;

        if (itemWindow.activeInHierarchy)
        {
            OpenItemWindow();
        }

        if (equipWindow.activeInHierarchy)
        {
            OpenEquipWindow();
        }


        if (itemWindow.activeInHierarchy)
        {
            btn = itemMenuItem0;
            SelectFirstButton();
        }

        if (equipWindow.activeInHierarchy)
        {
            btn = equipMenuItem0;
            SelectFirstButton();
        }
        if (!GameManager.instance.HasItem(activeItem.itemName))
        {
            SelectItem(null);
        }
    }

    public void SaveGame()
    {
        GameManager.instance.SaveData();
        QuestManager.instance.SaveQuestData();
    }

    public void PlayButtonSound(int buttonSound)
    {
        AudioManager.instance.PlaySFX(buttonSound);
    }

    public void QuitGame()
    {
        item.interactable = true;
        equip.interactable = true;
        skills.interactable = true;
        status.interactable = true;
        load.interactable = true;
        close.interactable = true;
        quit.interactable = true;
        CloseQuitPrompt();
        CloseMenu();

        //Reset all managers
        for (int i = 0; i < EventManager.instance.completedEvents.Length; i++)
        {
            EventManager.instance.completedEvents[i] = false;
        }

        for (int i = 0; i < ChestManager.instance.openedChests.Length; i++)
        {
            ChestManager.instance.openedChests[i] = false;
        }

        for (int i = 0; i < QuestManager.instance.completedQuests.Length; i++)
        {
            QuestManager.instance.completedQuests[i] = false;
        }

        SceneManager.LoadScene(mainMenuScene);
        /*
        Destroy(GameManager.instance.gameObject);
        Destroy(PlayerController.instance.gameObject);
        Destroy(AudioManager.instance.gameObject);     
        Destroy(ControlManager.instance.gameObject);
        Destroy(BattleManager.instance.gameObject);
        
        Destroy(instance.gameObject);
        Destroy(gameObject);
        */
    }

    public void Load()
    {
        item.interactable = true;
        equip.interactable = true;
        skills.interactable = true;
        status.interactable = true;
        load.interactable = true;
        close.interactable = true;
        quit.interactable = true;
        CloseMenu();
        SceneManager.LoadScene(loadGameScene);
    }

    //Ask player if they really want to load the game in game menu
    public void OpenLoadPrompt()
    {
        item.interactable = false;
        equip.interactable = false;
        skills.interactable = false;
        status.interactable = false;
        load.interactable = false;
        close.interactable = false;
        quit.interactable = false;


        loadPrompt.SetActive(true);
        GameManager.instance.loadPromt = true;

        if (ControlManager.instance.mobile == false)
        {
            btn = no;
            SelectFirstButton();
        }
        

    }

    //Close ask player if they really want to load the game in game menu
    public void CloseLoadPrompt()
    {
        item.interactable = true;
        equip.interactable = true;
        skills.interactable = true;
        status.interactable = true;
        load.interactable = true;
        close.interactable = true;
        quit.interactable = true;

        loadPrompt.SetActive(false);
        if (ControlManager.instance.mobile == false)
        {
            btn = item;
            SelectFirstButton();
        }

        if (ControlManager.instance.mobile == true)
        {
            EventSystem.current.SetSelectedGameObject(null);
        }

    }

    //Ask player if they really want to quit the game in game menu
    public void OpenQuitPrompt()
    {
        item.interactable = false;
        equip.interactable = false;
        skills.interactable = false;
        status.interactable = false;
        load.interactable = false;
        close.interactable = false;
        quit.interactable = false;

        quitPrompt.SetActive(true);
        GameManager.instance.quitPromt = true;

        if (ControlManager.instance.mobile == false)
        {
            btn = quitNo;
            SelectFirstButton();
        }

        if (ControlManager.instance.mobile == true)
        {
            EventSystem.current.SetSelectedGameObject(null);
        }

    }

    //Close ask player if they really want to quit the game in game menu
    public void CloseQuitPrompt()
    {
        item.interactable = true;
        equip.interactable = true;
        skills.interactable = true;
        status.interactable = true;
        load.interactable = true;
        close.interactable = true;
        quit.interactable = true;

        quitPrompt.SetActive(false);

        if (ControlManager.instance.mobile == false)
        {
            btn = item;
            SelectFirstButton();
        }

        if (ControlManager.instance.mobile == true)
        {
            EventSystem.current.SetSelectedGameObject(null);
        }

    }

    //Ask player if they really want to discard an item
    public void DiscardPrompt()
    {
        if (activeItem != null)
        {
            
                //disable every item button except for selected item button
                for (int i = 0; i < itemButtonsB.Length; i++)
                {
                    if (i != buttonValue)
                    {
                        itemButtonsB[i].interactable = false;
                    }

                }

                for (int i = 0; i < equipItemButtonsB.Length; i++)
                {
                    equipItemButtonsB[i].interactable = false;
                }
            

            for (int i = 0; i < equipItemButtonsB.Length; i++)
            {
                equipItemButtonsB[i].interactable = false;
            }

            discardItemText.text = "1x " + activeItem.itemName;
            discardPrompt.SetActive(true);

            if (ControlManager.instance.mobile == false)
            {
                btn = discardNo;
                SelectFirstButton();
            }

            if (ControlManager.instance.mobile == true)
            {
                EventSystem.current.SetSelectedGameObject(null);
            }
        }
        

    }

    //Close ask player if they really want to discard an item
    public void CloseDiscardPrompt()
    {
        for (int i = 0; i < itemButtonsB.Length; i++)
        {
            itemButtonsB[i].interactable = true;
        }

        for (int i = 0; i < equipItemButtonsB.Length; i++)
        {
            equipItemButtonsB[i].interactable = true;
        }

        buttonValue = 0;

        //Set button navigation mode to automatic
        customNav.mode = Navigation.Mode.Automatic;

        item.interactable = false;
        equip.interactable = false;
        skills.interactable = false;
        status.interactable = false;
        load.interactable = false;
        close.interactable = false;
        quit.interactable = false;

        if (!ControlManager.instance.mobile)
        {
            useItemButton.interactable = false;
            discardItemButton.interactable = false;
        }
        

        if (itemWindow.activeInHierarchy)
        {
            for (int i = 0; i < itemButtonsB.Length; i++)
            {
                //Set navigation mode of non-disabled buttons to automatic in order to avoid navigating into disabled buttons
                if (GameManager.instance.itemsHeld[i] != "")
                {
                    itemButtonsB[i].navigation = customNav;

                }

                //Make only those item buttons interactable which actually hold items 
                if (GameManager.instance.itemsHeld[i] == "")
                {
                    itemButtonsB[i].interactable = false;
                }
            }
        }

        if (equipWindow.activeInHierarchy)
        {
            for (int i = 0; i < equipItemButtonsB.Length; i++)
            {
                //Set navigation mode of non-disabled buttons to automatic in order to avoid navigating into disabled buttons
                if (GameManager.instance.equipItemsHeld[i] != "")
                {
                    equipItemButtonsB[i].navigation = customNav;

                }

                //Make only those item buttons interactable which actually hold items 
                if (GameManager.instance.equipItemsHeld[i] == "")
                {
                    equipItemButtonsB[i].interactable = false;

                }
            }
        }
        
        discardPrompt.SetActive(false);

        if (itemWindow.activeInHierarchy)
        {
            if (ControlManager.instance.mobile == false)
            {
                btn = itemUse;
            }

            if (ControlManager.instance.mobile == true)
            {
                btn = itemMenuItem0;
            }
            OpenItemWindow();
        }
        if (equipWindow.activeInHierarchy)
        {
            if (ControlManager.instance.mobile == false)
            {
                btn = equipUse;
            }

            if ( ControlManager.instance.mobile == true)
            {
                btn = equipMenuItem0;
            }
            OpenEquipWindow();
        }
        
            SelectFirstButton();
        
    }

    public void OpenEquipWindow()
    {
        buttonValue = 0;

        //Set button navigation mode to automatic
        customNav.mode = Navigation.Mode.Automatic;

        item.interactable = false;
        equip.interactable = false;
        skills.interactable = false;
        status.interactable = false;
        load.interactable = false;
        close.interactable = false;
        quit.interactable = false;

        if (!ControlManager.instance.mobile)
        {
            useEquipItemButton.interactable = false;
            discardEquipItemButton.interactable = false;
        }
        

        for (int i = 0; i < equipItemButtonsB.Length; i++)
        {
            equipItemButtonsB[i].interactable = true;
        }

        for (int i = 0; i < equipItemButtonsB.Length; i++)
        {
            //Set navigation mode of non-disabled buttons to automatic in order to avoid navigating into disabled buttons
            if (GameManager.instance.equipItemsHeld[i] != "")
            {
                equipItemButtonsB[i].navigation = customNav;

            }

            //Make only those item buttons interactable which actually hold items 
            if (GameManager.instance.equipItemsHeld[i] == "")
            {
                equipItemButtonsB[i].interactable = false;
                
            }
        }

        equipWindow.SetActive(true);
        GameManager.instance.equipMenu = true;
        EquipStatsChar();
        UpdateMainStats();
        ShowEquipItems();
        
            btn = equipMenuItem0;
            SelectFirstButton();      

    }

    public void EquipStatsChar()
    {
        playerStats = GameManager.instance.characterStatus;

        for (int i = 0; i < playerStats.Length; i++)
        {
            if (playerStats[i].gameObject.activeInHierarchy)
            {
                statusMenuCharacterSlots[i].SetActive(true);

                if (playerStats[i].equippedOffenseItem != "")
                {
                    equipWpnEqpd[i].text = playerStats[i].equippedOffenseItem;
                }
                equipWpnPwr[i].text = playerStats[i].offenseStrength.ToString();
                if (playerStats[i].equippedDefenseItem != "")
                {
                    equipArmrEqpd[i].text = playerStats[i].equippedDefenseItem;
                }
                equipArmrPwr[i].text = playerStats[i].defenseStrength.ToString();

            }
            else
            {
                statusMenuCharacterSlots[i].SetActive(false);
            }
        }

    }

    public void ShowEquipItems()
    {
        
        GameManager.instance.SortEquipItems();

        for (int i = 0; i < equipItemButtons.Length; i++)
        {
            equipItemButtons[i].buttonValue = i;
            hilightedEquipItemButtons[i].buttonValue = i; 

            if (GameManager.instance.equipItemsHeld[i] != "")
            {
                //equipItemButtons[i].buttonImage.gameObject.SetActive(true);
                //equipItemButtons[i].buttonImage.sprite = GameManager.instance.GetItemDetails(GameManager.instance.equipItemsHeld[i]).itemSprite;
                equipItemButtons[i].amountText.text = GameManager.instance.GetItemDetails(GameManager.instance.equipItemsHeld[i]).itemName;
                
            }
            else
            {
                //equipItemButtons[i].buttonImage.gameObject.SetActive(false);
                equipItemButtons[i].amountText.text = "";
                //equipItemSprite.color = new Color(1, 1, 1, 0);
            }

        }
    }

    public void SelectEquipItem(Item newItem)
    {
        if (newItem != null)
        {
            activeItem = newItem;
            if (activeItem.item)
            {
                equipUseButtonText.text = "Use";
            }
            if (activeItem.offense || activeItem.defense)
            {
                equipUseButtonText.text = "Equip";
            }

            equipItemName.text = activeItem.itemName;
            equipItemDescription.text = activeItem.description;
            //Set item sprite to visible
            equipItemSprite.color = new Color(1, 1, 1, 1);
            equipItemSprite.sprite = activeItem.itemSprite;
        }
        //f no item is selected/item inventory empty don't show anything in description panel
        if (activeItem == null)
        {
            itemSelected = false;
            activeItem = null;
            equipItemName.text = "";
            equipItemDescription.text = "";
            
        }
    }
}