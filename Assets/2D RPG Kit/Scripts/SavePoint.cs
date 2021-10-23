using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class SavePoint : MonoBehaviour
{
    private bool canOpen;
    private bool closed;

    // Start is called before the first frame update
    void Update()
    { 
        if (Input.GetButtonDown("RPGConfirmPC") || Input.GetButtonDown("RPGConfirmJoy") || CrossPlatformInputManager.GetButtonDown("RPGConfirmTouch"))
        {
            if (canOpen && PlayerController.instance.canMove && !Save.instance.saveMenu.activeInHierarchy && !GameManager.instance.gameMenuOpen)
            {
                canOpen = false;
                Save.instance.OpenSaveMenu();

                if(ControlManager.instance.mobile == false)
                {
                    GameMenu.instance.btn = GameMenu.instance.saveButton;
                    GameMenu.instance.SelectFirstButton();
                }
            }
        }

        if (!Save.instance.saveMenu.activeInHierarchy && closed)
        {
            canOpen = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player" && !Save.instance.saveMenu.activeInHierarchy)
        {
            canOpen = true;
            closed = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            canOpen = false;
            closed = false;
        }
    }
}
