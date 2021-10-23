using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DamageNumber : MonoBehaviour {

    [Header("Initialization")]
    //Game objects used by this code
    public Text damageText;

    [Header("Effect Settings")]
    public float lifetime = 1f;
    public float moveSpeed = 1f;

    public float placementJitter = .5f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        Destroy(gameObject, lifetime);
        //Move the damage number in the y-axis
        transform.position += new Vector3(0f, moveSpeed * Time.deltaTime, 0f);
	}

    public void SetDamage(int damageAmount)
    {
        damageText.text = damageAmount.ToString();
        //Show the damage number everytime in a slighty altered position
        transform.position += new Vector3(Random.Range(-placementJitter, placementJitter), Random.Range(-placementJitter, placementJitter), 0f);
        transform.position += new Vector3(Random.Range(-placementJitter, placementJitter), Random.Range(-placementJitter, placementJitter), 0f);
    }
}
