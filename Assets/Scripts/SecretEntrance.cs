using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SecretEntrance : MonoBehaviour {

    public int levelToLoad;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player") { SceneManager.LoadScene(levelToLoad); }
    }
}
