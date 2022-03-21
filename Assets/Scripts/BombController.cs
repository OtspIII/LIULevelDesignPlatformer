using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombController : CharController
{
    public ExplosionController Boom;
    
    public override void Knockback(Vector2 src, float amt){}

    public override void ApplyJSON(JSONData data)
    {
        base.ApplyJSON(data);
        if (data.Type == SpawnThings.Destructable)
        {
            HP = data.Amount > 0 ? Mathf.CeilToInt(data.Amount) : 1;
            Boom.Damage = 0;
        }
        if (data.Type == SpawnThings.Bomb)
        {
            HP = 1;
            Boom.Damage = data.Amount > 0 ? Mathf.CeilToInt(data.Amount) : 1;
        }
        if (data.Size2 > 0)
        {
            Boom.transform.localScale = new Vector3(data.Size2,data.Size2,1);
        }
    }

    public override void Die()
    {
        base.Die();
        if (Boom != null && Boom.Damage > 0)
            Boom.Activate();
    }
}
