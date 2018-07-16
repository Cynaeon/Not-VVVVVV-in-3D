using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lamp : MonoBehaviour {

    public Light lampLight;

    private bool lit = false;
    private Material _material;
    private Material _parentMaterial;

	void Start () {
        _material = GetComponent<Renderer>().material;
        _parentMaterial = transform.parent.GetComponent<Renderer>().material;
	}

	void Update () {

        
    }

    public void Toggle()
    {
        if (!lit)
        {
            lampLight.enabled = true;
            _material.EnableKeyword("_EMISSION");
            _parentMaterial.EnableKeyword("_EMISSION");
            lit = true;
        }
        else
        {
            lampLight.enabled = false;
            _material.DisableKeyword("_EMISSION");
            _parentMaterial.DisableKeyword("_EMISSION");
            lit = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Blast")
            Toggle();
    }
}
