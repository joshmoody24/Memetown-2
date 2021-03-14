using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;
using Cinemachine;

public class GunMenuUI : MonoBehaviour
{
    public GameObject gunMenu;
    public FirstPersonController fpc;
    public Player2D player2D;
    public CinemachineFreeLook cinemachineFreeLook;
    [SerializeField] private float delay = .1f;

    private float origCamSpeedX;
    private float origCamSpeedY;
    // Start is called before the first frame update
    void Start()
    {
        origCamSpeedX = cinemachineFreeLook.m_XAxis.m_MaxSpeed;
        origCamSpeedY = cinemachineFreeLook.m_YAxis.m_MaxSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetButtonDown("Fire2") && gunMenu.activeSelf == false){
            fpc.DisableMouseLook();
            player2D.DisablePlayerControl();
            cinemachineFreeLook.m_XAxis.m_MaxSpeed = 0;
            cinemachineFreeLook.m_YAxis.m_MaxSpeed = 0;
            gunMenu.SetActive(true);
        }
        else if(Input.GetButtonUp("Fire2") && gunMenu.activeSelf == true){
            fpc.EnableMouseLook();
            player2D.EnablePlayerControl();
            cinemachineFreeLook.m_XAxis.m_MaxSpeed = origCamSpeedX;
            cinemachineFreeLook.m_YAxis.m_MaxSpeed = origCamSpeedY;
            //cinemachineFreeLook.enabled = true;
        }
    }

    IEnumerator DisableMenu(){
        yield return new WaitForSeconds(delay);
        gunMenu.SetActive(false);
    }
}
