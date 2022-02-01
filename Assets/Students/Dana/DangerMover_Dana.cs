using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DangerMover_Dana : MonoBehaviour
{
    public float dirX, speed = 2f;
    public bool moveRight = true;
    public float dLeft = 0f;
    public float dRight = 0f;
    void Update()
    {
        if (transform.position.x >= dRight)
            moveRight = false;
        if (transform.position.x <= dLeft)
            moveRight = true;

        if (moveRight)
            transform.position = new Vector3(transform.position.x + speed * Time.deltaTime, transform.position.y);
        else
            transform.position = new Vector3(transform.position.x - speed * Time.deltaTime, transform.position.y);
    }
}
