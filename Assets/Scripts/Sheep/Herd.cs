using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Herd : MonoBehaviour {

    public GameObject herdPrefab;
    public int count;
    public float scalingRate = 1.25f;
    public float smoothingRate = 0.015f;
    public float radius;
    public List<GameObject> sheep;
    public GameObject leader;
    public float leaderResetRate;

    public Vector3 groupDirection;
    public float leaderDirectionWeight;

    private SphereCollider sphere;
    private Rigidbody rb;
    private bool isMerging;
    private float nextLeaderTime;

	// Use this for initialization
	void Start () {
        Destroy(GetComponent<SphereCollider>());
        sphere = gameObject.AddComponent<SphereCollider>();
        sphere.isTrigger = true;
        sphere.radius = 1f;

        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody>();
        }
        rb.useGravity = false;

        if (sheep == null)
        {
            sheep = new List<GameObject>();
            leader = sheep[Random.Range(0, count)];
        } else
        {
            nextLeaderTime = Time.time - 1;
        }
    }
	
	// Update is called once per frame
	void Update () {
        count = sheep.Count;
        if (count <= 0)
        {
            Destroy(this.gameObject);
        } else
        {
            RecalculateHerd();
        }
	}

    public void RecalculateHerd()
    {
        Vector3 center = Vector3.zero;
        Vector3 dir = Vector3.zero;
        foreach (GameObject shep in sheep)
        {
            center += shep.transform.position;
            dir += shep.GetComponent<Rigidbody>().velocity.normalized;
        }
        center = center / (float)count;
        radius = count * scalingRate;
        sphere.radius = radius;
        transform.position = Vector3.Lerp(transform.position, center, smoothingRate);
        //Choose new leader
        if (Time.time > nextLeaderTime)
        {
            leader = sheep[Random.Range(0, count)];
            nextLeaderTime = Time.time + 1f / leaderResetRate;
        }
        //Set Group Direction
        dir += leader.GetComponent<Rigidbody>().velocity.normalized * leaderDirectionWeight;
        groupDirection = dir;
    }

    public void MergeHerd(Herd otherHerd)
    {
        if (!isMerging)
        {
            otherHerd.SetMerging(true);
            isMerging = true;
            sphere.enabled = false;
            Debug.Log("Merging " + name + " and " + otherHerd.name);
            
            Vector3 thisPos = transform.position;
            Vector3 otherPos = otherHerd.transform.position;
            Vector3 newPos = Vector3.Lerp(thisPos, otherPos, (float)count / (float)(count + otherHerd.count));

            GameObject newHerdObj = Instantiate(herdPrefab);
            newHerdObj.name = "New Merged Herd";
            newHerdObj.transform.position = newPos;
            Herd newHerd = newHerdObj.AddComponent<Herd>();
            newHerd.sheep = new List<GameObject>();
            newHerd.GetComponent<Herd>().sheep.AddRange(sheep);
            newHerd.GetComponent<Herd>().sheep.AddRange(otherHerd.sheep);
            newHerd.leaderDirectionWeight = leaderDirectionWeight;
            newHerd.leaderResetRate = leaderResetRate;

            foreach(GameObject shep in newHerd.sheep)
            {
                shep.GetComponent<SheepAI>().herd = newHerd;
            }

            Destroy(otherHerd.gameObject);
            Destroy(this.gameObject);
        }
    }

    public void LeaveHerd(GameObject shep)
    {
        sheep.Remove(shep);
        GameObject newHerdObj = new GameObject("New Herd");
        newHerdObj.transform.position = shep.transform.position;
        Herd newHerd = newHerdObj.AddComponent<Herd>();
        newHerd.sheep = new List<GameObject>();
        newHerd.GetComponent<Herd>().sheep.Add(shep);
        newHerd.leaderDirectionWeight = leaderDirectionWeight;
        newHerd.leaderResetRate = leaderResetRate;
        shep.GetComponent<SheepAI>().herd = newHerd;

        RecalculateHerd();
    }

    public bool NotLeader(GameObject shep)
    {
        return shep != leader;
    }

    public void OnTriggerEnter(Collider other)
    {
        Herd herd = other.GetComponent<Herd>();
        if (herd != null)
        {
            MergeHerd(herd);
        }
    }

    public void OnTriggerExit(Collider other)
    {
        SheepAI shep = other.GetComponent<SheepAI>();
        if (shep != null)
        {
            LeaveHerd(shep.gameObject);
        }
    }

    public void SetMerging(bool value)
    {
        sphere.enabled = !value;
        isMerging = value;
    }

    public bool CanMerge()
    {
        return !isMerging;
    }
}
