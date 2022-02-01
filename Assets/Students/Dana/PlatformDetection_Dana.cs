using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformDetection_Dana : MonoBehaviour
{
    public BoxCollider2D BC;
    public float dirX= 0f;

    void Start()
    {
        BC = GetComponent<BoxCollider2D>();
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.name == ("Platform"))
            this.transform.parent = col.transform;
    }

    void OnCollisionExit2D(Collision2D col)
    {
        if (col.gameObject.name == ("Platform"))
            this.transform.parent = null;
    }
}
