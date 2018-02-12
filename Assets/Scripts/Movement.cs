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

    private bool hasJumped;
    private bool dead;
    private int midairJumpCount;
    private float respawnTime = 0.5f;
    private float freezeTime = 0.5f;
    private Gravity _gravity;
    private CharacterController _controller;
    private Vector3 movement;
    private float verticalMovement;

    void Start()
    {
        _gravity = GameObject.Find("Gravity").GetComponent<Gravity>();
        _controller = GetComponent<CharacterController>();
        blastTrigger.SetActive(false);
    }

    void Update () {

        if (!dead)
            PlayerInput();
    }

    private void PlayerInput()
    {
        // Fail-safe if the player falls out of bounds
        if (Vector3.Magnitude(transform.position) > 40)
            StartCoroutine(Die());

        if (IsGrounded())
        {
            // Reset vertical movement and jump count and allow gravity changing
            if (verticalMovement < 0)
                verticalMovement = 0;
            midairJumpCount = 0;

            if (Input.GetButtonDown("GravityRight"))
                _gravity.ChangeClockwise();
            else if (Input.GetButtonDown("GravityLeft"))
                _gravity.ChangeCounterclockwise();
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

        switch (_gravity.dirNumber)
        {
            case 0:
                movement = new Vector3(horizontal, verticalMovement, vertical);
                movement = Quaternion.Euler(0, 45, 0) * movement;
                break;
            case 1:
                movement = new Vector3(-verticalMovement, -vertical, horizontal);
                movement = Quaternion.Euler(-45, 0, 0) * movement;
                break;
            case 2:
                movement = new Vector3(vertical, -horizontal, -verticalMovement);
                movement = Quaternion.Euler(0, 0, -45) * movement;
                break;
        }
        movement *= speed;
        _controller.Move(movement * Time.deltaTime);
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

        Respawn();
    }

    private void Respawn()
    {
        dead = false;
        GetComponent<Renderer>().enabled = true;
        trail.SetActive(true);
        transform.position = new Vector3(-10, 1, -10);
        _gravity.dirNumber = 0;
    }

    private bool IsGrounded()
    {
        Vector3 dir = _gravity.direction;
        RaycastHit hit;
       
        if (Physics.Raycast(transform.position, dir, out hit, 0.7f))
        {
            if (hit.transform.tag == "Ground" || hit.transform.tag == "PushPanel")
                return true;
            else
                return false;
        }
        else
            return false;
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Danger")
            StartCoroutine(Die());
        if (other.tag == "PickUp")
            other.GetComponent<GoalSphere>().ChargeStart();

    }

    private void OnTriggerStay(Collider other)
    {

        if (other.tag == "Platform" && IsGrounded())
        {
            transform.parent = other.transform.parent;
        }
        else
            transform.parent = null;
    }

    
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Platform")
            transform.parent = null;
        if (other.tag == "PickUp")
            other.GetComponent<GoalSphere>().ChargeStop();
    }
}
