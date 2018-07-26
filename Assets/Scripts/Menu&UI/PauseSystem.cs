using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class PauseSystem : MonoBehaviour {

    public static PauseSystem instance = null;
    public GameObject pausePanel;
    public GameObject selectedButton;
    public static bool paused;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }

    void Update () {
		if ((Input.GetKeyDown(KeyCode.P) || Input.GetButtonDown("Pause")) && SceneManager.GetActiveScene().buildIndex != 0)
        {
            TogglePause();
        }
	}

    void SetSelected() { EventSystem.current.SetSelectedGameObject(selectedButton); }

    public void TogglePause()
    {
        if (!paused)
        {
            //Invoke("SetSelected", 0.1f);
            paused = true;
            Time.timeScale = 0;
            pausePanel.SetActive(true);
        }
        else
        {
            paused = false;
            Time.timeScale = 1;
            pausePanel.SetActive(false);
        }
    }
}
