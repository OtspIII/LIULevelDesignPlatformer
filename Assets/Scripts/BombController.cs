using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombController : CharController
{
    public override void Knockback(Vector2 src, float amt){}

    public override void ApplyJSON(JSONData data)
    {
        base.ApplyJSON(data);
        HP = data.Amount > 0 ? Mathf.CeilToInt(data.Amount) : 1;
    }

    public override void Die()
    {
        base.Die();
        if (JSON.Audio)GameManager.Me.PlaySound(JSON.Audio);
    }
}
