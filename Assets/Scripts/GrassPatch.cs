using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrassPatch : MonoBehaviour {

    public bool active;
    public int scoreAmount;
    public float grassAmount, maxAmount;
    public float perSheepRate;
    public List<GameObject> sheep;
    public float minHeight = 0.01f;
    public float maxHeight = 0.6f;

    private Vector3 scale;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
        if (active)
        {
            if (sheep.Count > 0)
            {
                grassAmount -= sheep.Count * perSheepRate * Time.deltaTime;
                if (grassAmount <= 0)
                {
                    GameController.instance.AddScore(scoreAmount);
                    active = false;
                    gameObject.SetActive(active);
                }
                UpdateGrass();
            }
        }
	}

    public void Activate()
    {
        grassAmount = maxAmount;
        active = true;
        gameObject.SetActive(active);
        UpdateGrass();
    }

    private void UpdateGrass()
    {
        float height = Mathf.Lerp(minHeight, maxHeight, grassAmount / maxAmount);
        scale = transform.localScale;
        scale.y = height;
        transform.localScale= scale;
        Vector3 pos = new Vector3(transform.position.x, height, transform.position.z);
        transform.position = pos;
    }

    public void OnTriggerEnter(Collider other)
    {
        SheepAINoHerd ai = other.GetComponent<SheepAINoHerd>();
        if (ai != null)
        {
            if (!sheep.Contains(other.gameObject))
            {
                sheep.Add(other.gameObject);
            }
        }
    }

    public void OnTriggerExit(Collider other)
    {
        SheepAINoHerd ai = other.GetComponent<SheepAINoHerd>();
        if (ai != null)
        {
            if (sheep.Contains(other.gameObject))
            {
                sheep.Remove(other.gameObject);
            }
        }
    }
}
