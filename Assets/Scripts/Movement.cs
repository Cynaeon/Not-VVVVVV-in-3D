using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour {

    public float speed;
    public float jumpForce;
    public float gravityPull;
    public int midairJumps;
    public float blastActiveTime;

    public GameObject blastTrigger;
    public GameObject trail;
    public ParticleSystem particlesBlast;
    public ParticleSystem particlesJump;
    public ParticleSystem particlesBuildUp;
    public ParticleSystem particlesDeath;
    public ParticleSystem particlesWarp;
    public ParticleSystem particlesSpawn;

    // Temp variables (hopefully) (not really xd)
    private bool onGravityBlock;

    private bool hasJumped;
    private bool dead;
    private bool disableInput;
    private int midairJumpCount;
    private float respawnTime = 0.5f;
    private float freezeTime = 0.5f;
    private CharacterController _controller;
    private Renderer _rend;
    private Vector3 movement;
    private float verticalMovement;
    private ParticleSystem p_instance;

    void Start()
    {
        _controller = GetComponent<CharacterController>();
        _rend = GetComponent<Renderer>();
        _rend.enabled = false;
        disableInput = true;
        blastTrigger.SetActive(false);
        StartCoroutine(Respawn());
    }

    void Update () {
        if (!dead && !disableInput)
            PlayerInput();
    }

    private void PlayerInput()
    {
        // Fail-safe if the player falls out of bounds
        if (Vector3.Magnitude(transform.position) > 40)
            StartCoroutine(Die());

        if (IsGrounded() || (!IsGrounded() && onGravityBlock))
        {
            // Reset vertical movement and jump count and allow gravity changing
            if (verticalMovement < 0)
                verticalMovement = 0;
            midairJumpCount = 0;

            if (Input.GetButtonDown("GravityRight"))
                Gravity.ChangeClockwise();
            else if (Input.GetButtonDown("GravityLeft"))
                Gravity.ChangeCounterclockwise();
        }
        else
        {
            // Apply gravity
            verticalMovement -= gravityPull * Time.deltaTime;
        }

        if (Input.GetButtonDown("Jump"))
        {
            if (IsGrounded())
                verticalMovement = jumpForce;
            else if (midairJumpCount < midairJumps)
            {
                verticalMovement = jumpForce;
                midairJumpCount++;
                Instantiate(particlesJump, transform.position, Quaternion.identity);
            }
        }

        if (Input.GetButtonDown("Blast"))
            StartCoroutine(Blast());

        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        Vector2 input = new Vector2(horizontal, vertical);
        if (input.magnitude > 1)
            input.Normalize();

        switch (Gravity.dirNumber)
        {
            case 0:
                movement = new Vector3(horizontal, verticalMovement, vertical);
                movement.x = SnapTo(movement.x) * input.magnitude;
                movement.z = SnapTo(movement.z) * input.magnitude;
                movement = Quaternion.Euler(0, 45, 0) * movement;
                break;
            case 1:
                movement = new Vector3(-verticalMovement, -vertical, horizontal);
                movement.y = SnapTo(movement.y) * input.magnitude;
                movement.z = SnapTo(movement.z) * input.magnitude;
                movement = Quaternion.Euler(-45, 0, 0) * movement;
                break;
            case 2:
                movement = new Vector3(vertical, -horizontal, -verticalMovement);
                movement.x = SnapTo(movement.x) * input.magnitude;
                movement.y = SnapTo(movement.y) * input.magnitude;
                movement = Quaternion.Euler(0, 0, -45) * movement;
                break;
        }
        movement *= speed;
        _controller.Move(movement * Time.deltaTime);

        // Face the movement direction
        if (movement.magnitude > 0)
            transform.rotation = Quaternion.LookRotation(movement);
    }

    // This here grants 8-directional movement
    float SnapTo(float value)
    {
        float abs = Mathf.Abs(value);
        if (abs > 0.2f)
            abs = .75f;
        else
            abs = 0;
        if (value < 0)
            abs = -abs;
        return abs;
    }

    IEnumerator Blast()
    {
        blastTrigger.SetActive(true);
        ParticleSystem blast = Instantiate(particlesBlast, transform.position, Quaternion.identity);
        blast.transform.parent = transform;
        yield return new WaitForSeconds(blastActiveTime);
        blastTrigger.SetActive(false);
    }

    IEnumerator Die()
    {
        Instantiate(particlesBuildUp, transform.position, Quaternion.identity);
        dead = true;
        Time.timeScale = 0;
        EventManager.TriggerEvent("playerDeath");
        yield return new WaitForSecondsRealtime(freezeTime);

        Time.timeScale = 1;
        EventManager.TriggerEvent("freezeEnd");
        Instantiate(particlesDeath, transform.position, Quaternion.identity);
        GetComponent<Renderer>().enabled = false;
        trail.SetActive(false);
        yield return new WaitForSeconds(respawnTime);

        StartCoroutine(Respawn());
    }

    IEnumerator Respawn()
    {
        Vector3 startPos;
        if (GameObject.FindGameObjectWithTag("Respawn")) 
            startPos = GameObject.FindGameObjectWithTag("Respawn").transform.position;
        else
            startPos = new Vector3(-10, 1, -10);
        transform.position = startPos;
        Gravity.dirNumber = 0;
        startPos.y = 15;
        p_instance = Instantiate(particlesWarp, startPos, Quaternion.identity);
        yield return new WaitForSeconds(p_instance.main.duration - .1f);

        p_instance = null;
        _rend.enabled = true;
        Instantiate(particlesSpawn, transform.position, Quaternion.identity);
        trail.SetActive(true);
        dead = false;
        disableInput = false;
    }

    /*
    private void Respawn()
    {
        dead = false;
        GetComponent<Renderer>().enabled = true;
        trail.SetActive(true);
        transform.position = new Vector3(-10, 1, -10);
        _gravity.dirNumber = 0;
    }
    */

    internal bool IsGrounded()
    {
        Vector3 dir = Gravity.direction;
        RaycastHit hit;
        
        if (Physics.SphereCast(transform.position, .35f, dir, out hit, 0.3f))
        {

            return true;
            /*
            if (hit.transform.tag == "Ground" || hit.transform.tag == "PushPanel" || hit.transform.tag == "Respawn")
                return true;
            else
                return false;
                */
        }
        else
            return false;
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Finish" && IsGrounded())
        {
            disableInput = true;
        }

        if (other.tag == "Danger" && !dead)
            StartCoroutine(Die());
        if (other.tag == "PickUp")
            other.GetComponent<GoalSphere>().ChargeStart();
        if (other.tag == "GravityBlock" && IsGrounded())
        {
            transform.parent = other.transform.parent;
            onGravityBlock = true;
        }
    }

    private void OnTriggerStay(Collider other)
    {

        if (other.tag == "Finish" && IsGrounded())
        {
            disableInput = true;
        }

        if (other.tag == "GravityBlock" && IsGrounded())
        {
            transform.parent = other.transform.parent;
            onGravityBlock = true;
        }

        if (other.tag == "Platform" && IsGrounded())
            transform.parent = other.transform;

    }

    
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Platform")
            transform.parent = null;
        if (other.tag == "PickUp")
            other.GetComponent<GoalSphere>().ChargeStop();
        if (other.tag == "GravityBlock")
        {
            onGravityBlock = false;
            transform.parent = null;
        }
    }
}
