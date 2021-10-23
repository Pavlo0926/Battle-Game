using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleTargetButton : MonoBehaviour {

    public string moveName;
    public int activeBattlerTarget;
    public Text targetName;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Press()
    {
        //StartCoroutine(DelayCo());

        BattleManager.instance.targetEnemyMenu.SetActive(false);
        BattleManager.instance.PlayerAttack(moveName, activeBattlerTarget);

        //BattleManager.instance.skillMenu.SetActive(false);
        
    }

    //Adds a slight delay between choosing the target and attacking
    public IEnumerator DelayCo()
    {
        yield return new WaitForSeconds(.5f);
        BattleManager.instance.targetEnemyMenu.SetActive(false);
        BattleManager.instance.PlayerAttack(moveName, activeBattlerTarget);
        
    }
}
