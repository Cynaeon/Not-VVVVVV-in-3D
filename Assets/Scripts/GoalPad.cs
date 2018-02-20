using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalPad : MonoBehaviour {

    public ParticleSystem p_goal;

    private ParticleSystem instance;
    private bool activated;

    void Start () {
    }

    private void Update()
    {
        if (instance)
        {
            if (!instance.IsAlive())
            {
                EventManager.TriggerEvent("levelCleared");
                Destroy(instance);
            }
        }
    }

    void TriggerEffects()
    {
        instance = Instantiate(p_goal, transform.parent.position, transform.parent.rotation);
        activated = true;
    }

    private void OnTriggerStay(Collider other)
    {
        if (!activated && other.tag == "Player")
        {
            if (other.GetComponent<Movement>().IsGrounded())
            {
                if (!instance)
                    TriggerEffects();
            }
        }
    }
}
