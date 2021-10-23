using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{

    [Header("Item Type")]
    public bool item;
    public bool revive;
    public bool offense;
    public bool defense;

    [Header("Item Details")]
    public string itemName;
    public string description;
    public int price;
    public int sellPrice;
    public Sprite itemSprite;

    [Header("Item Details")]
    public bool affectHP;
    public bool affectMP;
    public int amountToChange;
    
    [Header("Weapon/Armor Details")]
    public int offenseStrength;

    public int defenseStrength;


    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void UseBattleItem(int charToUseOn)
    {
        if (GameManager.instance.battleActive)
        {
            if (item)
            {
                //Check if item affects HP but doesn't revive
                if (affectHP && !revive)
                {
                    //Check if character has at least 1 HP & isn't fully healed 
                    if (BattleManager.instance.activeBattlers[charToUseOn].currentHp > 0 && BattleManager.instance.activeBattlers[charToUseOn].currentHp != BattleManager.instance.activeBattlers[charToUseOn].maxHP)
                    {
                        //Tell battlemanager that the item is able to be used
                        BattleManager.instance.usable = true;

                        BattleManager.instance.UpdateCharacterStatus();
                        BattleManager.instance.UpdateBattle();

                        BattleManager.instance.activeBattlers[charToUseOn].currentHp += amountToChange;

                        //Make item healing within the characters max HP
                        if (BattleManager.instance.activeBattlers[charToUseOn].currentHp > BattleManager.instance.activeBattlers[charToUseOn].maxHP)
                        {
                            BattleManager.instance.activeBattlers[charToUseOn].currentHp = BattleManager.instance.activeBattlers[charToUseOn].maxHP;
                        }

                        GameManager.instance.RemoveItem(itemName);
                        AudioManager.instance.PlaySFX(2);
                        BattleManager.instance.CloseItemCharChoice();
                        BattleManager.instance.itemMenu.SetActive(false);
                        BattleManager.instance.battleMenu.SetActive(true);
                        
                    }
                    else
                    {
                        AudioManager.instance.PlaySFX(3);
                    }

                }

                //Check if item affects HP and revives
                if (affectHP && revive)
                {
                    //Check if character is defeated before reviving
                    if (BattleManager.instance.activeBattlers[charToUseOn].currentHp == 0)
                    {
                        //Tell battlemanager that the item is able to be used
                        BattleManager.instance.usable = true;

                        BattleManager.instance.UpdateCharacterStatus();
                        BattleManager.instance.UpdateBattle();

                        BattleManager.instance.activeBattlers[charToUseOn].currentHp += amountToChange;

                        //Make item healing within the characters max HP
                        if (BattleManager.instance.activeBattlers[charToUseOn].currentHp > BattleManager.instance.activeBattlers[charToUseOn].maxHP)
                        {
                            BattleManager.instance.activeBattlers[charToUseOn].currentHp = BattleManager.instance.activeBattlers[charToUseOn].maxHP;
                        }

                        GameManager.instance.RemoveItem(itemName);
                        AudioManager.instance.PlaySFX(2);
                        BattleManager.instance.CloseItemCharChoice();
                        BattleManager.instance.itemMenu.SetActive(false);
                        BattleManager.instance.battleMenu.SetActive(true);
                        
                    }
                    else
                    {
                        AudioManager.instance.PlaySFX(3);
                    }

                }

                //Check if item affects SP & character is not defeated
                if (affectMP && BattleManager.instance.activeBattlers[charToUseOn].currentHp > 0)
                {
                    //Check if SP needs to be healed
                    if (BattleManager.instance.activeBattlers[charToUseOn].currentSP != BattleManager.instance.activeBattlers[charToUseOn].maxSP)
                    {
                        //Tell battlemanager that the item is able to be used
                        BattleManager.instance.usable = true;

                        BattleManager.instance.UpdateCharacterStatus();
                        BattleManager.instance.UpdateBattle();

                        BattleManager.instance.activeBattlers[charToUseOn].currentSP += amountToChange;

                        //Make item healing within the characters max SP
                        if (BattleManager.instance.activeBattlers[charToUseOn].currentSP > BattleManager.instance.activeBattlers[charToUseOn].maxSP)
                        {
                            BattleManager.instance.activeBattlers[charToUseOn].currentSP = BattleManager.instance.activeBattlers[charToUseOn].maxSP;
                        }

                        GameManager.instance.RemoveItem(itemName);
                        AudioManager.instance.PlaySFX(2);
                        BattleManager.instance.CloseItemCharChoice();
                        BattleManager.instance.itemMenu.SetActive(false);
                        BattleManager.instance.battleMenu.SetActive(true);
                        
                    }
                    else
                    {
                        AudioManager.instance.PlaySFX(3);
                    }

                }
                if (affectMP && BattleManager.instance.activeBattlers[charToUseOn].currentHp == 0)
                {
                    AudioManager.instance.PlaySFX(3);
                }
            }
        }
    }

    public void Use( int charToUseOn)
    {

        CharacterStatus selectedChar = GameManager.instance.characterStatus[charToUseOn];
        
        if (item)
        {
            if (affectHP && !revive)
            {
                if (selectedChar.currentHP > 0 && selectedChar.currentHP != selectedChar.maxHP)
                {
                    selectedChar.currentHP += amountToChange;
                    BattleManager.instance.affectHP = true;

                    if (selectedChar.currentHP > selectedChar.maxHP)
                    {
                        selectedChar.currentHP = selectedChar.maxHP;
                    }

                    GameManager.instance.RemoveItem(itemName);
                    AudioManager.instance.PlaySFX(2);
                    GameMenu.instance.CompleteUseItem();
                }
                else
                {
                    AudioManager.instance.PlaySFX(3);
                }

            }

            if (affectHP && revive)
            {
                if (selectedChar.currentHP == 0)
                {
                    selectedChar.currentHP += amountToChange;
                    BattleManager.instance.affectHP = true;

                    if (selectedChar.currentHP > selectedChar.maxHP)
                    {
                        selectedChar.currentHP = selectedChar.maxHP;
                    }

                    GameManager.instance.RemoveItem(itemName);
                    AudioManager.instance.PlaySFX(2);
                    GameMenu.instance.CompleteUseItem();
                }
                else
                {
                    AudioManager.instance.PlaySFX(3);
                }

            }

            if (affectMP && selectedChar.currentHP > 0)
            {
                if (selectedChar.currentSP != selectedChar.maxSP)
                {
                    selectedChar.currentSP += amountToChange;
                    BattleManager.instance.affectSP = true;

                    if (selectedChar.currentSP > selectedChar.maxSP)
                    {
                        selectedChar.currentSP = selectedChar.maxSP;
                    }

                    GameManager.instance.RemoveItem(itemName);
                    AudioManager.instance.PlaySFX(2);
                    GameMenu.instance.CompleteUseItem();
                }
                else
                {
                    AudioManager.instance.PlaySFX(3);
                }

            }
            if (affectMP && selectedChar.currentHP == 0)
            {
                AudioManager.instance.PlaySFX(3);
            }

            /*if (affectStr)
              {
                  selectedChar.strength += amountToChange;

                  GameManager.instance.RemoveItem(itemName);
                  AudioManager.instance.PlaySFX(2);
                  GameMenu.instance.CompleteUseItem();
            }*/


        }

        if (offense)
        {
            if (selectedChar.equippedOffenseItem != "")
            {

                GameManager.instance.EquipItem(selectedChar.equippedOffenseItem);
            }

            selectedChar.equippedOffenseItem = itemName;
            selectedChar.offenseStrength = offenseStrength;

            GameManager.instance.RemoveItem(itemName);
            AudioManager.instance.PlaySFX(2);
            GameMenu.instance.CompleteUseItem();
        }

        if (defense)
        {
            if (selectedChar.equippedDefenseItem != "")
            {
                GameManager.instance.EquipItem(selectedChar.equippedDefenseItem);
            }

            selectedChar.equippedDefenseItem = itemName;
            selectedChar.defenseStrength = defenseStrength;

            GameManager.instance.RemoveItem(itemName);
            AudioManager.instance.PlaySFX(2);
            GameMenu.instance.CompleteUseItem();
        }

    }
}

