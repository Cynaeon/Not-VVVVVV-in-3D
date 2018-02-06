using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gravity : MonoBehaviour {

    public int direction;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void ChangeClockwise()
    {
        direction++;
        if (direction > 2)
            direction = 0;
    }

    public void ChangeCounterclockwise()
    {
        direction--;
        if (direction < 0)
            direction = 2;
    }
}
