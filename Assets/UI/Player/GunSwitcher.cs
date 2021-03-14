using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GunSwitcher : MonoBehaviour
{

    public Weapon[] weapons;
    private int activeWeapon = 0;
    public Slider ammoSlider;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.E)){
            SwitchToGun(activeWeapon + 1);
        }
    }

    public void ResetAllGuns(){
        foreach(Gun gun in weapons){
            gun.ammo = gun.maxammo;
            gun.isRestoreDelayed = false;
            gun.canshoot = true;
        }
        ammoSlider.value = ammoSlider.maxValue;
    }

    public void SwitchToGun(int index){
        activeWeapon = index;
        if(activeWeapon >= weapons.Length){
            activeWeapon = 0;
        }
        for(int i = 0; i < weapons.Length; i++){
            if(i == activeWeapon){
                weapons[i].gameObject.SetActive(true);
            }
            else{
                weapons[i].gameObject.SetActive(false);
            }
        }
        if(weapons[activeWeapon].GetType().Name == "Gun" || weapons[activeWeapon].GetType().IsSubclassOf(typeof(Gun))){
            Gun gun = (Gun)weapons[activeWeapon];
            gun.isRestoreDelayed = false;
            if(gun.ammo > 0){
                gun.canshoot = true;
            }
            gun.canshoot = true;
            ammoSlider.maxValue = gun.maxammo;
            ammoSlider.value = gun.ammo;
        }
    }
}
