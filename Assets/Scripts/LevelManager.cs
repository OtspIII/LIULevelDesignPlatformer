﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : GameManager
{
    public Dictionary<string,List<List<string>>> Levels = new Dictionary<string, List<List<string>>>();
    public List<SpawnPair> PrefabPairs;
    public Dictionary<SpawnThings,ThingController> Prefabs = new Dictionary<SpawnThings, ThingController>();

    public Dictionary<string,Dictionary<char,JSONData>> Datas = new Dictionary<string, Dictionary<char, JSONData>>();
//    public GameObject WallPrefab;
//    public GameObject FloorPrefab;
//    public GameObject LavaPrefab;
//    public GameObject UBeltPrefab;
//    public GameObject DBeltPrefab;
//    public GameObject LBeltPrefab;
//    public GameObject RBeltPrefab;
//    public GameObject Key1Prefab;
//    public GameObject Door1Prefab;
//    public GameObject Key2Prefab;
//    public GameObject Door2Prefab;
//    public GameObject Key3Prefab;
//    public GameObject Door3Prefab;
    
    protected override void LoadAssets()
    {
        foreach (SpawnPair sp in PrefabPairs)
            Prefabs.Add(sp.A,sp.B);
        GameManager.LevelMode = true;
        base.LoadAssets();
        TextAsset[] levels = Resources.LoadAll<TextAsset>("Levels");
        foreach (TextAsset ta in levels)
        {
            Levels.Add(ta.name,new List<List<string>>());
            string[] lvlStr = ta.text.Split('\n');
            List<string> current = new List<string>();
            foreach (string str in lvlStr)
            {
                if (str.Length <= 2 && current.Count > 0)
                {
                    Levels[ta.name].Add(current);
                    current = new List<string>();
                }
                else
                    current.Add(str);
            }
            if (current.Count > 0)
            {
                Levels[ta.name].Add(current);
            }
        }

        TextAsset[] jsons = Resources.LoadAll<TextAsset>("JSON");
        foreach (TextAsset json in jsons)
        {
            Datas.Add(json.name,new Dictionary<char, JSONData>());
            JSONTemp[] j = JsonHelper.FromJson<JSONTemp>(json.text);
            foreach (JSONTemp t in j)
            {
                JSONData data = new JSONData(t,json.name);
                Datas[json.name].Add(data.Symbol,data);
            }
        }
    }

    public override void PickCreator()
    {
        
    }

    public List<string> GetLevel(string who, int lvl)
    {
        if (!Levels.ContainsKey(who) || Levels[who].Count <= lvl - 1) return null;
        return Levels[who][lvl-1];
    }

    public override void SpawnLevel(int level)
    {
        StartCoroutine(LevelBuild(level));
    }

    public IEnumerator LevelBuild(int level)
    {
        foreach (ThingController tile in Tiles)
        {
            Destroy(tile.gameObject);
        }
        Tiles.Clear();
        PC.Reset();
        foreach(EnemyController e in AllEnemies)
            e.Reset();
        
        if (level > 1)
            yield return new WaitForSeconds(0.5f);
        AnnounceText.text = "LEVEL " + level + "\n\n"+Creator;
        LevelText.text = "Level " + level;
        
        string[] creators = Levels.Keys.ToArray();
        string chosen = Creator != "" ? Creator : creators[Random.Range(0, creators.Length)];
        CreditsText.text = chosen;
        List<string> lvl = GetLevel(chosen,level);
        if (lvl == null)
        {
            SceneManager.LoadScene("YouWin");
            yield break;
        }

        for (int y = 0; y < lvl.Count; y++)
        {
            for (int x = 0; x < lvl[y].Length; x++)
            {
                Vector3 pos = new Vector3(x,-y,0);
                char tile = lvl[y][x];
                JSONData data = GetData(tile, Creator);
                if (data?.Type == null)
                {
                    continue;
                }
                switch (data.Type)
                {
                    case SpawnThings.Player:
                    {
                        PC.transform.position = pos;
                        PC.Setup(MonDict[chosen][MColors.Player]);
                        break;
                    }
                    case SpawnThings.Enemy:
                    {
                        EnemyController enemy = (EnemyController)Instantiate(Prefabs[SpawnThings.Enemy], pos, Quaternion.identity);
                        enemy.Setup(MonDict[chosen][data.Color]);
                        break;
                    }
                    default:
                    {
                        pos.z = data.Type== SpawnThings.Floor ? 20 : 10;
                        Tiles.Add(Instantiate(Prefabs[data.Type], pos, Quaternion.identity));
                        break;
                    }
                }
            }
        }
        yield return new WaitForSeconds(0.5f);
        PC.Reset();
        AnnounceText.text = "";
        Paused = false;
    }
    
    public JSONData GetData(char symbol, string author="Misha")
    {
        if (!Datas.ContainsKey(author)) author = "Misha";
        if (!Datas[author].ContainsKey(symbol))
        {
            if(Datas["Misha"].ContainsKey(symbol))
                return Datas["Misha"][symbol];
            return null;
        }
        return Datas[author][symbol];
    }
}


[System.Serializable]
public class SpawnPair
{
    public SpawnThings A;
    public ThingController B;
}