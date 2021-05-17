using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.Experimental.Rendering.Universal;
using TMPro;

public class GameManager : MonoBehaviour
{
    public GameObject[] obstacles;
    public GameObject player;
    public GameObject[] spawnners;
    public GameObject gameOverUI;
    public GameObject pauseMenu;
    public GameObject shieldButton;
    public TMP_Text scoreText;
    public TMP_Text coinsText;
    public TMP_Text gameOverScoreText;
    public TMP_Text gameOverHighScoreText;
    bool isGameRunning;
    bool isGamePaused;
    int maxObstacles;
    int noOfObstacles;
    float score;
    int coins;
    float nextScore;
    float shieldScore;
    bool isShielded;
   
    // Start is called before the first frame update
    void Start()
    {
        gameOverUI.SetActive(false);
        shieldButton.SetActive(false);
        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            isGameRunning = false;
        }
        else
        {
            isGameRunning = true;
        }
        maxObstacles = 20;
        noOfObstacles = 0;
        score = 0;
        coins = PlayerPrefs.GetInt("Coins", 0);
        UpdateCoins();
        nextScore = 50;
        shieldScore = 200;
        isShielded = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (isGameRunning&&!isGamePaused)
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
        UpdateSpeed();
        CheckShield();
    }

    private void CheckShield()
    {
        if(score>shieldScore&&!isShielded)
        {
            shieldButton.SetActive(true);
        }
    }

    public void GetShield()
    {
        shieldButton.SetActive(false);
        isShielded = true;
        Player p = player.GetComponent<Player>();
        p.SetShield(true);
    }

    public void ShieldDestroyed()
    {
        isShielded = false;
        Player p = player.GetComponent<Player>();
        p.SetShield(false);
        shieldScore = score + 200;
        DestroyAll();
    }

    private void UpdateSpeed()
    {
        if(score>=nextScore)
        {
            nextScore = score + (score / 1.5f);
            player.GetComponent<Player>().AddSpeed(0.3f);
        }
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
        UpdateGameOverScreen();
        AudioManager.instance.StartPlaying("GameOver");
    }

    private void UpdateGameOverScreen()
    {
        try
        {
            UpdateGameOverScore();
            UpdateGameOverHighScore();
        }
        catch(Exception e)
        { }
    }

    private void UpdateGameOverHighScore()
    {
        int highScore = PlayerPrefs.GetInt("HighScore",0);
        string message;
        if((int)score>highScore)
        {
            highScore = (int)score;
            message = "New High Score !!!:" + highScore;
            PlayerPrefs.SetInt("HighScore", highScore);
        }
        else
        {
            message = "High Score:" + highScore;
        }
        gameOverHighScoreText.text = message;
    }

    private void UpdateGameOverScore()
    {
        gameOverScoreText.text = "You scored:" + (int)score;
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
    public void MainMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
    }
    public void Play()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    public void Quit()
    {
        Application.Quit();
    }
    public void Resume()
    {
        if (isGameRunning)
        {
            Time.timeScale = 1;
        isGamePaused = false;
        pauseMenu.SetActive(false);
        }
    }
    public void Reset()
    {
        PlayerPrefs.DeleteAll();
    }
    public void Pause()
    {
        isGamePaused = true;
        Time.timeScale = 0;
        pauseMenu.SetActive(true);
    }
    public void AddCoin(int amount)
    {
        coins += amount;
        UpdateCoins();
    }

    private void UpdateCoins()
    {
        try
        {
            coinsText.text = coins.ToString();
            PlayerPrefs.SetInt("Coins", coins);
        }
        catch(Exception e)
        {

        }
    }
    public void DestroyAll()
    {
       Obstacle[] liveObstacles = FindObjectsOfType<Obstacle>();
        foreach(Obstacle o in liveObstacles)
        {
            Destroy(o.gameObject);
        }
        noOfObstacles = 0;
        AudioManager.instance.StartPlaying("GameOver");
    }
}
