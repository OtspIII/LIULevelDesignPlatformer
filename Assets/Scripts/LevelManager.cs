using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : GameManager
{
    public Dictionary<string,List<List<string>>> Levels = new Dictionary<string, List<List<string>>>();
    public List<SpawnPair> PrefabPairs;

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
                else if (str.Length > 2)
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
                //Debug.Log("X: " + json.name + " / " + t.Symbol);
                JSONData data = new JSONData(t,json.name,json);
                if (data.Type == SpawnThings.Bullet)
                {
                    if(!Bullets.ContainsKey(json.name)) Bullets.Add(json.name,new Dictionary<char, JSONData>());
                    Bullets[json.name].Add(data.Symbol,data);
                }
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
        CameraController.Me.SetZoom(1);
        Tags.Clear();
        foreach(BulletController bc in GameObject.FindObjectsOfType<BulletController>())
            Destroy(bc.gameObject);
        
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
                SpawnThing(tile,chosen,pos);
                
            }
        }
        yield return new WaitForSeconds(0.5f);
        PC.Reset();
        AnnounceText.text = "";
        Paused = false;
    }

    public override IEnumerator gameOver()
    {
        
        AnnounceText.text = "GAME OVER\n\nLEVEL " + Level + "\n\n"+Creator+"\n\nHit 'x' To Continue";
        while(!Input.GetKeyDown(KeyCode.X))
            yield return null;
        PC.Reset();
        PC.HP = PC.MaxHP;
        Paused = true;
        Victory = false;
        GoalExists = false;
        SpawnLevel(Level);
        
    }
}


[System.Serializable]
public class SpawnPair
{
    public SpawnThings A;
    public ThingController B;
}