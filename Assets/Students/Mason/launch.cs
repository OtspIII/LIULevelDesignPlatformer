using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class launch : MonoBehaviour
{
    private Rigidbody2D rb;

    public void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 14)
        {
            this.rb.velocity = new Vector2(10, 8);
        }
    }
}
