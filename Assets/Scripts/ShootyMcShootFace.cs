using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShootyMcShootFace : Gun {

    public AudioSource gunAudio2;
    public ParticleSystem muzzleFlash2;

    protected override void Shoot()
    {  
        //swap the two gun variables before calling the shoot function to alternate which gun fires
        AudioSource tempA = gunAudio;
        ParticleSystem tempP = muzzleFlash;
        gunAudio = gunAudio2;
        muzzleFlash = muzzleFlash2;
        gunAudio2 = tempA;
        muzzleFlash2 = tempP;
        base.Shoot();
    }

}
