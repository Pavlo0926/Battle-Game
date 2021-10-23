using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainObjectsLoader : MonoBehaviour {

    public GameObject UIScreen;
    public GameObject player;
    public GameObject gameMan;
    public GameObject audioMan;
    public GameObject battleMan;
    public GameObject controllMan;
    public GameObject directionalLight;

	// Use this for initialization
	void Start () {		

        if (ScreenFade.instance == null)
        {
            ScreenFade.instance = Instantiate(UIScreen).GetComponent<ScreenFade>();
        }

        if (PlayerController.instance == null)
        {
            PlayerController clone = Instantiate(player).GetComponent<PlayerController>();
            PlayerController.instance = clone;
        }

        if (GameManager.instance == null)
        {
            GameManager.instance = Instantiate(gameMan).GetComponent<GameManager>();
        }

        if(AudioManager.instance == null)
        {
            AudioManager.instance = Instantiate(audioMan).GetComponent<AudioManager>();
        }

        if(BattleManager.instance == null)
        {
            BattleManager.instance = Instantiate(battleMan).GetComponent<BattleManager>();
        }

        if(ControlManager.instance == null)
        {
            ControlManager.instance = Instantiate(controllMan).GetComponent<ControlManager>();
        }

        if (DirectionalLight.instance == null)
        {
            DirectionalLight.instance = Instantiate(directionalLight).GetComponent<DirectionalLight>();
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
