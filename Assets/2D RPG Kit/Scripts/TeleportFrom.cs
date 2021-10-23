using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportFrom : MonoBehaviour {

    public string teleportName;

	// Use this for initialization
	void Start () {
		if(teleportName == PlayerController.instance.areaTransitionName)
        {
            PlayerController.instance.transform.position = transform.position;
            PlayerController.instance.GetComponent<SpriteRenderer>().sortingLayerName = "Player";
        }

        ScreenFade.instance.FadeFromBlack();
        GameManager.instance.fadingBetweenAreas = false;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
