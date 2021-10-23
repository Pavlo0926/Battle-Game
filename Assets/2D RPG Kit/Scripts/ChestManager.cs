using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestManager : MonoBehaviour
{
    //Make instance of this script to be able reference from other scripts!
    public static ChestManager instance;

    [Header("Chest Settings")]
    public string[] chests;
    public bool[] openedChests;

    // Use this for initialization
    void Start()
    {
        instance = this;

        openedChests = new bool[chests.Length];
    }

    // Update is called once per frame
    void Update()
    {

    }

    //Get the number of a quest
    public int GetChestNumber(string chestToFind)
    {
         for (int i = 0; i < chests.Length; i++)
         {
             if (chests[i] == chestToFind)
             {
                 return i;
             }
         }

         Debug.LogError("Chest " + chestToFind + " does not exist");
         return 0;
    }

    //Check if a quest was completed
    public bool CheckIfOpened(string chestToCheck)
    {
        if (GetChestNumber(chestToCheck) != 0)
        {
            return openedChests[GetChestNumber(chestToCheck)];
        }

        return false;
    }

    //Complete quest
    public void MarkChestOpened(string chestToMark)
    {
        openedChests[GetChestNumber(chestToMark)] = true;

        UpdateLocalChestObjects();
    }

    //Put a completed quest back to incomplete
    public void MarkQuestIncomplete(string chestToMark)
    {
        openedChests[GetChestNumber(chestToMark)] = false;

        UpdateLocalChestObjects();
    }

    //Update game objects associated with a quest
    public void UpdateLocalChestObjects()
    {
        ChestObjectActivator[] chestObjects = FindObjectsOfType<ChestObjectActivator>();

        if (chestObjects.Length > 0)
        {
            for (int i = 0; i < chestObjects.Length; i++)
            {
                chestObjects[i].CheckCompletion();
            }
        }
    }

    //Save quest data
    public void SaveChestData()
    {
        for (int i = 0; i < chests.Length; i++)
        {
            if (openedChests[i])
            {
                PlayerPrefs.SetInt("ChestMarker_" + chests[i], 1);
            }
            else
            {
                PlayerPrefs.SetInt("ChestMarker_" + chests[i], 0);
            }
        }
    }

    //Load quest data
    public void LoadChestData()
    {
        for (int i = 0; i < chests.Length; i++)
        {
            int valueToSet = 0;
            if (PlayerPrefs.HasKey("ChestMarker_" + chests[i]))
            {
                valueToSet = PlayerPrefs.GetInt("ChestMarker_" + chests[i]);
            }

            if (valueToSet == 0)
            {
                openedChests[i] = false;
            }
            else
            {
                openedChests[i] = true;
            }
        }
    }
}
