using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Moon : MonoBehaviour
{

    public GameObject wholeMoon;
    public GameObject fracturedMoon;

    public float explosionForce = 100f;

    public void Fracture(){
        wholeMoon.SetActive(false);
        fracturedMoon.SetActive(true);
        foreach(Rigidbody rb in GetComponentsInChildren<Rigidbody>()){
            rb.AddExplosionForce(explosionForce, transform.position, 100f);
        }
    }
}
