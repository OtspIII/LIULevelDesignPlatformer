using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    public MonsterData Shooter;
    public Rigidbody2D RB;
    public float Speed = 10;
    
    void Start()
    {
        RB = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        RB.velocity = transform.right * -Speed;
    }

    public void Setup(CharController shooter)
    {
        Shooter = shooter.Data;
        Speed = Shooter.AttackSpeed;
        if (shooter.Player)
            gameObject.layer = 10;
        GetComponent<BoxCollider2D>().enabled = true;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        CharController c = other.gameObject.GetComponent<CharController>();
        if (c) c.TakeDamage(Shooter.Damage);
        Destroy(gameObject);
    }
}
