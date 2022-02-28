using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZachPower : GenericPower
{
    public float Speed;
    public float JumpForce;

    public float DashForce;
    public float StartDashTimer;

    public float CurrentDashTimer;
    public float DashDirection;
    
    //bool isGrounded = false;
    public bool isDashing;

    Rigidbody2D rb;
    float movX;
    public override void Activate()
    {
        if (!Player.OnGround()&& rb.velocity.x != 0)
        {
            isDashing = true;
            CurrentDashTimer = StartDashTimer;
            DashDirection = rb.velocity.x > 0 ? 1 : -1;
            rb.velocity = Vector2.zero;
            Player.SetInControl(false);
        }
        
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        

        if (isDashing)
        {
            rb.velocity = transform.right * DashDirection * DashForce;

            CurrentDashTimer -= Time.deltaTime;

            if (CurrentDashTimer <= 0)
            {
                isDashing = false;
                Player.SetInControl(true);
            }
        }
    }
}
