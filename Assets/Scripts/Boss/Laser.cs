using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Laser : MonoBehaviour {

    public Transform startPos;
    public Transform endPos;
    public Transform target;
    private LineRenderer lineRenderer;
    public float fireSpeed = 100f;

    public ParticleSystem impact;

    private bool isExtending = false;
    private Vector3 hitPos;
    private float t;

    public float explosionRadius = 1f;
    public float explosionStrength = 1f;

    private AudioSource audiosource;

    private const float impactlength = 1.9f;

    private EdgeCollider2D lasercollision;

    private bool hitPlayer;
    public Player player;
    public CharacterController playerCC;
    public Boss boss;

    private bool hasHit;

    // Use this for initialization
    void Start () {
        lineRenderer = GetComponent<LineRenderer>();
        endPos.position = startPos.position;
        isExtending = false;
        audiosource = GetComponent<AudioSource>();
        lasercollision = GetComponent<EdgeCollider2D>();
        playerCC = GameObject.FindWithTag("Player").GetComponent<CharacterController>();
	}

    // Update is called once per frame
    void Update () {
        lineRenderer.SetPosition(0, startPos.position);
        lineRenderer.SetPosition(1, endPos.position);

        if (isExtending)
        {
            CheckCollisionWithPlayer();
            //dividing by distance makes the laser travel at an absolute speed. In playtesting this was less fun
            t += (fireSpeed * Time.deltaTime); // / Vector3.Distance(startPos.position, hitPos);
            if (t >= 1 && hasHit == false)
            {
                hasHit = true;
                endPos.position = hitPos;
                StartCoroutine(StopLaser());
                //hit effect
                ParticleSystem hit = Instantiate(impact, hitPos, Quaternion.identity);
                hit.Play();
                hit.GetComponent<AudioSource>().Play();
                StartCoroutine(DespawnImpact(hit.gameObject));
            }
            else{
                endPos.position = Vector3.Lerp(startPos.position, hitPos, t);
            }
        }
        else{
            lineRenderer.SetPosition(1, startPos.position);
            hasHit = false;
        }
	}

    public void FireLaser()
    {
        hitPlayer = false;
        audiosource.Play();
        //simple aim-at-player, doesn't check for other objects
        //hitPos = target.position;
        hitPos = FireLaserRaycast();
        if(hitPos != Vector3.zero)
        {
            t = 0;
            isExtending = true;
        }
        else
        {
            Debug.Log("laser didn't hit anything?");
        }
    }

    Vector3 FireLaserRaycast()
    {
        //ignore player and boss layers (so the boss fires at whatever is behind the player)
        string[] ignoreLayers = {"Boss", "Player"};
        LayerMask layermask = LayerMask.GetMask(ignoreLayers);
        layermask = ~layermask;

        RaycastHit hit;
        Ray ray = new Ray();
        ray.origin = startPos.position;
        
        //50% chance to shot lead, 50% chance to aim directly at player
        float randy = Random.Range(0,1f);
        if(randy < 0.5f){
            //shotlead version
            float distanceToPlayer = Vector3.Distance(target.position, startPos.position);
            //lasers currently have relative speed. For absolute, replace / firespeed with comment
            Vector3 shotLeadPos = target.position + (playerCC.velocity * Time.timeScale) / fireSpeed;// * (distanceToPlayer/fireSpeed);
            ray.direction = (shotLeadPos - startPos.position).normalized;
        }
        else{
            //direct aim version
            ray.direction = (target.position - startPos.position).normalized;
        }

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, layermask))
        {
            return hit.point;
        }
        else
        {
            return Vector3.zero;
        }
    }

    //can probably fix the laser movement glitch by updating the laser position while stop laser is waiting
    IEnumerator StopLaser()
    {
        yield return new WaitForSeconds(.5f);
        endPos.position = startPos.position;
        isExtending = false;
    }

    IEnumerator DespawnImpact(GameObject toDestroy)
    {
        yield return new WaitForSeconds(impactlength);
        Destroy(toDestroy);
    }

    void CheckCollisionWithPlayer()
    {
        //check to see if player was hit by the tip of the laser and
        //check to see if player was hit by the explosion
        if((Vector3.Distance(endPos.position, target.position) < explosionRadius) && hitPlayer == false)
        {
            hitPlayer = true;
            Debug.Log("hit player with explosion for damage of " + boss.GetStatValue(boss.stats["ATK"]));
            player.TakeDamage(boss.GetStatValue(boss.stats["ATK"]));
        }
        else
        {
            //Debug.Log("Player distance: " + Vector3.Distance(endPos.position, target.position));
        }
    }
}
