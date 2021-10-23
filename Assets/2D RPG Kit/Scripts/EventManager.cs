using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    //Make instance of this script to be able reference from other scripts!
    public static EventManager instance;

    [Header("Event Settings")]
    public string[] events;
    public bool[] completedEvents;

    // Use this for initialization
    void Start()
    {
        instance = this;

        completedEvents = new bool[events.Length];
    }

    // Update is called once per frame
    void Update()
    {

    }

    //Get the number of a quest
    public int GetEventNumber(string eventToFind)
    {
        for (int i = 0; i < events.Length; i++)
        {
            if (events[i] == eventToFind)
            {
                return i;
            }
        }

        Debug.LogError("Event " + eventToFind + " does not exist");
        return 0;
    }

    //Check if an event was completed
    public bool CheckIfComplete(string eventToCheck)
    {
        if (GetEventNumber(eventToCheck) != 0)
        {
            return completedEvents[GetEventNumber(eventToCheck)];
        }

        return false;
    }

    //Complete event
    public void MarkEventComplete(string eventToMark)
    {
        completedEvents[GetEventNumber(eventToMark)] = true;

        UpdateLocalEventObjects();
    }

    //Put a completed event back to incomplete
    public void MarkEventIncomplete(string questToMark)
    {
        completedEvents[GetEventNumber(questToMark)] = false;

        UpdateLocalEventObjects();
    }

    //Update game objects associated with an event
    public void UpdateLocalEventObjects()
    {
        EventObjectActivator[] eventObjects = FindObjectsOfType<EventObjectActivator>();

        if (eventObjects.Length > 0)
        {
            for (int i = 0; i < eventObjects.Length; i++)
            {
                eventObjects[i].CheckCompletion();
            }
        }
    }

    //Save quest data
    public void SaveEventData()
    {
        for (int i = 0; i < events.Length; i++)
        {
            if (completedEvents[i])
            {
                PlayerPrefs.SetInt("EventMarker_" + events[i], 1);
            }
            else
            {
                PlayerPrefs.SetInt("EventMarker_" + events[i], 0);
            }
        }
    }

    //Load quest data
    public void LoadEventData()
    {
        for (int i = 0; i < events.Length; i++)
        {
            int valueToSet = 0;
            if (PlayerPrefs.HasKey("EventMarker_" + events[i]))
            {
                valueToSet = PlayerPrefs.GetInt("EventMarker_" + events[i]);
            }

            if (valueToSet == 0)
            {
                completedEvents[i] = false;
            }
            else
            {
                completedEvents[i] = true;
            }
        }
    }
}
