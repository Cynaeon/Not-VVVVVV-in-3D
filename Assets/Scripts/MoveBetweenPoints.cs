using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveBetweenPoints : MonoBehaviour {

    public float speed;

    public Transform pointA;
    public Transform pointB;

    private Transform target;

	void Start () {
        target = pointA;
	}
	
	void Update () {
        float step = speed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, target.position, step);
        if (transform.position == target.position)
        {
            if (target == pointA)
                target = pointB;
            else
                target = pointA;
        }
	}
}
