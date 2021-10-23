using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScreenFade : MonoBehaviour {

    //Make instance of this script to be able reference from other scripts!
    public static ScreenFade instance;

    [Header("Initialization")]
    //Game objects used by this code
    public GameObject fadeScreenObject;
    public Image fadeScreenImage;

    [Header("Fade Settings")]
    //For changing the duration of the fade
    public float fadeSpeed;
    
    //Check to allow fade in/out
    [HideInInspector]
    public bool fadeToBlack;
    [HideInInspector]
    public bool fadeFromBlack;
    [HideInInspector]
    public bool fading = false;

	// Use this for initialization
	void Start () {
        instance = this;
        DontDestroyOnLoad(gameObject);

	}
	
	// Update is called once per frame
	void Update () {

        //Set alpha of fade screen image to 1 over time (fadespeed) in order to fade to black
        if (fadeToBlack)
        {
            fadeScreenImage.color = new Color(fadeScreenImage.color.r, fadeScreenImage.color.g, fadeScreenImage.color.b, Mathf.MoveTowards(fadeScreenImage.color.a, 1f, fadeSpeed * Time.deltaTime));

            if(fadeScreenImage.color.a == 1f)
            {
                fadeToBlack = false;
            }
        }

        //Set alpha of fade screen image to 0 over time (fadespeed) in order to fade from black
        if (fadeFromBlack)
        {
            fadeScreenImage.color = new Color(fadeScreenImage.color.r, fadeScreenImage.color.g, fadeScreenImage.color.b, Mathf.MoveTowards(fadeScreenImage.color.a, 0f, fadeSpeed * Time.deltaTime));

            if (fadeScreenImage.color.a == 0f)
            {
                fadeFromBlack = false;
            }
        }
    }

    //Method to activae fading
    public void FadeToBlack()
    {
        fadeToBlack = true;
        fadeFromBlack = false;
        fading = true;

    }

    //Method to activae fading
    public void FadeFromBlack()
    {
        fadeToBlack = false;
        fadeFromBlack = true;
        fading = false;
    }
}
