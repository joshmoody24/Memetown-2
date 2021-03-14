using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEditor;
using UnityEngine.Audio;
using UnityEngine.UI;

public class TimeController : MonoBehaviour
{

    public PostProcessVolume normalPP;
    public PostProcessVolume slowMoPP;
    public AudioMixerSnapshot normalTime;
    public AudioMixerSnapshot slowTime;
    private AudioSource[] audioSources;
    public float transitionSpeed = 1f;

    public float slowMotionScale = .2f;
    public float audioDistortionScale = .8f;
    private bool slowMotion;
    private float fixedDeltaTime;
    
    public Slider artSlider;
    public float slowMoDuration = 10f;
    public float rechargeTime = 10f;
    private bool artCharged;

    void Start(){
        this.fixedDeltaTime = Time.fixedDeltaTime;
        slowMotion = false;
        normalPP.weight = 1f;
        slowMoPP.weight = 0f;
        audioSources = GetAllAudioSourcesInScene();//FindObjectsOfType<AudioSource>(); only works with active objects
        artCharged = true;
    }

    void Update(){
        if(Input.GetKeyDown(KeyCode.P) && artCharged){
            ToggleSlowMotion();
        }
        if(slowMotion){
            slowMoPP.weight = Mathf.Lerp(slowMoPP.weight, 0.99f, Time.unscaledDeltaTime * transitionSpeed);            
            normalPP.weight = Mathf.Lerp(normalPP.weight, 0.01f, Time.unscaledDeltaTime  * transitionSpeed);
            artSlider.value -= Time.unscaledDeltaTime / slowMoDuration;
            if(artSlider.value <= 0){
                ToggleSlowMotion();
            }
        }
        else{
            normalPP.weight = Mathf.Lerp(normalPP.weight, 0.99f, Time.unscaledDeltaTime * transitionSpeed);
            slowMoPP.weight = Mathf.Lerp(slowMoPP.weight, 0.01f, Time.unscaledDeltaTime * transitionSpeed);
            artSlider.value += Time.unscaledDeltaTime / rechargeTime;
            if(artSlider.value == artSlider.maxValue){
                artCharged = true;
            }         
        }
    }

    public void ToggleSlowMotion(){
        artCharged = false;
        slowMotion = !slowMotion;
        float newAudioPitch;
        if(slowMotion){
            Time.timeScale = slowMotionScale;
            newAudioPitch = audioDistortionScale;
            slowTime.TransitionTo(transitionSpeed/10);
        }
        else{
            Time.timeScale = 1f;
            newAudioPitch = 1f;
            normalTime.TransitionTo(transitionSpeed/10);
        }
        Time.fixedDeltaTime = this.fixedDeltaTime * Time.timeScale;
        foreach(AudioSource audio in audioSources){
            audio.pitch = newAudioPitch;
        }
    }
    public void ResetTimeScale(){
        Time.timeScale = 1f;
        Time.fixedDeltaTime = this.fixedDeltaTime;
        foreach(AudioSource audio in audioSources){
            audio.pitch = 1f;
        }
    }


    AudioSource[] GetAllAudioSourcesInScene()
    {
        List<AudioSource> objectsInScene = new List<AudioSource>();

        foreach (AudioSource a in Resources.FindObjectsOfTypeAll(typeof(AudioSource)) as AudioSource[])
        {
            if (!EditorUtility.IsPersistent(a.transform.root.gameObject) && !(a.hideFlags == HideFlags.NotEditable || a.hideFlags == HideFlags.HideAndDontSave))
                objectsInScene.Add(a);
        }

        return objectsInScene.ToArray();
    }
}
