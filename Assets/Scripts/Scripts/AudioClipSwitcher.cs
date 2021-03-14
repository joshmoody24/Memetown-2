using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioClipSwitcher : MonoBehaviour {
    public static void SwitchAudioClip(AudioSource source, AudioClip clip)
    {
        source.clip = clip;
    }
}
