﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TylerPower : GenericPower
{
    public float Timer = 0;

    public int direction=0;
    public bool xAllowed = false;
    bool teleporty,isTouchy;
    public GameObject fivemeters;
    public override void Activate()
    {
        Timer = 1;
        Player.SetInControl(false);
        Player.RB.gravityScale = 0;
    }

    void Update()
    {

        if (direction == 1)
        {
            fivemeters.transform.position = new Vector2(transform.position.x, transform.position.y+5);
        }
        if (direction == 2)
        {
            fivemeters.transform.position = new Vector2(transform.position.x - 5, transform.position.y);
        }
        if (direction == 3)
        {
            fivemeters.transform.position = new Vector2(transform.position.x, transform.position.y - 5);
        }
        if (direction == 4)
        {
            fivemeters.transform.position = new Vector2(transform.position.x + 5, transform.position.y);
        }

        isTouchy = fivemeters.GetComponent<FiveMeterCollisionScript>().isTouchy();
        Debug.Log(isTouchy);

       

        if (Input.GetKeyDown(KeyCode.W))
        {
            direction = 1;
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            direction = 2;
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            direction = 3;
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            direction = 4;
        }



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


        if (Input.GetKeyDown(KeyCode.X) && teleporty)
        {
            //transform.position = new Vector2(transform.position.x+5,transform.position.y);
            if (direction == 1)
            {
                transform.position = new Vector2(transform.position.x, transform.position.y + 5);
            }
            if (direction == 2)
            {
                transform.position = new Vector2(transform.position.x - 5, transform.position.y);
            }
            if (direction == 3)
            {
                transform.position = new Vector2(transform.position.x, transform.position.y - 5);
            }
            if (direction == 4)
            {
                transform.position = new Vector2(transform.position.x + 5, transform.position.y);
            }
        }
    }



    void FixedUpdate()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.right,5);
        Debug.DrawRay(transform.position, Vector2.right*5, Color.yellow);
        Debug.Log(hit.collider.tag);
        if (isTouchy==false)
        {
            teleporty = true;
        }
        else
        {
            teleporty = false;
        }
        
    }

   
}
