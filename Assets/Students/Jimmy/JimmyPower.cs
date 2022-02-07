using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JimmyPower : GenericPower
{
    public override void Activate()
    {
        Player.RB.gravityScale *= -1;
    }

    void Update()
    {
        
    }
}
