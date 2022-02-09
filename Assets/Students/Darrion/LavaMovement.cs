using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LavaMovement : MonoBehaviour
{
    public bool moveRight, moveLeft, moveUp, moveDown;

    public float xPos, yPos, zPos, speed;
    public float rightLimit, leftLimit, upLimit, downLimit;
    

    // Start is called before the first frame update
    void Start()
    {
        xPos = transform.position.x;
        yPos = transform.position.y;
        zPos = transform.position.z;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (xPos > rightLimit)
        {
            moveRight = false;
            moveLeft = true;
        }     
        
        if (xPos < leftLimit)
        {
            moveLeft = false;
            moveRight = true;
        }

        if (yPos > upLimit)
        {
            moveUp = false;
            moveDown = true;
        }
        
        if (yPos < downLimit)
        {
            moveDown = false;
            moveUp = true;
        }

        if (moveRight == true)
        {
            xPos = xPos + speed;
        }
        
        if (moveLeft == true)
        {
            xPos = xPos - speed;
        }

        if (moveUp == true)
        {
            yPos = yPos + speed;
        }   
        
        if (moveDown == true)
        {
            yPos = yPos - speed;
        }

        transform.position = new Vector3(xPos, yPos, zPos);
    }
}
