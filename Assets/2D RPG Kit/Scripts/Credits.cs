using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Credits : MonoBehaviour
{
    public GameObject credits;
    public Text creditsText;
    public string nextScene;
    public GameObject creditsCanvas;
    public int creditsMusic;

    // Start is called before the first frame update
    void Start()
    {
        
        AudioManager.instance.PlayBGM(creditsMusic);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("RPGConfirmPC") || Input.GetButtonDown("RPGConfirmJoy"))
        {
            StartCoroutine(EndCreditsCo());
            //Destroy(AudioManager.instance.gameObject);
        }
    }

    public IEnumerator EndCreditsCo()
    {
        credits.SetActive(true);
        yield return new WaitForSeconds(1.5f);
        ScreenFade.instance.FadeToBlack();
        yield return new WaitForSeconds(3);
        creditsText.color = new Color(0, 0, 0, 0);
        ScreenFade.instance.FadeFromBlack();
        yield return new WaitForSeconds(1);        
        SceneManager.LoadScene(nextScene);
        AudioManager.instance.StopMusic();

        Destroy(GameManager.instance.gameObject);
        Destroy(PlayerController.instance.gameObject);
        Destroy(GameMenu.instance.gameObject);
        Destroy(AudioManager.instance.gameObject);
        Destroy(BattleManager.instance.gameObject);

        //DestroyObject(credits);
        credits.SetActive(false);
        yield return new WaitForSeconds(.5f);
        //UIFade.instance.FadeFromBlack();
        ScreenFade.instance.fadeScreenImage.color = new Color(ScreenFade.instance.fadeScreenImage.color.r, ScreenFade.instance.fadeScreenImage.color.g, ScreenFade.instance.fadeScreenImage.color.b, 0);
        yield return new WaitForSeconds(3);        
        Destroy(creditsCanvas);
    }
}
