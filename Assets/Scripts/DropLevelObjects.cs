using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropLevelObjects : MonoBehaviour {

    public float fallSpeed = 80;
    public float dropInterval = 0.05f;
    public int dropDistance = 40;

    public AudioClip blockLanding;

    private List<Transform> levelObjects = new List<Transform>();

	void Start () {
        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).tag == "Danger" || transform.GetChild(i).tag == "PushPanel")
                continue;
            levelObjects.Add(transform.GetChild(i));
            transform.GetChild(i).position += Vector3.up * dropDistance;
        }
        dropInterval = 1 / (float) transform.childCount;
        StartCoroutine(StartDropping());
        
	}
	
    IEnumerator StartDropping()
    {
        foreach (Transform levelObject in levelObjects)
        {
            StartCoroutine(DropObject(levelObject));
            yield return new WaitForSeconds(dropInterval);
        }
    }

    IEnumerator DropObject(Transform levelObject)
    {
        Vector3 startPos = levelObject.position;

        while (levelObject.position.y > startPos.y - dropDistance)
        {
            levelObject.position -= Vector3.up * fallSpeed * Time.deltaTime;
            yield return null;
        }
        levelObject.position = new Vector3(levelObject.position.x, startPos.y - dropDistance, levelObject.position.z);
        float vol = Random.Range(0.1f, 0.4f);
        AudioSource source = levelObject.GetComponent<AudioSource>();
        if (source)
        {
            source.pitch = Random.Range(.5f, 1f);
            source.PlayOneShot(blockLanding, vol);
        }

    }
}
