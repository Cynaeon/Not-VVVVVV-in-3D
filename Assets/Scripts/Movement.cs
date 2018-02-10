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
    private float respawnTime = 1;
    private float freezeTime = 0.5f;
    private float timeDead;
    private float timeBlast;
    private Gravity _gravity;
    private CharacterController _controller;
    [SerializeField] private Vector3 movement;
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
        else
        {
            timeDead += Time.unscaledDeltaTime;
            if (Time.timeScale == 0 && timeDead >= respawnTime / 2)
            {
                Time.timeScale = 1;
                EventManager.TriggerEvent("freezeEnd");
                Instantiate(particlesDeath, transform.position, Quaternion.identity);
                GetComponent<Renderer>().enabled = false;
                trail.SetActive(false);
                
            }
            if (timeDead >= respawnTime)
                Respawn();
        }
    }

    private void PlayerInput()
    {
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
        {
            Blast();
        }

        if (blastTrigger.activeSelf)
        {
            timeBlast += Time.deltaTime;
            if (timeBlast >= blastActiveTime)
            {
                timeBlast = 0;
                blastTrigger.SetActive(false);
            }
        }

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

    private void Blast()
    {
        blastTrigger.SetActive(true);
        ParticleSystem blast = Instantiate(particlesBlast, transform.position, Quaternion.identity);
        blast.transform.parent = transform;
    }

    private void Die()
    {
        Instantiate(particlesBuildUp, transform.position, Quaternion.identity);
        dead = true;
        timeDead = 0;
        Time.timeScale = 0;
        EventManager.TriggerEvent("playerDeath");
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
            Die();
        if (other.tag == "PickUp")
        {
            other.GetComponent<GoalSphere>().ChargeStart();
        }
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
