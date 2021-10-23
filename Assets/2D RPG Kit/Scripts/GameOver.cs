using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOver : MonoBehaviour {

    [Header("Initialization")]
    //Game objects used by this code
    //For UI button highlighting
    public Button loadButton;

    [Header("Menu Settings")]
    public int gameOverMusic;
    public string mainMenuScene;
    public string loadGameScene;

	// Use this for initialization
	void Start () {

        if(ControlManager.instance.mobile == false)
        {
            GameMenu.instance.btn = loadButton;
            GameMenu.instance.SelectFirstButton();
        }
        
        AudioManager.instance.PlayBGM(gameOverMusic);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void QuitToMain()
    {

        /*
        Destroy(PlayerController.instance.gameObject);
        Destroy(GameManager.instance.gameObject);
        Destroy(GameMenu.instance.gameObject);
        Destroy(AudioManager.instance.gameObject);
        Destroy(BattleManager.instance.gameObject);
        */
        SceneManager.LoadScene(mainMenuScene);
    }

    public void LoadLastSave()
    {
        /*
        Destroy(GameManager.instance.gameObject);
        Destroy(PlayerController.instance.gameObject);
        Destroy(GameMenu.instance.gameObject);
        Destroy(AudioManager.instance.gameObject);
        Destroy(BattleManager.instance.gameObject);
        */

        SceneManager.LoadScene(loadGameScene);
    }
}
