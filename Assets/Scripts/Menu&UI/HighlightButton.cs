using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class HighlightButton : MonoBehaviour {

    public Button selectedButton;

	void Start () {
        StartCoroutine(SelectButton());
        selectedButton.Select();
    }

    private void OnEnable()
    {
        StartCoroutine(SelectButton());
        selectedButton.Select();
    }

    void Update () {
        if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1) || Input.GetMouseButtonDown(2))
            EventSystem.current.SetSelectedGameObject(selectedButton.gameObject);
        if (EventSystem.current.currentSelectedGameObject == null && Input.GetAxis("Vertical") != 0)
            EventSystem.current.SetSelectedGameObject(selectedButton.gameObject);
    }

    IEnumerator SelectButton()
    {
        EventSystem.current.SetSelectedGameObject(null);
        yield return null;
        selectedButton.Select();
    }
}
