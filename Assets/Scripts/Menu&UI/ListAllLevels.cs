using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class ListAllLevels : MonoBehaviour {

    public Transform levelSelectPanel;
    public GameObject button;

	void Start () {
        int sceneCount = SceneManager.sceneCountInBuildSettings;

        for (int i = 2; i < sceneCount - 1; i++)
        {
            GameObject newButton = Instantiate(button);
            newButton.transform.SetParent(levelSelectPanel, false);
            newButton.name = "Level" + i + "Button";
            newButton.GetComponent<Button>().interactable = (GameManager.instance.currentLevel >= i);
            newButton.GetComponentInChildren<Text>().text = i.ToString();
            newButton.GetComponent<LoadOnClick>().levelIndex = i;
            if (i == 1)
                EventSystem.current.firstSelectedGameObject = newButton;
        }
    }
}
