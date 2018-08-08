using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SheepAINoHerd : MonoBehaviour {

    public List<GameObject> neighbors;
    public float speed;
    public float alignmentWeight, cohesionWeight, seperationWeight;
    public float seperationVariance;
    public float tooCloseDistance = 1.25f;

    private Rigidbody rb;

	// Use this for initialization
	void Start () {
        neighbors = new List<GameObject>();
        rb = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {
        rb.velocity = Vector3.Lerp(rb.velocity,(ComputeAlignment() * alignmentWeight + ComputeCohesion() * cohesionWeight + ComputeSeperation() * seperationWeight).normalized * speed,0.35f);
        //transform.LookAt(ComputeAlignment()+transform.position);
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

    public Vector3 GetVelocity()
    {
        return rb.velocity;
    }
}
