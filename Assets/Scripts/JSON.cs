using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class JSONWeapon
{
    public string Text;
    public WeaponTypes Type;
    public IColors Color;
    public int Shots;
    public int Ammo;
    public int Damage;
    public float RateOfFire;
    public float Accuracy;
    public float Speed;
    public float Lifetime;
    public float Gravity;
    public float ExplodeRadius;
    public int ExplodeDamage;
    public float Knockback;
    public float Bounce;
    public bool SelfDamage;
    
    public JSONWeapon(JSONTempWeapon source)
    {
        Type = source.Type != null ? (WeaponTypes)Enum.Parse(typeof(WeaponTypes), source.Type) : WeaponTypes.Projectile;
        Color = source.Color != null ? (IColors)Enum.Parse(typeof(IColors), source.Color) : IColors.None;
        Text = source.Text;
        Damage = source.Damage > 0 ? (int)source.Damage : 10;
        Shots = source.Shots > 0 ? source.Shots : 1;
        Ammo = source.Ammo;
        RateOfFire = source.RateOfFire > 0 ? source.RateOfFire : 0.2f;
        Accuracy = source.Accuracy;
        Speed = source.Speed > 0 ? source.Speed : 50;
        Lifetime = source.Lifetime;
        Gravity = source.Gravity;
        ExplodeRadius = source.ExplodeRadius;
        ExplodeDamage = source.ExplodeDamage > 0 ? (int)source.ExplodeDamage : 10;
        Knockback = source.Knockback;
        Bounce = source.Bounce;
        SelfDamage = source.SelfDamage;
    }
}

[System.Serializable]
public class JSONItem
{
    public string Text = "";
    public float Amount;
    public ItemTypes Type;
    public IColors Color;
    

    public JSONItem(JSONTempItem source)
    {
        Type = source.Type != null ? (ItemTypes)Enum.Parse(typeof(ItemTypes), source.Type) : ItemTypes.None;
        Color = source.Color != null ? (IColors)Enum.Parse(typeof(IColors), source.Color) : IColors.None;
        Text = source.Text;
        Amount = source.Amount;
    }
}

[System.Serializable]
public class JSONCreator
{
    public string Author;
    public int PointsToWin;
    public int PlayerHP;
    public float MoveSpeed;
    public float SprintSpeed;
    public float Gravity;
    public GameModes Mode;
    public List<IColors> Teams = new List<IColors>();
    public List<JSONItem> Items = new List<JSONItem>();
    public List<JSONWeapon> Weapons = new List<JSONWeapon>();
    

    public JSONCreator(JSONTempCreator source,string author,TextAsset ta)
    {
        Author = author;
        Mode = source.Mode != null ? (GameModes)Enum.Parse(typeof(GameModes), source.Mode) : GameModes.Deathmatch;
        PointsToWin = source.PointsToWin;
        PlayerHP = source.PlayerHP;
        foreach(JSONTempItem i in source.Items)
            Items.Add(new JSONItem(i));
        foreach(JSONTempWeapon i in source.Weapons)
            Weapons.Add(new JSONWeapon(i));
        MoveSpeed = source.MoveSpeed > 0 ? source.MoveSpeed : 10;
        SprintSpeed = source.SprintSpeed > 0 ? source.SprintSpeed : 1.5f;
        Gravity = source.Gravity > 0 ? source.Gravity : 1;

//        if (source.Symbol == null)
//        {
//            Debug.Log("JSON CRASH: " + author + " / " + source + " / " + ta.text);
//        }
//        Type = source.Type != null ? (SpawnThings)Enum.Parse(typeof(SpawnThings), source.Type) : SpawnThings.None;
//        if (source.Sprite != null) Sprite = GameManager.GetResourceSprite(source.Sprite, author);
    }
}

[System.Serializable]
public class JSONTempItem
{
    public string Text;
    public string Type;
    public string Color;
    public float Amount;
}

[System.Serializable]
public class JSONTempWeapon
{
    public string Text;
    public string Type;
    public string Color;
    public int Shots;
    public int Ammo;
    public float Damage;
    public float RateOfFire;
    public float Accuracy;
    public float Speed;
    public float Lifetime;
    public float Gravity;
    public float ExplodeRadius;
    public float ExplodeDamage;
    public float Knockback;
    public float Bounce;
    public bool SelfDamage;
}

[System.Serializable]
public class JSONTempCreator
{
    public int PointsToWin;
    public int PlayerHP;
    public float MoveSpeed;
    public float SprintSpeed;
    public float Gravity;
    public string Mode;
    public JSONTempItem[] Items;
    public JSONTempWeapon[] Weapons;
}

public static class JsonHelper
{
    public static T[] FromJson<T>(string json)
    {
        Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(json);
        return wrapper.Items;
    }

    public static string ToJson<T>(T[] array)
    {
        Wrapper<T> wrapper = new Wrapper<T>();
        wrapper.Items = array;
        return JsonUtility.ToJson(wrapper);
    }

    public static string ToJson<T>(T[] array, bool prettyPrint)
    {
        Wrapper<T> wrapper = new Wrapper<T>();
        wrapper.Items = array;
        return JsonUtility.ToJson(wrapper, prettyPrint);
    }

    [System.Serializable]
    private class Wrapper<T>
    {
        public T[] Items;
    }
}
