using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class IntroTimeline : MonoBehaviour {

    public GameObject menu;

    private PlayableDirector pd;
    private bool menuActivated;

	void Start () {
        pd = GetComponent<PlayableDirector>();
	}

	void Update () {
        if (!menuActivated && pd.time >= 4)
        {
            menu.SetActive(true);
            menuActivated = true;
        }

        else if (!menuActivated && pd.state == PlayState.Playing && Input.anyKeyDown)
        {
            pd.time = pd.duration;
        }
    }
}
