using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalPad : MonoBehaviour {

    public ParticleSystem p_goal;
    public Transform goalEffects;
    public float effectsAscendSpeed;
    public float timeUntilLevelChange;

    private GameObject player;
    private Vector3 pos;
    private Transform effectsInstance;
    private ParticleSystem p_instance;
    private bool activated;

    void Start () {
        player = GameObject.Find("Player");
    }

    private void Update()
    {
        if (effectsInstance)
        {
            effectsInstance.transform.Translate(-Gravity.direction * effectsAscendSpeed);
        }
    }

    IEnumerator TriggerEffects()
    {
        Vector3 rot = Vector3.zero;
        if (Gravity.dirNumber == 0)
            rot = new Vector3(0, 0, 0);
        if (Gravity.dirNumber == 1)
            rot = new Vector3(0, 0, -90);
        if (Gravity.dirNumber == 2)
            rot = new Vector3(-90, 0, 0);
        p_instance = Instantiate(p_goal, pos, Quaternion.Euler(rot));
        activated = true;
        yield return new WaitForSeconds(p_instance.main.duration / 2);

        player.GetComponent<Renderer>().enabled = false;
        yield return new WaitForSeconds(p_instance.main.duration / 2);

        effectsInstance = Instantiate(goalEffects, player.transform.position, Quaternion.identity);
        yield return new WaitForSeconds(timeUntilLevelChange);

        LevelManager.LoadNextLevel();
    }
    
    /*
    void TriggerEffects()
    { 
        instance = Instantiate(p_goal, pos, transform.parent.rotation);
        activated = true;
    }
    */

    private void OnTriggerStay(Collider other)
    {
        if (!activated && other.tag == "Player")
        {
            if (other.GetComponent<Movement>().IsGrounded())
            {
                if (!p_instance)
                {
                    pos = other.transform.position;
                    
                    StartCoroutine(TriggerEffects());
                }
            }
        }
    }
}
