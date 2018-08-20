using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour {

    public static GameController instance;
    public int score;
    public bool debug;
	// Use this for initialization
	void Start () {
        instance = this;
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown("r"))
        {
            SceneManager.LoadScene(0);
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            debug = !debug;
        }
	}

    public void GameOver()
    {
        Debug.Log("Game Over!");
    }

    public void AddScore(int amount)
    {
        score += amount;
    }
}
