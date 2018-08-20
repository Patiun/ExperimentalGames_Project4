using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WolfISAI : MonoBehaviour {

    public float sneakSpeed, sprintSpeed;
    public float maxForce;
    public Vector3 position, velocity, acceleration;

    public GameObject target;
    public GameObject clothing;
    public float waitTime;

    public bool sneaking, hunting, fleeing;

    private WolfSpawner spawner;
    private float huntTime;
    private float buffer = 5f;
    // Use this for initialization
    void Start()
    {
        spawner = WolfSpawner.instance;
        position = transform.position;
        velocity = Vector3.zero;
        acceleration = Vector3.zero;
        sneaking = true;
    }

    // Update is called once per frame
    void Update()
    {
        ApplyBehavior();
        //velocity += acceleration;
        velocity = Vector3.Lerp(velocity, velocity + acceleration, 0.45f);
        velocity = Vector3.ClampMagnitude(velocity, sneakSpeed);
        position += velocity;
        transform.position = position;
        acceleration = Vector3.zero;
        if (Input.GetKeyDown("a") && !GameController.instance.debug)
        {
            Scare();
        }
        if (velocity.magnitude > 0)
        {
            transform.rotation = Quaternion.LookRotation(velocity);
        }
    }

    public void ApplyBehavior()
    {
        if (sneaking)
        {

            if (target == null)
            {
                target = SheepManager.instance.GetNearestSheep(position,true);
                if (target == null) {
                    Scare();
                    return;
                }
            }
            if (target.GetComponent<SheepAINoHerd>().GetVelocity().magnitude > 0)
            {
                Arrive(target.transform.position - 2f * target.GetComponent<SheepAINoHerd>().GetVelocity(), 5f);
            } else
            {
                velocity = Vector3.zero;
            }
            if (Vector3.Distance(target.transform.position,position) < 5f)
            {
                huntTime = Time.time + waitTime;
                sneaking = false;
                hunting = true;
            }
        }
        else if (hunting)
        {
            //sneakSpeed = target.GetComponent<SheepAINoHerd>().speed;
            if (target == null)
            {
                Scare();
                return;
            }
            if (Time.time > huntTime && tag != "Scary")
            {
                clothing.SetActive(false);
                List<GameObject> sheps = SheepManager.instance.GetNearestSheeps(position, 10f);
                foreach(GameObject shep in sheps)
                {
                    shep.GetComponent<SheepAINoHerd>().Terrify(this.gameObject);
                }
                tag = "Scary";
            } else
            {
                if (target.GetComponent<SheepAINoHerd>().GetVelocity().magnitude > 0)
                {
                    Arrive(target.transform.position - 3f * target.GetComponent<SheepAINoHerd>().GetVelocity(), 5f);
                }
                else
                {
                    velocity = Vector3.zero;
                }
            }
        }
        else if (fleeing)
        {
            Flee((transform.position - Vector3.zero) * 100);
            if (Vector3.Distance(Vector3.zero, position) > 80)
            {
                spawner.RemoveWolf(gameObject);
                Destroy(gameObject);
            }
        }
    }

    public void ApplyForce(Vector3 force)
    {
        acceleration += force;
    }

    public void Arrive(Vector3 pos, float radius)
    {
        Vector3 desired = pos - transform.position;
        float d = desired.magnitude;

        if (d < radius)
        {
            //float m = Mathf.Abs(sneakSpeed * d / radius);
            float m = Mathf.Lerp(0f, sneakSpeed, Mathf.InverseLerp(0, 1, d / radius));
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

    public void Scare()
    {
        fleeing = true;
        sneaking = false;
        hunting = false;
    }
}
