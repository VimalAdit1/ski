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
    public GameObject jumpParticle;
    public GameObject trailParticle;
    public GameObject shield;
    Vector3 direction;
    Vector3 scale;
    Animator animator;
    bool inAir;
    bool isJumping;
    float speed;
    float horizontal = 0;
    public int price;
    bool isShielded;
    bool isSwipeEnabled;
    Dictionary<string, List<string>> messages;
    float spawnTime ;
    float spawnStart = 0.08f;
    Vector3 startPosition;
    Vector3 endPosition;
    void Start()
    {
        shield.SetActive(false);
        initializeMessages();
        direction = Vector2.down;
        spawnner.transform.up = direction;
        scale = transform.localScale;
        speed = 8f;
        animator = GetComponent<Animator>();
        animator.SetBool("isStraight", true);
        isSwipeEnabled = PlayerPrefs.GetInt("Swipe", 0) == 1;
        isShielded = false;
        inAir = false;
        isJumping = false;
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
        if (!isSwipeEnabled)
        {
            checkInput();
        }
        else
        {
            GetSwipeInput();
        }
        Move();
        if (!inAir)
        {
            if (spawnTime <= 0)
            {
                Instantiate(trailParticle, transform.position, Quaternion.identity);
                spawnTime = spawnStart;
            }
            else
            {
                spawnTime -= Time.deltaTime;
            }
        }
    }

    private void Move()
    {
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
        transform.position = transform.position + direction * Time.deltaTime * speed;
        spawnner.transform.up = direction;
    }

    private void checkInput()
    {
        if (!inAir)
        {
            //horizontal = Input.GetAxis("Horizontal");
            horizontal = Input.acceleration.x;
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
        if (!isJumping)
        {
            isJumping = true;
            ShowPopUp(getRandomMessage("Jump"));
            gameManager.AddScore(12f);
            gameManager.AddCoin(15);
            animator.SetBool("isJumping", true);
            yield return new WaitForSeconds(.5f);

            animator.SetBool("isJumping", false);
        }
    }
    public void SetSpeed(float f)
    {
        speed = f;
    }
    public void TriggerJump()
    {
        if(inAir)
        {
            AudioManager.instance.StartPlaying("Land");
            isJumping = false;
            Camera.instance.ShakeScreen(3, 0.2f);
        }
        else
        {
            AudioManager.instance.StartPlaying("Jump");
            Camera.instance.ShakeScreen(1, 0.2f);
        }
        Instantiate(jumpParticle, this.transform.position, Quaternion.identity);
        inAir = !inAir;
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
    void GetSwipeInput()
    {
        foreach (Touch touch in Input.touches)
        {
            if (touch.phase == TouchPhase.Began)
            {
                startPosition = touch.position;
                endPosition = touch.position;
            }

            if (touch.phase == TouchPhase.Moved|| touch.phase == TouchPhase.Ended)
            {
                endPosition = touch.position;
                if ((endPosition - startPosition).magnitude > 20)
                {
                    DetectSwipe();
                }
            }
        }
    }
    void DetectSwipe()
    {
        if (!inAir)
        {
            Vector2 Distance = endPosition - startPosition;
            float xDistance = Mathf.Abs(Distance.x);
            float yDistance = Mathf.Abs(Distance.y);
            if (xDistance > yDistance)
            {
                if (Distance.x > 0)
                {
                    horizontal = 1;
                }
                else
                {
                    horizontal = -1;
                }
            }
            else
            {
                if (Distance.y < 0)
                {
                    horizontal = 0;
                }
                else
                {
                    StartCoroutine(Jump());
                }
            }
        }
    }
}
