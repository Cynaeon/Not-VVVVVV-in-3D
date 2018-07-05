using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuButton : MonoBehaviour {

    public float scaleSpeed = 10;
    public float spawningSpeed = 30;

    private float delay;
    private float time;
    private float scale;

    void Start()
    {
        delay = transform.GetSiblingIndex();
    }

    void OnEnable () {
        transform.localScale = Vector3.up;
        scale = 0;
        time = 0;
	}
	
	void Update () {

        if (time < delay)
        {
            time += Time.unscaledDeltaTime * spawningSpeed;
            return;
        }

        scale += scaleSpeed * Time.unscaledDeltaTime;
        scale = Mathf.Clamp(scale, 0, 1);
        transform.localScale = new Vector3(scale, 1, 1);
	}
}
