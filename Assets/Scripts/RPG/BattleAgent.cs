using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BattleAgent : MonoBehaviour
{
    //number that grows exponentially based on agent level
    [System.Serializable]
    public class Stat{
        public string name;
        //this number should never change after it's initialized
        public float baseStat;
        //how fast the stat increases based on the level of the character
        public float levelScale;

        public Stat(){
            name = "name";
            baseStat = 1;
            levelScale = 1.25f;
        }
        public Stat(string _statName, float _baseStat, float _levelScale){
            name = _statName;
            baseStat = _baseStat;
            levelScale = _levelScale;
        }
    }

    [SerializeField] protected float level = 1;
    [SerializeField] protected float hp;
    public float hpScale = 1.25f;
    [SerializeField] protected float mp;
    public float mpScale = 1.1f;
    [SerializeField] protected float atk;
    public float atkScale = 1.25f;

    public UnityEvent OnKill;

    protected bool isAlive = true;

    //there are two types of stats: stats and skills
    //stats are controlled by the base level of the character
    //skills are totaly independent. You can be level 1 character with level 999 skill.
    //But if you're a level 10 character than all your stats are also level 10
    public Dictionary<string,Stat> stats;

    protected void Start(){
        stats = new Dictionary<string, Stat>();
        stats.Add("maxHP",new Stat("maxHP", hp, hpScale));
        stats.Add("ATK",new Stat("ATK", atk, atkScale));
        stats.Add("maxMP", new Stat("maxMP", mp, mpScale));
        hp = GetStatValue(stats["maxHP"]);
        mp = GetStatValue(stats["maxMP"]);
        atk = GetStatValue(stats["ATK"]);
    }

    public float GetStatValue(Stat s){
        return Mathf.Round(s.baseStat * Mathf.Pow(s.levelScale, level-1));
    }

    public void Attack(BattleAgent target){
        target.TakeDamage(GetStatValue(stats["ATK"]));
    }
    public virtual void Die(){
        isAlive = false;
        OnKill.Invoke();
    }

    public void TakeDamage(float amount){
        hp -= amount;
        if(hp <= 0){
            hp = 0;
            Die();
        }
    }
    public float GetHP(){
        return hp;
    }
    public float GetLevel(){
        return level;
    }
}