using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletMoveLeft : MonoBehaviour
{
    public Vector3 position;
    public float speed;

    // Start is called before the first frame update
    void Start()
    {
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
    }
}
