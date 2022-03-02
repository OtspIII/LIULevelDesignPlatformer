﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharController : MonoBehaviour
{
    public MonsterData Data;
    public int HP;
    public bool Alive = true;
    public Rigidbody2D RB;
    public SpriteRenderer SR;
    public Collider2D Coll;
    public bool Player;
    public float BulletCooldown = 999;
    public float Shaking = 0;

    void Awake()
    {
        RB = GetComponent<Rigidbody2D>();
        Coll = GetComponent<Collider2D>();
        OnAwake();
    }
    
    public virtual void OnAwake()
    {
        
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
    }
    
    public virtual void OnUpdate()
    {
        
    }
    
    public void Setup(MonsterData data)
    {
        Data = data;
        gameObject.name = data.Color + "(" + data.Creator + ")";
        Color c = Color.white;
        switch (data.Color)
        {
            case MColors.Player: c = Color.cyan; break;
            case MColors.Red: c = Color.red; break;
            case MColors.Green: c = Color.green; break;
            case MColors.Yellow: c = Color.yellow; break;
            case MColors.Pink: c = Color.magenta; break;
            case MColors.Orange: c = new Color(1,0.5f,0); break;
            case MColors.Blue: c = Color.blue; break;
            case MColors.Purple: c = new Color(0.5f,0,1); break;
            case MColors.White: c = new Color(0.8f,0.8f,0.8f); break;
        }
        SR.color = c;
        float skinny = data.Type == MTypes.Leaper ? 0.25f : 0.5f;
        transform.localScale = new Vector3(data.Size*0.5f,data.Size*skinny,1);
        if (HP == 0) HP = data.HP;
        if (data.Color == MColors.Player) Debug.Log(data.Color);
    }
    
    public virtual void TakeDamage(int amt)
    {
        HP -= amt;
        if (HP <= 0)
            Die();
        else
            Shaking = 0.2f;
    }

    public virtual void Die()
    {
        SR.color = Color.gray;
        Alive = false;
        RB.velocity = Vector2.zero;
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