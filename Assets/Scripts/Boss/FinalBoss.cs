using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityStandardAssets.Characters.FirstPerson;

public class FinalBoss : MonoBehaviour {

    public int health = 200;
    public int lowHealthThreshold = 50;
    public Transform player;
    public Transform laserTarget;
    public GameObject smokingParticle;

    public float attackRadius = 20f;

    public float cooldown = 7f;
    public float lowHealthCooldown = 4f;

    private NavMeshAgent agent;

    private bool canShoot;

    public Animator laserAnim;

    public Slider hpSlider;

    public Transform laser;

    public float rotSpeed = 1f;

    private int initialHealth;

    private Vector3 initialPos;
    private Vector3 initialPlayerPos;

    public UnityEvent OnKill;

	// Use this for initialization
	void Start () {
        agent = GetComponent<NavMeshAgent>();
        canShoot = false;
        StartCoroutine(CoolDown());
        hpSlider.maxValue = health;
        hpSlider.minValue = 0;
        hpSlider.value = health;
        initialHealth = health;
        initialPos = transform.position;
        initialPlayerPos = player.position;
	}
	
	// Update is called once per frame
	void Update () {
        //transform.LookAt(player.position);
        transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);
        if(agent.isActiveAndEnabled == true)
        {
            agent.SetDestination(player.position);
        }
        /*
        if(Vector3.Distance(player.position, transform.position) <= minDistance)
        {
            agent.isStopped = true;
        }
        else
        {
            agent.isStopped = false;
        }
        */
        if(Vector3.Distance(player.position,transform.position) < agent.stoppingDistance)
        {
            transform.LookAt(player.position);
            transform.eulerAngles = new Vector3(0f, transform.eulerAngles.y, transform.eulerAngles.z);
        }
        if(Vector3.Distance(player.position, transform.position) <= attackRadius && canShoot)
        {
            if (checkLineOfSight())
            {
                Shoot();
            }
        }
	}

    public void Shoot()
    {
        laserAnim.SetTrigger("shoot");
        canShoot = false;
        StopAllCoroutines();
        StartCoroutine(CoolDown());
    }

    public IEnumerator CoolDown()
    {
        if(health > lowHealthThreshold)
        {
            yield return new WaitForSeconds(cooldown);
        }
        else
        {
            Debug.Log("low health cooldown");
            yield return new WaitForSeconds(lowHealthCooldown);
        }
        canShoot = true;
    }

    public void takeDamage(int amount)
    {
        health -= amount;
        hpSlider.value = health;
        if(health <= 0)
        {
            amount = 0;
            kill();
        }
        if(health < lowHealthThreshold)
        {
            smokingParticle.SetActive(true);
        }
    }
    public void kill()
    {
        StopAllCoroutines();
        agent.enabled = false;
        player.GetComponent<FirstPersonController>().enabled = false;
        Debug.Log("killed boss");
        canShoot = false;
        OnKill.Invoke();
    }

    bool checkLineOfSight()
    {
        //ignore boss layer
        int layermask = 1 << 11;
        layermask = ~layermask;

        RaycastHit hit;
        Ray ray = new Ray();
        ray.origin = laser.position;
        ray.direction = (laserTarget.position - laser.position).normalized;
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, layermask))
        {
            Debug.DrawLine(ray.origin, hit.point);
            if(hit.collider.tag == "Player")
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            return false;
        }
    }

    public void Restart()
    {
        StopAllCoroutines();
        canShoot = false;
        StartCoroutine(CoolDown());
        health = initialHealth;
        //transform.position = initialPos;
        //this is the best way to do it, apparently
        agent.Warp(initialPos);
        FirstPersonController fpc = player.GetComponent<FirstPersonController>();
        fpc.ResetRotation();
        //character controller overrides setting player position. Got to disable it first
        CharacterController c = player.GetComponent<CharacterController>();
        c.enabled = false;
        player.position = initialPlayerPos;
        hpSlider.value = initialHealth;
        c.enabled = true;
        smokingParticle.SetActive(false);
    }
}
