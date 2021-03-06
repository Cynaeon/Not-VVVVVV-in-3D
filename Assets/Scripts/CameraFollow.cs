﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {

    public float rotationSpeed;
    public bool cameraSway;
    public float swayLength;
    public float swaySpeed;
    public float zoomSpeed;
    public float lookAroundDistance = 20;
    public float multiplier = 1;
    public float zoomLevel0 = 8;
    public float zoomLevel1 = 14;
    public float zoomLevel2 = 20;
    public float offset = 4;

    private Transform target;
    private float camSize;
    private Vector3 offsetVector;
    private Vector3 rotation;
    private int zoomLevel = 0;
    private float horizontal;
    private float vertical;
    private float swayTime;
    private bool swayDir;


    void Start()
    {
        if (GameObject.Find("X Wall").transform.localScale.x > 15)
            zoomLevel = 1;
        target = GameObject.FindGameObjectWithTag("Player").transform;
        camSize = Camera.main.orthographicSize * 5;
        Camera.main.orthographicSize = camSize;
    }

    void Update()
    {
        if (target)
        {
            float value = (target.position.x + target.position.z) / 2;
            Vector3 middlePoint = Vector3.zero;
            switch (Gravity.dirNumber)
            {
                case 0:
                    middlePoint = new Vector3(value, 0, value);
                    break;
                case 1: 
                    middlePoint = new Vector3(0, -value, value);
                    break;
                case 2:
                    middlePoint = new Vector3(value, -value, 0);
                    break;
            }
            Vector3 camPosition = (target.position + middlePoint) / 2;
            transform.position = Vector3.Lerp(transform.position, (camPosition + -transform.forward * 30) + offsetVector, 0.1f);
        }

        horizontal = Input.GetAxisRaw("Right_Horizontal");
        vertical = Input.GetAxisRaw("Right_Vertical");

        if (Mathf.Abs(horizontal) + Mathf.Abs(vertical) < 0.4f)
        {
            horizontal = 0;
            vertical = 0;
        }

        Vector3 look = Vector3.zero;

        if (Gravity.dirNumber == 0)
        {
            rotation = new Vector3(45, 45, 0);
            offsetVector = new Vector3(0, offset, 0);
        }
        else if (Gravity.dirNumber == 1)
        {
            rotation = new Vector3(30, 55, 125);
            offsetVector = new Vector3(-offset, 0, 0);
        }
        else if (Gravity.dirNumber == 2)
        {
            rotation = new Vector3(30, 35, 235);
            offsetVector = new Vector3(0, 0, -offset);
        }

        if (cameraSway) 
            CameraSway();

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
                camSize = zoomLevel0;
                break;
            case 1:
                camSize = zoomLevel1;
                break;
            case 2:
                camSize = zoomLevel2;
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

    void CameraSway()
    {
        if (swayDir)
        {
            if (swayTime > 1)
                swayDir = false;
            else
                swayTime += Time.deltaTime * swaySpeed;
        }
        else
        {
            if (swayTime < 0)
                swayDir = true;
            else
                swayTime -= Time.deltaTime * swaySpeed;
        }

        horizontal = Mathf.SmoothStep(-swayLength, swayLength, swayTime);
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
