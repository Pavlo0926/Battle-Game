using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Save : MonoBehaviour
{
    //Make instance of this script to be able reference from other scripts!
    public static Save instance;

    [Header("Initialization")]
    //Game objects used by this code
    public GameObject saveMenu;
    public GameObject statesMenu;
    public GameObject savePrompt;
    public GameObject saving;
    public Text savingText;

    //For UI button higlighting
    public Button noButton;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetButtonDown("RPGCanclePC") || Input.GetButtonDown("RPGCancleJoy"))
        {
            if (saveMenu.activeInHierarchy)
            {
                //CloseSaveMenu(); //Uncomment if you want to close the save menu with the cancle button

                if (ControlManager.instance.mobile == false)
                {
                    //savePrompt.SetActive(false);
                    GameMenu.instance.btn = GameMenu.instance.closeButtonSave;
                    GameMenu.instance.SelectFirstButton();
                }
            }
        }
    }

    public void OpenSaveMenu()
    {
        GameMenu.instance.touchMenuButton.SetActive(false);
        GameMenu.instance.touchController.SetActive(false);
        GameMenu.instance.touchConfirmButton.SetActive(false);

        GameManager.instance.gameMenuOpen = true;

        saveMenu.SetActive(true);

        GameManager.instance.saveMenuActive = true;
    }

    public void OpenStatesMenu()
    {
        statesMenu.SetActive(true);
    }

    public void CloseSaveMenu()
    {
        if (ControlManager.instance.mobile == true)
        {
            GameMenu.instance.touchMenuButton.SetActive(true);
            GameMenu.instance.touchController.SetActive(true);
            GameMenu.instance.touchConfirmButton.SetActive(true);
        }

        GameManager.instance.gameMenuOpen = false;

        saveMenu.SetActive(false);

        GameManager.instance.saveMenuActive = false;
        
    }

    public void SaveGame()
    {
        StartCoroutine(SavingCo());
        GameManager.instance.SaveData();
        QuestManager.instance.SaveQuestData();
        ChestManager.instance.SaveChestData();
        EventManager.instance.SaveEventData();
        CloseSavePromt();
        CloseSaveMenu();
    }

    public void OpenSavePromt()
    {
        savePrompt.SetActive(true);

        if(ControlManager.instance.mobile == false)
        {
            GameMenu.instance.btn = noButton;
            GameMenu.instance.SelectFirstButton();
        }        
    }

    public void CloseSavePromt()
    {       
        savePrompt.SetActive(false);

        if(ControlManager.instance.mobile == false)
        {
            GameMenu.instance.btn = GameMenu.instance.saveButton;
            GameMenu.instance.SelectFirstButton();
        }        
    }

    public IEnumerator SavingCo()
    {
        saving.SetActive(true);
        savingText.text = "Saving";
        yield return new WaitForSeconds(.5f);
        savingText.text = "Saving .";
        yield return new WaitForSeconds(.5f);
        savingText.text = "Saving ..";
        yield return new WaitForSeconds(.5f);
        savingText.text = "Saving ...";
        yield return new WaitForSeconds(.5f);
        saving.SetActive(false);
    }
}
