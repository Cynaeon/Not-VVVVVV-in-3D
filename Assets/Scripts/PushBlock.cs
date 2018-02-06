using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushBlock : MonoBehaviour {

    public float speed;
    public ParticleSystem particles_push;

    private Vector3 moveTarget;

	void Start () {
        moveTarget = transform.position;
	}
	
	void Update () {
        float step = speed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, moveTarget, step);
    }

    private void Push(Vector3 dir)
    {
        
        moveTarget += dir * 2;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Blast")
        {
            Vector3 dir = transform.position - other.transform.position;
            dir = SnapToCardinalDir(dir);
            Push(dir);

            Vector3 pos = other.gameObject.GetComponent<Collider>().ClosestPointOnBounds(transform.position);
            Instantiate(particles_push, pos, Quaternion.identity);
            Debug.DrawRay(transform.position, dir, Color.green, 2);
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
