using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class JSONData
{
    public char Symbol;
    public SpawnThings Type;
    public MColors Color;
    public Sprite Sprite;
    public AudioClip Audio;
    public string Text = "";
    public float Amount;

    public JSONData(JSONTemp source,string author)
    {
        Symbol = source.Symbol.Length > 0 ? source.Symbol[0] : ' ';
        Type = source.Type != null ? (SpawnThings)Enum.Parse(typeof(SpawnThings), source.Type) : SpawnThings.None;
        Color = source.Color != null ? (MColors)Enum.Parse(typeof(MColors), source.Color) : MColors.None;
        if (source.Sprite != null) Sprite = GameManager.GetResourceSprite(source.Sprite, author);
        if (source.Audio != null) Audio = Resources.Load<AudioClip>("Assets/"+author+"/"+source.Audio);
        if (source.Text != null) Text = source.Text;
        Amount = source.Amount;
    }
}

[System.Serializable]
public class JSONTemp
{
    public string Symbol;
    public string Type;
    public string Color;
    public string Sprite;
    public string Audio;
    public string Text;
    public float Amount;
}

[System.Serializable]
public class JSONList
{
    public JSONTemp[] List;
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