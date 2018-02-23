using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityBlock : MonoBehaviour {

    public float rotationSpeed;

	void Start () { }
	
	void Update () {
        Vector3 target = Vector3.zero;

        switch (Gravity.dirNumber)
        {
            case 0:
                target = new Vector3(0, -1, 0);
                break;
            case 1:
                target = new Vector3(1, 0, 0);
                break;
            case 2:
                target = new Vector3(0, 0, 1);
                break;
        }
        float step = rotationSpeed * Time.deltaTime;
        Vector3 newDir = Vector3.RotateTowards(transform.forward, target, step, 0.0F);
        transform.rotation = Quaternion.LookRotation(newDir);
	}
}
