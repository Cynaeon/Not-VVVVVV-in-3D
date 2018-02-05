using UnityEngine;
using System.Collections;

public class OrbitAround : MonoBehaviour
{
    public Transform target;
    public float maxRadius;
    public float radiusSpeed;
    public float speed = 80.0f;
    public float cycleSpeed;

    public float radius;
    private Vector3 desiredPosition;

    void Start()
    {
        transform.position = (transform.position - target.position).normalized * radius + target.position;
    }
    
    void Update()
    {
        radius = Mathf.PingPong(Time.time * cycleSpeed, maxRadius);
        transform.RotateAround(target.position, Vector3.up, speed * Time.deltaTime);
        desiredPosition = (transform.position - target.position).normalized * radius + target.position;
        transform.position = Vector3.MoveTowards(transform.position, desiredPosition, Time.deltaTime * radiusSpeed);
    }
}