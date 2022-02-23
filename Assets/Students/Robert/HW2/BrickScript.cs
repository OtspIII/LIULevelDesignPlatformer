using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrickScript : MonoBehaviour
{
    private float duration;
    public bool isOriginal;

    // Start is called before the first frame update
    void Start()
    {
        duration = 12;
    }

    // Update is called once per frame
    void Update()
    {
        if (duration >= 0 && !isOriginal)
        {
            duration -= Time.deltaTime;
        }
        if (duration <= 0)
        {
            transform.position = new Vector2(transform.position.x, transform.position.y - .25f);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Platforms" && duration <= 0)
        {
            Destroy(this.gameObject);
        }
    }
}
