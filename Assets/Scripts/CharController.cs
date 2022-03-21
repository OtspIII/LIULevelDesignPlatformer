using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class CharController : ThingController
{
    public MonsterData Data;
    public int HP;
    public bool Alive = true;
    public Rigidbody2D RB;
    public Collider2D Coll;
    public bool Player;
    public float BulletCooldown = 999;
    public float Shaking = 0;
    public Vector2 Knock;
    public bool Belted = false;

    public override void OnAwake()
    {
        base.OnAwake();
        RB = GetComponent<Rigidbody2D>();
        Coll = GetComponent<Collider2D>();
    }


    void Start()
    {
        OnStart();
    }
    
    public virtual void OnStart()
    {
        
    }

    void Update()
    {
        Belted = false;
        BulletCooldown += Time.deltaTime;
        if (!Alive || GameManager.Me.Paused) return;
        if (Shaking > 0)
        {
            Shaking -= Time.deltaTime;
            if (Shaking > 0)
                SR.transform.localPosition = new Vector3(Random.Range(-0.1f,0.1f),Random.Range(-0.1f,0.1f),0);
            else
                SR.transform.localPosition = Vector3.zero;
        }
        OnUpdate();

        if (Knock != Vector2.zero)
        {
            Knock = Vector2.Lerp(Knock,Vector2.zero,0.1f);
            Knock = Vector2.MoveTowards(Knock,Vector2.zero,0.1f);
        }
    }
    
    public virtual void OnUpdate()
    {
        
    }
    
    public void Setup(MonsterData data)
    {
        Data = data;
        gameObject.name = data.Color + "(" + data.Creator + ")";
        SetColor(data.Color);
        if(data.Color == MColors.Player)GameSettings.CurrentPlayerSpeed = data.Speed;
        float skinny = data.Type == MTypes.Leaper ? 0.25f : 0.5f;
        transform.localScale = new Vector3(data.Size*0.5f,data.Size*skinny,1);
        if (HP == 0) HP = data.HP;
        //if (data.Color == MColors.Player) Debug.Log(data.Color);
    }
    
    public virtual void TakeDamage(int amt)
    {
        HP -= amt;
        if (HP <= 0)
            Die();
        else
            Shaking = 0.2f;
    }

    public virtual void Knockback(Vector2 src, float amt)
    {
        Vector2 dir = (Vector2)transform.position - src;
        Knock = dir * amt * GameSettings.Knockback;
    }

    public virtual void Die()
    {
        SR.color = Color.gray;
        Alive = false;
        if(RB != null) RB.velocity = Vector2.zero;
        Coll.enabled = false;
    }
    
    public virtual void Reset()
    {
    }

    public virtual void Shoot()
    {
        if (BulletCooldown < Data.AttackRate) return;
        //Debug.Log("PEW: " + Time.time);
        Vector3 rot = transform.rotation.eulerAngles;
        Vector3 pos = transform.position + (transform.right * transform.localScale.x * -0.5f);
        if (Data.AttackSpread > 0) rot.z += Random.Range(0, Data.AttackSpread) - (Data.AttackSpread / 2);
        BulletController b = Instantiate(GameManager.Me.BPrefab, pos, Quaternion.Euler(rot));
        b.Setup(this);
        BulletCooldown = 0;
    }
}
