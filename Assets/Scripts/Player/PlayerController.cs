using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    public int controllerNumber;
    public float speed;

    private WolfSpawner wolfSpawner;
    private float radius;
    private Rigidbody rb;
	// Use this for initialization
	void Start () {
        wolfSpawner = WolfSpawner.instance;
        radius = GetComponent<SphereCollider>().radius;
        rb = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {
        Vector3 velocity = Vector3.zero;
        if (GameController.instance.debug)
        {
            velocity = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
            if (Input.GetButtonDown("Jump"))
            {
                Bark();
                Debug.Log("WOOF!");
            }
        }
        else
        {
            velocity = new Vector3(Input.GetAxis("Horizontal " + controllerNumber), 0, Input.GetAxis("Vertical " + controllerNumber));
            if (Input.GetButtonDown("Bark " + controllerNumber))
            {
                Bark();
                Debug.Log("WOOF!");
            }
        }
        velocity = velocity.normalized * speed;
        transform.LookAt(velocity + transform.position);
        rb.velocity = velocity;
	}

    public void Bark()
    {
        if (wolfSpawner == null)
        {
            wolfSpawner = WolfSpawner.instance;
        }
        List<GameObject> wolves = wolfSpawner.GetNearbyWolves(transform.position, radius);
        foreach(GameObject wolf in wolves)
        {
            wolf.GetComponent<WolfAI>().Scare();
        }
    }
}
