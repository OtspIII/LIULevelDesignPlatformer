using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyController : CharController
{
    public PlayerController Target;
    public float Rotation;
    public bool Active = false;
    public float Windup = 0;
    public float Leaping = 0;
    public Vector2 LeapStart;
    
    
    public override void OnStart()
    {
        GameManager.Me.Enemies.Add(this);
        GameManager.Me.AllEnemies.Add(this);
    }

    private void OnDestroy()
    {
        GameManager.Me.AllEnemies.Remove(this);
        GameManager.Me.Enemies.Remove(this);
    }


    public override void OnUpdate()
    {
        base.OnUpdate();
        Vector2 vel = Knock;
        if (!Active && GameManager.LevelMode)
        {
            PlayerController pc = GameManager.Me.PC;
//            Debug.Log("DIST: " + Vector2.Distance(transform.position,pc.transform.position) + " / " + Data.VisionRange);
            if(pc.Moved && Vector2.Distance(transform.position,pc.transform.position) < Data.VisionRange)
                Activate();
        }
        if (Active)
        {
            float speed = Data.Speed;
            if (Leaping <= 0)
            {
                float desired = Mathf.Atan2(transform.position.y - Target.transform.position.y,
                                    transform.position.x - Target.transform.position.x) * Mathf.Rad2Deg;
                Rotation = Mathf.LerpAngle(Rotation, desired, 0.05f);
                transform.rotation = Quaternion.Euler(0, 0, Rotation);
            }
            else
            {
                Leaping -= Time.deltaTime / 5f;
                if (Vector2.Distance(LeapStart, transform.position) >= Leaping)
                    Leaping = 0;
                else
                    speed = Data.AttackSpeed;
            }
            if (Windup > 0)
            {
//                RB.velocity = vel;
                Windup -= Time.deltaTime;
                if (Windup > 0)
                    Shaking = 1;
                else
                {
                    Shaking = 0;
                    SR.transform.localPosition = Vector3.zero;
                    if (Data.Type == MTypes.Leaper)
                    {
                        Vector3 rot = transform.rotation.eulerAngles;
                        if (Data.AttackSpread > 0) rot.z += Random.Range(0, Data.AttackSpread) - (Data.AttackSpread / 2);
                        transform.rotation = Quaternion.Euler(rot);
                        Leaping = Data.AttackRange * 2f;
                        LeapStart = transform.position;
                    }
                }
            }
            else
            {
                float dist = Vector2.Distance(Target.transform.position, transform.position);
                if (Data.Type == MTypes.Shooter && dist < Data.AttackRange)
                {
                    if (dist < Data.AttackRange * 0.75f)
                        speed *= -1;
                    else
                        speed = 0;
                }
                    
                vel += (Vector2)transform.right * -speed;
            }
        }

        if (Target != null && Vector2.Distance(transform.position, Target.transform.position) < Data.AttackRange)
        {
            switch (Data.Type)
            {
                case MTypes.Shooter: Shoot();
                    break;
                case MTypes.Leaper:
                    if (Windup > 0 || Leaping > 0) break;
                    Windup = Data.AttackRate;
                    break;
            }
        }

        RB.velocity = vel;

    }

    

    public void Activate()
    {
        Target = GameManager.Me.PC;
        Active = true;
        Rotation = Mathf.Atan2(transform.position.y - Target.transform.position.y, transform.position.x - Target.transform.position.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0,0,Rotation);
        BulletCooldown = Random.Range(0, Data.AttackRate);
    }

    public override void Die()
    {
        base.Die();
        GameManager.Me.Enemies.Remove(this);
    }

    public override void Reset()
    {
        Destroy(gameObject);
    }

    public override void TakeDamage(int amt)
    {
        base.TakeDamage(amt);
        if(!Active) Activate();
    }

    private void OnCollisionStay2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Wall")) Leaping = 0;
        if (Data.Type == MTypes.Shooter) return;
        PlayerController pc = other.gameObject.GetComponent<PlayerController>();
        if (pc != null)
        {
            pc.Knockback(transform.position,Data.Knockback);
            pc.TakeDamage(Data.Damage);
        }
    }

}
