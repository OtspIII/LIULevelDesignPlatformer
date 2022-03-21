using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombController : CharController
{
    public int Damage = 1;
    
    public override void Knockback(Vector2 src, float amt){}

    public override void ApplyJSON(JSONData data)
    {
        base.ApplyJSON(data);
        if (data.Type == SpawnThings.Destructable)
        {
            HP = data.Amount > 0 ? Mathf.CeilToInt(data.Amount) : 1;
            Damage = 0;
        }
        if (data.Type == SpawnThings.Bomb)
        {
            HP = 1;
            Damage = data.Amount > 0 ? Mathf.CeilToInt(data.Amount) : 1;
        }
    }
}
