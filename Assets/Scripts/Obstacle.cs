
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    public GameObject player;
    public GameManager gameManager;
    public bool isProp = false;
    Renderer renderer;
    // Start is called before the first frame update
    void Start()
    {
        renderer = gameObject.GetComponent<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {
        checkPosition();
        if (player.transform.position.y > transform.position.y)
        {
          // correctPosition();
        }
    }

    private void correctPosition()
    {
        if(!renderer.isVisible)
        {
            Debug.Log("Inside adjusting position");
            if(player.transform.position.x-transform.position.x>5f)
            {
                //transform.position = new Vector2(transform.position.x+8f,transform.position.y);
                //transform.position = (Vector2)player.transform.position + (Vector2)player.transform.up * 15 + Random.insideUnitCircle*5 ;
                gameManager.Destroyed();
                Destroy(gameObject);
                Debug.Log("Adjusting position");
            }
            else if(transform.position.x- player.transform.position.x>5f)
            {
                //transform.position = new Vector2(transform.position.x-8f,transform.position.y);
                //transform.position = (Vector2)player.transform.position + (Vector2)player.transform.up * 15 + Random.insideUnitCircle*5 ;
                gameManager.Destroyed();
                Destroy(gameObject);
                Debug.Log("Adjusting position");
            }
        }
    }

    private void checkPosition()
    {
        if(transform.position.y>player.transform.position.y&&!renderer.isVisible)
        {
            if(isProp)
            {
                gameManager.DestroyProp();
            }
            gameManager.Destroyed();
            Destroy(gameObject);
        }
    }
}
