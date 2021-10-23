using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CommonEvents : MonoBehaviour
{
    [HideInInspector]
    public bool blackScreen;
    [HideInInspector]
    public bool fadeFromBlack;

    [Header("Transition Settings")]
    public bool activateScreenFade;
    public float fadeTime = 1;
    

    [Header("Display Settings")]
    public bool blockGameMenu;
    public bool hideTouchButtons;
    public bool showTouchButtons;

    [Header("Event Settings")]
    public bool markEventCompleteAfterFade;
    public bool markEventCompleteAtTheEnd;
    public string eventToMark;

    [Header("Quest Settings")]
    public bool markQuestCompleteAfterFade;
    public bool markQuestCompleteAtTheEnd;
    public string questToMark;

    [Header("Player Settings")]
    public bool lockPlayer;
    public bool hidePlayer;
    public bool facePlayerDown;
    public bool facePlayerLeft;
    public bool facePlayerUp;
    public bool facePlayerRight;
    public bool transposePlayer;
    public float x;
    public float y;
    public float z;

    [Header("Environment Settings")]
    public bool changeBGM;
    public int BGM;
    public bool dayTime;
    public bool nightTime;

    [Header("Teleport Settings")]
    public bool changeScene;
    public string scene;
    public float transitionTime = 1f;

    // Start is called before the first frame update
    void Start()
    {
        if (blackScreen)
        {
            ScreenFade.instance.fadeFromBlack = false;
            ScreenFade.instance.fadeScreenImage.color = new Color(0,0,0,1);
        }

        if (changeBGM)
        {
            AudioManager.instance.PlayBGM(BGM);
        }
        
        if(transposePlayer)
        {
            PlayerController.instance.transform.position = new Vector3(x, y, z);
        }
        

        if(hidePlayer)
        {
            PlayerController.instance.GetComponent<SpriteRenderer>().sortingLayerName = "Default";
        }else
        {
            PlayerController.instance.GetComponent<SpriteRenderer>().sortingLayerName = "Player";
        }
        
        if (facePlayerDown)
        {

            //PlayerController.instance.rigidBody.velocity = new Vector2(-30, 0);
            PlayerController.instance.animator.SetFloat("lastMoveY", -1f);
        }

        if (facePlayerLeft)
        {

            //PlayerController.instance.rigidBody.velocity = new Vector2(-30, 0);
            PlayerController.instance.animator.SetFloat("lastMoveX", -1f);
        }

        if (facePlayerUp)
        {

            //PlayerController.instance.rigidBody.velocity = new Vector2(-30, 0);
            PlayerController.instance.animator.SetFloat("lastMoveY", 1f);
        }

        if (facePlayerRight)
        {

            //PlayerController.instance.rigidBody.velocity = new Vector2(-30, 0);
            PlayerController.instance.animator.SetFloat("lastMoveX", 1f);
        }


    }

    // Update is called once per frame
    void Update()
    {
        if(blockGameMenu)
        {
            GameManager.instance.cutSceneActive = true;
        }else
        {
            GameManager.instance.cutSceneActive = false;
        }

        if(hideTouchButtons)
        {
            //Hide touch interface
            GameMenu.instance.touchConfirmButton.SetActive(false);
            GameMenu.instance.touchMenuButton.SetActive(false);
            GameMenu.instance.touchController.SetActive(false);
        }
        if(showTouchButtons)
        {
            if (ControlManager.instance.mobile == true)
            {
                GameMenu.instance.touchMenuButton.SetActive(true);
                GameMenu.instance.touchController.SetActive(true);
                GameMenu.instance.touchConfirmButton.SetActive(true);
            }
        }


        if(lockPlayer)
        {
            //Disable player movement
            PlayerController.instance.canMove = false;
        }

        

        if (activateScreenFade)
        {
            ScreenFade.instance.FadeToBlack();
            fadeTime -= Time.deltaTime;
            if (fadeTime <= 0)
            {
                if(markQuestCompleteAfterFade)
                {
                    //DialogManager.instance.ShouldActivateQuestAtEnd(questToMark, markQuestCompleteAfterFade);
                    QuestManager.instance.MarkQuestComplete(questToMark);
                }

                if (markEventCompleteAfterFade)
                {
                    EventManager.instance.MarkEventComplete(eventToMark);
                }
                ScreenFade.instance.FadeFromBlack();
                activateScreenFade = false;
            }
        }

        if (fadeFromBlack)
        {
            ScreenFade.instance.FadeFromBlack();
        }

        if (markQuestCompleteAtTheEnd)
        {
                    QuestManager.instance.MarkQuestComplete(questToMark);
                    GameManager.instance.cutSceneActive = false;
        }

        if (markEventCompleteAtTheEnd)
        {
            EventManager.instance.MarkEventComplete(eventToMark);
            GameManager.instance.cutSceneActive = false;
        }

        if (changeScene)
        {

            //GameManager.instance.fadingBetweenAreas = true;

            ScreenFade.instance.FadeToBlack();
            transitionTime -= Time.deltaTime;
            if (transitionTime <= 0)
            {
                SceneManager.LoadScene(scene);
            }
            
           // }
        }

        if (nightTime)
        {
            //Set directional light to night time
            DirectionalLight.instance.GetComponent<Light>().intensity = .1f;
            DirectionalLight.instance.GetComponent<Light>().color = new Color(0, .2f, 1, 1);
            nightTime = false;
        }

        if (dayTime)
        {
            //Set directional light to night time
            DirectionalLight.instance.GetComponent<Light>().intensity = 1.4f;
            DirectionalLight.instance.GetComponent<Light>().color = new Color(1, 1, 1, 1);
            dayTime = false;
        }
    } 
}
