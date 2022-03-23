using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThingController : MonoBehaviour
{
    public SpriteRenderer SR;
    public JSONData JSON;

    void Awake()
    {
        if (SR == null) SR = GetComponent<SpriteRenderer>();
        OnAwake();
    }
    
    public virtual void OnAwake()
    {
        
    }
    
    public virtual void ApplyJSON(JSONData data)
    {
        JSON = data;
        if (data.Sprite != null)
        {
            if (SR == null) SR = gameObject.AddComponent<SpriteRenderer>();
            SR.sprite = data.Sprite;
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
    }

    private void OnDestroy()
    {
        GameManager.Me.Tiles.Remove(this);
    }

    public void SetColor(MColors color)
    {
        if (SR == null) return;
        Color c = Color.white;
        switch (color)
        {
            case MColors.Player:
            {
                
                c = Color.cyan; 
                break;
            }
            case MColors.Red: c = Color.red; break;
            case MColors.Green: c = Color.green; break;
            case MColors.Yellow: c = Color.yellow; break;
            case MColors.Pink: c = Color.magenta; break;
            case MColors.Orange: c = new Color(1,0.5f,0); break;
            case MColors.Blue: c = Color.blue; break;
            case MColors.Purple: c = new Color(0.5f,0,1); break;
            case MColors.White: c = new Color(0.8f,0.8f,0.8f); break;
            case MColors.Ebony: c = new Color(0.1f,0.1f,0.1f); break;
            case MColors.Tan: c = new Color(0.8f,0.7f,0.6f); break;
            case MColors.Algea: c = new Color(0.1f,0.66f,0.56f); break;
            case MColors.Slate: c = new Color(0.4f,0.4f,0.4f); break;
            case MColors.WallWhite: c = Color.white; break;
        }
        SR.color = c;
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
}