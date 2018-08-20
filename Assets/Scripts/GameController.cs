using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour {

    public static GameController instance;
    public int score;
    public bool debug;
    private float nextTime;

    public GameObject gameover;
    public Text scoreTxt;

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
        if (Time.time > nextTime)
        {
            score += 1;
            nextTime = Time.time + 1f;
        }
	}

    public void GameOver()
    {
        Debug.Log("Game Over!");
        Time.timeScale = 0;
        scoreTxt.text = "Score: " + score;
        gameover.SetActive(true);
    }

    public void AddScore(int amount)
    {
        score += amount;
    }
}
