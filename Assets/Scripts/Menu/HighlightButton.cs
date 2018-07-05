using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class HighlightButton : MonoBehaviour {

    public Button selectedButton;

	// Use this for initialization
	void Start () {
        StartCoroutine(SelectButton());
        selectedButton.Select();
    }

    private void OnEnable()
    {
        selectedButton.Select();
    }

    // Update is called once per frame
    void Update () {
        if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1) || Input.GetMouseButtonDown(2))
            EventSystem.current.SetSelectedGameObject(selectedButton.gameObject);
    }

    IEnumerator SelectButton()
    {
        EventSystem.current.SetSelectedGameObject(null);
        yield return null;
        selectedButton.Select();
    }
}
