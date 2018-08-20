using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SheepManager : MonoBehaviour {

    public static SheepManager instance;

    public List<GameObject> sheep;

    public List<GameObject> hunted;

	// Use this for initialization
	void Start () {
        instance = this;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public GameObject GetNearestSheep(Vector3 pos)
    {
        float shortest = 10000f;
        GameObject shep = null;
        foreach(GameObject s in sheep)
        {
            if (Vector3.Distance(pos,s.transform.position) < shortest && !hunted.Contains(s))
            {
                shortest = Vector3.Distance(pos, s.transform.position);
                shep = s;
            }
        }
        if (sheep != null) { hunted.Add(shep); }
       
        return shep;
    }

    public GameObject GetNearestSheep(Vector3 pos, bool t)
    {
        float shortest = 10000f;
        GameObject shep = null;
        foreach (GameObject s in sheep)
        {
            if (Vector3.Distance(pos, s.transform.position) < shortest && !hunted.Contains(s))
            {
                shortest = Vector3.Distance(pos, s.transform.position);
                shep = s;
            }
        }

        return shep;
    }

    public void RemoveSheep(GameObject shep)
    {
        sheep.Remove(shep);
        hunted.Remove(shep);
        if (sheep.Count == 0)
        {
            GameController.instance.GameOver();
        }
    }

    public void AddHunted(GameObject shep)
    {
        hunted.Add(shep);
    }

    public void RemoveHunted(GameObject shep)
    {
        hunted.Remove(shep);
    }

    public List<GameObject> GetNearestSheeps(Vector3 position, float radius)
    {
        List<GameObject> sheps = new List<GameObject>();

        foreach(GameObject shep in sheep)
        {
            if (Vector3.Distance(position,shep.transform.position) < radius)
            {
                sheps.Add(shep);
            }
        }

        return sheps;
    }
}
