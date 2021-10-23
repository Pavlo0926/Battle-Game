using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ItemButton : MonoBehaviour {

    public Image buttonImage;
    public Text amountText;
    public int buttonValue;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Press()
    {
        //Check if item window of game menu is currently active
        if (GameMenu.instance.itemWindow.activeInHierarchy)
        {
            if (GameManager.instance.itemsHeld[buttonValue] != "")
            {
                GameMenu.instance.buttonValue = buttonValue;
                GameMenu.instance.SelectItem(GameManager.instance.GetItemDetails(GameManager.instance.itemsHeld[buttonValue]));

                if (!GameMenu.instance.itemCharChoiceMenu.activeInHierarchy)
                {
                    //Enable use and discard buttons
                    GameMenu.instance.useItemButton.interactable = true;
                    GameMenu.instance.discardItemButton.interactable = true;
                }

                //In non-mobile disable every item button except for selected item button
                if (!ControlManager.instance.mobile)
                {
                    for (int i = 0; i < GameMenu.instance.itemButtonsB.Length; i++)
                    {
                        if (i != buttonValue)
                        {
                            GameMenu.instance.itemButtonsB[i].interactable = false;
                        }
                        
                    }
                    
                }

                if (ControlManager.instance.mobile == false)
                {
                    GameMenu.instance.btn = GameMenu.instance.itemUse;
                    GameMenu.instance.SelectFirstButton();

                }
            }else
            {
                GameMenu.instance.activeItem = null;
            }
        }

        //Check if equip item window of game menu is currently active
        if (GameMenu.instance.equipWindow.activeInHierarchy)
        {
            if (GameManager.instance.equipItemsHeld[buttonValue] != "")
            {
                GameMenu.instance.buttonValue = buttonValue;
                GameMenu.instance.SelectEquipItem(GameManager.instance.GetItemDetails(GameManager.instance.equipItemsHeld[buttonValue]));

                if(!GameMenu.instance.equipCharChoiceMenu.activeInHierarchy)
                {
                    //Enable use and discard buttons
                    GameMenu.instance.useEquipItemButton.interactable = true;
                    GameMenu.instance.discardEquipItemButton.interactable = true;
                }
                
                //In non-mobile disable every item button except for selected item button
                if (!ControlManager.instance.mobile)
                {
                    for (int i = 0; i < GameMenu.instance.equipItemButtonsB.Length; i++)
                    {
                        if (i != buttonValue)
                        {
                            GameMenu.instance.equipItemButtonsB[i].interactable = false;
                        }

                    }

                }

                if (ControlManager.instance.mobile == false)
                {
                    GameMenu.instance.btn = GameMenu.instance.equipUse;
                    GameMenu.instance.SelectFirstButton();

                }
            }else
            {
                GameMenu.instance.activeItem = null;
            }
        }

        if (Shop.instance.shopMenu.activeInHierarchy)
        {
            if(Shop.instance.buyMenu.activeInHierarchy)
            {
                if (Shop.instance.itemsForSale[buttonValue] != "")
                {
                    Shop.instance.SelectBuyItem(GameManager.instance.GetItemDetails(Shop.instance.itemsForSale[buttonValue]));
                    Shop.instance.buyPrompt.SetActive(true);

                    if (ControlManager.instance.mobile == false)
                    {
                        GameMenu.instance.btn = GameMenu.instance.yesButtonBuy;
                        GameMenu.instance.SelectFirstButton();

                    }

                    //In non-mobile disable every item button except for selected item button
                    if (!ControlManager.instance.mobile)
                    {
                        for (int i = 0; i < Shop.instance.itemsForSale.Length; i++)
                        {
                            if (i != buttonValue)
                            {
                                Shop.instance.buyItemButtonsB[i].interactable = false;
                            }

                        }

                    }

                    
                }
                if (Shop.instance.itemsForSale[buttonValue] == "")
                {
                    Shop.instance.buyPrompt.SetActive(false);
                    
                }
            }

            if(Shop.instance.sellMenu.activeInHierarchy)
            {
                if (GameManager.instance.itemsHeld[buttonValue] != "")
                {
                    Shop.instance.SelectSellItem(GameManager.instance.GetItemDetails(GameManager.instance.itemsHeld[buttonValue]));

                    Shop.instance.sellPrompt.SetActive(true);

                    if (ControlManager.instance.mobile == false)
                    {
                        GameMenu.instance.btn = GameMenu.instance.yesButtonSell;
                        GameMenu.instance.SelectFirstButton();

                    }
                }
                if (GameManager.instance.itemsHeld[buttonValue] == "")
                {
                    Shop.instance.sellPrompt.SetActive(false);

                }
            }

            if (Shop.instance.sellEquipItemsMenu.activeInHierarchy)
            {
                if (GameManager.instance.equipItemsHeld[buttonValue] != "")
                {
                    Shop.instance.SelectSellItem(GameManager.instance.GetItemDetails(GameManager.instance.equipItemsHeld[buttonValue]));
                    Shop.instance.sellEquipItemsPrompt.SetActive(true);

                    if (ControlManager.instance.mobile == false)
                    {
                        GameMenu.instance.btn = GameMenu.instance.yesButtonSellEqipItem;
                        GameMenu.instance.SelectFirstButton();

                    }
                }
                if (GameManager.instance.equipItemsHeld[buttonValue] == "")
                {
                    Shop.instance.sellEquipItemsPrompt.SetActive(false);

                }
            }
        }

        if (BattleManager.instance.battleScene.activeInHierarchy)
        {

            BattleManager.instance.buttonValue = buttonValue;

            if (GameManager.instance.itemsHeld[buttonValue] != "")
            {
                
                if (ControlManager.instance.mobile == false)
                {
                    GameMenu.instance.btn = BattleManager.instance.useItemButton;
                    GameMenu.instance.SelectFirstButton();

                }

                if (ControlManager.instance.mobile == true)
                {
                    EventSystem.current.SetSelectedGameObject(null);
                }

                BattleManager.instance.SelectBattleItem(GameManager.instance.GetItemDetails(GameManager.instance.itemsHeld[buttonValue]));
                
                
            }
        }
    }
    
}
