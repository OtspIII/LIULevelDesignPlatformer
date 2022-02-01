using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformMover_Dana : MonoBehaviour
{
    public float speed = 2f;
    public float platLeft = 0f;
    public float platRight = 0f;
    public bool moveRight = true;

    void Update()
    {
        if (transform.position.x >= platRight)
            moveRight = false;
        if (transform.position.x <= platLeft)
            moveRight = true;

        if (moveRight)
            transform.position = new Vector3(transform.position.x + speed * Time.deltaTime, transform.position.y);
        else
            transform.position = new Vector3(transform.position.x - speed * Time.deltaTime, transform.position.y);
    }
}
