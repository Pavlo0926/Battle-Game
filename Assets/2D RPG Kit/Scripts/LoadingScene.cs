using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingScene : MonoBehaviour {
    [Header("Load Settings")]
    public float waitToLoad;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(waitToLoad > 0)
        {
            waitToLoad -= Time.deltaTime;
            if(waitToLoad <= 0)
            {
                SceneManager.LoadScene(PlayerPrefs.GetString("Current_Scene"));

                GameManager.instance.LoadData();
                QuestManager.instance.LoadQuestData();
                ChestManager.instance.LoadChestData();
                EventManager.instance.LoadEventData();
            }
        }
	}
}
