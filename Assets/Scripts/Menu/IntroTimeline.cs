using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class IntroTimeline : MonoBehaviour {

    private PlayableDirector pd;

	// Use this for initialization
	void Start () {
        pd = GetComponent<PlayableDirector>();
        
	}
	
	// Update is called once per frame
	void Update () {
        if (pd.state == PlayState.Playing && Input.anyKeyDown)
        {
            pd.time = pd.duration;
        }
    }
}
