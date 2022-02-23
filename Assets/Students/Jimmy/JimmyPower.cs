using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JimmyPower : GenericPower
{

    public float score;

    public GameObject gate;
    public override void Activate()
    {
        Player.RB.gravityScale *= -1;
    }

    void Update()
    {
       
    }

    public void CheckStatus()
    {

        if(score >= 4)
        {
            Destroy(gate.gameObject);
        }

    }
    
  
    void OnTriggerEnter2D(Collider2D col)
    {
        Debug.Log(col.gameObject.name);

        if (col.gameObject.tag == "coin")
        {
       
            Destroy(col.gameObject);
            score++;
            CheckStatus();
        }
     
    }
}
