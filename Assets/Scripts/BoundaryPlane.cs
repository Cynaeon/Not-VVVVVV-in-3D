using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoundaryPlane : MonoBehaviour {

    private Collider collider;

	// Use this for initialization
	void Start () {
        collider = GetComponent<Collider>();
	}
	
	// Update is called once per frame
	void Update () {
        collider.enabled = transform.up != Gravity.direction;
	}
}
