using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject[] obstacles;
    public GameObject player;
    public GameObject[] spawnners;
    public GameObject gameOverUI;
    bool isGameRunning;
    int maxObstacles;
    int noOfObstacles;
    // Start is called before the first frame update
    void Start()
    {
        gameOverUI.SetActive(false);
        isGameRunning = true;
        maxObstacles = 30;
        noOfObstacles = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (isGameRunning && noOfObstacles<maxObstacles)
        {
            SpawnObstacles();
        }
    }

    private void SpawnObstacles()
    {
       foreach(GameObject spawnner in spawnners)
        {if (noOfObstacles < maxObstacles)
            {
                GameObject obstacle = Instantiate(obstacles[(int)Random.Range(0, obstacles.Length)]);
                obstacle.GetComponent<Obstacle>().player = player;
                obstacle.GetComponent<Obstacle>().gameManager = this;
                obstacle.transform.position = (Vector2)spawnner.transform.position + Random.insideUnitCircle * 5;
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
}
