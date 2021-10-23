using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOverLifetime : MonoBehaviour
{

    public float lifetime;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Destroy(gameObject, lifetime);
    }

}
