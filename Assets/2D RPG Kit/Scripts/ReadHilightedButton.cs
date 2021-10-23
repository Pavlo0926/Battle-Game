using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;

//This script reads the currently highlighted item button and updates the item information in the info panel

public class ReadHilightedButton : MonoBehaviour, ISelectHandler
{

    public int buttonValue;
    

    // ItemButton is hilighted show item information
    public void OnSelect(BaseEventData eventData)
    {
        // Show message in debug log for testing
        Debug.Log("<color=red>Event:</color> Completed selection.");

        
        // If viewing item window in game menu
        if (GameMenu.instance.itemWindow.activeInHierarchy)
        {
            if (GameManager.instance.itemsHeld[buttonValue] != "")
            {
                GameMenu.instance.SelectItem(GameManager.instance.GetItemDetails(GameManager.instance.itemsHeld[buttonValue]));
            }else
            {
                GameMenu.instance.itemName.text = "Select an item!";
                GameMenu.instance.itemDescription.text = "No items!";
                GameMenu.instance.itemSprite.color = new Color(1, 1, 1, 0);
            }
        }

        // If viewing equip window in game menu
        if (GameMenu.instance.equipWindow.activeInHierarchy)
        {
            if (GameManager.instance.equipItemsHeld[buttonValue] != "")
            {
                GameMenu.instance.SelectEquipItem(GameManager.instance.GetItemDetails(GameManager.instance.equipItemsHeld[buttonValue]));
            }else
            {
                GameMenu.instance.equipItemName.text = "Select an item!";
                GameMenu.instance.equipItemDescription.text = "No equipment!";
                //Set item sprite to invisible
                GameMenu.instance.equipItemSprite.color = new Color(1, 1, 1, 0);
            }

        }

        // If viewing buy menu in shop
        if (Shop.instance.buyMenu.activeInHierarchy)
        {
            if (Shop.instance.itemsForSale[buttonValue] != "")
            {
                Shop.instance.SelectBuyItem(GameManager.instance.GetItemDetails(Shop.instance.itemsForSale[buttonValue]));
            }
            else
            {
                Shop.instance.buyItemName.text = "Select an item!";
                Shop.instance.buyItemDescription.text = "";
            }
        }

        // If viewing sell items menu in shop
        if (Shop.instance.sellMenu.activeInHierarchy)
        {
            if (GameManager.instance.itemsHeld[buttonValue] != "")
            {
                Shop.instance.SelectSellItem(GameManager.instance.GetItemDetails(GameManager.instance.itemsHeld[buttonValue]));
            }
            else
            {
                Shop.instance.sellItemName.text = "No items to sell!";
                Shop.instance.sellItemDescription.text = "";
                Shop.instance.sellItemSprite.color = new Color(1, 1, 1, 0);
            }
        }

        // If viewing sell equip items menu in shop
        if (Shop.instance.sellEquipItemsMenu.activeInHierarchy)
        {
            if (GameManager.instance.equipItemsHeld[buttonValue] != "")
            {
                Shop.instance.SelectSellItem(GameManager.instance.GetItemDetails(GameManager.instance.equipItemsHeld[buttonValue]));
            }
            else
            {
                Shop.instance.sellEquipItemName.text = "No equipment to sell!";
                Shop.instance.sellEquipItemDescription.text = "";
                Shop.instance.sellEquipItemSprite.color = new Color(1, 1, 1, 0);
            }
        }

        // If viewing item menu during battle
        if (BattleManager.instance.itemMenu.activeInHierarchy)
        {
            BattleManager.instance.buttonValue = buttonValue;

            if (GameManager.instance.itemsHeld[buttonValue] != "")
            {
                BattleManager.instance.itemSprite.color = new Color(1, 1, 1, 1);
                BattleManager.instance.SelectBattleItem(GameManager.instance.GetItemDetails(GameManager.instance.itemsHeld[buttonValue]));
            }else
            {
                BattleManager.instance.battleItemName.text = "No items!";
                BattleManager.instance.battleItemDescription.text = "";
                BattleManager.instance.itemSprite.color = new Color(1, 1, 1, 0);
            }            
        }        
    }
}
