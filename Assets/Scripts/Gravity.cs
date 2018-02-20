using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gravity : MonoBehaviour {

    public int dirNumber;
    public Vector3 direction;

	void Update () {
        if (dirNumber == 0)
            direction = Vector3.down;
        else if (dirNumber == 1)
            direction = Vector3.right;
        else if (dirNumber == 2)
            direction = Vector3.forward;

        Physics.gravity = direction;

	}

    public void ChangeClockwise()
    {
        dirNumber++;
        if (dirNumber > 2)
            dirNumber = 0;
    }

    public void ChangeCounterclockwise()
    {
        dirNumber--;
        if (dirNumber < 0)
            dirNumber = 2;
    }
}
