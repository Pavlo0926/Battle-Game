using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityStandardAssets.CrossPlatformInput;

public class BattleManager : MonoBehaviour
{
    public bool usable;
    Navigation customNav = new Navigation();
    
    public int buttonValue;

    //Make instance of this script to be able reference from other scripts!
    public static BattleManager instance;

    
    //For checking if a battle is active
    private bool battleActive;

    //Initiates correct battle background image
    [HideInInspector]
    public SpriteRenderer spriteRenderer;
    [HideInInspector]
    public Sprite battleBG;

    //For correct calculation of whether heal HP or SP with items
    [HideInInspector]
    public bool affectHP = false;
    [HideInInspector]
    public bool affectSP = false;

    [Header("Initialization")]
    //Game objects used by this code
    public GameObject battleScene;
    public GameObject[] CharacterSlot;
    public GameObject targetCharacterMenu;
    public GameObject battleMenu;
    public GameObject targetEnemyMenu;
    public GameObject skillMenu;
    public GameObject itemMenu;
    public GameObject[] currentTurnIndicator;
    public ReadHilightedButton[] hilightedBattleItem;
    public BattleNotification battlePrompts;
    public Image[] portrait;
    private CharacterStatus[] playerStats;

    //Text objects used by this code
    public Text[] targetCharacterName;
    public Text[] characterName;
    public Text[] characterHP;
    public Slider[] HPSlider;
    public Text[] characterSP;
    public Slider[] SPSlider;
    public Text[] characterLevel;
    public Text battleUseButtonText;
    public Text battleItemName;
    public Text battleItemDescription;

    //For initiation of the correct number of characters and enemies
    public List<BattleCharacter> activeBattlers = new List<BattleCharacter>();

    //For initiation of the correct number of enemie buttons in the enemy target menu
    public BattleTargetButton[] targetEnemyButtons;

    //Music
    public int battleMusicIntro;
    public int battleMusic;
    public int victoryMusicIntro;
    public int victoryMusic;

    [Header("Menu Buttons")]
    //This holds the touch back button for mobile input
    public GameObject touchBackButton;

    //These are being used for highlighting the correct target enemy button
    public Button targetEnemyMenuButton0;
    public Button targetEnemyMenuButton1;
    public Button targetEnemyMenuButton2;
    public Button targetEnemyMenuButton3;
    public Button targetEnemyMenuButton4;
    public Button targetEnemyMenuButton5;

    //These are being used for showing the correct number of target enemy buttons depending on how many monsters you are fighting with
    public GameObject objTargetMenuButton0;
    public GameObject objTargetMenuButton1;
    public GameObject objTargetMenuButton2;
    public GameObject objTargetMenuButton3;
    public GameObject objTargetMenuButton4;
    public GameObject objTargetMenuButton5;

    //These are being used for highlighting the correct menu button for non-mobile input
    public Button attackButton;
    public Button skillButton;
    public Button itemButton;
    public Button retreatButton;
    public Button skillButton0;
    public Button itemButton0;
    public Button useItemButton;
    public Button targetCharacterButton1;

    [Header("Battle Positions")]
    //Positions of characters & enemies
    public Transform[] characterPositions;
    public Transform[] enemyPositions;

    [Header("Battle Prefabs")]
    //References to character & enemy prefabs
    public BattleCharacter[] characterPrefabs;
    public BattleCharacter[] enemyPrefabs;

    [Header("Battle Effects")]
    //References to battle effect prefabs
    public GameObject enemyAttackEffect;
    public GameObject characterTurnIndicator;
    public DamageNumber theDamageNumber;

    [Header("Battle Turns")]
    //For indication of the current turn
    public int currentTurn;
    public bool waitForTurn;

    [Header("Skills")]
    //Initiates a list of all available skills
    public BattleSkill[] skillList;
    public int skillCost;

    //For displaying the correct skill of each character
    public SelectSkill[] skillButtons;
    public Button[] skillButtonsB;

    [Header("Items")]
    //For displaying held items
    public ItemButton[] itemButtons;
    public Button[] itemButtonsB;
    public Image itemSprite;

    //For checking the currently selected item
    public Item activeItem;

