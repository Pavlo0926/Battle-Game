using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestObjectActivator : MonoBehaviour {

    public GameObject objectToActivate;

    public string questToCheck;

    public bool activeIfComplete;

    public bool waitBeforeActivate;
    public float waitTime;

    private bool initialCheckDone;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(!initialCheckDone)
        {
            initialCheckDone = true;

            CheckCompletion();
        }
	}

    public void CheckCompletion()
    {
        if(QuestManager.instance.CheckIfComplete(questToCheck))
        {
            if (waitBeforeActivate)
            {
                StartCoroutine(waitCo());
            }else
            {
                objectToActivate.SetActive(activeIfComplete);
            }
            
        }
    }

    IEnumerator waitCo()
    {
        yield return new WaitForSeconds(waitTime);
        objectToActivate.SetActive(activeIfComplete);
    }
}
