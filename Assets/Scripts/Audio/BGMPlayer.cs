using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMPlayer : MonoBehaviour {

    public float delayTime = 1f;

    private bool isFadingIn;
    private bool isFadingOut;
    private bool isSwitching;

    //set to whatever it is in the inspector
    private float maxVolume;

    private new AudioSource audio;

    public float fadeSpeed = 1f;

    private AudioClip playingNext;

    public AudioClip silenceClip;

    private void Start()
    {
        isFadingIn = false;
        isFadingOut = false;
        isSwitching = false;
        audio = GetComponent<AudioSource>();
        maxVolume = audio.volume;
    }

    private void Update()
    {
        if (isFadingIn)
        {
            if (audio.volume < maxVolume)
            {
                audio.volume += Time.deltaTime * fadeSpeed;
            }
            else
            {
                isFadingIn = false;
                audio.volume = maxVolume;
            }
        }
        else if (isFadingOut)
        {
            if (audio.volume > 0)
            {
                audio.volume -= Time.deltaTime * fadeSpeed;
            }
            else
            {
                isFadingOut = false;
                audio.volume = 0;
                audio.Stop();
                //if its fading out because its switching to new BGM
                if (isSwitching)
                {
                    audio.clip = playingNext;
                    playingNext = null;
                    StartCoroutine(FadeIn(delayTime));
                }
            }
        }
    }

    private IEnumerator FadeIn(float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        isFadingOut = false;
        isFadingIn = true;
        audio.Play();
    }

    private void FadeOut()
    {
        isFadingIn = false;
        isFadingOut = true;
        Debug.Log("fading out");
    }

    public void StopPlaying()
    {
        FadeOut();
        Debug.Log("Stopping audio");
    }

    public void StartPlaying(AudioClip newBGM)
    {
        audio.clip = newBGM;
        StartCoroutine(FadeIn(delayTime));
    }

    //in retrospect, I should have checked to see if newBGM was already playing but oh well
    public void SwitchBGM(AudioClip newBGM)
    {
        playingNext = newBGM;
        isSwitching = true;
        FadeOut();
    }
    public void SwitchBGMImmediately(AudioClip newBGM)
    {
        audio.clip = newBGM;
        audio.Play();
    }
    public AudioClip getClip()
    {
        if (isSwitching)
        {
            return playingNext;
        }
        else
        {
            return audio.clip;
        }
    }

    public void SilenceBGM()
    {
        SwitchBGM(silenceClip);
    }

}
