using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SheepAI : MonoBehaviour
{
    public GameObject herdPrefab;
    public Herd herd;
    private Rigidbody rb;
    public float recalculateRate;

    private float recalculateTime;

    // Use this for initialization
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody>();
        }

        if (herd == null)
        {
            GameObject newHerdObj = Instantiate(herdPrefab);
            newHerdObj.transform.position = transform.position;
            newHerdObj.name = "New Herd";
            herd = newHerdObj.GetComponent<Herd>();
            herd.sheep.Add(this.gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {

        if (Time.time > recalculateTime)
        {
            if (herd.NotLeader(this.gameObject) && Vector3.Distance(transform.position, herd.transform.position) > herd.radius / 2f)
            {
                if (Random.Range(0f, 100f) <= 85f) //Fixed Escape Chance
                {
                    transform.rotation = Quaternion.LookRotation(herd.transform.position - transform.position, transform.up);
                }
            }
            else
            {
                float angle = Random.Range(-360f, 360f);
                Vector3 dir = Quaternion.Euler(0, angle, 0) * transform.forward;

                dir = Vector3.Lerp(dir, herd.groupDirection, Random.Range(0.3f, 1f));
                angle = Vector3.Angle(transform.forward, dir);
                transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y + angle, 0);

                recalculateTime = Time.time + 1f / recalculateRate;
            }
        }
        rb.velocity = transform.forward;
    }
}
