using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventObjectActivator : MonoBehaviour
{
    public GameObject objectToActivate;

    public string eventToCheck;

    public bool activeIfComplete;

    public bool waitBeforeActivate;
    public float waitTime;

    private bool initialCheckDone;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (!initialCheckDone)
        {
            initialCheckDone = true;

            CheckCompletion();
        }
    }

    public void CheckCompletion()
    {
        if (EventManager.instance.CheckIfComplete(eventToCheck))
        {
            if (waitBeforeActivate)
            {
                StartCoroutine(waitCo());
            }
            else
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
