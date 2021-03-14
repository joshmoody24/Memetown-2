using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JanitorManager : MonoBehaviour
{
    public static JanitorManager instance = null;
    void Awake(){
        if(instance == null){
            instance = this;
        }
        else if(instance != this){
            Destroy(gameObject);
        }
    }

    public float hourlyRate = 7.25f;

    GarbageCan[] garbageCans;
    public float fillChance = .8f;

    public Text todoListText;

    void Start(){
        garbageCans = GameObject.FindObjectsOfType<GarbageCan>();
        PopulateGarbageCans();
        UpdateTodoList();
    }

    void Update(){

    }

    void PopulateGarbageCans(){
        foreach(GarbageCan can in garbageCans){
            float random = Random.Range(0f,1f);
            if(random < fillChance){
                can.Fill();
            }
        }
    }

    public void UpdateTodoList(){

        todoListText.text = GetRemainingGarbageCans() + " out of " + garbageCans.Length + " garbage cans remaining";
    }

    public int GetRemainingGarbageCans(){
        int remainingCans = 0;
        foreach(GarbageCan can in garbageCans){
            if(!can.isEmpty()){
                remainingCans++;
            }
        }
        return remainingCans;
    }

    public float CalculateScore(){
        float garbageGrade = 1 - (float)GetRemainingGarbageCans()/garbageCans.Length;
        return garbageGrade;
    }

    public void PrintScoreToConsole(){
        Debug.Log("You did " + CalculateScore()*100 + " percent of the work and worked for " + TimeManager.instance.HoursSinceStart() + " hours. Therefore, you get $" + CalculateScore() * hourlyRate * TimeManager.instance.HoursSinceStart());
    }

}
