using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class Player2D : MonoBehaviour {

    private CharacterMotor motor;
    public GameObject lantern;
    public Renderer bossRenderer;
    public PostProcessVolume normalPP;
    public float maxFocusDistance = 20f;
    private DepthOfField dof;

    private bool hidLantern;
    private Camera thirdPersonCam;
	// Use this for initialization
	void Start () {
        motor = GetComponent<CharacterMotor>();	
        thirdPersonCam = motor.camera.GetComponent<Camera>();
        if(!normalPP.profile.TryGetSettings<DepthOfField>(out dof)){
            Debug.Log("Depth of Field not found on default post processing. Will not be used!");
        }

	}
	
    void Update(){
        //by default, focus on player. If boss is raycasted at, focus on the midpoint between them instead
        //note: this is assuming the camera is focused on the player's head
        float focusDistance = Vector3.Distance(thirdPersonCam.transform.position, motor.playerChild.position);
        if(bossRenderer != null){
            if(bossRenderer.isVisible){
                focusDistance = Mathf.Min(Vector3.Distance(thirdPersonCam.transform.position, bossRenderer.transform.position), maxFocusDistance);
            }
        }
        //set depth of field to either the player or the boss
    
        dof.focusDistance.value = Mathf.Lerp(dof.focusDistance.value, focusDistance, Time.unscaledDeltaTime);
    }

    public void DisablePlayerControl()
    {
        motor.StopMovement();
        Debug.Log("disabled");
    }

    public void EnablePlayerControl()
    {
        motor.ResumeMovement();
        Debug.Log("enabled player");
    }

    public void HidePlayer()
    {
        DisablePlayerControl();
        foreach(SpriteRenderer sr in GetComponentsInChildren<SpriteRenderer>())
        {
            sr.enabled = false;
        }
        if(lantern != null)
        {
            if (lantern.activeSelf)
            {
                lantern.SetActive(false);
                hidLantern = true;
            }
            else
            {
                hidLantern = false;
            }
        }
    }
    public void ShowPlayer()
    {
        EnablePlayerControl();
        foreach (SpriteRenderer sr in GetComponentsInChildren<SpriteRenderer>())
        {
            sr.enabled = true;
        }
        if(lantern != null)
        {
            if (hidLantern)
            {
                hidLantern = false;
                lantern.SetActive(true);
            }
        }
    }
    public void TeleportPlayer(Transform newPos)
    {
        transform.position = newPos.position;
    }
}
