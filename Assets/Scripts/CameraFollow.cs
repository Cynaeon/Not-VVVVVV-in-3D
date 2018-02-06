using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {

    public Transform target;
    public float rotationSpeed;
    public float multiplier = 1;

    private Gravity _gravity;
    private Vector3 offset;
    private Vector3 targetRot;

    private void Start()
    {
        _gravity = GameObject.Find("Gravity").GetComponent<Gravity>();
        offset = transform.position - target.position;
    }

    void Update()
    {
        if (target)
        {
            transform.position = Vector3.Lerp(transform.position, target.position + offset * multiplier, 0.1f);
        }

        if (_gravity.direction == 0)
        {
            targetRot = new Vector3(45, 45, 0);
        }
        else if (_gravity.direction == 1)
        {
            targetRot = new Vector3(30, 55, 125);
        }
        else if (_gravity.direction == 2)
        {
            targetRot = new Vector3(30, 35, 235);
        }

        if (Input.GetButtonDown("Zoom"))
        {
            if (multiplier == 1)
                multiplier = 2;
            else
                multiplier = 1;
        }

        if (Vector3.Distance(transform.eulerAngles, targetRot) > 0.1f)
        {
            transform.eulerAngles = AngleLerp(transform.rotation.eulerAngles, targetRot, Time.deltaTime * 4);
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