    [Header("Battle Settings")]
    //Probability to retreat
    public int retreatRate = 35;
    private bool retreating;

    //Name of the game over scene
    public string gameOverScene;

    [Header("Rewards")]
    //Initialisation of rewards for the current/last battle
    public int rewardXP;
    public int rewardGold;
    public string[] rewardItems;
    public string[] rewardEquipItems;

    [Header("General")]
    //For checking if you are able to retreat from the current battle
    public bool noRetreat;

    //For the DelayCo() so the function knows which character is selected when using items
    int selectCharForItem;


    //For Reference
    public GameObject TileMap, Player, PlayerController;


    // Use this for initialization
    void Start()
    {
        instance = this;
        DontDestroyOnLoad(gameObject);
       
    }

    // Update is called once per frame
    void Update()
    {   
        //Check if any of these menus are active to be able to cancle out of them
        if(itemMenu.activeInHierarchy || targetEnemyMenu.activeInHierarchy || skillMenu.activeInHierarchy)
        {
            if(Input.GetButtonDown("RPGCanclePC") || Input.GetButtonDown("RPGCancleJoy") || CrossPlatformInputManager.GetButtonDown("RPGCancleTouch"))
                if(targetCharacterMenu.activeInHierarchy)
                {
                    //Highlight the correct button for non-mobile input when closing the menu
                    if (ControlManager.instance.mobile == false)
                    {
                        GameMenu.instance.btn = itemButton0;
                        GameMenu.instance.SelectFirstButton();
                    }
                    AudioManager.instance.PlaySFX(3);
                    targetCharacterMenu.SetActive(false);

                    ShowItems();

                    battleMenu.SetActive(true);
                    
                }else
                {
                    AudioManager.instance.PlaySFX(3);
                    //Highlight the correct button for non-mobile input when closing the menu
                    if (ControlManager.instance.mobile == false)
                    {
                        GameMenu.instance.btn = attackButton;
                        GameMenu.instance.SelectFirstButton();
                    }                    

                    //Close the menus
                    itemMenu.SetActive(false);

                    attackButton.interactable = true;
                    skillButton.interactable = true;
                    itemButton.interactable = true;
                    retreatButton.interactable = true;

                    //targetEnemyMenu.SetActive(false);
                    activeBattlers[currentTurn].currentSP += skillCost;
                    skillCost = 0;

                    if (!targetEnemyMenu.activeInHierarchy)
                    {
                        skillMenu.SetActive(false);
                        battleMenu.SetActive(true);
                    }

                    if (targetEnemyMenu.activeInHierarchy)
                    {
                        targetEnemyMenu.SetActive(false);

                        if (skillMenu.activeInHierarchy)
                        {
                            for (int i = 0; i < skillButtonsB.Length; i++)
                            {
                                skillButtonsB[i].interactable = true;
                            }

                            if (ControlManager.instance.mobile == false)
                            {
                                GameMenu.instance.btn = skillButton0;
                                GameMenu.instance.SelectFirstButton();
                            }
                        }
                        

                        if (!skillMenu.activeInHierarchy)
                        {
                            battleMenu.SetActive(true);
                        }
                    }

                    
                    //battleMenu.SetActive(true);
                }
        }

        //Check if a battle is active, display the battle menu and show turn indicator
        if (battleActive)
        {
            if (waitForTurn)
            {
                Instantiate(characterTurnIndicator, activeBattlers[currentTurn].transform.position, activeBattlers[currentTurn].transform.rotation);
                if (activeBattlers[currentTurn].character && !targetEnemyMenu.activeInHierarchy)
                {
                    //battleMenu.SetActive(true);
                    if (!targetCharacterMenu.activeInHierarchy)
                    {
                        currentTurnIndicator[currentTurn].SetActive(true);
                    }else
                    {
                        currentTurnIndicator[currentTurn].SetActive(false);
                    }

                    
                }
                else //Hide battle menu and start enemy's turn
                {
                    //battleMenu.SetActive(false);

                    if (!targetEnemyMenu.activeInHierarchy)
                    {
                        //Let enemy attack
                        StartCoroutine(EnemyMoveCo());
                    }
                }
            }
        }
    }

