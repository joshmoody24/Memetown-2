using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class DamageVFX : MonoBehaviour
{

    private PostProcessVolume ppv;
    private float weight;
    public float easeIn = .1f;
    public float easeOut = .5f;
    bool beingDamaged;
    bool midpoint;
    // Start is called before the first frame update
    void Start()
    {
        ppv = GetComponent<PostProcessVolume>();
        beingDamaged = false;
        midpoint = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(beingDamaged){
            if(weight <= 1f && !midpoint){
                weight += Time.deltaTime / easeIn;
                if(weight >= 1f){
                    weight = 1f;
                    midpoint = true;
                }
            }

            else if(weight >= 0 && midpoint){
                weight -= Time.deltaTime / easeOut;
                if(weight <= 0){
                    weight = 0;
                    beingDamaged = false;
                }
            }
        }
        ppv.weight = weight;
    }

    public void StartDamageVFX(){
        beingDamaged = true;
        midpoint = false;
    }


}
