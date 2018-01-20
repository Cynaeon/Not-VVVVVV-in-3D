using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour {

    public float speed;
    public float jumpForce;
    public float gravity;
    public int midairJumps;

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
    private CharacterController _controller;
    [SerializeField] private Vector3 movement;
    private float verticalMovement;
    public int gravityDir;

    void Start()
    {
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
            if (verticalMovement < 0)
                verticalMovement = 0;
            midairJumpCount = 0;

            if (Input.GetButtonDown("GravityRight"))
            {
                gravityDir++;
                if (gravityDir > 2)
                    gravityDir = 0;
            }
            else if (Input.GetButtonDown("GravityLeft"))
            {
                gravityDir--;
                if (gravityDir < 0)
                    gravityDir = 2;
            }
        }
        else
            verticalMovement -= gravity * Time.deltaTime;

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
            blastTrigger.SetActive(true);
            Blast();
        }

        if (gravityDir == 0)
        {
            movement = new Vector3(Input.GetAxis("Horizontal"), verticalMovement, Input.GetAxis("Vertical"));
            movement = Quaternion.Euler(0, 45, 0) * movement;
        }
        else if (gravityDir == 1)
        {
            movement = new Vector3(-verticalMovement, -Input.GetAxis("Vertical"), Input.GetAxis("Horizontal"));
            movement = Quaternion.Euler(-45, 0, 0) * movement;
        }
        else if (gravityDir == 2)
        {
            movement = new Vector3(Input.GetAxis("Vertical"), -Input.GetAxis("Horizontal"), -verticalMovement);
            movement = Quaternion.Euler(0, 0, -45) * movement;
        }
        //movement = transform.TransformDirection(movement);
        //movement = Camera.main.transform.TransformVector(move);
        //movement.y = 0;
        movement *= speed;
        _controller.Move(movement * Time.deltaTime);
    }

    private void Blast()
    {
        Instantiate(particlesBlast, transform.position, Quaternion.identity);
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
        gravityDir = 0;
    }

    private bool IsGrounded()
    {
        Vector3 dir = Vector3.zero;
        if (gravityDir == 0)
            dir = Vector3.down;
        else if (gravityDir == 1)
            dir = Vector3.right;
        else if (gravityDir == 2)
            dir = Vector3.forward;

        RaycastHit hit;
       
        if (Physics.Raycast(transform.position, dir, out hit,0.7f))
        {
            if (hit.transform.tag != "Danger")
                return true;
            else
                return false;
        }
        else
            return false;
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Platform")
        {
            transform.parent = other.transform;
        }
        if (other.tag == "Danger")
            Die();
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Platform")
            transform.parent = null;
    }

}
