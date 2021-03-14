using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PitchShifter : MonoBehaviour {

    public AudioSource source;
    public float center = 1;
    public float range = .1f;

    void Start()
    {
        center = source.pitch;
    }

    public void RandomPitch()
    {
        source.pitch = Random.Range(center - range, center + range);
    }
}
