using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.Playables;

public class Boss : BattleAgent
{
    public Slider bossHPSlider;
    public Text bossHPText;
    public float lowhpThreshold = .5f;
    public Transform player;
    public Transform laserTarget;
    public GameObject smokingParticle;

    public float attackRadius = 20f;

    private NavMeshAgent agent;

    public Animator laserAnim;

    public Transform laser;

    public float rotSpeed = 1f;

    private Vector3 initialPos;

    public Animator[] laserArray;

    private Animator animator;
    public float laserArrayDuration = 5f;

    public float explosionPower = 10000f;
    public float explosionDamage = 5f;

    new void Start(){
        //todo, make sure this line works as intended
        base.Start();
        
        bossHPSlider.maxValue = (float)GetStatValue(stats["maxHP"]);
        bossHPSlider.minValue = 0;
        bossHPSlider.value = (float)hp;
        bossHPText.text = hp.ToString();
        agent = GetComponent<NavMeshAgent>();
        initialPos = transform.position;
        animator = GetComponent<Animator>();
    }

    public new void TakeDamage(float amount){
        base.TakeDamage(amount);
        bossHPSlider.value = (float)hp;
        bossHPText.text = hp.ToString();
        if(hp < GetStatValue(stats["maxHP"]) * lowhpThreshold && animator.GetInteger("Phase") == 1){
            NextPhase();
        }
    }
	
	// Update is called once per frame
	void Update () {
        //transform.LookAt(player.position);
        transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);
        if(agent.isActiveAndEnabled == true)
        {
            agent.SetDestination(player.position);
        }
        if(Vector3.Distance(player.position,transform.position) < agent.stoppingDistance)
        {
            //transform.LookAt(player.position);
            //slower version:
            Vector3 dirToTarget = player.position - transform.position;
            dirToTarget.y = 0.0f;
            Quaternion lookRotation = Quaternion.LookRotation(dirToTarget);
            transform.rotation = Quaternion.Lerp(transform.rotation, lookRotation, Time.deltaTime * rotSpeed);
            transform.eulerAngles = new Vector3(0f, transform.eulerAngles.y, transform.eulerAngles.z);
        }
        if(Vector3.Distance(player.position, transform.position) <= attackRadius)
        {
            if (checkLineOfSight())
            {
                animator.SetBool("CanSeePlayer", true);
            }
            else{
                animator.SetBool("CanSeePlayer", false);
            }
        }
        animator.SetFloat("DistanceToPlayer", Vector3.Distance(player.position, transform.position));
	}

    public void ShootLaser(){
        //debug
        laserAnim.SetTrigger("shoot");
    }

    public void FireLaserArray(){
        foreach(Animator a in laserArray){
            StartCoroutine("StartLaserWithRandomDelay",a);
        }
    }

    private IEnumerator StartLaserWithRandomDelay(Animator anim){
        yield return new WaitForSeconds(Random.Range(0,laserArrayDuration));
        anim.SetTrigger("shoot");
    }

    //todo: incorporate into TakeDamage new
    /*
    public void takeDamage(int amount)
    {
        hp -= amount;
        bossHPSlider.value = (float)hp;
        if(hp <= 0)
        {
            amount = 0;
            kill();
        }
        if(hp < lowhpThreshold)
        {
            smokingParticle.SetActive(true);
        }
    }
    */
    public void kill()
    {
        StopAllCoroutines();
        agent.enabled = false;
        player.GetComponent<Player>().DisableControl();
        Debug.Log("killed boss");
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
        animator.SetTrigger("Restart");
        hp = GetStatValue(stats["maxHP"]);
        bossHPSlider.value = (float)hp;
        bossHPText.text = hp.ToString();
        //transform.position = initialPos;
        //this is the best way to do it, apparently
        agent.Warp(initialPos);
        player.GetComponent<Player>().Restart();
        smokingParticle.SetActive(false);
    }

    public void DestroyMoon(){
        Moon moon = GameObject.Find("Moon").GetComponent<Moon>();
        if(moon != null){
            moon.Fracture();
        }
        else{
            Debug.Log("Error: no moon to destroy");
        }
    }

    public void NextPhase(){
        animator.SetInteger("Phase", animator.GetInteger("Phase") + 1);
        Debug.Log("Phase " + animator.GetInteger("Phase") + " initiated");
    }

    public void SetMoonDestroyed(){
        animator.SetBool("MoonDestroyed", true);
    }

    public void Explode(){
        player.GetComponent<Player>().TakeDamage(GetStatValue(stats["ATK"])*explosionDamage);
        Rigidbody prb = player.GetComponent<Rigidbody>();
        float EXPLOSION_UPWARDS_MODIFIER = 6f;
        player.GetComponent<CharacterMotor>().AddExplosionForce(explosionPower, transform.position, 100f, EXPLOSION_UPWARDS_MODIFIER);
        GetComponent<AudioSource>().Play();
    }
}
