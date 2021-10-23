using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStatus : MonoBehaviour {
    [Header("Default Character Settings")]
    public string characterName;
    public int level = 1;
    public int maxLevel;
    public int currentHP;
    public int maxHP;
    public int currentSP;
    public int maxSP;
    public int currentEXP;
    public string[] skills;

    [Header("EXP Settings")]
    public int firstNextLevelEXP;
    public float multiplicationFactor = 1.06f;
    public bool manualEXP;    
    public int[] eXPToNextLevel;

    [Header("Level Bonus Settings")]
    //Check this bool if you want to assign the hp bonus manually
    public bool manualHpBonus;
    public int[] HpLevelBonus;
    //Check this bool if you want to assign the sp bonus manually
    public bool manualSpBonus;
    public int[] SpLevelBonus;

    [Header("Battle values")]
    public int strength;
    public int defence;
    public int offenseStrength;
    public int defenseStrength;
    public string equippedOffenseItem;
    public string equippedDefenseItem;
    public Sprite characterIamge;
    
    // Use this for initialization
    void Start () {
        
        if (!manualEXP)
        {
            eXPToNextLevel = new int[maxLevel];
            eXPToNextLevel[1] = firstNextLevelEXP;

            for (int i = 2; i < eXPToNextLevel.Length; i++)
            {
                eXPToNextLevel[i] = Mathf.FloorToInt(eXPToNextLevel[i - 1] * multiplicationFactor + 10);
            }
        }
        
	}
	
	// Update is called once per frame
	void Update () {

	}

    public void AddExp(int expToAdd)
    {
        currentEXP += expToAdd;

        if (level < maxLevel)
        {
            if (currentEXP >= eXPToNextLevel[level])
            {
                currentEXP -= eXPToNextLevel[level];

                level++;

                //determine whether to add to str or def based on odd or even
                if (level % 2 == 0)
                {
                    strength++;
                }
                else
                {
                    defence++;
                }

                if (manualHpBonus)
                {
                    maxHP += HpLevelBonus[level];
                }else
                {
                    maxHP = Mathf.FloorToInt(maxHP * 1.05f);
                }
                currentHP = maxHP;

                
                if (manualSpBonus)
                {
                    maxSP += SpLevelBonus[level];
                }else
                {
                    maxSP = Mathf.FloorToInt(maxSP * 1.2f);
                }
                currentSP = maxSP;
            }
        }

        if(level >= maxLevel)
        {
            currentEXP = 0;
        }
    }
}
