using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inn : MonoBehaviour
{
    //Make instance of this script to be able reference from other scripts!
    public static Inn instance;

    [Header("Initialization")]
    //Game objects used by this code
    public GameObject innMenu;
    public GameObject innPrompt;
    public Text goldText;
    public Text innPromtText;
    public Text goldPrompt;
    public GameObject prompt;
    private bool stay;

    [Header("Inn Settings")]
    public string[] lines;
    public string[] sayGoodBye;
    public int price;
    

    

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        //Uncomment if you want to close the inn with the cancle button
        if (Input.GetButtonDown("RPGCanclePC") || Input.GetButtonDown("RPGCancleJoy"))
        {
            if (innPrompt.activeInHierarchy)
            {
                AudioManager.instance.PlaySFX(3);
            }

            if (innMenu.activeInHierarchy)
            {
                //CloseInn(); //Uncomment if you want to close the inn with the cancle button 
                GameMenu.instance.btn = GameMenu.instance.exitInnButton;
                GameMenu.instance.SelectFirstButton();
                CloseInnPrompt();
            }
        }
        
    }

    public void OpenInn()
    {
        GameMenu.instance.touchMenuButton.SetActive(false);
        GameMenu.instance.touchController.SetActive(false);
        GameMenu.instance.touchConfirmButton.SetActive(false);

        //DialogManager.instance.ShowDialog(lines, true);
        GameManager.instance.gameMenuOpen = true;
        innMenu.SetActive(true);

        if (ControlManager.instance.mobile == false)
        {
            GameMenu.instance.btn = GameMenu.instance.stayButton;
            GameMenu.instance.SelectFirstButton();
        }
        
        GameManager.instance.innActive = true;
        goldText.text = GameManager.instance.currentGold.ToString() + "g";
    }

    public void OpenInnPrompt()
    {
        innPrompt.SetActive(true);

        if (ControlManager.instance.mobile == false)
        {
            GameMenu.instance.btn = GameMenu.instance.noInnButton;
            GameMenu.instance.SelectFirstButton();
        }
        
        innPromtText.text = "Do you want to stay for " + DialogManager.instance.innPrice + "g?";
    }

    public void CloseInnPrompt()
    {
        innPrompt.SetActive(false);

        if(ControlManager.instance.mobile == false)
        {
            GameMenu.instance.btn = GameMenu.instance.stayButton;
            GameMenu.instance.SelectFirstButton();
        }        
    }

    public void Stay()
    {
        stay = true;
            if (GameManager.instance.currentGold >= DialogManager.instance.innPrice)
            {
                CloseInnPrompt();
                StartCoroutine(StayCo());
            }
            else
            {
                CloseInnPrompt();
                goldPrompt.text = "Not enough gold!";
                StartCoroutine(PromtCo());
                stay = false;
            }
    }

    public IEnumerator PromtCo()
    {
        prompt.SetActive(true);
        yield return new WaitForSeconds(1.5f);
        prompt.SetActive(false);
    }

    public IEnumerator StayCo()
    {
        for (int i = 0; i < GameManager.instance.characterStatus.Length; i++)
        {
            GameManager.instance.characterStatus[i].currentHP = GameManager.instance.characterStatus[i].maxHP;
            GameManager.instance.characterStatus[i].currentSP = GameManager.instance.characterStatus[i].maxSP;
            
        }
        //CloseInn();
        innMenu.SetActive(false);
        GameManager.instance.currentGold -= DialogManager.instance.innPrice;
        ScreenFade.instance.FadeToBlack();
        yield return new WaitForSeconds(1.5f);
        AudioManager.instance.PlaySFX(6);
        yield return new WaitForSeconds(1);
        ScreenFade.instance.FadeFromBlack();
        yield return new WaitForSeconds(1);
        GameManager.instance.gameMenuOpen = false;
        GameManager.instance.innActive = false;
        CloseInn();
    }

    public void CloseInn()
    {
        if (ControlManager.instance.mobile == true)
        {
            GameMenu.instance.touchMenuButton.SetActive(true);
            GameMenu.instance.touchController.SetActive(true);
            GameMenu.instance.touchConfirmButton.SetActive(true);
        }

        DialogManager.instance.isInn = false;
        innMenu.SetActive(false);

        GameManager.instance.innActive = false;

        GameManager.instance.gameMenuOpen = false;
        if (stay)
        {
            DialogManager.instance.SayGoodByeInn(sayGoodBye, true, false);
            stay = false;
        }else
        {
            DialogManager.instance.SayGoodByeInn(sayGoodBye, true, true);
        }
        
    }
}
