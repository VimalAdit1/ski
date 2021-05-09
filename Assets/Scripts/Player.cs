﻿using System;
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
    public GameObject particle;
    public GameObject shield;
    TrailRenderer trail;
    Vector3 direction;
    Vector3 scale;
    Animator animator;
    bool isJumping = false;
    public bool inAir = false;
    float speed;
    public int price;
    bool isShielded;
    Dictionary<string, List<string>> messages;
    void Start()
    {
        shield.SetActive(false);
        initializeMessages();
        direction = Vector2.down;
        spawnner.transform.up = direction;
        scale = transform.localScale;
        speed = 5f;
        animator = GetComponent<Animator>();
        trail = GetComponentInChildren<TrailRenderer>();
        animator.SetBool("isStraight", true);
        isShielded = false;
    }

    private void initializeMessages()
    {
        messages = new Dictionary<string, List<string>>();
        messages.Add("Jump", new List<string>() { "Wooosh", "boink", "Pop", "Boom" });
        messages.Add("CloseOne", new List<string>() { "Close...", "Phew", "WooooW", "Niceee","Not today","Bigiluuuuu" });
        messages.Add("Coins", new List<string>() { "Money money money...", "$$$$$", "Hot cash", "Im Rich" });
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
            //float horizontal = Input.GetAxis("Horizontal");
            float horizontal = Input.acceleration.x;
            if (horizontal > 0.2)
            {
                horizontal = 1;
            }
            else if (horizontal < -0.2)
            {
                horizontal = -1;
            }
            else
            {
                horizontal = 0;
            }
            if (Input.GetButtonDown("Jump")||Input.touchCount>0)
            {

                StartCoroutine(Jump());
            }
            if (horizontal == 1)
            {
                direction = new Vector2(1f, -1f);
                scale = transform.localScale;
                if (scale.x < 0)
                {
                    scale.x = -scale.x;
                }
                transform.localScale = scale;
                animator.SetBool("isStraight", false);
            }
            else if (horizontal == -1)

            {
                direction = new Vector2(-1f, -1f);
                scale = transform.localScale;
                if (scale.x > 0)
                {
                    scale.x = -scale.x;
                }
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
            gameManager.AddCoin(15);
            ShowPopUp(getRandomMessage("CloseOne"));
        }
        else if (collision.CompareTag("Coin"))
        {
            Destroy(collision.gameObject);
            gameManager.Destroyed();
            gameManager.AddCoin(50);
            ShowPopUp(getRandomMessage("Coins"));
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        checkCollision(collision);
    }

    private void checkCollision(Collider2D collider)
    {
        if (!inAir)
        {
            if (collider.CompareTag("Obstacle"))
            {
                checkShield(collider);
            }
        }
        else
        {
            if (collider.CompareTag("Tree"))
            {
                checkShield(collider);
            }
        }
    }
    void checkShield(Collider2D collider)
    {
        if (!isShielded)
        {
            gameManager.GameOver();
        }
        else
        {
            gameManager.ShieldDestroyed();
            Destroy(collider.gameObject);
            gameManager.Destroyed();
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
        gameManager.AddCoin(15);
        animator.SetBool("isJumping",true);
        yield return new WaitForSeconds(.5f);
       
        animator.SetBool("isJumping", false);
        isJumping = false;
        
    }
    public void SetSpeed(float f)
    {
        speed = f;
    }
    public void TriggerJump()
    {
        trail.emitting = !trail.emitting;
        if(trail.emitting)
        {
            AudioManager.instance.StartPlaying("Land");
        }
        else
        {
            AudioManager.instance.StartPlaying("Jump");
        }
        Instantiate(particle, this.transform.position, Quaternion.identity);
    }
    public void AddSpeed(float f)
    {
        speed += f;
    }
    public void SetShield(bool state)
    {
        isShielded = state;
        shield.SetActive(state);
    }
}
