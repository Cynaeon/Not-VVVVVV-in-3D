using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalSphere : MonoBehaviour {

    public ParticleSystem p_feathers;
    public ParticleSystem p_blast;
    public ParticleSystem p_charge;

    private float chargeTime_max = 2.0f;
    private float chargeTime_current;
    private bool charging;
    private ParticleSystem p_charge_instance = null;

	void Update () {
		if (charging)
        {
            if (p_charge_instance == null)
            {
                p_charge_instance = Instantiate(p_charge, transform.position, Quaternion.identity);
            }
            chargeTime_current += Time.deltaTime;
            if (chargeTime_current >= chargeTime_max)
                Collected();
        }
        else
        {
            if (p_charge_instance)
            {
                Destroy(p_charge_instance);
            }
            chargeTime_current = 0;
        }
	}

    public void ChargeStart()
    {
        charging = true;
    }

    public void ChargeStop()
    {
        charging = false;
    }

    public void Collected()
    {
        Instantiate(p_feathers, transform.position, Quaternion.identity);
        Instantiate(p_blast, transform.position, Quaternion.identity);
        EventManager.TriggerEvent("levelCleared");
        Destroy(gameObject);
    }
}
