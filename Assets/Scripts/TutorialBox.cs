using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialBox : MonoBehaviour {

    public float waitUntilShow;
    public float showDuration;
    public float speed;

    private Vector3 endPosition;
    private Vector3 startPosition;

	void Start () {
        endPosition = transform.position;
        startPosition = endPosition - new Vector3(0, 200, 0);
        transform.position = startPosition;
	}
	
	void Update () {
		if (waitUntilShow > 0)
        {
            waitUntilShow -= Time.deltaTime;
        }
        else if (showDuration > 0)
        {
            transform.position = Vector3.MoveTowards(transform.position, endPosition, speed * Time.deltaTime);
            showDuration -= Time.deltaTime;
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, startPosition, speed * Time.deltaTime);
        }
	}
}
