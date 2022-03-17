using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : GameManager
{
    public Dictionary<string,List<List<string>>> Levels = new Dictionary<string, List<List<string>>>();
    public GameObject WallPrefab;
    public GameObject FloorPrefab;
    public GameObject LavaPrefab;
    public GameObject UBeltPrefab;
    public GameObject DBeltPrefab;
    public GameObject LBeltPrefab;
    public GameObject RBeltPrefab;
    public List<GameObject> Tiles = new List<GameObject>();
    
    protected override void LoadAssets()
    {
        GameManager.LevelMode = true;
        base.LoadAssets();
        TextAsset[] levels = Resources.LoadAll<TextAsset>("Levels");
        Debug.Log("LVLS: " + levels.Length);
        foreach (TextAsset ta in levels)
        {
            Debug.Log("NAME: " + ta.name);
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
    }

    public override void PickCreator()
    {
        
    }

    public List<string> GetLevel(string who, int lvl)
    {
        Debug.Log("GETLVL: " + lvl + " / " + who + " / " + Levels.ContainsKey(who));
        if (!Levels.ContainsKey(who) || Levels[who].Count <= lvl - 1) return null;
        return Levels[who][lvl-1];
    }

    public override void SpawnLevel(int level)
    {
        StartCoroutine(LevelBuild(level));
    }

    public IEnumerator LevelBuild(int level)
    {
        foreach (GameObject tile in Tiles)
        {
            Destroy(tile);
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
            //Debug.Log("Y" +y+": " + lvl[y]);
            for (int x = 0; x < lvl[y].Length; x++)
            {
                Vector3 pos = new Vector3(x,-y,0);
                char tile = lvl[y][x];
                MColors mon = MColors.None;
                switch (tile)
                {
                    case 'x':
                    {
                        pos.z = 10;
                        Tiles.Add(Instantiate(WallPrefab, pos, Quaternion.identity));
                        break;
                    }
                    case 'l':
                    {
                        pos.z = 10;
                        Tiles.Add(Instantiate(LavaPrefab, pos, Quaternion.identity));
                        break;
                    }
                    case '<':
                    {
                        pos.z = 10;
                        Tiles.Add(Instantiate(LBeltPrefab, pos, Quaternion.identity));
                        break;
                    }
                    case '>':
                    {
                        pos.z = 10;
                        Tiles.Add(Instantiate(RBeltPrefab, pos, Quaternion.identity));
                        break;
                    }
                    case 'v':
                    {
                        pos.z = 10;
                        Tiles.Add(Instantiate(DBeltPrefab, pos, Quaternion.identity));
                        break;
                    }
                    case '^':
                    {
                        pos.z = 10;
                        Tiles.Add(Instantiate(UBeltPrefab, pos, Quaternion.identity));
                        break;
                    }
//                    case ' ':
//                    {
//                        pos.z = 20;
//                        Tiles.Add(Instantiate(FloorPrefab, pos, Quaternion.identity));
//                        break;
//                    }
                    case 'P':
                    {
                        PC.transform.position = pos;
                        PC.Setup(MonDict[chosen][MColors.Player]);
                        break;
                    }
                    case 'R':mon = MColors.Red;break;
                    case 'G':mon = MColors.Green;break;
                    case 'Y':mon = MColors.Yellow;break;
                    case 'I':mon = MColors.Pink;break;
                    case 'U':mon = MColors.Purple;break;
                    case 'B':mon = MColors.Blue;break;
                    case 'O':mon = MColors.Orange;break;
                    case 'W':mon = MColors.White;break;
                }

                if (mon != MColors.None)
                {
                    EnemyController enemy = Instantiate(Prefab, pos, Quaternion.identity);
                    enemy.Setup(MonDict[chosen][mon]);
                }
            }
        }
        
//        List<MColors> colors = new List<MColors>();
//        foreach (MColors c in MonDict[chosen].Keys)
//        {
//            if (MonDict[chosen][c].MinLevel > level) continue;
//            colors.Add(c);
//        }
////        colors.AddRange(MonDict[chosen].Keys.ToArray());
//        colors.Remove(MColors.Player);
//        List<MColors> options = new List<MColors>();
//        options.AddRange(colors);
//        PC.Setup(MonDict[chosen][MColors.Player]);
//        float budget = GameSettings.BaseBudget + (GameSettings.ExtraBudget * level);
//        int safety = 99;
//        while (budget > 0 && safety > 0)
//        {
//            safety--;
//            MColors chosenC = colors[Random.Range(0, colors.Count)];
//            options.Remove(chosenC);
//            options.AddRange(colors);
//            if (Spawns.Count == 0) Spawns.Add(new Vector2(Random.Range(-8f,8f),3));
//            Vector3 where = Spawns[Random.Range(0, Spawns.Count)];
//            if (MonDict[chosen][chosenC].Cost > budget) continue;
//            Spawns.Remove(where);
//            EnemyController enemy = Instantiate(Prefab, where, Quaternion.identity);
//            enemy.Setup(MonDict[chosen][chosenC]);
//            budget -= MonDict[chosen][chosenC].Cost;
//            yield return new WaitForSeconds(0.1f);
//        }
        yield return new WaitForSeconds(0.5f);
        PC.Reset();
        AnnounceText.text = "";
        Paused = false;
    }
}
