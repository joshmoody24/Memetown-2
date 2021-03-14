using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class GameOverScreenFinalBoss : MonoBehaviour {

    public Player player;
    public Boss boss;
    public Text gameOverText;
    public float experienceMultiplier = 2f;
    public UnityEvent OnPressAnyKey;
    private bool canRestart;

	// Use this for initialization
	void Start () {
		
	}

    private void OnEnable()
    {
        canRestart = false;
        StartCoroutine(RestartDelay());

        player.GainXP(ComputeExperience());
        gameOverText.text = "Damage to Boss x" + experienceMultiplier.ToString() + " = " + ComputeExperience() + " experience. You are now level: " + player.GetLevel();
    }

    IEnumerator RestartDelay()
    {
        yield return new WaitForSeconds(.8f);
        canRestart = true;
    }

    // Update is called once per frame
    void Update () {
        if (Input.anyKey && canRestart)
        {
            OnPressAnyKey.Invoke();
        }
	}

    public float ComputeExperience(){
        //compute the amount of experience based on damage to boss
        return (boss.GetStatValue(boss.stats["maxHP"]) - boss.GetHP()) * experienceMultiplier;
    }
}
