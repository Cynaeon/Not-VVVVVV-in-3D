using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {

    public float rotationSpeed;
    public float zoomSpeed;
    public float lookAroundDistance = 20;
    public float multiplier = 1;

    private Transform target;
    private Gravity _gravity;
    private float camSize;
    private Vector3 offset;
    private Vector3 rotation;
    private int zoomLevel = 1;

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

        float horizontal = Input.GetAxisRaw("Right_Horizontal");
        float vertical = Input.GetAxisRaw("Right_Vertical");

        if (Mathf.Abs(horizontal) + Mathf.Abs(vertical) < 0.4f)
        {
            horizontal = 0;
            vertical = 0;
        }

        Vector3 look = Vector3.zero;

        if (_gravity.dirNumber == 0)
        {
            rotation = new Vector3(45, 45, 0);
            offset = new Vector3(-20, 9, -20);
        }
        else if (_gravity.dirNumber == 1)
        {
            rotation = new Vector3(30, 55, 125);
            offset = new Vector3(-9, 20, -20);
        }
        else if (_gravity.dirNumber == 2)
        {
            rotation = new Vector3(30, 35, 235);
            offset = new Vector3(-20, 20, -9);
        }

        rotation += new Vector3(-vertical * lookAroundDistance, -horizontal * lookAroundDistance, 0);

        if (Input.GetButtonDown("Zoom"))
        {
            zoomLevel++;
            if (zoomLevel >= 3)
                zoomLevel = 0;
        }

        switch (zoomLevel)
        {
            case 0:
                camSize = 5;
                break;
            case 1:
                camSize = 7;
                break;
            case 2:
                camSize = 14;
                break;
        }

        float step = zoomSpeed * Time.deltaTime;
        Camera.main.orthographicSize = Mathf.Lerp(Camera.main.orthographicSize, camSize, step);

        if (Vector3.Distance(transform.eulerAngles, rotation) > 0.1f)
        {
            transform.eulerAngles = AngleLerp(transform.rotation.eulerAngles, rotation, Time.deltaTime * rotationSpeed);
        }
        else
        {
            transform.eulerAngles = rotation;
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
