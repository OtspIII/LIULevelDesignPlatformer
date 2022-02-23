using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mason_Destruct : MonoBehaviour
{
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 14)
        {
            this.transform.position = new Vector2(9999, 9999);
        }
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 15)
        {
            this.transform.position = new Vector2(9999, 9999);
        }
    }
}
