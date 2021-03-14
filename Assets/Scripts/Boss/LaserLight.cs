using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserLight : MonoBehaviour {

    private Laser[] lasers;

    public void Start()
    {
        lasers = GetComponentsInChildren<Laser>();
    }

    public void FireLaser()
    {
        foreach(Laser laser in lasers)
        {
            laser.FireLaser();
        }
    }
	
}
