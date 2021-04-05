using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    public GameObject[] obstacles;
    public GameObject player;
    public GameObject[] spawnners;
    public GameObject gameOverUI;
    public TMP_Text scoreText;
    bool isGameRunning;
    int maxObstacles;
    int noOfObstacles;
    float score;
    // Start is called before the first frame update
    void Start()
    {
        gameOverUI.SetActive(false);
        isGameRunning = true;
        maxObstacles = 30;
        noOfObstacles = 0;
        score = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (isGameRunning)
        {
            if (noOfObstacles < maxObstacles)
            {
                SpawnObstacles();
                //score += 1;
            }
            score += 0.05f;
            UpdateScore();
        }
    }

    private void UpdateScore()
    {
        scoreText.text = ((int)score).ToString();
    }

    private void SpawnObstacles()
    {
       foreach(GameObject spawnner in spawnners)
        {if (noOfObstacles < maxObstacles)
            {
                GameObject obstacle = Instantiate(obstacles[(int)UnityEngine.Random.Range(0, obstacles.Length)]);
                obstacle.GetComponent<Obstacle>().player = player;
                obstacle.GetComponent<Obstacle>().gameManager = this;
                obstacle.transform.position = (Vector2)spawnner.transform.position + UnityEngine.Random.insideUnitCircle * 5;
                noOfObstacles++;
            }
        }
    }

    public void GameOver()
    {
        isGameRunning = false;
        Time.timeScale = 0f;
        gameOverUI.SetActive(true);
    }
    public void Retry()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public  void Destroyed()
    {
        noOfObstacles--;
    }

    public void AddScore(float amount)
    {
        score += amount;
    }
}
