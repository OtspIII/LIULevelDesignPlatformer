using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class ThingController : MonoBehaviour
{
    public SpriteRenderer SR;
    public JSONData JSON;
    public AudioSource AS;
    public Rigidbody2D RB;
    public Collider2D Coll;
    public float Shaking = 0;
    public Vector2 Knock;
    public bool Belted = false;
    public bool Alive = true;
    public bool Mobile = false;
    public ThingController Source;

    void Awake()
    {
        if (SR == null) SR = GetComponent<SpriteRenderer>();
        if (AS == null) AS = GetComponent<AudioSource>();
        if (RB == null) RB = GetComponent<Rigidbody2D>();
        if (Coll == null) Coll = GetComponent<Collider2D>();
        OnAwake();
    }
    
    public virtual void OnAwake()
    {
        
    }
    
    public virtual void OnUpdate()
    {
        
    }
    
    void Update()
    {
        Belted = false;
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
    
    public virtual void ApplyJSON(JSONData data)
    {
        JSON = data;
        if (data.Sprite != null)
        {
            if (SR == null) SR = gameObject.AddComponent<SpriteRenderer>();
            SR.sprite = data.Sprite;
            SR.color = Color.white;
        }

        if (data.Color != MColors.None && (data.Sprite == null || (data.Type != SpawnThings.Enemy && data.Type != SpawnThings.Player)) )
        {
            if (SR == null) SR = gameObject.AddComponent<SpriteRenderer>();
            SetColor(data.Color);
        }

        if (data.Size > 0)
        {
            transform.localScale = new Vector3(data.Size,data.Size,1);
        }

        if (data.Tag != "")
        {
            GameManager.Me.AddTag(data.Tag,this);
        }

        if (data.Layer > 0)
        {
            gameObject.layer = data.Layer;
        }
    }

    private void OnDestroy()
    {
        GameManager.Me.Tiles.Remove(this);
    }

    public void SetColor(MColors color)
    {
        if (SR == null) return;
        
        SR.color = God.GetColor(color);
    }
}


public enum SpawnThings
{
    None=0,
    Player=1,
    Enemy=2,
    Lava=3,
    ConveyorU=4,
    ConveyorR=5,
    ConveyorD=6,
    ConveyorL=7,
    Key1=8,
    Door1=9,
    Key2=10,
    Door2=11,
    Key3=12,
    Door3=13,
    Wall=14,
    Floor=15,
    Event=16,
    Destructable=17,
    Bomb=18,
    Music=19,
    Bullet=20,
    Explosion=21,
    Zoom=22,
    Health=23,
    Goal=24,
    Destructible=25,
}