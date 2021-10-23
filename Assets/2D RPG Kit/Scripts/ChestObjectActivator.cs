using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestObjectActivator : MonoBehaviour
{
    public GameObject objectToActivate;

    public string chestToCheck;

    public bool activeIfComplete;

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
        if (ChestManager.instance.CheckIfOpened(chestToCheck))
        {
                objectToActivate.SetActive(activeIfComplete);
        }
    }
}
