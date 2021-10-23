using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIHelper : MonoBehaviour
{
    private void Awake()
    {
        if (GameObject.FindObjectOfType<ControlManager>().mobile)
            gameObject.SetActive(true);
        else
            gameObject.SetActive(false);
    }

}
