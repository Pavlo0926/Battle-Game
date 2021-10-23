using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectSkill : MonoBehaviour {

    public string skill;
    public int skillCost;
    public Text nameText;
    public Text costText;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Press()
    {
        if (BattleManager.instance.activeBattlers[BattleManager.instance.currentTurn].currentSP >= skillCost)
        {
            //BattleManager.instance.skillMenu.SetActive(false);
            BattleManager.instance.OpenTargetEnemyMenu(skill);
            BattleManager.instance.skillCost = skillCost;
            BattleManager.instance.activeBattlers[BattleManager.instance.currentTurn].currentSP -= BattleManager.instance.skillCost;
        } else
        {
            //let player know there is not enough SP
            BattleManager.instance.battlePrompts.notificationText.text = "Not Enough SP!";
            BattleManager.instance.battlePrompts.Activate();
            BattleManager.instance.skillMenu.SetActive(false);
            
            GameMenu.instance.btn = BattleManager.instance.attackButton;
            GameMenu.instance.SelectFirstButton();
        }
    }
}
