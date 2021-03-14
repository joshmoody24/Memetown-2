using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour {

    private Vector3 originPosition;
    private Quaternion originRotation;
    public float start_shake_decay;
    public float start_shake_intensity;

    private float shake_decay;
    private float shake_intensity;

    // Use this for initialization
    void Start () {
        originPosition = transform.position;
    }

    void Update()
    {
        if (shake_intensity > 0)
        {
            transform.position += Random.insideUnitSphere * shake_intensity;
            transform.rotation = new Quaternion(
            originRotation.x + Random.Range(-shake_intensity, shake_intensity) * .2f,
            originRotation.y + Random.Range(-shake_intensity, shake_intensity) * .2f,
            originRotation.z + Random.Range(-shake_intensity, shake_intensity) * .2f,
            originRotation.w + Random.Range(-shake_intensity, shake_intensity) * .2f);
            shake_intensity -= shake_decay * Time.deltaTime;
        }
        else
        {
            transform.position = originPosition;
        }
    }

    public void Shake()
    {
        originRotation = transform.rotation;
        shake_intensity = start_shake_intensity;
        shake_decay = start_shake_decay;
    }
}
