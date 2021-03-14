using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecurityCamera : MonoBehaviour
{
    public Transform player;
    public Light indicatorLight;
    public Camera camera;
    private Color normalLightColor;
    public Color detectedLightColor;
    // Start is called before the first frame update
    void Start()
    {
        normalLightColor = indicatorLight.color;
        if(player == null){
            player = GameObject.FindWithTag("Player").transform;
        }
    }

    // Update is called once per frame
    void Update()
    {
        //test if the player is in the viewport bounds
        Vector2 screenPoint = camera.WorldToViewportPoint(player.position);
        if(screenPoint.x > 0 && screenPoint.x < 1 && screenPoint.y > 0 && screenPoint.y < 1){
            indicatorLight.color = detectedLightColor;
        }
        else{
            indicatorLight.color = normalLightColor;
        }
    }
}
