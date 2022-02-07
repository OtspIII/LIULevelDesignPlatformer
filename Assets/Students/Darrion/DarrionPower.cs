using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DarrionPower : GenericPower
{
    public int sizeState;

    float speed, jumpPower, jumpTime, gravity;
    public float smallSpeed, smallJumpPower, smallJumpTime, smallGravity;
    public float bigSpeed, bigJumpPower, bigJumpTime, bigGravity;

    public float size, smallSize, bigSize;

    private void Start()
    {
        sizeState = 2;
        speed = GetComponent<PlayerController>().Speed;
        jumpPower = GetComponent<PlayerController>().JumpPower;
        jumpTime = GetComponent<PlayerController>().JumpTime;
        gravity = GetComponent<PlayerController>().Gravity;
    }

    public override void Activate()
    {
        sizeState++;
        //Debug.Log("It's Working");
    }

    void Update()
    {
        if (sizeState > 2)
        {
            sizeState = 1;
        }

        if (sizeState == 1)
        {
            GetComponent<PlayerController>().Speed = smallSpeed;
            GetComponent<PlayerController>().JumpPower = smallJumpPower;
            GetComponent<PlayerController>().JumpTime = smallJumpTime;
            GetComponent<PlayerController>().Gravity = smallGravity;
            transform.localScale = new Vector3(smallSize, smallSize, smallSize);
        }       
        
        if (sizeState == 2)
        {
            GetComponent<PlayerController>().Speed = speed;
            GetComponent<PlayerController>().JumpPower = jumpPower;
            GetComponent<PlayerController>().JumpTime = jumpTime;
            GetComponent<PlayerController>().Gravity = gravity;
            transform.localScale = new Vector3(size, size, size);
        }       
        
        if (sizeState == 3)
        {
            GetComponent<PlayerController>().Speed = bigSpeed;
            GetComponent<PlayerController>().JumpPower = bigJumpPower;
            GetComponent<PlayerController>().JumpTime = bigJumpTime;
            GetComponent<PlayerController>().Gravity = bigGravity;
            transform.localScale = new Vector3(bigSize, bigSize, bigSize);
        }

        Debug.Log(sizeState);
    }
}
