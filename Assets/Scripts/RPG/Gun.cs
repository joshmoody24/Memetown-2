using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Gun : Weapon
{
    public BattleAgent wielder;

    public Camera fpsCam;
    public bool automatic;
    //float lol
    public float ammo = 20f;
    public float maxammo;
    public float ammoRestoreSpeed = .5f;
    public float restoreDelayAmount = .5f;
    public Slider ammoSlider;

    public float cooldowntime = .1f;

    public AudioClip gunshotSound;
    public AudioClip emptyMag;

    public bool canshoot;
    protected bool canRestore;
    public bool isRestoreDelayed;

    public GameObject impactParticle;
    private Queue<GameObject> impactParticles;
    private const int MAX_IMPACTS = 5;

    public AudioSource gunAudio;
    public ParticleSystem muzzleFlash;

    protected void Start () {
        canshoot = true;
        impactParticles = new Queue<GameObject>();
        ammoSlider.maxValue = ammo;
        ammoSlider.minValue = 0;
        ammoSlider.value = ammo;
        maxammo = ammo;
        canRestore = false;
        isRestoreDelayed = false;
    }

    // Update is called once per frame
	void Update () {
        if ((Input.GetButton("Fire1") && automatic) || (Input.GetButtonDown("Fire1") && !automatic))
        {
            if (canshoot)
            {
                canshoot = false;
                canRestore = false;
                isRestoreDelayed = false;
                StartCoroutine(CoolDown());
                Shoot();
            }
        }
        else
        {
            if (canRestore)
            {
                isRestoreDelayed = false;
                ammo += ammoRestoreSpeed * Time.deltaTime;
                if (ammo > maxammo)
                {
                    ammo = maxammo;
                }
                ammoSlider.value = ammo;
            }
            
            else if(isRestoreDelayed == false && ammo < maxammo)
            {
                StartCoroutine(RestoreDelay());
            }
            
        }
	}

    protected virtual void Shoot()
    {
        ammoSlider.value--;
        if(ammo > 0)
        {
            ammo--;
            if(ammo < 0)
            {
                ammo = 0;
                ammoSlider.value = 0;
            }

            //play the visual and auditory effects regardless of the results of the raycast
            gunAudio.clip = gunshotSound;
            muzzleFlash.Play();
            gunAudio.Play();

            Ray ray = fpsCam.ViewportPointToRay(new Vector3(.5f, .5f, 0));
            RaycastHit hit;
            string[] layers = {"Default", "Boss"};
            LayerMask mask = LayerMask.GetMask(layers);
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, mask))
            {
                if(hit.transform.parent != null){
                    Boss boss = hit.transform.parent.GetComponent<Boss>();
                    if (boss != null)
                    {
                        boss.TakeDamage(wielder.GetStatValue(wielder.stats["ATK"])*damage);
                    }
                    //Blood/Explosion effect
                    //if (hit.collider.gameObject.name != "Boundaries")
                    //{
                    ParticleSystem newImpact = Instantiate(impactParticle, hit.point, Quaternion.identity).GetComponent<ParticleSystem>();
                    newImpact.Play();
                    impactParticles.Enqueue(newImpact.gameObject);
                    if (impactParticles.Count > MAX_IMPACTS)
                    {
                        Destroy(impactParticles.Dequeue());
                    }
                }
            }
        }
    }

    IEnumerator CoolDown()
    {
        yield return new WaitForSecondsRealtime(cooldowntime);
        canshoot = true;
    }

    IEnumerator RestoreDelay()
    {
        isRestoreDelayed = true;
        canRestore = false;
        yield return new WaitForSecondsRealtime(restoreDelayAmount);
        canRestore = true;
    }

}
