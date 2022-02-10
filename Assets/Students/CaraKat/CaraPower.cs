using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaraPower : GenericPower
{
    public float Timer = 0;
    public Vector3 moveDirection;
    public float dashSpeed;
    public float dashTime;
    public bool grounded;
    
    public override void Activate()
    {
        Timer = 1;
        //dashTime = 1;
        Player.SetInControl(false);
        Player.RB.gravityScale = 0;
        Player.RB.velocity = new Vector2(0,0);
        grounded = true;

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

        if (Input.GetKey(KeyCode.X))
        {
            float horizontalInput = Input.GetAxis("Horizontal");
            float verticalInput = Input.GetAxis("Vertical");
            grounded = false;

            transform.position = transform.position + new Vector3(horizontalInput * dashTime * dashSpeed * Time.deltaTime, verticalInput * dashTime * dashSpeed * Time.deltaTime, 0);
            dashTime -= Time.deltaTime / 0.5f;
            if(dashTime <= 0.5f)
            {
                dashTime = 1.2f;
                grounded = true;
            }

        }
        
     //GOAL: Make a short dash/teleport type power (think Celeste but can go through danger wall)
     //GOAL 2: match timer for dash to be the same with the spin movement
    }
}