using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour {

    public float timeTillLoad = 3.0f;
    private float timeLoaded;
    private bool loading;

    private UnityAction levelClear;

    void Awake()
    {
        DontDestroyOnLoad(transform.gameObject);
    }

    private void Update()
    {
        if (SceneManager.GetActiveScene().name == "menu")
        {
            LoadNextLevel();
            return;
        } 
    }

    public static void LoadNextLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    void OnEnable()
    {
        EventManager.StartListening("levelCleared", levelClear);
    }

    private void OnLevelWasLoaded(int level)
    {
        EventManager.StartListening("levelCleared", levelClear);
    }

    void OnDisable()
    {
        EventManager.StopListening("levelCleared", levelClear);
    }
}
