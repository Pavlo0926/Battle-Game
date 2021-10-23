using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackEffect : MonoBehaviour {

    [Header("Effect Settings")]
    public float effectLength;
    public int soundEffect;

	// Use this for initialization
	void Start () {
        AudioManager.instance.PlaySFX(soundEffect);
	}
	
	// Update is called once per frame
	void Update () {
        Destroy(gameObject, effectLength);
	}
}
