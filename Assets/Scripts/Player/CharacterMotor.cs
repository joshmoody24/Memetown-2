using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMotor : MonoBehaviour {

    public new GameObject camera;
    public Transform playerChild;
    private Rigidbody rb;
    public float speed = 10f;
    public float idleDelay = .1f;
    public float gravityScale = 1f;
    public float jumpSpeed = 1f;

    private float stickToGroundForce = 1f;
    private bool jump;
    private bool jumping;
    private bool previouslyGrounded;
    private Vector3 moveDir = Vector3.zero;
    private Vector3 explosionVector = Vector3.zero;

    public GameObject[] movementBlockers;

    private Animator[] animatables;
    private CharacterController cc;
    //used to communicate between fixedupdate and update
    private float x;
    private float z;

    private bool canMove;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        //get all animatable components
        animatables = GetComponentsInChildren<Animator>();
        canMove = true;
        cc = GetComponent<CharacterController>();
        jump = false;
        jumping = false;
    }

	void LateUpdate () {
		//ensure the sprite is always facing the camera
        float x = playerChild.eulerAngles.x;
        float z = playerChild.eulerAngles.z;
        playerChild.LookAt(playerChild.transform.position + camera.transform.rotation * Vector3.forward,
        camera.transform.rotation * Vector3.up);
        playerChild.rotation = Quaternion.Euler(new Vector3(x,playerChild.eulerAngles.y,z));

	}

    void FixedUpdate()
    {
        if (canMove)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Vector3 desiredMove = camera.transform.forward*z + camera.transform.right*x;
            // get a normal for the surface that is being touched to move along it
            RaycastHit hitInfo;
            Physics.SphereCast(transform.position, cc.radius, Vector3.down, out hitInfo,
                               cc.height/2f, Physics.AllLayers, QueryTriggerInteraction.Ignore);
            desiredMove = Vector3.ProjectOnPlane(desiredMove, hitInfo.normal).normalized;
            moveDir.x = desiredMove.x * speed;
            moveDir.z = desiredMove.z * speed;
            if (cc.isGrounded)
            {
                moveDir.y = -stickToGroundForce;

                if (jump)
                {
                    //remove the sqrt thing and uncomment the line a few lines down for more accurate but less fun slowmo
                    moveDir.y = jumpSpeed*Mathf.Sqrt(Time.timeScale);
                    jump = false;
                    jumping = true;
                }
            }
            else
            {
                moveDir += Physics.gravity*gravityScale*Time.fixedDeltaTime;// use for actual slow mo but less fun *(1/Time.timeScale);
            }
            moveDir += explosionVector;
            if(explosionVector != Vector3.zero){
                explosionVector.y = 0;
            }
            //m_CollisionFlags = m_CharacterController.Move(m_MoveDir*Time.fixedDeltaTime*(1/Time.timeScale));
            cc.Move(moveDir * Time.fixedDeltaTime* (1/Time.timeScale));
        }
    }

    void Update()
    {
        if (canMove && NoMovementBlockers())
        {
            Vector3 desiredMove = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
            x = desiredMove.x;
            z = desiredMove.z;
            //only update the animation values when player is moving. Otherwise, they stay the same to avoid flicker
            if (Input.GetButton("Horizontal") || Input.GetButton("Vertical"))
            {
                foreach (Animator anim in animatables)
                {
                    anim.SetFloat("HorizontalSpeed", x);
                    anim.SetFloat("VerticalSpeed", z);

                    anim.SetBool("Walking", true);
                }
            }
            else
            {
                foreach (Animator anim in animatables)
                {
                    anim.SetBool("Walking", false);
                }
            }
        }
        if (Input.GetButtonDown("Jump") && cc.isGrounded){
            jump = true;
            jumping = true;
        }
        if(!previouslyGrounded && cc.isGrounded){
            jumping = false;
            explosionVector = Vector3.zero;
        }
        previouslyGrounded = cc.isGrounded;
    }

    public void StopMovement()
    {
        /*
        foreach (Animator anim in animatables)
        {
            anim.SetBool("Walking", false);
        }
        */
        canMove = false;
        foreach (Animator anim in animatables)
        {
            //not setting to zero so that it doesn't snap to downward
            //also make sure it's not really small or it'll round to zero and snap down
            float currentHorizontal = anim.GetFloat("HorizontalSpeed");
            float currentVertical = anim.GetFloat("VerticalSpeed");
            if (anim.GetFloat("HorizontalSpeed") > 0.0001)
            {
                anim.SetFloat("HorizontalSpeed", currentHorizontal * 0.0001f);
            }
            if(anim.GetFloat("VerticalSpeed") > 0.0001)
            {
                anim.SetFloat("VerticalSpeed", currentVertical * 0.0001f);
            }

            anim.SetBool("Walking", false);
        }
    }
    public void ResumeMovement()
    {
        canMove = true;
    }

    //helps occasionally
    public void FaceDown()
    {
        foreach (Animator anim in animatables)
        {
            anim.SetFloat("HorizontalSpeed", 0);
            anim.SetFloat("VerticalSpeed", -.1f);

            anim.SetBool("Walking", false);
        }
    }
    //helps occasionally
    public void FaceUp()
    {
        foreach (Animator anim in animatables)
        {
            anim.SetFloat("HorizontalSpeed", 0);
            anim.SetFloat("VerticalSpeed", .1f);

            anim.SetBool("Walking", false);
        }
    }

    bool NoMovementBlockers()
    {
        bool canMove = true;
        foreach (GameObject obj in movementBlockers)
        {
            if (obj != null && obj.activeSelf)
            {
                canMove = false;
            }
        }
        return canMove;
    }

    //pauses desiredMove at one moment and that's it
    public void StopMovementInstantaneously()
    {
        rb.velocity = Vector2.zero;
        x = 0;
        z = 0;
        foreach (Animator anim in animatables)
        {
            //not setting to zero so that it doesn't snap to downward
            //also make sure it's not really small or it'll round to zero and snap down
            float currentHorizontal = anim.GetFloat("HorizontalSpeed");
            float currentVertical = anim.GetFloat("VerticalSpeed");
            if (anim.GetFloat("HorizontalSpeed") > 0.0001)
            {
                anim.SetFloat("HorizontalSpeed", currentHorizontal * 0.0001f);
            }
            if (anim.GetFloat("VerticalSpeed") > 0.0001)
            {
                anim.SetFloat("VerticalSpeed", currentVertical * 0.0001f);
            }

            anim.SetBool("Walking", false);
        }
    }

    public void AddExplosionForce(float force, Vector3 origin, float radius, float upwardsModifier){
        //not physicially accurate because there is no falloff
        Vector3 explosion = (transform.position + new Vector3(0, upwardsModifier, 0) - origin).normalized * force * Mathf.Sqrt(Time.timeScale);
        //physically accurate version
        //Vector3 explosion = (transform.position + new Vector3(0, upwardsModifier, 0) - origin).normalized * force * (1/Mathf.Pow(Vector3.Distance(transform.position, origin),2));
        explosionVector = explosion;
    }
}
