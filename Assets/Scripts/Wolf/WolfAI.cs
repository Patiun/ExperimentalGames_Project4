using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WolfAI : MonoBehaviour {

    public float sneakSpeed, sprintSpeed;
    public float maxForce;
    public Vector3 position, velocity, acceleration;

    public GameObject target;
    public Vector3 huntPosition;
    public float waitTime;

    public bool sneaking, hunting, fleeing;

    private WolfSpawner spawner;
    private float huntTime;
    private float buffer = 5f;
	// Use this for initialization
	void Start () {
        spawner = WolfSpawner.instance;
        position = transform.position;
        velocity = Vector3.zero;
        acceleration = Vector3.zero;
        sneaking = true;
	}
	
	// Update is called once per frame
	void Update () {
        ApplyBehavior();
        velocity += acceleration;
        if (sneaking)
        {
            velocity = Vector3.ClampMagnitude(velocity, sneakSpeed);
        } else if (hunting || fleeing)
        {
            velocity = Vector3.ClampMagnitude(velocity, sprintSpeed);
        }
        position += velocity;
        transform.position = position;
        acceleration = Vector3.zero;
        if (Input.GetKeyDown("a") && !GameController.instance.debug)
        {
            Scare();
        }
        if (velocity.magnitude > 0 && !hunting)
        {
            transform.rotation = Quaternion.LookRotation(velocity);
        }
    }

    public void ApplyBehavior()
    {
        if (sneaking)
        {
            //Seperate();
            Arrive(huntPosition, 5f);
            AvoidCenter(spawner.radius);
            Debug.Log(Vector3.Distance(position, huntPosition));
            if (Vector3.Distance(position,huntPosition) <= 5f)
            {
                hunting = true;
                sneaking = false;
                velocity = Vector3.zero;
                huntTime = Time.time + waitTime;
                target = SheepManager.instance.GetNearestSheep(position);
                if (target == null)
                {
                    Scare();
                }
            }
        } else if (hunting)
        {
            if (target == null)
            {
                target = SheepManager.instance.GetNearestSheep(position);
                if (target == null) { Scare(); }
            }
            else
            {
                if (Time.time > huntTime)
                {
                    Hunt(target);
                }
                else
                {
                    transform.LookAt(target.transform.position);
                }
            }
        } else if (fleeing)
        {
            Flee((transform.position - Vector3.zero)*100);
            if (Vector3.Distance(Vector3.zero,position) > 80 )
            {
                spawner.RemoveWolf(gameObject);
                if (target != null)
                {
                    SheepManager.instance.RemoveHunted(target);
                }
                Destroy(gameObject);
            }
        }
    }

    public void ApplyForce(Vector3 force)
    {
        acceleration += force;
    }

    public void Hunt(GameObject target)
    {
        Vector3 desired = (target.transform.position + target.GetComponent<Rigidbody>().velocity) - position;
        float d = desired.magnitude;

        desired = desired.normalized * sprintSpeed;

        Vector3 steer = desired - velocity;
        steer = Vector3.ClampMagnitude(steer, maxForce);

        ApplyForce(steer);
    }

    public void Arrive(Vector3 pos, float radius)
    {
        Vector3 desired = pos - transform.position;
        float d = desired.magnitude;

        if (d < radius)
        {
            //float m = Mathf.Abs(sneakSpeed * d / radius);
            float m = Mathf.Lerp(0f, sneakSpeed, Mathf.InverseLerp(0,1,d/radius));
            desired = Vector3.ClampMagnitude(desired, m);
        }
        else
        {
            desired = desired.normalized * sneakSpeed;
        }

        Vector3 steering = desired - velocity;
        steering = Vector3.ClampMagnitude(steering, maxForce);

        ApplyForce(steering);
    }

    public void Flee(Vector3 pos)
    {
        Vector3 desired = pos - transform.position;
        float d = desired.magnitude;

        desired = desired.normalized * sprintSpeed;

        Vector3 steering = desired - velocity;
        steering = Vector3.ClampMagnitude(steering, maxForce);

        ApplyForce(steering);
    }

    public void AvoidCenter(float radius)
    {
        Vector3 desired = position - Vector3.zero;

        float d = desired.magnitude;
        if (d > radius + buffer)
        {
            //float m = Mathf.Abs(sneakSpeed * d / radius);
            desired = -desired.normalized * sneakSpeed;
        }
        else
        {
            desired = desired.normalized * sneakSpeed;
        }

        Vector3 steering = desired - velocity;
        steering = Vector3.ClampMagnitude(steering, maxForce);

        ApplyForce(steering);
    }

    public void Seperate()
    {
        List<GameObject> nearby = spawner.GetNearbyWolves(position, 5f);
        Vector3 desired = Vector3.zero;
        foreach(GameObject wolf in nearby)
        {
            desired += (position - wolf.transform.position);
        }
        desired = (desired / nearby.Count).normalized * sneakSpeed;
        Vector3 steering = desired - velocity;

        ApplyForce(Vector3.ClampMagnitude(steering,maxForce));
    }

    public void Scare()
    {
        fleeing = true;
        sneaking = false;
        hunting = false;
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<SheepAINoHerd>() != null) {
            SheepManager.instance.RemoveSheep(collision.gameObject);
            Destroy(collision.gameObject);
        }
        hunting = false;
        sneaking = false;
        velocity = Vector3.zero;
        Scare();
    }
}
