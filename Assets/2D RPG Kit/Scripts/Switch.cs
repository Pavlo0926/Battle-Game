using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class Switch : MonoBehaviour
{
    [Header("Target Settings")]
    public GameObject targetObject;

    [Header("Activation Settings")]
    public bool onPressButton = false;
    public bool onEnter = false;

    private bool canActivate;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("RPGConfirmPC") || Input.GetButtonDown("RPGConfirmJoy") || CrossPlatformInputManager.GetButtonDown("RPGConfirmTouch"))
        {
            if (canActivate == true)// && onPressButton == true)
            {
                if (!targetObject.activeInHierarchy)
                {
                    targetObject.SetActive(true);
                }
            }
        }

        if (canActivate == true && onEnter == true)
        {
            if (!targetObject.activeInHierarchy)
            {
                targetObject.SetActive(true);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            canActivate = true;
            DialogManager.instance.dontOpenDialogAgain = false;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            canActivate = false;
        }
    }
}
