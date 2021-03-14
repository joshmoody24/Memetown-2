using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class TimeManager : MonoBehaviour
{
    public static TimeManager instance;
    public float startingHour = 17f;
    public bool format12hour = true;
    public float shiftHours = 3f;
    //one real second is 30 seconds in game
    public float gameTimeScale = 30f;
    private float gameTime;

    public Slider timeSlider;
    public Text timeText;

    const float SECONDS_PER_HOUR = 3600f;
    const float SECONDS_PER_MINUTE = 60f;
    const float MINUTE_ROUND_AMOUNT = 5f;

    void Awake(){
        if(instance == null){
            instance = this;
        }
        else if(instance != this){
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        gameTime = 0f;
        timeSlider.maxValue = startingHour + shiftHours;
        timeSlider.minValue = startingHour;
        timeSlider.value = timeSlider.minValue;
        UpdateTimeUI();
    }

    // Update is called once per frame
    void Update()
    {
        IncrementGameTime(Time.deltaTime);
        UpdateTimeUI();
    }

    void IncrementGameTime(float deltaTime){
        float incr = deltaTime * gameTimeScale;
        gameTime += incr;
    }

    public string GetTimeString(){
        float temp = gameTime + startingHour * SECONDS_PER_HOUR;
        if(temp > SECONDS_PER_HOUR * 24f){
            temp -= Mathf.Floor(temp/(SECONDS_PER_HOUR * 24f));
        }
        float hours = (Mathf.Floor(temp/3600));
        temp -= hours * SECONDS_PER_HOUR;
        float minutes = Mathf.Floor(temp/SECONDS_PER_MINUTE);
        temp -= minutes * SECONDS_PER_MINUTE;
        float seconds = temp;
        string meridian = "AM";
        if(hours > 12){
            meridian = "PM";
            if(format12hour){
                hours -= 12f;
            }
        }

        string output;
        output = (int)hours + ":" + String.Format("{0:00}", Mathf.Floor(minutes/MINUTE_ROUND_AMOUNT)*MINUTE_ROUND_AMOUNT);
        if(format12hour){
            output += " " + meridian;
        }
        return output;
    }

    void UpdateTimeUI(){
        timeSlider.value = (gameTime/SECONDS_PER_HOUR) + startingHour;
        timeText.text = GetTimeString();
    }

    public float HoursSinceStart(){
        return gameTime / SECONDS_PER_HOUR;
    }
}
