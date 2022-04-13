using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class JSONItem
{
    public string Text = "";
    public float Amount;
    

    public JSONItem(JSONTempItem source)
    {
//        if (source.Symbol == null)
//        {
//            Debug.Log("JSON CRASH: " + author + " / " + source + " / " + ta.text);
//        }
//        Type = source.Type != null ? (SpawnThings)Enum.Parse(typeof(SpawnThings), source.Type) : SpawnThings.None;
//        if (source.Sprite != null) Sprite = GameManager.GetResourceSprite(source.Sprite, author);
    }
}

[System.Serializable]
public class JSONCreator
{
    public int PointsToWin;
    public int PlayerHP;
    public List<JSONItem> Items;
    

    public JSONCreator(JSONTempCreator source,string author,TextAsset ta)
    {
        PointsToWin = source.PointsToWin;
        PlayerHP = source.PlayerHP;
        foreach(JSONTempItem i in source.Items)
            Items.Add(new JSONItem(i));

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
    public float Amount;
}

[System.Serializable]
public class JSONTempCreator
{
    public int PointsToWin;
    public int PlayerHP;
    public JSONTempItem[] Items;
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
