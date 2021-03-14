using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PitchShifter))]
public class KeyboardSound : MonoBehaviour {

    public AudioSource mouseClick;
    public bool randomPitch;

    private PitchShifter shifter;

    // Use this for initialization
    void Start () {
        shifter = GetComponent<PitchShifter>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.anyKeyDown)
        {
            if (randomPitch)
            {
                shifter.RandomPitch();
            }
            mouseClick.Play();
        }
    }
}
