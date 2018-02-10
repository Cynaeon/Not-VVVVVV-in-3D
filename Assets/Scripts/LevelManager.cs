using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour {

    public float timeTillLoad = 3.0f;
    private float timeLoaded;
    private bool loading;

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

        if (!FindObjectOfType<GoalSphere>())
        {
            loading = true;
        }
        else
            loading = false;

        if (loading)
        {
            timeLoaded += Time.deltaTime;
            if (timeLoaded >= timeTillLoad)
                LoadNextLevel();

        }
    }

    public void PrepareToLoad()
    {
        loading = true;
    }

    public void LoadNextLevel()
    {
        timeLoaded = 0;
        loading = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
