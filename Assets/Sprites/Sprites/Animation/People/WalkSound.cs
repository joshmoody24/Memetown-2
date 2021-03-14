using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkSound : MonoBehaviour {
    public AudioSource audioSource;
	public void Footstep()
    {
        audioSource.Play();
    }
}
