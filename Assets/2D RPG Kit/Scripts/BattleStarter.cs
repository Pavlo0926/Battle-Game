using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class BattleStarter : MonoBehaviour {
    
    [Header("Enemy settings")]
    public float encounterRate;
    public BattleType[] randomBattles;

    [Header("Battle Settings")]
    public Sprite battleBG;
    public bool activateOnEnter;
    public bool activateOnExit;
    public bool singleBattle;
    public bool noRetreat;
    public bool completeQuest;
    public string QuestToComplete;

    private bool inArea;
    public float countdown;
    
	// Use this for initialization
	void Start () {
        countdown = Random.Range(1, encounterRate);
        //countdown = 10;
    }
	
	// Update is called once per frame
	void Update () {
		if(inArea && PlayerController.instance.canMove)
        {
            if(Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0 || CrossPlatformInputManager.GetAxisRaw("Horizontal") !=0 || CrossPlatformInputManager.GetAxisRaw("Vertical") != 0)
            {
                countdown -= Time.deltaTime;

                if (countdown < 0f)
                {
                    countdown = Random.Range(1, encounterRate);
                    //countdown = 10;

                    StartCoroutine(StartBattleCo());
                }
            }

            
        }
	}

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            if (activateOnEnter)
            {
                StartCoroutine(StartBattleCo());
            }
            else
            {
                inArea = true;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            if (activateOnExit)
            {
                StartCoroutine(StartBattleCo());
            }
            else
            {
                inArea = false;
            }
        }
    }

    public IEnumerator StartBattleCo()
    {
        //Play battle music
        AudioManager.instance.PlayBGM(BattleManager.instance.battleMusicIntro);
        
        ScreenFade.instance.FadeToBlack();
        BattleManager.instance.battleBG = battleBG;
        GameManager.instance.battleActive = true;
        GameMenu.instance.gotItemMessage.SetActive(false);

        int selectedBattle = Random.Range(0, randomBattles.Length);

        BattleManager.instance.rewardItems = randomBattles[selectedBattle].rewardItems;
        BattleManager.instance.rewardEquipItems = randomBattles[selectedBattle].rewardEquipItems;
        BattleManager.instance.rewardXP = randomBattles[selectedBattle].rewardXP;
        BattleManager.instance.rewardGold = randomBattles[selectedBattle].rewardGold;
        BattleManager.instance.TileMap = GameObject.Find("Tilemap");
        BattleManager.instance.Player = GameObject.FindGameObjectWithTag("Player");
        if (GameObject.FindObjectOfType<ControlManager>().mobile)
            BattleManager.instance.PlayerController = GameObject.Find("CF2-Rig");
        yield return new WaitForSeconds(1.5f);

        BattleManager.instance.BattleStart(randomBattles[selectedBattle].enemies, noRetreat);
        BattleManager.instance.UpdateCharacterStatus();
        BattleManager.instance.UpdateBattle();


        GameObject.FindObjectOfType<Camera>().orthographic = false;
        BattleManager.instance.Player.SetActive(false);
        BattleManager.instance.TileMap.SetActive(false);
        if (GameObject.FindObjectOfType<ControlManager>().mobile)
            BattleManager.instance.PlayerController.SetActive(false);

        
        ScreenFade.instance.FadeFromBlack();

        if(singleBattle)
        {
            gameObject.SetActive(false);
        }

        RewardScreen.instance.markQuestComplete = completeQuest;
        RewardScreen.instance.questToMark = QuestToComplete;
    }
}
