using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour {
    
    Navigation customNav = new Navigation();

    

    //Make instance of this script to be able reference from other scripts!
    public static Shop instance;

    [Header("Initialization")]
    //Game objects used by this code
    public GameObject shopMenu;
    public GameObject buyMenu;
    public GameObject sellMenu;
    public GameObject sellEquipItemsMenu;
    public GameObject goldprompt;
    public GameObject buyPrompt;
    public GameObject sellPrompt;
    public GameObject sellEquipItemsPrompt;
    public GameObject noBuyItemButton;
    public GameObject noSellItemButton;
    public GameObject noSellEquipItemButton;
    public Text goldText;
    public ItemButton[] buyItemButtons;
    public ItemButton[] sellItemButtons;
    public ItemButton[] sellEquipItemButtons;
    public Button[] buyItemButtonsB;
    public Item selectedItem;
    public Text buyItemName, buyItemDescription, buyItemPrice;
    public Text sellItemName, sellItemDescription, sellItemPrice;
    public Text sellEquipItemName, sellEquipItemDescription, sellEquipItemPrice;
    public ReadHilightedButton[] hilightedBuyItemButtons;
    public ReadHilightedButton[] hilightedSellItemButtons;
    public ReadHilightedButton[] hilightedSellEquipItemButtons;
    public Image buyItemSprite;
    public Image sellItemSprite;
    public Image sellEquipItemSprite;
    public int numberOfItemsHeld;
    public int numberOfEquipItemsHeld;
    public Text promptText;

    [Header("Shop Settings")]
    public string[] itemsForSale;
    public string[] sayGoodBye;

    // Use this for initialization
    void Start () {
        instance = this;
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetButtonDown("RPGCanclePC") || Input.GetButtonDown("RPGCancleJoy"))
        {
            if (shopMenu.activeInHierarchy)
            {
                AudioManager.instance.PlaySFX(3);
            }
            if (buyPrompt.activeInHierarchy)
            {
                buyPrompt.SetActive(false);
                GameMenu.instance.btn = GameMenu.instance.buyMenuItem0;
                GameMenu.instance.SelectFirstButton();

                for (int i = 0; i < buyItemButtons.Length; i++)
                {
                    buyItemButtons[i].buttonValue = i;
                    hilightedBuyItemButtons[i].buttonValue = i;

                    //Set button navigation mode to automatic
                    customNav.mode = Navigation.Mode.Automatic;

                    //Set navigation mode of non-disabled buttons to automatic in order to avoid navigating into disabled buttons
                    if (itemsForSale[i] != "")
                    {
                        buyItemButtonsB[i].navigation = customNav;
                        GameMenu.instance.buyItemButtons[i].interactable = true;
                    }

                    if (itemsForSale[i] == "")
                    {
                        GameMenu.instance.buyItemButtons[i].interactable = false;
                    }

                }

                }else if (sellPrompt.activeInHierarchy)
            {
                sellPrompt.SetActive(false);
                GameMenu.instance.btn = GameMenu.instance.sellMenuItem0;
                GameMenu.instance.SelectFirstButton();
            }else if (sellEquipItemsPrompt.activeInHierarchy)
            {
                sellEquipItemsPrompt.SetActive(false);
                GameMenu.instance.btn = GameMenu.instance.sellMenuEquipItem0;
                GameMenu.instance.SelectFirstButton();
            }else
            {
                buyMenu.SetActive(false);
                GameMenu.instance.buyButton.image.color = new Color(1, 1, 1, 1);
                
                sellMenu.SetActive(false);
                GameMenu.instance.sellItemButton.image.color = new Color(1, 1, 1, 1);

                sellEquipItemsMenu.SetActive(false);
                GameMenu.instance.sellEquipItemButton.image.color = new Color(1, 1, 1, 1);

                //Reactivate shop buttons 
                GameMenu.instance.buyButton.interactable = true;
                GameMenu.instance.sellItemButton.interactable = true;
                GameMenu.instance.sellEquipItemButton.interactable = true;
                GameMenu.instance.exitShopButton.interactable = true;
            }
            

            if (shopMenu.activeInHierarchy)
            {
                if (!buyMenu.activeInHierarchy && !sellMenu.activeInHierarchy && !sellEquipItemsMenu.activeInHierarchy)
                {
                    GameMenu.instance.btn = GameMenu.instance.exitShopButton;
                    GameMenu.instance.SelectFirstButton();
                }
            }
        }
	}

    public void OpenShop()
    {
            shopMenu.SetActive(true);
        GameMenu.instance.touchMenuButton.SetActive(false);
        GameMenu.instance.touchController.SetActive(false);
        GameMenu.instance.touchConfirmButton.SetActive(false);

        if (ControlManager.instance.mobile == false)
        {
            GameMenu.instance.btn = GameMenu.instance.buyButton;
            GameMenu.instance.SelectFirstButton();
        }            

            GameManager.instance.shopActive = true;

            goldText.text = GameManager.instance.currentGold.ToString() + "g";
    }

    public void CloseShop()
    {
        if (ControlManager.instance.mobile == true)
        {
            GameMenu.instance.touchMenuButton.SetActive(true);
            GameMenu.instance.touchController.SetActive(true);
            GameMenu.instance.touchConfirmButton.SetActive(true);
        }

        DialogManager.instance.isShop = false;
        shopMenu.SetActive(false);
        GameManager.instance.shopActive = false;
        buyMenu.SetActive(false);
        sellMenu.SetActive(false);
        sellEquipItemsMenu.SetActive(false);
        DialogManager.instance.SayGoodBye(sayGoodBye, true);
        DialogManager.instance.dontOpenDialogAgain = true;
    }

    public void OpenBuyMenu()
    {
        
        //Highlight buy button to indicate which shop menu is currently active
        GameMenu.instance.buyButton.image.color = new Color(.1333333f, .2313726f, .3294118f, 1);

        //Turn other buttons to normal
        GameMenu.instance.sellItemButton.image.color = new Color(1, 1, 1, 1);
        GameMenu.instance.sellEquipItemButton.image.color = new Color(1, 1, 1, 1);

        if (!ControlManager.instance.mobile)
        {
            //Deactivate shop buttons 
            GameMenu.instance.buyButton.interactable = false;
            GameMenu.instance.sellItemButton.interactable = false;
            GameMenu.instance.sellEquipItemButton.interactable = false;
            GameMenu.instance.exitShopButton.interactable = false;
        }
        

        if (ControlManager.instance.mobile == true)
        {
            noBuyItemButton.SetActive(false);
        }

        buyItemButtons[0].Press();

        buyMenu.SetActive(true);
        sellMenu.SetActive(false);
        sellEquipItemsMenu.SetActive(false);

        if (ControlManager.instance.mobile == true)
        {
            buyPrompt.SetActive(true);
        }

        GameMenu.instance.btn = GameMenu.instance.buyMenuItem0;
        GameMenu.instance.SelectFirstButton();

        for (int i = 0; i < buyItemButtons.Length; i++)
        {
            buyItemButtons[i].buttonValue = i;
            hilightedBuyItemButtons[i].buttonValue = i;

            //Set button navigation mode to automatic
            customNav.mode = Navigation.Mode.Automatic;

            //Set navigation mode of non-disabled buttons to automatic in order to avoid navigating into disabled buttons
            if (itemsForSale[i] != "")
            {
                buyItemButtonsB[i].navigation = customNav;

            }

            if (itemsForSale[i] == "")
            {
                GameMenu.instance.buyItemButtons[i].interactable = false;
            }
            if (itemsForSale[i] != "")
            {
                //buyItemButtons[i].buttonImage.gameObject.SetActive(true);
                //buyItemButtons[i].buttonImage.sprite = GameManager.instance.GetItemDetails(itemsForSale[i]).itemSprite;
                buyItemButtons[i].amountText.text = GameManager.instance.GetItemDetails(itemsForSale[i]).itemName + " - " + GameManager.instance.GetItemDetails(itemsForSale[i]).price + "g";
            }
            else
            {
                //buyItemButtons[i].buttonImage.gameObject.SetActive(false);
                buyItemButtons[i].amountText.text = "";
            }
        }
    }

    public void CloseBuyPrompt()
    {
        if (ControlManager.instance.mobile == false)
        {
            buyPrompt.SetActive(false);
        }
            GameMenu.instance.btn = GameMenu.instance.buyMenuItem0;
            GameMenu.instance.SelectFirstButton();

        for (int i = 0; i < buyItemButtons.Length; i++)
        {
            GameMenu.instance.buyItemButtons[i].interactable = true;

            if (itemsForSale[i] == "")
            {
                GameMenu.instance.buyItemButtons[i].interactable = false;
            }
        }
    }

    public void CloseSellPromt()
    {
        if (sellMenu.activeInHierarchy)
        {
            if (ControlManager.instance.mobile == false)
            {
                sellPrompt.SetActive(false);
            }
                GameMenu.instance.btn = GameMenu.instance.sellMenuItem0;
                GameMenu.instance.SelectFirstButton();
        }

        if (sellEquipItemsMenu.activeInHierarchy)
        {
            if (ControlManager.instance.mobile == false)
            {
                sellEquipItemsPrompt.SetActive(false);
            }
                GameMenu.instance.btn = GameMenu.instance.sellMenuEquipItem0;
                GameMenu.instance.SelectFirstButton();
        }
       
    }

    public void OpenSellMenu()
    {
        //Highlight sell button to indicate which shop menu is currently active
        GameMenu.instance.sellItemButton.image.color = new Color(.1333333f, .2313726f, .3294118f, .5f);

        //Turn other buttons to normal
        GameMenu.instance.buyButton.image.color = new Color(1, 1, 1, 1);
        GameMenu.instance.sellEquipItemButton.image.color = new Color(1, 1, 1, 1);

        if (!ControlManager.instance.mobile)
        {
            //Deactivate shop buttons 
            GameMenu.instance.buyButton.interactable = false;
            GameMenu.instance.sellItemButton.interactable = false;
            GameMenu.instance.sellEquipItemButton.interactable = false;
            GameMenu.instance.exitShopButton.interactable = false;
        }

        if (ControlManager.instance.mobile == true)
        {
            noSellItemButton.SetActive(false);
        }

        sellItemButtons[0].Press();

        buyMenu.SetActive(false);
        sellMenu.SetActive(true);
        sellEquipItemsMenu.SetActive(false);

        if (ControlManager.instance.mobile == true)
        {
            sellPrompt.SetActive(true);
        }

        GameMenu.instance.btn = GameMenu.instance.sellMenuItem0;
        GameMenu.instance.SelectFirstButton();

        ShowSellItems();
        
    }

    public void OpenSellEquipItemsMenu()
    {
        //Highlight sell button to indicate which shop menu is currently active
        GameMenu.instance.sellEquipItemButton.image.color = new Color(.1333333f, .2313726f, .3294118f, 1);

        //Turn other buttons to normal
        GameMenu.instance.buyButton.image.color = new Color(1, 1, 1, 1);
        GameMenu.instance.sellItemButton.image.color = new Color(1, 1, 1, 1);

        if (!ControlManager.instance.mobile)
        {
            //Deactivate shop buttons 
            GameMenu.instance.buyButton.interactable = false;
            GameMenu.instance.sellItemButton.interactable = false;
            GameMenu.instance.sellEquipItemButton.interactable = false;
            GameMenu.instance.exitShopButton.interactable = false;
        }

        if (ControlManager.instance.mobile == true)
        {
            noSellEquipItemButton.SetActive(false);
        }

        sellItemButtons[0].Press();

        buyMenu.SetActive(false);
        sellMenu.SetActive(false);
        sellEquipItemsMenu.SetActive(true);

        

        GameMenu.instance.btn = GameMenu.instance.sellMenuEquipItem0;
        GameMenu.instance.SelectFirstButton();

        ShowSellEquipItems();

    }

    private void ShowSellItems()
    {
        GameManager.instance.SortItems();
        for (int i = 0; i < sellItemButtons.Length; i++)
        {
            GameMenu.instance.sellItemButtons[i].interactable = true;
            
            sellItemButtons[i].buttonValue = i;
            hilightedSellItemButtons[i].buttonValue = i;

            //Set button navigation mode to automatic
            customNav.mode = Navigation.Mode.Automatic;

            //Set navigation mode of non-disabled buttons to automatic in order to avoid navigating into disabled buttons
            if (GameManager.instance.itemsHeld[i] != "")
            {
                GameMenu.instance.sellItemButtons[i].navigation = customNav;

            }

            if (GameManager.instance.itemsHeld[i] == "")
            {
                
                GameMenu.instance.sellItemButtons[i].interactable = false;
            }

            if (GameManager.instance.itemsHeld[i] != "")
            {
                //sellItemButtons[i].buttonImage.gameObject.SetActive(true);
                //sellItemSprite.sprite = GameManager.instance.GetItemDetails(GameManager.instance.itemsHeld[i]).itemSprite;
                sellItemButtons[i].amountText.text = GameManager.instance.GetItemDetails(GameManager.instance.itemsHeld[i]).itemName + " - " + GameManager.instance.GetItemDetails(GameManager.instance.itemsHeld[i]).sellPrice + "g";
            }
            else
            {
                //sellItemButtons[i].buttonImage.gameObject.SetActive(false);
                sellItemButtons[i].amountText.text = "";
            }
        }
    }

    private void ShowSellEquipItems()
    {
        GameManager.instance.SortItems();
        for (int i = 0; i < sellEquipItemButtons.Length; i++)
        {
            GameMenu.instance.sellEquipItemButtons[i].interactable = true;

            sellEquipItemButtons[i].buttonValue = i;
            hilightedSellEquipItemButtons[i].buttonValue = i;

            //Set button navigation mode to automatic
            customNav.mode = Navigation.Mode.Automatic;

            //Set navigation mode of non-disabled buttons to automatic in order to avoid navigating into disabled buttons
            if (GameManager.instance.equipItemsHeld[i] != "")
            {
                GameMenu.instance.sellEquipItemButtons[i].navigation = customNav;

            }

            if (GameManager.instance.equipItemsHeld[i] == "")
            {
                GameMenu.instance.sellEquipItemButtons[i].interactable = false;
            }

            if (GameManager.instance.equipItemsHeld[i] != "")
            {
                //sellEquipItemButtons[i].buttonImage.gameObject.SetActive(true);
                //sellEquipItemButtons[i].buttonImage.sprite = GameManager.instance.GetItemDetails(GameManager.instance.equipItemsHeld[i]).itemSprite;
                sellEquipItemButtons[i].amountText.text = GameManager.instance.GetItemDetails(GameManager.instance.equipItemsHeld[i]).itemName + " - " + GameManager.instance.GetItemDetails(GameManager.instance.equipItemsHeld[i]).sellPrice + "g";
            }
            else
            {
                //sellEquipItemButtons[i].buttonImage.gameObject.SetActive(false);
                sellEquipItemButtons[i].amountText.text = "";
            }
        }
    }

    public void SelectBuyItem(Item buyItem)
    {
        selectedItem = buyItem;
        buyItemName.text = selectedItem.itemName;
        buyItemDescription.text = selectedItem.description;
        buyItemPrice.text = "Buy for " + selectedItem.price + "g?";
        buyItemSprite.sprite = selectedItem.itemSprite;
    }

    public void SelectSellItem(Item newItem)
    {
        if (sellMenu.activeInHierarchy)
        {
            if (newItem != null)
            {
                selectedItem = newItem;
                sellItemName.text = selectedItem.itemName;
                sellItemDescription.text = selectedItem.description;
                sellItemPrice.text = "Sell for " + selectedItem.sellPrice + "g?";
                sellItemSprite.sprite = selectedItem.itemSprite;
            }
            else
            {
                selectedItem = null;
                sellItemName.text = "";
                sellItemDescription.text = "";
                sellItemPrice.text = "";
            }
        }

        if (sellEquipItemsMenu.activeInHierarchy)
        {
            if (newItem != null)
            {
                selectedItem = newItem;
                sellEquipItemName.text = selectedItem.itemName;
                sellEquipItemDescription.text = selectedItem.description;
                sellEquipItemPrice.text = "Sell for " + selectedItem.sellPrice + "g?";
                sellEquipItemSprite.sprite = selectedItem.itemSprite;
            }
            else
            {
                selectedItem = null;
                sellEquipItemName.text = "";
                sellEquipItemDescription.text = "";
                sellEquipItemPrice.text = "";
            }
        }
        
    }

    public void BuyItem()
    {
        Debug.LogError("BuyItem()");
        //Calculate the amount of items / equipment held in inventory to prevent buying more items if inventory is full
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

        GameMenu.instance.buyItemButton.interactable = false;
        if (selectedItem != null)
        {
            if (GameManager.instance.currentGold < selectedItem.price)
            {
                promptText.text = "Not enough gold!";
                StartCoroutine(PromptCo());
            }

            if (GameManager.instance.currentGold >= selectedItem.price)
            {
                if (selectedItem.item)
                {
                    if (numberOfItemsHeld < GameManager.instance.itemsHeld.Length)
                    {
                        GameManager.instance.currentGold -= selectedItem.price;
                        GameManager.instance.AddItem(selectedItem.itemName);
                        numberOfItemsHeld++;
                    }
                    else
                    {
                        promptText.text = "Your invontory is full!";
                        StartCoroutine(PromptCo());
                    }
                }
                if (selectedItem.defense || selectedItem.offense)
                {
                    if (numberOfEquipItemsHeld < GameManager.instance.equipItemsHeld.Length)
                    {
                        GameManager.instance.currentGold -= selectedItem.price;
                        GameManager.instance.AddItem(selectedItem.itemName);
                        numberOfEquipItemsHeld++;
                    }
                    else
                    {
                        promptText.text = "Your invontory is full!";
                        StartCoroutine(PromptCo());
                    }
                }
                    
                
                
            }
        }

        goldText.text = GameManager.instance.currentGold.ToString() + "g";

        if (!ControlManager.instance.mobile)
        {
            CloseBuyPrompt();
        }

        for (int i = 0; i < buyItemButtons.Length; i++)
        {
            GameMenu.instance.buyItemButtons[i].interactable = true;

            if (itemsForSale[i] == "")
            {
                GameMenu.instance.buyItemButtons[i].interactable = false;
            }
        }
        
        
    }

    public void SellItem()
    {
        if (selectedItem != null)
        {
            GameManager.instance.currentGold += selectedItem.sellPrice;

            GameManager.instance.RemoveSoldItem(selectedItem.itemName);
        }

        goldText.text = GameManager.instance.currentGold.ToString() + "g";

        if (sellMenu.activeInHierarchy)
        {
            ShowSellItems();
        }
        
        if (sellEquipItemsMenu.activeInHierarchy)
        {
            ShowSellEquipItems();
        }        

        if (!GameManager.instance.HasItem(selectedItem.itemName))
        {
            SelectSellItem(null);
        }
    }

    public IEnumerator PromptCo()
    {
        goldprompt.SetActive(true);
        yield return new WaitForSeconds(1.5f);
        goldprompt.SetActive(false);
    }
}
