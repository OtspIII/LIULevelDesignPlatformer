using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class launch : MonoBehaviour
{
    private Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 14)
        {
            rb.AddForce(new Vector2(5, 5), ForceMode2D.Force);
        }
    }
}