    public IEnumerator waitForSound()
    {
        //Wait Until Sound has finished playing
        while (AudioManager.instance.bgm[battleMusicIntro].isPlaying)
        {
            yield return null;
        }

        //Auidio has finished playing, disable GameObject
        AudioManager.instance.PlayBGM(battleMusic);
    }

    public void PlayButtonSound(int buttonSound)
    {
        AudioManager.instance.PlaySFX(buttonSound);
    }

    //Method to start battle, expects a string array of enemies and a bool to enable/disable retreat
    public void BattleStart(string[] enemiesToSpawn, bool setCannotFlee)
    {
        battleMenu.SetActive(true);

        

        //Check if mobile controlls are enabled and hide them during battle
        if (ControlManager.instance.mobile == true)
        {
            if (ControlManager.instance.mobile == true)
            {
                GameMenu.instance.touchMenuButton.SetActive(false);
                GameMenu.instance.touchController.SetActive(false);
                GameMenu.instance.touchConfirmButton.SetActive(false);
                touchBackButton.SetActive(true);
            }
        }

        //Highlight the attack button for non-mobile input
        if (ControlManager.instance.mobile == false)
        {
            GameMenu.instance.btn = attackButton;
            GameMenu.instance.SelectFirstButton();
        }

        //Put the correct values into the character status within the battle menu
        UpdateCharacterStatus();


        if (!battleActive)
        {
            //Will be true or false depending on the setting within the BattleStarter script
            noRetreat = setCannotFlee;

            battleActive = true;

            GameManager.instance.battleActive = true;

            //Put the battle background sprite into place
            transform.position = new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y, transform.position.z);
            spriteRenderer.sprite = battleBG;
            battleScene.SetActive(true);

            //Play battle music
            StartCoroutine(waitForSound());

            //Go through the cahracters participating in the battle and put the correct values for Names, HP and SP
            for (int i = 0; i < characterPositions.Length; i++)
            {
                //Check if caharacter at position i is active
                if (GameManager.instance.characterStatus[i].gameObject.activeInHierarchy)
                {
                    for (int j = 0; j < characterPrefabs.Length; j++)
                    {
                        if (characterPrefabs[j].characterName == GameManager.instance.characterStatus[i].characterName)
                        {
                            //Instantiate every active character at their i positions
                            BattleCharacter newCaracter = Instantiate(characterPrefabs[j], characterPositions[i].position, characterPositions[i].rotation);
                            newCaracter.transform.parent = characterPositions[i];
                            activeBattlers.Add(newCaracter);
                            
                            //Give each character the correct stats from the GameManager script
                            CharacterStatus character = GameManager.instance.characterStatus[i];
                            activeBattlers[i].currentHp = character.currentHP;
                            activeBattlers[i].maxHP = character.maxHP;
                            activeBattlers[i].currentSP = character.currentSP;
                            activeBattlers[i].maxSP = character.maxSP;
                            activeBattlers[i].strength = character.strength;
                            activeBattlers[i].defense = character.defence;
                            activeBattlers[i].weaponStrength = character.offenseStrength;
                            activeBattlers[i].armorStrength = character.defenseStrength;
                        }
                    }
                }
            }

            //Go through the list of enemies and put them in position
            for (int i = 0; i < enemiesToSpawn.Length; i++)
            {
                if (enemiesToSpawn[i] != "")
                {
                    for (int j = 0; j < enemyPrefabs.Length; j++)
                    {
                        if (enemyPrefabs[j].characterName == enemiesToSpawn[i])
                        {
                            //Instantiate every enemy at their i positions
                            BattleCharacter newEnemy = Instantiate(enemyPrefabs[j], enemyPositions[i].position, enemyPositions[i].rotation);
                            newEnemy.transform.parent = enemyPositions[i];
                            activeBattlers.Add(newEnemy);
                            
                        }
                    }
                }
            }

            /*
             * Wird jetzt von Animator übernommen
             * 
            for (int i = 0; i < activeBattlers.Count; i++)
            {
                if (activeBattlers[i].characterName == "Rem")
                {
                    activeBattlers[i].anim["Battle_idle"].wrapMode = WrapMode.Loop;
                    activeBattlers[i].anim.Play("Battle_idle");
                }else
                {
                    activeBattlers[i].anim["Battle_idle"].wrapMode = WrapMode.Loop;
                    activeBattlers[i].anim.Play("Battle_idle");
                }

                

            }
            */

            //Randomize turn order
            waitForTurn = true;
            currentTurn = Random.Range(0, activeBattlers.Count);

            if (!activeBattlers[currentTurn].character)
            {
                attackButton.interactable = false;
                skillButton.interactable = false;
                itemButton.interactable = false;
                retreatButton.interactable = false;
            }else
            {
                attackButton.interactable = true;
                skillButton.interactable = true;
                itemButton.interactable = true;
                retreatButton.interactable = true;
            }

            UpdateCharacterStatus();
        }
    }

    //Method to start a new turn
    public void NextTurn()
    {
        skillMenu.SetActive(false);
        
            attackButton.interactable = true;
            skillButton.interactable = true;
            itemButton.interactable = true;
            retreatButton.interactable = true;
        
        

        battleMenu.SetActive(true);
        if (ControlManager.instance.mobile == false)
        {
            GameMenu.instance.btn = attackButton;
            GameMenu.instance.SelectFirstButton();
        }        

        currentTurnIndicator[currentTurn].SetActive(false);
        currentTurn++;
        if (currentTurn >= activeBattlers.Count)
        {
            currentTurn = 0;
        }

        waitForTurn = true;

        UpdateBattle();
        UpdateCharacterStatus();

        if (!activeBattlers[currentTurn].character)
        {
            attackButton.interactable = false;
            skillButton.interactable = false;
            itemButton.interactable = false;
            retreatButton.interactable = false;
        }
    }

    //Method for updating battle objects
    public void UpdateBattle()
    {
        bool allEnemiesDead = true;
        bool allPlayersDead = true;

        for (int i = 0; i < activeBattlers.Count; i++)
        {
            if (activeBattlers[i].currentHp < 0)
            {
                activeBattlers[i].currentHp = 0;
            }

            if (activeBattlers[i].currentHp == 0)
            {
                //Show dead character
                if (activeBattlers[i].character)
                {
                    //activeBattlers[i].spriteRenderer.sprite = activeBattlers[i].defeatedSprite;
                    activeBattlers[i].anim.SetTrigger("Defeated");

                    activeBattlers[i].defeated = true;
                }
                else
                {
                    activeBattlers[i].EnemyFade();
                                    }

            }
            else
            {
                activeBattlers[i].anim.SetTrigger("Battle_idle");
                if (activeBattlers[i].character)
                {
                    allPlayersDead = false;
                    activeBattlers[i].spriteRenderer.sprite = activeBattlers[i].aliveSprite;
                }
                else
                {
                    allEnemiesDead = false;
                }
            }
        }

        if (allEnemiesDead || allPlayersDead)
        {
            if (allEnemiesDead)
            {
                //Battle won
                StartCoroutine(EndBattleCo());
            }
            else
            {
                //Battle lost
                StartCoroutine(GameOverCo());
            }
        }
        else
        {
            while (activeBattlers[currentTurn].currentHp == 0)
            {
                currentTurn++;
                if (currentTurn >= activeBattlers.Count)
                {
                    currentTurn = 0;
                }
            }
        }
    }

    //Coroutine to wait some seconds between enemy attacks
    public IEnumerator EnemyMoveCo()
    {
        waitForTurn = false;
        yield return new WaitForSeconds(1f);
        EnemyAttack();
        yield return new WaitForSeconds(1f);
        NextTurn();
    }

    //Method for enemy attacks
    public void EnemyAttack()
    {
        attackButton.interactable = false;
        skillButton.interactable = false;
        itemButton.interactable = false;
        retreatButton.interactable = false;

        List<int> players = new List<int>();
        for (int i = 0; i < activeBattlers.Count; i++)
        {
            if (activeBattlers[i].character && activeBattlers[i].currentHp > 0)
            {
                players.Add(i);
            }
        }
        int selectedTarget = players[Random.Range(0, players.Count)];

        int selectAttack = Random.Range(0, activeBattlers[currentTurn].skills.Length);
        int movePower = 0;
        for (int i = 0; i < skillList.Length; i++)
        {
            if (skillList[i].skillName == activeBattlers[currentTurn].skills[selectAttack])
            {
                Instantiate(skillList[i].effect, activeBattlers[selectedTarget].transform.position, activeBattlers[selectedTarget].transform.rotation);
                movePower = skillList[i].skillPower;
            }
        }

        //Instantiate(enemyAttackEffect, activeBattlers[currentTurn].transform.position, activeBattlers[currentTurn].transform.rotation);
        activeBattlers[currentTurn].anim.SetTrigger("Attack");

               
        DealDamage(selectedTarget, movePower);
    }

    //Method for calculating dealt damage
    public void DealDamage(int target, int movePower)
    {
        float atkPwr = activeBattlers[currentTurn].strength + activeBattlers[currentTurn].weaponStrength;
        float defPwr = activeBattlers[target].defense + activeBattlers[target].armorStrength;

        float damageCalc = (atkPwr / defPwr) * movePower * Random.Range(.9f, 1.1f);
        int damageToGive = Mathf.RoundToInt(damageCalc);

        Debug.Log(activeBattlers[currentTurn].characterName + " is dealing " + damageCalc + "(" + damageToGive + ") damage to " + activeBattlers[target].characterName);

        activeBattlers[target].currentHp -= damageToGive;

        Instantiate(theDamageNumber, activeBattlers[target].transform.position, activeBattlers[target].transform.rotation).SetDamage(damageToGive);
        
        //Play Take_Damage animation on character
        if (activeBattlers[target].character)
        {
            activeBattlers[target].anim.SetTrigger("Take_Damage");
        }

        //Play Take_Damage animation on enemy
        if (!activeBattlers[target].character)
        {
            activeBattlers[target].anim.SetTrigger("Take_Damage");
        }

        UpdateCharacterStatus();

        StartCoroutine(WaitCo(target));
    }

    public IEnumerator WaitCo(int target)
    {
        yield return new WaitForSeconds(1);
        activeBattlers[target].anim.SetTrigger("Battle_idle");
        activeBattlers[currentTurn].anim.SetTrigger("Battle_idle");
    }

    //Method for updating character status
    public void UpdateCharacterStatus()
    {
        playerStats = GameManager.instance.characterStatus;
        for (int i = 0; i < characterName.Length; i++)
        {
            if (activeBattlers.Count > i)
            {
                if (activeBattlers[i].character)
                {
                    BattleCharacter playerData = activeBattlers[i];

                    CharacterSlot[i].SetActive(true);
                    characterName[i].gameObject.SetActive(true);
                    characterName[i].text = playerData.characterName;
                    characterHP[i].text = Mathf.Clamp(playerData.currentHp, 0, int.MaxValue) + "/" + playerData.maxHP;
                    HPSlider[i].maxValue = playerData.maxHP;
                    HPSlider[i].value = playerData.currentHp;
                    characterSP[i].text = Mathf.Clamp(playerData.currentSP, 0, int.MaxValue) + "/" + playerData.maxSP;
                    SPSlider[i].maxValue = playerData.maxSP;
                    SPSlider[i].value = playerData.currentSP;
                    portrait[i].sprite = playerData.portrait;
                    characterLevel[i].text = "Lv " + playerStats[i].level;
                }
                else
                {
                    characterName[i].gameObject.SetActive(false);
                }
            }
            else
            {
                characterName[i].gameObject.SetActive(false);
            }
        }
    }

    //Method for player attack
    public void PlayerAttack(string moveName, int selectedTarget)
    {
        StartCoroutine(DelayAttackCo(moveName, selectedTarget));

    }

    //Adds a slight delay between choosing the target and affecting the target with the item
    public IEnumerator DelayAttackCo(string moveName, int selectedTarget)
    {
        

        yield return new WaitForSeconds(.5f);

        targetEnemyMenu.SetActive(false);
        
        int movePower = 0;

        
            for (int i = 0; i < skillList.Length; i++)
            {
                if (skillList[i].skillName == moveName)
                {
                    Instantiate(skillList[i].effect, activeBattlers[selectedTarget].transform.position, activeBattlers[selectedTarget].transform.rotation);
                    movePower = skillList[i].skillPower;
                }
            }
        

        //Instantiate(enemyAttackEffect, activeBattlers[currentTurn].transform.position, activeBattlers[currentTurn].transform.rotation);
        activeBattlers[currentTurn].anim.SetTrigger("Attack");
        DealDamage(selectedTarget, movePower);
        yield return new WaitForSeconds(1);
        activeBattlers[currentTurn].anim.SetTrigger("Battle_idle");
        battleMenu.SetActive(false);


        NextTurn();
    }

    //Method to opening the target enemy menu. Also shows the correct number of enemy buttons
    public void OpenTargetEnemyMenu(string attackName)
    {

        if (!skillMenu.activeInHierarchy)
        {
            skillButton.interactable = false;
            itemButton.interactable = false;
            retreatButton.interactable = false;
        }
        

        for (int i = 0; i < skillButtonsB.Length; i++)
        {
            if (attackName != skillButtons[i].nameText.text)
            {
                skillButtonsB[i].interactable = false;
            }
        }
        

        targetEnemyMenu.SetActive(true);
        List<int> Enemies = new List<int>();
        for (int i = 0; i < activeBattlers.Count; i++)
        {
            if (!activeBattlers[i].character)
            {
                Enemies.Add(i);
            }
        }

        for (int i = 0; i < targetEnemyButtons.Length; i++)
        {
            if (Enemies.Count > i && activeBattlers[Enemies[i]].currentHp > 0)
            {
                targetEnemyButtons[i].gameObject.SetActive(true);

                targetEnemyButtons[i].moveName = attackName;
                targetEnemyButtons[i].activeBattlerTarget = Enemies[i];
                targetEnemyButtons[i].targetName.text = activeBattlers[Enemies[i]].characterName + " " + (1+i);
            }
            else
            {
                targetEnemyButtons[i].gameObject.SetActive(false);
            }
        }

        if (!objTargetMenuButton0.activeInHierarchy)
        {
            if (objTargetMenuButton1.activeInHierarchy || objTargetMenuButton2.activeInHierarchy || objTargetMenuButton3.activeInHierarchy || objTargetMenuButton4.activeInHierarchy || objTargetMenuButton5.activeInHierarchy)
            {
                if(ControlManager.instance.mobile == false)
                {
                    
                    if (objTargetMenuButton3.activeInHierarchy)
                    {
                        GameMenu.instance.btn = targetEnemyMenuButton3;
                        GameMenu.instance.SelectFirstButton();
                    }
                    if (objTargetMenuButton2.activeInHierarchy)
                    {
                        GameMenu.instance.btn = targetEnemyMenuButton2;
                        GameMenu.instance.SelectFirstButton();
                    }
                    if (objTargetMenuButton1.activeInHierarchy)
                    {
                        GameMenu.instance.btn = targetEnemyMenuButton1;
                        GameMenu.instance.SelectFirstButton();
                    }
                }                
            }                
        }
        else
        {
            if(ControlManager.instance.mobile == false)
            {
                GameMenu.instance.btn = targetEnemyMenuButton0;
                GameMenu.instance.SelectFirstButton();
            }            
        }
    }

    public void OpenSkillMenu()
    {
        for (int i = 0; i < skillButtonsB.Length; i++)
        {
            skillButtonsB[i].interactable = true;
        }

        //closeMenu = true;
        battleMenu.SetActive(false);
        if(ControlManager.instance.mobile == false)
        {
            GameMenu.instance.btn = skillButton0;
            GameMenu.instance.SelectFirstButton();
        }


        skillMenu.SetActive(true);

        for (int i = 0; i < skillButtons.Length; i++)
        {
            if (activeBattlers[currentTurn].skills.Length > i)
            {
                skillButtons[i].gameObject.SetActive(true);

                skillButtons[i].skill = activeBattlers[currentTurn].skills[i];
                skillButtons[i].nameText.text = skillButtons[i].skill;

                for (int j = 0; j < skillList.Length; j++)
                {
                    if (skillList[j].skillName == skillButtons[i].skill)
                    {
                        skillButtons[i].skillCost = skillList[j].skillCost;
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

    //Method to calculate retreat possibility
    public void Retreat()
    {
        //Shows the following message if retreat from battle is disabled
        if (noRetreat)
        {
            battlePrompts.notificationText.text = "Retreat is impossible!";
            battlePrompts.Activate();
        }
        else
        {
            //Calculation of retreat possibility
            int fleeSuccess = Random.Range(0, 100);
            if (fleeSuccess < retreatRate)
            {
                retreating = true;
                StartCoroutine(EndBattleCo());
            }
            else
            {
                //Shows the following message if retreat failed
                NextTurn();
                battlePrompts.notificationText.text = "Retreat failed!";
                battlePrompts.Activate();
            }
        }

    }

    void ResetBattleScene()
    {
        attackButton.interactable = false;
        skillButton.interactable = false;
        itemButton.interactable = false;
        retreatButton.interactable = false;

        //Deactivate battle scene
        battleActive = false;

        targetEnemyMenu.SetActive(false);
        skillMenu.SetActive(false);
    }

    //Coroutine to end a battle
    public IEnumerator EndBattleCo()
    {
        ResetBattleScene();

        yield return new WaitForSeconds(.5f);
        AudioManager.instance.PlayBGM(victoryMusicIntro);

        //Wait Until Sound has finished playing
        while (AudioManager.instance.bgm[victoryMusicIntro].isPlaying)
        {
            yield return null;
        }

        AudioManager.instance.PlayBGM(victoryMusic);

        //battleMenu.SetActive(false);
        
        yield return new WaitForSeconds(.5f);

        ScreenFade.instance.FadeToBlack();

        yield return new WaitForSeconds(1.5f);

        //Update current HP and SP in GameManager script
        for (int i = 0; i < activeBattlers.Count; i++)
        {
            if (activeBattlers[i].character)
            {
                for (int j = 0; j < GameManager.instance.characterStatus.Length; j++)
                {
                    if (activeBattlers[i].characterName == GameManager.instance.characterStatus[j].characterName)
                    {
                        GameManager.instance.characterStatus[j].currentHP = activeBattlers[i].currentHp;
                        GameManager.instance.characterStatus[j].currentSP = activeBattlers[i].currentSP;
                    }
                }
            }

            Destroy(activeBattlers[i].gameObject);
        }



        ScreenFade.instance.FadeFromBlack();
        battleScene.SetActive(false);
        activeBattlers.Clear();
        currentTurn = 0;

        GameObject.FindObjectOfType<Camera>().orthographic = true;
        TileMap.gameObject.SetActive(true);
        Player.SetActive(true);
        if(GameObject.FindObjectOfType<ControlManager>().mobile)
            PlayerController.SetActive(true);

        if (retreating)
        {
            GameManager.instance.battleActive = false;
            retreating = false;
            AudioManager.instance.PlayBGM(FindObjectOfType<CameraController>().musicToPlay);

            if (ControlManager.instance.mobile == true)
            {
                GameMenu.instance.touchMenuButton.SetActive(true);
                GameMenu.instance.touchController.SetActive(true);
                GameMenu.instance.touchConfirmButton.SetActive(true);
            }
        }
        else
        {
            RewardScreen.instance.OpenRewardScreen(rewardXP, rewardGold, rewardItems, rewardEquipItems);
        }

        //AudioManager.instance.PlayBGM(FindObjectOfType<CameraController>().musicToPlay);
    }

    //Coroutine to show game over screen
    public IEnumerator GameOverCo()
    {
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


        //ResetBattleScene();

        attackButton.interactable = false;
        skillButton.interactable = false;
        itemButton.interactable = false;
        retreatButton.interactable = false;

        //Deactivate battle scene

        
        battleActive = false;

        targetEnemyMenu.SetActive(false);
        skillMenu.SetActive(false);

        ScreenFade.instance.FadeToBlack();
        yield return new WaitForSeconds(1.5f);

        GameObject.FindObjectOfType<Camera>().orthographic = true;
        TileMap.gameObject.SetActive(true);
        Player.SetActive(true);
        if (GameObject.FindObjectOfType<ControlManager>().mobile)
            PlayerController.SetActive(true);

        //Destroy(activeBattlers[0]);
        battleScene.SetActive(false);

        for (int i = 0; i < activeBattlers.Count; i++)
        {
            Destroy(activeBattlers[i].gameObject);
        }

        activeBattlers.Clear();
        currentTurn = 0;
        GameManager.instance.battleActive = false;

        SceneManager.LoadScene(gameOverScene);
    }

    //Method for showing the correct amount of items during battle
    public void ShowItems()
    {
        GameManager.instance.SortItems();
        
        itemMenu.SetActive(true);

        itemButton.interactable = false;
        skillButton.interactable = false;
        attackButton.interactable = false;
        retreatButton.interactable = false;

        //Set button navigation mode to automatic
        customNav.mode = Navigation.Mode.Automatic;

        for (int i = 0; i < itemButtons.Length; i++)
        {
            itemButtonsB[i].interactable = true;
        }

        for (int i = 0; i < itemButtons.Length; i++)
        {
            itemButtons[i].buttonValue = i;
            hilightedBattleItem[i].buttonValue = i;

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

            //Checks if there are any items stored in the GameManager script and sorts them in the battle item menu
            if (GameManager.instance.itemsHeld[i] != "")
            {
                //itemButtons[i].buttonImage.gameObject.SetActive(true);
                //itemButtons[i].buttonImage.sprite = GameManager.instance.GetItemDetails(GameManager.instance.itemsHeld[i]).itemSprite;
                itemButtons[i].amountText.text = GameManager.instance.GetItemDetails(GameManager.instance.itemsHeld[i]).itemName;
            }
            else
            {
                //itemButtons[i].buttonImage.gameObject.SetActive(false);
                itemButtons[i].amountText.text = "";
            }
        }
        
            GameMenu.instance.btn = itemButton0;
            GameMenu.instance.SelectFirstButton();
            
    }

    //Method for returning the chosen item and show the "use" button during battle
    public void SelectBattleItem(Item newItem)
    {
        activeItem = newItem;
        GameMenu.instance.activeItem = newItem;

        if (activeItem.item)
        {
            battleUseButtonText.text = "Use";
        }

        if (activeItem.offense || activeItem.defense)
        {
            battleUseButtonText.text = "Equip";
        }

        battleItemName.text = activeItem.itemName;
        battleItemDescription.text = activeItem.description;
        itemSprite.sprite = activeItem.itemSprite;
    }

    //Method for showing the item chracter choice menu. Also shows the correct number of character buttons
    public void OpenItemCharChoice()
    {
        
        //In non-mobile disable every item button except for selected item button
        for (int i = 0; i < GameManager.instance.itemsHeld.Length; i++)
            {
                if (i != buttonValue)
                {
                    itemButtonsB[i].interactable = false;
                }

            }

        if (ControlManager.instance.mobile == false)
        {
            GameMenu.instance.btn = targetCharacterButton1;
            GameMenu.instance.SelectFirstButton();
        }

        targetCharacterMenu.SetActive(true);

        for (int i = 0; i < targetCharacterName.Length; i++)
        {
            targetCharacterName[i].text = GameManager.instance.characterStatus[i].characterName;
            targetCharacterName[i].transform.parent.gameObject.SetActive(GameManager.instance.characterStatus[i].gameObject.activeInHierarchy);
        }
    }

    public void CloseItemCharChoice()
    {
        targetCharacterMenu.SetActive(false);
    }

    public void UseItem(int selectChar)
    {
        //Set selectCharForItem to be the same as the target character for DelayCo()
        selectCharForItem = selectChar;
        //StartCoroutine(DelayItemCo());

        activeItem.UseBattleItem(selectChar);
        battleMenu.SetActive(false);
        itemButton.interactable = true;
        skillButton.interactable = true;
        attackButton.interactable = true;
        retreatButton.interactable = true;

        //Check if the item could be used before progressing to next turn
        if (usable)
        {
            StartCoroutine(DelayItemCo());
            activeBattlers[selectChar].spriteRenderer.sprite = activeBattlers[selectChar].aliveSprite;
            usable = false;
        }
    }

    public IEnumerator DelayItemCo()
    {

            yield return new WaitForSeconds(1);
            Instantiate(theDamageNumber, activeBattlers[selectCharForItem].transform.position, activeBattlers[selectCharForItem].transform.rotation).SetDamage(activeItem.amountToChange);
            
            NextTurn();
        
    }
   
}
