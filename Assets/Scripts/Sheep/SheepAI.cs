using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SheepAI : MonoBehaviour
{
    public GameObject TEMP_TARGET;

    public Vector3 position, velocity, acceleration;
    public float maxForce, maxVelocity;

    // Use this for initialization
    void Start()
    {
        position = transform.position;
        velocity = Vector3.zero;
        acceleration = Vector3.zero;
    }

    // Update is called once per frame
    void Update()
    {
        ApplyBehavior();
        acceleration = Vector3.ClampMagnitude(acceleration, maxForce);
        velocity += acceleration;

        velocity = Vector3.ClampMagnitude(velocity, maxVelocity);
        position += velocity;
        transform.position = position;
        transform.LookAt(transform.position + velocity);

        acceleration = Vector3.zero;
    }

    public void ApplyBehavior()
    {
        Seek(TEMP_TARGET.transform.position);
    }

    public void ApplyForce(Vector3 force)
    {
        acceleration += force;
    }

    public void Seek(Vector3 target)
    {
        Vector3 desired = target - position;

        desired = Vector3.ClampMagnitude(desired,maxForce);

        ApplyForce(desired);
    }
    
    public void Seperate()
    {

    }
}
