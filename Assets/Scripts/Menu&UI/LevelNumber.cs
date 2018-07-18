using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelNumber : MonoBehaviour {
	void Start () {
        GetComponent<Text>().text = SceneManager.GetActiveScene().buildIndex.ToString("00");
    }
}
