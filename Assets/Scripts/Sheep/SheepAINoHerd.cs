using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SheepAINoHerd : MonoBehaviour {

    public List<GameObject> neighbors;
    public float speed;
    public float alignmentWeight, cohesionWeight, seperationWeight, fearWeight;
    public float seperationVariance;
    public float tooCloseDistance = 1.25f;
    public float rotationSpeed = 1f;
    public bool scared;
    public List<GameObject> scaryThings;

    private Rigidbody rb;
    private float originalCohesion, originalSeperation, originalSpeed;

	// Use this for initialization
	void Start () {
        neighbors = new List<GameObject>();
        rb = GetComponent<Rigidbody>();
        originalCohesion = cohesionWeight;
        originalSeperation = seperationWeight;
        originalSpeed = speed;
	}
	
	// Update is called once per frame
	void Update () {
        rb.velocity = Vector3.Lerp(rb.velocity,(ComputeAlignment() * alignmentWeight + ComputeCohesion() * cohesionWeight + ComputeSeperation() * seperationWeight + ComputeFear() * fearWeight).normalized * speed,0.35f);
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(rb.velocity), Time.deltaTime * rotationSpeed);
	}

    public void OnTriggerEnter(Collider other)
    {
        SheepAINoHerd shep = other.GetComponent<SheepAINoHerd>();
        if (shep != null)
        {
            if (!neighbors.Contains(other.gameObject))
            {
                neighbors.Add(other.gameObject);
            }
        }

        if (other.tag == "Scary")
        {
            scared = true;
            cohesionWeight *= 2f;
            seperationVariance *= 0.8f;
            speed *= 1.5f;
            if (!scaryThings.Contains(other.gameObject))
            {
                scaryThings.Add(other.gameObject);
            }
        }
    }

    public void OnTriggerExit(Collider other)
    {
        SheepAINoHerd shep = other.GetComponent<SheepAINoHerd>();
        if (shep != null)
        {
            if (neighbors.Contains(other.gameObject))
            {
                neighbors.Remove(other.gameObject);
            }
        }

        if (other.tag == "Scary")
        {
            scared = false;
            cohesionWeight = originalCohesion;
            seperationVariance = originalSeperation;
            speed = originalSpeed;
            if (scaryThings.Contains(other.gameObject))
            {
                scaryThings.Remove(other.gameObject);
            }
        }
    }

    //Switch to a flock controller unit for better performance at a loss of agency?
    public Vector3 ComputeAlignment()
    {
        Vector3 dir = Vector3.zero;

        foreach(GameObject shep in neighbors)
        {
            SheepAINoHerd ai = shep.GetComponent<SheepAINoHerd>();
            dir += ai.GetVelocity();
        }
        dir = dir / neighbors.Count;

        return dir.normalized;
    }

    public Vector3 ComputeCohesion()
    {
        Vector3 dir = Vector3.zero;

        foreach (GameObject shep in neighbors)
        {
            dir += shep.transform.position;
        }
        dir = dir / neighbors.Count;

        return (dir - transform.position).normalized;
    }

    public Vector3 ComputeSeperation()
    {
        Vector3 dir = Vector3.zero;

        foreach (GameObject shep in neighbors)
        {
            Vector3 dif = shep.transform.position - transform.position;
            if (dif.magnitude <= tooCloseDistance + Random.Range(-seperationVariance,seperationVariance))
            {
                dir += -dif;
            }
        }
        dir = dir / neighbors.Count;

        return dir;
    }

    //TODO Gets stuck in corners because rear of flock aren't scared but leaders are
    public Vector3 ComputeFear()
    {
        Vector3 dir = Vector3.zero;
        if (scared)
        {
            foreach (GameObject thing in scaryThings)
            {
                Vector3 dif = thing.transform.position - transform.position;
                dir -= dif;
            }
        }
        dir = dir / scaryThings.Count;
        return dir.normalized;
    }

    public Vector3 GetVelocity()
    {
        return rb.velocity;
    }
}
