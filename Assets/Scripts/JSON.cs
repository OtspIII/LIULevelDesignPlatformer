using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
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
    public float Size;
    public float Size2;
    public char Bullet = '.';
    public char Drop = ' ';
    public string Tag = "";
    public string Toggle = "";
    public Targets Target;
    public int Layer = -1;

    public JSONData(JSONTemp source,string author,TextAsset ta)
    {
        if (source.Symbol == null)
        {
            Debug.Log("JSON CRASH: " + author + " / " + source + " / " + ta.text);
        }
        Symbol = source.Symbol.Length > 0 ? source.Symbol[0] : ' ';
        Type = source.Type != null ? (SpawnThings)Enum.Parse(typeof(SpawnThings), source.Type) : SpawnThings.None;
        Color = source.Color != null ? (MColors)Enum.Parse(typeof(MColors), source.Color) : MColors.None;
        if (source.Sprite != null) Sprite = GameManager.GetResourceSprite(source.Sprite, author);
        if (source.Audio != null) Audio = Resources.Load<AudioClip>("Assets/"+author+"/"+source.Audio);
        if (source.Text != null) Text = source.Text;
        if (source.Tag != null) Tag = source.Tag;
        if (source.Toggle != null) Toggle = source.Toggle;
        Amount = source.Amount;
        Size = source.Size;
        Size2 = source.Size2;
        if (source.Bullet != null)
            Bullet = source.Bullet.Length > 0 ? source.Bullet[0] : '.';
        if (source.Drop != null)
            Drop = source.Drop.Length > 0 ? source.Drop[0] : ' ';
        Target = source.Target != null ? (Targets)Enum.Parse(typeof(Targets), source.Target) : Targets.None;
        if (source.Layer != null) Layer = LayerMask.NameToLayer(source.Layer);
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
    public float Size;
    public float Size2;
    public string Bullet;
    public string Drop;
    public string Tag;
    public string Toggle;
    public string Target;
    public string Layer;
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

public enum Targets
{
    None,
    All,
    Player,
    Enemies,
    Others
}