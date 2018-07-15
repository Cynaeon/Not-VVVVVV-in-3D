using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuButton : MonoBehaviour {

    public float scaleSpeed = 10;
    public float spawningSpeed = 30;
    public AudioClip menuNavigate;
    public AudioClip menuPress;

    private float delay;
    private float time;
    private float scale;
    private AudioSource _audio;

    void Start()
    {
        _audio = GameObject.Find("Audio Source").GetComponent<AudioSource>();
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

    public void MenuNavigate()
    {
        if (_audio)
            _audio.PlayOneShot(menuNavigate);
    }

    public void MenuPress()
    {
        if (_audio)
            _audio.PlayOneShot(menuPress);
    }
}
