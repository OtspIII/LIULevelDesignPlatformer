﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaraPower : GenericPower
{
    public float Timer = 0;
    public Vector3 moveDirection;
     public const float maxDashTime = 1.0f;
     public float dashDistance = 10;
     public float dashStoppingSpeed = 0.1f;
     float currentDashTime = maxDashTime;
     float dashSpeed = 6;
    
    public override void Activate()
    {
        Timer = 1;
        Player.SetInControl(false);
        Player.RB.gravityScale = 0;

    }

    void Update()
    {
     //Player does a spin
        if (Timer > 0)
        {
            Timer -= Time.deltaTime / 0.5f;
            Player.Body.transform.rotation = Quaternion.Euler(0, 0, Timer * 360);
            if (Timer <= 0)
            {
                Player.RB.gravityScale = Player.Gravity;
                Player.Body.transform.rotation = Quaternion.Euler(0, 0, 0);
                Player.SetInControl(true);
            }
        }
        
     //Goal: Make a short dash/teleport type power
     if (Input.GetKey(KeyCode.X))
         {
             currentDashTime = 0;                
         }
         if(currentDashTime < maxDashTime)
         {
             moveDirection = transform.forward * dashDistance;
             currentDashTime += dashStoppingSpeed;
         }
         else
         {
             moveDirection = Vector3.zero;
         }
         controller.Move(moveDirection * Time.deltaTime * dashSpeed);

    }
}
