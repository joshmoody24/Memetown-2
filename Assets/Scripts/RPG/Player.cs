using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using UnityStandardAssets.Characters.FirstPerson;

public class Player : BattleAgent
{
    //number that grows exponentially independent of agent level
    [System.Serializable]
    public class Skill : Stat{
        [SerializeField] private int statLevel;
        public Skill() : base(){
            statLevel = 1;
        }
        public Skill(string _statName, int _baseStat, float _levelScale, int _statLevel)
            : base(_statName, _baseStat, _levelScale){
                statLevel = _statLevel;
        }
        public void LevelUp(){
            statLevel++;
        }
        public void LevelDown(){
            statLevel--;
        }
        public float GetCurrentSkillValue(){
            return baseStat * Mathf.Pow(levelScale,(statLevel-1));
        }
    }

    [SerializeField] private float xp;
    [SerializeField] private float maxXP = 10;
    [SerializeField] private float xpScale;
    public Slider playerHPSlider;
    public Text playerHPText;
    public GunSwitcher guns;
    public DamageVFX damageVFX;
    private Vector3 initialPlayerPos;

    //todo: add gun skill
    //public Dictionary<string, Skill> skills;

    new void Start(){
        base.Start();
        stats.Add("maxXP", new Stat("maxXP", maxXP, xpScale));
        maxXP = GetStatValue(stats["maxXP"]);
        playerHPSlider.maxValue = (float)GetStatValue(stats["maxHP"]);
        playerHPSlider.minValue = 0;
        playerHPSlider.value = playerHPSlider.maxValue;
        hp = playerHPSlider.maxValue;
        playerHPText.text = playerHPSlider.value.ToString();
    }

    public void GainXP(float amount){
        float xpToNextLevel = GetStatValue(stats["maxXP"]);
        //protect from infinite loop
        if(xpToNextLevel <= 0){
            xpToNextLevel = 1f;
        }
        xp += amount;
        //keep leveling up until xp is less than the threshold
        while(xp >= xpToNextLevel){
          xp = xp - xpToNextLevel;
          LevelUp();
        }
    }
    public void LevelUp(){
        level++;
    }
    public new void TakeDamage(float amount){
        Debug.Log(hp + ", " + amount);
        base.TakeDamage(amount);
        playerHPSlider.value = (float)hp;
        playerHPText.text = hp.ToString();
        damageVFX.StartDamageVFX();
    }

    public void Restart(){
        FirstPersonController fpc = GetComponent<FirstPersonController>();
        //DEBUG!!!! fpc.ResetRotation();
        //character controller overrides setting player position. Got to disable it first
        CharacterController c = GetComponent<CharacterController>();
        c.enabled = false;
        transform.position = initialPlayerPos;
        c.enabled = true;
        playerHPSlider.maxValue = GetStatValue(stats["maxHP"]);
        playerHPSlider.value = GetStatValue(stats["maxHP"]);
        playerHPText.text = GetStatValue(stats["maxHP"]).ToString();
        hp = GetStatValue(stats["maxHP"]);
        guns.ResetAllGuns();
    }

    public void DisableControl(){
        GetComponent<FirstPersonController>().enabled = false;
    }
    public void EnableControl(){
        GetComponent<FirstPersonController>().enabled = true;
    }
}
