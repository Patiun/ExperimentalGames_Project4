using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SheepManager : MonoBehaviour {

    public static SheepManager instance;

    public List<GameObject> sheep;

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
            if (Vector3.Distance(pos,s.transform.position) < shortest)
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
    }
}
