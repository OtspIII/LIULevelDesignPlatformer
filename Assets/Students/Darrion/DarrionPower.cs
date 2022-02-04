using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DarrionPower : GenericPower
{
    public int sizeState;
    float speed, jumpPower, jumpTime, gravity;
    public float smallSpeed, smallJumpPower, smallJumpTime, smallGravity;
    public float bigSpeed, bigJumpPower, bigJumpTime, bigGravity;

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
        if (sizeState > 3)
        {
            sizeState = 1;
        }

        if (sizeState == 1)
        {
            GetComponent<PlayerController>().Speed = smallSpeed;
            GetComponent<PlayerController>().JumpPower = smallJumpPower;
            GetComponent<PlayerController>().JumpTime = smallJumpTime;
            GetComponent<PlayerController>().Gravity = smallGravity;
        }       
        
        if (sizeState == 2)
        {
            GetComponent<PlayerController>().Speed = speed;
            GetComponent<PlayerController>().JumpPower = jumpPower;
            GetComponent<PlayerController>().JumpTime = jumpTime;
            GetComponent<PlayerController>().Gravity = gravity;
        }       
        
        if (sizeState == 3)
        {
            GetComponent<PlayerController>().Speed = bigSpeed;
            GetComponent<PlayerController>().JumpPower = bigJumpPower;
            GetComponent<PlayerController>().JumpTime = bigJumpTime;
            GetComponent<PlayerController>().Gravity = bigGravity;
        }

        Debug.Log(sizeState);
    }
}
