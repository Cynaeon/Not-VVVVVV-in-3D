using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GoalPad : MonoBehaviour {

    public ParticleSystem p_goal;
    public Transform goalEffects;
    public float effectsAscendSpeed;
    public float timeUntilLevelChange;
    public AudioClip charging;
    public AudioClip success;
    public AudioClip blast;
    public AudioClip chimes;

    private GameObject player;
    private Vector3 pos;
    private Transform effectsInstance;
    private ParticleSystem p_instance;
    private AudioSource _audio;
    private bool activated;

    void Start () {
        player = GameObject.Find("Player");
        _audio = GetComponent<AudioSource>();
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
        {
            rot = new Vector3(0, 0, 0);
            pos.y += 24;
        }
        else if (Gravity.dirNumber == 1)
        {
            rot = new Vector3(0, 0, -90);
            pos.x -= 24;
        }
        else if (Gravity.dirNumber == 2)
        {
            rot = new Vector3(-90, 0, 0);
            pos.z -= 24;
        }
        p_instance = Instantiate(p_goal, pos, Quaternion.Euler(rot));
        _audio.pitch = 1;
        _audio.PlayOneShot(charging);
        _audio.PlayOneShot(success);
        activated = true;
        yield return new WaitForSeconds(p_instance.main.duration / 2);

        player.GetComponent<Renderer>().enabled = false;
        yield return new WaitForSeconds(p_instance.main.duration / 2);

        _audio.Stop();
        _audio.pitch = 1.5f;
        _audio.PlayOneShot(blast, 0.3f);
        _audio.PlayOneShot(chimes, 0.3f);
        
        effectsInstance = Instantiate(goalEffects, player.transform.position, Quaternion.identity);
        yield return new WaitForSeconds(timeUntilLevelChange);

        LevelManager.LoadNextLevel();
    }

    private void OnTriggerStay(Collider other)
    {
        if (!activated && other.tag == "Player")
        {
            if (other.GetComponent<Movement>().IsGrounded())
            {
                if (!p_instance)
                {
                    pos = other.transform.position;
                    if (GameManager.instance && GameManager.instance.currentLevel <= SceneManager.GetActiveScene().buildIndex)
                        GameManager.instance.currentLevel = 1 + SceneManager.GetActiveScene().buildIndex;

                    StartCoroutine(TriggerEffects());
                }
            }
        }
    }
}
