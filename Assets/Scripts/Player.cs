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
    void Start()
    {
        direction = Vector2.down;
        spawnner.transform.up = direction;
        scale = new Vector3(0.5f, 0.5f, 0.5f);
        animator = GetComponent<Animator>();
        animator.SetBool("isStraight", true);
    }

    // Update is called once per frame
    void Update()
    {
        checkInput(); 
        Move(); 
    }

    private void Move()
    {
        transform.position = transform.position  + direction* Time.deltaTime*10f;
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
            ShowPopUp("CloseOne");
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
        Debug.Log(message);
        GameObject popup = Instantiate(popUpPrefab, transform.position, Quaternion.identity);
        TMP_Text text= popup.GetComponentInChildren<TMP_Text>();
        text.text = message;
    }

    IEnumerator Jump()
    {
        isJumping = true;
        ShowPopUp("jump");
        gameManager.AddScore(12f);
        animator.SetBool("isJumping",true);
        yield return new WaitForSeconds(.5f);
        animator.SetBool("isJumping", false);
        isJumping = false;
    }
}
