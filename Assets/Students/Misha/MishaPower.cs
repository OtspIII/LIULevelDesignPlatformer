using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MishaPower : GenericPower
{
    public float Timer = 0;
    
    public override void Activate()
    {
        Timer = 1;
        Player.SetInControl(false);
        Player.RB.gravityScale = 0;
        Player.RB.velocity = new Vector2(30,0);
//        float dist = Vector2.Distance(transform.position, brick.transform.position);
    }

    void Update()
    {
        if (Timer > 0)  
        {
            Timer -= Time.deltaTime / 0.5f;
            Player.Body.transform.rotation = Quaternion.Euler(0, 0, Timer * 360);
            if (Timer <= 0)
            {
                Player.RB.gravityScale = Player.Gravity; 
                Player.Body.transform.rotation = Quaternion.Euler(0,0,0);
                Player.SetInControl(true);
            }
        }
    }
}
