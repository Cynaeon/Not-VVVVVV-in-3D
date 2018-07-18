using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PressAtoExit : MonoBehaviour {

    public Text timeText;

    private int gameTimeInSeconds;

    private void Start()
    {
        gameTimeInSeconds = (int)GameManager.gameTime;
        int minutes = 0;
        int hours = 0;
        while (gameTimeInSeconds >= 60)
        {
            minutes++;
            gameTimeInSeconds -= 60;
        }
        while (minutes > 60)
        {
            hours++;
            minutes -= 60;
        }

        timeText.text = "Game Time: " + hours.ToString("00") + ":" + minutes.ToString("00");
        
    }

    void Update () {
        if (Input.anyKey)
            SceneManager.LoadScene(0);
	}
}
