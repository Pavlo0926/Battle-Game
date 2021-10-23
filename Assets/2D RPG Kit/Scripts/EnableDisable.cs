using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableDisable : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnEnable()
    {
        Debug.Log("Enable");
    }

    private void OnDisable()
    {
        Debug.Log("Disable");
    }
}
