using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrassSpawner : MonoBehaviour {

    public List<GrassPatch> patches;
    public int activePatch;
    public float patchDelay;
    public float patchVariance;

    private float nextPatch;

	// Use this for initialization
	void Start () {
        activePatch = -1;
        nextPatch = Time.time + patchDelay + Random.Range(0, patchVariance);
    }
	
	// Update is called once per frame
	void Update () {
        if (activePatch != -1)
        {
            if (patches[activePatch].active == false)
            {
                activePatch = -1;
                nextPatch = Time.time + patchDelay + Random.Range(0, patchVariance);
            }
        } else
        {
            if (Time.time > nextPatch)
            {
                activePatch = Random.Range(0, patches.Count);
                patches[activePatch].Activate();
            }
        }
	}
}
