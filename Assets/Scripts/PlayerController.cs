using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.XR;

public class PlayerController : CharController
{
    public bool Moved = false;
    public float Invincible = 0;

    public override void OnAwake()
    {
        base.OnAwake();
        Player = true;
    }

    public override void OnUpdate()
    {
        GameManager.Me.HPText.text = "HP: " + HP;
        bool input = false;
        Vector2 vel = RB.velocity;

        if (Input.GetKey(KeyCode.D))
            vel.x = Data.Speed;
        else if (Input.GetKey(KeyCode.A))
            vel.x = -Data.Speed;
        else
            vel.x = 0;
        if (Input.GetKey(KeyCode.W))
            vel.y = Data.Speed;
        else if (Input.GetKey(KeyCode.S))
            vel.y = -Data.Speed;
        else
            vel.y = 0;
        
        RB.velocity = vel;

        Vector2 target = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        float aimAt = Mathf.Atan2(transform.position.y - target.y,
                            transform.position.x - target.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, aimAt);

        if (Input.GetMouseButton(0))
        {
            Shoot();
            input = true;
        }

        if (vel != Vector2.zero) input = true;
        
        if (!Moved && input)
        {
            Moved = true;
            foreach(EnemyController e in GameManager.Me.Enemies)
            {
                e.Activate();
            }
        }

        if (Invincible > 0)
        {
            Invincible -= Time.deltaTime;
            Color c = SR.color;
            if (Invincible > 0)
                c.a = (int) (Time.time * 6) % 2 == 0 ? 1 : 0.5f;
            else
                c.a = 1;
            SR.color = c;
        }

    }

    public override void Reset()
    {
        base.Reset();
        Moved = false;
        RB.velocity = Vector2.zero;
    }

    public override void TakeDamage(int amt)
    {
        if (Invincible > 0) return;
        base.TakeDamage(amt);
        GameManager.Me.HPText.text = "HP: " + HP;
        Invincible = 0.5f;
    }

    public override void Die()
    {
        base.Die();
        GameManager.Me.GameOver();
    }
}
