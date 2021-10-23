using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LogoLoadingScene : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Invoke("LoadScene", 10f);
    }

    // Update is called once per frame
    void LoadScene()
    {
        SceneManager.LoadScene(1);
    }
}
