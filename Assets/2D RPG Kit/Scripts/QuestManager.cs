using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour {
    //Make instance of this script to be able reference from other scripts!
    public static QuestManager instance;

    [Header("Quest Settings")]
    public string[] quests;
    public bool[] completedQuests;
    
	// Use this for initialization
	void Start () {
        instance = this;

        completedQuests = new bool[quests.Length];
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    //Get the number of a quest
    public int GetQuestNumber(string questToFind)
    {
        for(int i = 0; i < quests.Length; i++)
        {
            if(quests[i] == questToFind)
            {
                return i;
            }
        }

        Debug.LogError("Quest " + questToFind + " does not exist");
        return 0;
    }

    //Check if a quest was completed
    public bool CheckIfComplete(string questToCheck)
    {
        if(GetQuestNumber(questToCheck) != 0)
        {
            return completedQuests[GetQuestNumber(questToCheck)];
        }

        return false;
    }

    //Complete quest
    public void MarkQuestComplete(string questToMark)
    {
        completedQuests[GetQuestNumber(questToMark)] = true;

        UpdateLocalQuestObjects();
    }

    //Put a completed quest back to incomplete
    public void MarkQuestIncomplete(string questToMark)
    {
        completedQuests[GetQuestNumber(questToMark)] = false;

        UpdateLocalQuestObjects();
    }

    //Update game objects associated with a quest
    public void UpdateLocalQuestObjects()
    {
        QuestObjectActivator[] questObjects = FindObjectsOfType<QuestObjectActivator>();

        if(questObjects.Length > 0)
        {
            for(int i = 0; i < questObjects.Length; i++)
            {
                questObjects[i].CheckCompletion();
            }
        }
    }

    //Save quest data
    public void SaveQuestData()
    {
        for(int i = 0; i < quests.Length; i++)
        {
            if(completedQuests[i])
            {
                PlayerPrefs.SetInt("QuestMarker_" + quests[i], 1);
            } else
            {
                PlayerPrefs.SetInt("QuestMarker_" + quests[i], 0);
            }
        }
    }

    //Load quest data
    public void LoadQuestData()
    {
        for(int i = 0; i < quests.Length; i++)
        {
            int valueToSet = 0;
            if(PlayerPrefs.HasKey("QuestMarker_" + quests[i]))
            {
                valueToSet = PlayerPrefs.GetInt("QuestMarker_" + quests[i]);
            }

            if(valueToSet == 0)
            {
                completedQuests[i] = false;
            } else
            {
                completedQuests[i] = true;
            }
        }
    }
}
