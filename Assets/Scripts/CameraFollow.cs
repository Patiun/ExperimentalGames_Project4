using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {

    public float height;
    public Vector3 center;
    public List<GameObject> followable;
    public float smoothrate;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        Follow();
        transform.position = Vector3.Lerp(transform.position, center, smoothrate);
	}

    public void Follow()
    {
        Vector3 temp = Vector3.zero;
        foreach(GameObject obj in followable)
        {
            temp += obj.transform.position;
        }
        center = temp / followable.Count;
        center.y = height;
    }
}
