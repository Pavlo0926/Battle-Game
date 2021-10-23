using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleNotification : MonoBehaviour {

    public float awakeTime;
    private float awakeCounter;
    public Text notificationText;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(awakeCounter > 0)
        {
            awakeCounter -= Time.deltaTime;
            if(awakeCounter <= 0)
            {
                gameObject.SetActive(false);
                BattleManager.instance.battleMenu.SetActive(true);
            }
        }
	}

    public void Activate()
    {
        gameObject.SetActive(true);
        awakeCounter = awakeTime;
    }
}
