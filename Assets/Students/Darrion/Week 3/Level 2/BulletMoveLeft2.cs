using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletMoveLeft2 : MonoBehaviour
{
    public Vector3 position;
    public float speed;
    public GameObject gameManager;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("GameManager");

        position = transform.position;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        position.x = position.x - speed;

        transform.position = position;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Platforms")
        {
            //Debug.Log("Bullet Destroyed");
            Destroy(gameObject);
        }

        if (collision.gameObject.tag == "Red")
        {
            //Debug.Log("Bullet Destroyed");
            Destroy(collision.gameObject);
            Destroy(gameObject);
        }

        if (collision.gameObject.tag == "Blue")
        {
            //Debug.Log("Bullet Destroyed");
            Destroy(gameObject);
        }

        if (collision.gameObject.tag == "Purple")
        {
            gameManager.GetComponent<GameManager>().coinsDestroyed++;
            //Debug.Log("Bullet Destroyed");
            Destroy(collision.gameObject);
            Destroy(gameObject);
        }
    }
}
