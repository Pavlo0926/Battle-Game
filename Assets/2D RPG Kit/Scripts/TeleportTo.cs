using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TeleportTo : MonoBehaviour {

    public string scene;

    public string teleportName;

    public TeleportFrom entry;

    public float transitionTime = 1f;
    private bool openScene;

	// Use this for initialization
	void Start () {
        entry.teleportName = teleportName;

    }
	
	// Update is called once per frame
	void Update () {
		if(openScene)
        {
            transitionTime -= Time.deltaTime;
            if(transitionTime <= 0)
            {
                openScene = false;
                SceneManager.LoadScene(scene);
            }
        }
	}

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            openScene = true;
            GameManager.instance.fadingBetweenAreas = true;

            GameMenu.instance.gotItemMessage.SetActive(false);

            ScreenFade.instance.FadeToBlack();

            PlayerController.instance.areaTransitionName = teleportName;
        }
    }
}
