using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressAtoExit : MonoBehaviour {

	void Update () {
        if (Input.anyKey)
            Application.Quit();
	}
}
