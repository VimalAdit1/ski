using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Player : MonoBehaviour
{
    // Start is called before the first frame update
    public GameManager gameManager;
    public Transform spawnner;
    public Transform dontFlip;
    public GameObject popUpPrefab;
    Vector3 direction;
    Vector3 scale;
    Animator animator;
    bool isJumping = false;
    public bool inAir = false;
    float speed;
    Dictionary<string, List<string>> messages;
    void Start()
    {
        initializeMessages();
        direction = Vector2.down;
        spawnner.transform.up = direction;
        scale = new Vector3(0.5f, 0.5f, 0.5f);
        speed = 5f;
        animator = GetComponent<Animator>();
        animator.SetBool("isStraight", true);
    }

    private void initializeMessages()
    {
        messages = new Dictionary<string, List<string>>();
        messages.Add("Jump", new List<string>() { "Wooosh", "boink", "Pop", "Boom" });
        messages.Add("CloseOne", new List<string>() { "Close...", "Phew", "WooooW", "Niceee","Not today" });
    }
    private string getRandomMessage(string key)
    {
        string message = key;
        List<string> messageList = messages[key];
        if(messageList.Count!=0)
        {
            var random = new System.Random();
            int index = random.Next(messageList.Count);
            message = messageList[index];
        }
        return message;
    }
    // Update is called once per frame
    void Update()
    {
        checkInput(); 
        Move(); 
    }

    private void Move()
    {
        transform.position = transform.position  + direction* Time.deltaTime*speed;
        spawnner.transform.up = direction;
    }

    private void checkInput()
    {
        if (!isJumping)
        {
            float horizontal = Input.GetAxis("Horizontal");
            if (Input.GetButtonDown("Jump"))
            {

                StartCoroutine(Jump());
            }
            if (horizontal > 0)
            {
                direction = new Vector2(1f, -1f);
                scale = new Vector3(0.5f, 0.5f, 0.5f);
                transform.localScale = scale;
                animator.SetBool("isStraight", false);
            }
            else if (horizontal < 0)

            {
                direction = new Vector2(-1f, -1f);
                scale = new Vector3(-0.5f, 0.5f, 0.5f);
                transform.localScale = scale;
                animator.SetBool("isStraight", false);
            }
            else
            {
                direction = Vector2.down;
                animator.SetBool("isStraight", true);
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        checkCollision(collision);

        if (collision.CompareTag("Close"))
        {
            gameManager.AddScore(15f);
            ShowPopUp(getRandomMessage("CloseOne"));
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        checkCollision(collision);
    }

    private void checkCollision(Collider2D collider)
    {
        Debug.Log("Collided");
        if (!inAir)
        {
            if (collider.CompareTag("Obstacle"))
            {
                gameManager.GameOver();
            }
        }
        else
        {
            if (collider.CompareTag("Tree"))
            {
                gameManager.GameOver();
            }
        }
    }

    private void ShowPopUp(string message)
    {
        GameObject popup = Instantiate(popUpPrefab, transform.position, Quaternion.identity);
        TMP_Text text= popup.GetComponentInChildren<TMP_Text>();
        text.text = message;
    }

    IEnumerator Jump()
    {
        isJumping = true;
        ShowPopUp(getRandomMessage("Jump"));
        gameManager.AddScore(12f);
        animator.SetBool("isJumping",true);
        yield return new WaitForSeconds(.5f);
        animator.SetBool("isJumping", false);
        isJumping = false;
    }
    public void SetSpeed(float f)
    {
        speed = f;
    }
    public void AddSpeed(float f)
    {
        speed += f;
    }
}
