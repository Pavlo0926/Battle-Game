using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectionalLight : MonoBehaviour
{
    public static DirectionalLight instance;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;

        DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
