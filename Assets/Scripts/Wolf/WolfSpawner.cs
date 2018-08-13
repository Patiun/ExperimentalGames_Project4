using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WolfSpawner : MonoBehaviour {

    public static WolfSpawner instance;

    public bool canSpawn;
    public float radius;
    public float spawnRate;
    public List<GameObject> wolves;
    public GameObject wolfPrefab;
    public GameObject wolfInSheepsClothingPrefab;
    public float chanceForSheepsClothing;

    private float nextSpawn;
    private int spawnAmount = 1;

	// Use this for initialization
	void Start () {
        instance = this;
        radius = GetComponent<SphereCollider>().radius + 5f;
        wolves = new List<GameObject>();
	}

    // Update is called once per frame
    void Update() {
        if (!GameController.instance.debug)
        {
            if (Input.GetKeyDown("w"))
            {
                SpawnWolf();
            }
            if (Input.GetKeyDown("s"))
            {
                canSpawn = !canSpawn;
                nextSpawn = Time.time + 1 / spawnRate;
            }
        }
        if (canSpawn)
        {
            if(Time.time > nextSpawn)
            {
                SpawnWolf();
            }
        }
	}

    public void SpawnWolf()
    {
        Vector3 startLocation = Quaternion.Euler(0, Random.Range(-360, 360), 0) * transform.forward * radius;
        Vector3 targetLocation = Quaternion.Euler(0, Random.Range(-360, 360), 0) * transform.forward * radius;
        GameObject wolf = null;
        if (Random.Range(0, 100f) < chanceForSheepsClothing)
        {
            wolf = Instantiate(wolfInSheepsClothingPrefab, startLocation, Quaternion.identity, transform);
        }
        else
        {
            wolf = Instantiate(wolfPrefab, startLocation, Quaternion.identity, transform);
        }
        wolf.GetComponent<WolfAI>().huntPosition = targetLocation;
        wolves.Add(wolf);
        nextSpawn = Time.time + 1 / spawnRate;
    }

    public void RemoveWolf(GameObject wolf)
    {
        wolves.Remove(wolf);
    }

    public List<GameObject> GetNearbyWolves(Vector3 position, float radius)
    {
        List<GameObject> nearby = new List<GameObject>();
        foreach(GameObject wolf in wolves)
        {
            if (Vector3.Distance(position, wolf.transform.position) < radius)
            {
                nearby.Add(wolf);
            }
        }
        return nearby;
    }
}
