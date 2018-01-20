using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lamp : MonoBehaviour {

    public Light lampLight;

    private bool lit = true;
    private Material _material;

	void Start () {
        _material = GetComponent<Renderer>().material;
	}

	void Update () {

        if (Input.GetKeyDown(KeyCode.Q))
            Toggle();
    }

    public void Toggle()
    {
        if (!lit)
        {
            lampLight.enabled = true;
            _material.EnableKeyword("_EMISSION");
            lit = true;
        }
        else
        {
            lampLight.enabled = false;
            _material.DisableKeyword("_EMISSION");
            lit = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Blast")
            Toggle();
    }
}
