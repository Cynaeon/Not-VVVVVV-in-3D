using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropLevelObjects : MonoBehaviour {

    public float fallSpeed;
    public float dropInterval;
    public int dropDistance;

    private List<Transform> levelObjects = new List<Transform>();

	// Use this for initialization
	void Start () {
		
        for (int i = 0; i < transform.childCount; i++)
        {
            levelObjects.Add(transform.GetChild(i));
            transform.GetChild(i).position += Vector3.up * dropDistance;
        }
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

    }

	// Update is called once per frame
	void Update () {
		
	}
}
