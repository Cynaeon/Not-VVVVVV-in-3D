using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InvertColors : MonoBehaviour {

    public float expandingSpeed;

    private float x;
    private UnityAction deathListener;
    private UnityAction freezeListener;
    private bool active;

    void Awake()
    {
        deathListener = new UnityAction(Activate);
        freezeListener = new UnityAction(Deactivate);
    }

    void OnEnable()
    {
        EventManager.StartListening("playerDeath", deathListener);
        EventManager.StartListening("freezeEnd", freezeListener);
    }

    void OnDisable()
    {
        EventManager.StopListening("playerDeath", deathListener);
        EventManager.StopListening("freezeEnd", freezeListener);
    }

    private void Update()
    {
        if (active)
        {
            if (x < 1)
            {
                x += expandingSpeed * Time.unscaledDeltaTime;
                transform.localScale = new Vector3(x, transform.localScale.y, transform.localScale.z);
            }
        }
    }

    void Activate()
    {
        active = true;
    }

    void Deactivate()
    {
        active = false;
        transform.localScale = new Vector3(0, transform.localScale.y, transform.localScale.z);
        x = 0;
    }


}
