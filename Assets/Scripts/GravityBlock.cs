using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityBlock : MonoBehaviour {

    public float rotationSpeed;
    public Transform player;

    private Gravity _gravity;

	// Use this for initialization
	void Start () {
        _gravity = GameObject.Find("Gravity").GetComponent<Gravity>();
	}
	
	// Update is called once per frame
	void Update () {
        Vector3 target = Vector3.zero;

        switch (_gravity.dirNumber)
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
        print(target);
	}
}
