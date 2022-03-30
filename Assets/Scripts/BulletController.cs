using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : ThingController
{
    public MonsterData Shooter;
    public float Speed = 10;
    
    void Start()
    {
        RB = GetComponent<Rigidbody2D>();
    }

    public override void ApplyJSON(JSONData data)
    {
        base.ApplyJSON(data);
        if (data.Audio)GameManager.Me.PlaySound(data.Audio);
    }

    void Update()
    {
        RB.velocity = transform.right * -Speed;
    }

    public void Setup(CharController shooter)
    {
        Shooter = shooter.Data;
        Source = shooter;
        Speed = Shooter.AttackSpeed;
        if (shooter.Player)
            gameObject.layer = 10;
        GetComponent<BoxCollider2D>().enabled = true;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        CharController c = other.gameObject.GetComponent<CharController>();
        if (c)
        {
            if (!c.Tile || Shooter.Color == MColors.Player)
            {
                c.Knockback(transform.position, Shooter.Knockback);
                c.TakeDamage(Shooter.Damage);
            }
        }
        if (JSON.Drop != ' ')
        {
            ThingController drop = GameManager.Me.SpawnThing(JSON.Drop,GameManager.Me.Creator,transform.position);
            if (drop != null) drop.Source = Source;
        }
        Destroy(gameObject);
    }
}
