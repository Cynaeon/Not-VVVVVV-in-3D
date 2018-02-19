using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {

    public Transform target;
    public float rotationSpeed;
    public float zoomSpeed;
    public float multiplier = 1;

    private Gravity _gravity;
    private float camSize;
    private Vector3 offset;
    private Vector3 targetRot;
    private bool zoomed;

    private void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
        _gravity = GameObject.Find("Gravity").GetComponent<Gravity>();
        offset = new Vector3(-20, 9, -20);
        camSize = Camera.main.orthographicSize;
    }

    void Update()
    {
        if (target)
        {
            transform.position = Vector3.Lerp(transform.position, target.position + -transform.forward * 30, 0.1f);
        }

        if (_gravity.dirNumber == 0)
        {
            targetRot = new Vector3(45, 45, 0);
            offset = new Vector3(-20, 9, -20);
        }
        else if (_gravity.dirNumber == 1)
        {
            targetRot = new Vector3(30, 55, 125);
            offset = new Vector3(-9, 20, -20);
        }
        else if (_gravity.dirNumber == 2)
        {
            targetRot = new Vector3(30, 35, 235);
            offset = new Vector3(-20, 20, -9);
        }

        if (Input.GetButtonDown("Zoom"))
        {
            if (zoomed)
            {
                camSize /= 2;
                zoomed = false;
            }
            else
            {
                camSize *= 2;
                zoomed = true;
            }
        }

        float step = zoomSpeed * Time.deltaTime;
        Camera.main.orthographicSize = Mathf.Lerp(Camera.main.orthographicSize, camSize, step);

        if (Vector3.Distance(transform.eulerAngles, targetRot) > 0.1f)
        {
            transform.eulerAngles = AngleLerp(transform.rotation.eulerAngles, targetRot, Time.deltaTime * rotationSpeed);
        }
        else
        {
            transform.eulerAngles = targetRot;
        }
    }

    Vector3 AngleLerp(Vector3 StartAngle, Vector3 FinishAngle, float t)
    {
        float xLerp = Mathf.LerpAngle(StartAngle.x, FinishAngle.x, t);
        float yLerp = Mathf.LerpAngle(StartAngle.y, FinishAngle.y, t);
        float zLerp = Mathf.LerpAngle(StartAngle.z, FinishAngle.z, t);
        Vector3 Lerped = new Vector3(xLerp, yLerp, zLerp);
        return Lerped;
    }
}
