using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushBlock : MonoBehaviour {

    public float speed;
    public ParticleSystem particles_push;
    public AudioClip sliding;
    private bool moving;
    private Vector3 moveTarget;
    private AudioSource _audio;

	void Awake () {
        _audio = GetComponent<AudioSource>();
        moveTarget = transform.position;
	}
	
	void Update () {
        float step = speed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, moveTarget, step);
        if (transform.position == moveTarget)
            moving = false;
    }

    private void Push(Vector3 dir)
    {
        RaycastHit hit;

        // Dismiss if going to move against the gravity (i.e. into the air)
        if (Vector3.Dot(dir, Gravity.direction) == 1)
            return;

        // Dismiss if going to go through a solid obstacle
        if (Physics.Raycast(transform.position, dir, out hit, 1.5f))
            return;

        Vector3 dirAngledDown = (dir + (Gravity.direction / 1.3f)) * 2;
        //Debug.DrawRay(transform.position, dirAngledDown, Color.red, 1.5f);

        // Check if the spot we are going to is Push Panel
        if (Physics.Raycast(transform.position, dirAngledDown, out hit, 2f))
        {
            if (hit.transform.tag == "PushPanel")
            {
                if (_audio)
                {
                    _audio.pitch = 1.6f;
                    _audio.PlayOneShot(sliding);
                }
                moveTarget += dir * 2;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!moving && other.tag == "Blast")
        {
            Vector3 dir = transform.position - other.transform.position;
            dir = SnapToCardinalDir(dir);
            Push(dir);
            moving = true;
            Vector3 pos = other.gameObject.GetComponent<Collider>().ClosestPointOnBounds(transform.position);
            Instantiate(particles_push, pos, Quaternion.LookRotation(-dir));
        }
    }

    private Vector3 SnapToCardinalDir(Vector3 dir)
    {
        float x = Mathf.Abs(dir.x);
        float y = Mathf.Abs(dir.y);
        float z = Mathf.Abs(dir.z);
        float highest = 0;
        if (x > y)
            if (x > z)
                highest = x;
            else
                highest = z;
        else if (y > z)
            highest = y;
        else
            highest = z;

        if (highest == x)
        {
            if (dir.x >= 0)
                x = 1;
            else
                x = -1;
            y = 0;
            z = 0;
        } 
        else if (highest == y)
        {
            if (dir.y >= 0)
                y = 1;
            else
                y = -1;
            x = 0;
            z = 0;
        }
        else if (highest == z)
        {
            if (dir.z >= 0)
                z = 1;
            else
                z = -1;
            x = 0;
            y = 0;
        }
        Vector3 snappedDir = new Vector3(x, y, z);
        return snappedDir;
    }
}
