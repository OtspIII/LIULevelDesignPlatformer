using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    public static GameManager Me;
    public static bool LevelMode = false;
    public string Creator;
    public bool Paused = true;
    public PlayerController PC;
    public List<EnemyController> Enemies;
    public List<EnemyController> AllEnemies;
    public int Level = 1;

    public TextMeshPro HPText;
    public TextMeshPro CreditsText;
    public TextMeshPro LevelText;
    public TextMeshPro AnnounceText;

    public EnemyController Prefab;
    public BulletController BPrefab;
    public List<MonsterData> MonsterDatas = new List<MonsterData>();
    public static Dictionary<string, Dictionary<MColors, MonsterData>> MonDict = new Dictionary<string, Dictionary<MColors, MonsterData>>();
    public static List<string> Creators = new List<string>();
    public static int CurrentCreator = -1;
    public static bool Setup = false;
    public List<ThingController> Tiles = new List<ThingController>();
    
    void Awake()
    {
        GameManager.Me = this;
        if (!Setup)
        {
            LoadAssets();
            Setup = true;
        }

        PickCreator();
    }

    public virtual void PickCreator()
    {
        if (Creator == "")
        {
            CurrentCreator++;
            if (CurrentCreator >= Creators.Count)
                CurrentCreator = 0;
            Creator = Creators[CurrentCreator];
        }
    }

    protected virtual void LoadAssets()
    {
        TextAsset monData = Resources.Load<TextAsset>("Game Data - Monsters");
        string[] monStr = monData.text.Split('\n');

        for (int i = 1; i < monStr.Length; i++)
        {
            string[] mon = monStr[i].Split(',');
            MonsterData d = new MonsterData(mon);
            AddMonster(d);
        }
        Creators.Sort();
    }

    void Start()
    {
        SpawnLevel(Level);
    }

    void Update()
    {
        if (Enemies.Count <= 0 && !Paused)
        {
            Paused = true;
            Level++;
            SpawnLevel(Level);
        }
    }

    void AddMonster(MonsterData mon)
    {
        MonsterDatas.Add(mon);
        if (!MonDict.ContainsKey(mon.Creator))
        {
            MonDict.Add(mon.Creator,new Dictionary<MColors, MonsterData>());
            Creators.Add(mon.Creator);
        }
        MonDict[mon.Creator].Add(mon.Color,mon);
    }

    public virtual void SpawnLevel(int level)
    {
        StartCoroutine(SpawnMonsters(level));
    }
    
    public IEnumerator SpawnMonsters(int level)
    {
        PC.Reset();
        foreach(EnemyController e in AllEnemies)
            e.Reset();
        
        if (level > 1)
            yield return new WaitForSeconds(0.5f);
        AnnounceText.text = "LEVEL " + level + "\n\n"+Creator;
        LevelText.text = "Level " + level;
        List<Vector2> Spawns = new List<Vector2>();
        for(float x= -8;x <= 8;x+=1f)
        for (float y = -4; y <= 4; y += 1f)
        {
            Vector2 slot = new Vector2(x, y);
            if (Vector2.Distance(slot,PC.transform.position) > 2.5f)
                Spawns.Add(slot);
        }

        string[] creators = MonDict.Keys.ToArray();
        string chosen = Creator != "" ? Creator : creators[Random.Range(0, creators.Length)];
        CreditsText.text = chosen;
        List<MColors> colors = new List<MColors>();
        foreach (MColors c in MonDict[chosen].Keys)
        {
            if (MonDict[chosen][c].MinLevel > level) continue;
            colors.Add(c);
        }
//        colors.AddRange(MonDict[chosen].Keys.ToArray());
        colors.Remove(MColors.Player);
        List<MColors> options = new List<MColors>();
        options.AddRange(colors);
        PC.Setup(MonDict[chosen][MColors.Player]);
        float budget = GameSettings.BaseBudget + (GameSettings.ExtraBudget * level);
        int safety = 99;
        while (budget > 0 && safety > 0)
        {
            safety--;
            MColors chosenC = colors[Random.Range(0, colors.Count)];
            options.Remove(chosenC);
            options.AddRange(colors);
            if (Spawns.Count == 0) Spawns.Add(new Vector2(Random.Range(-8f,8f),3));
            Vector3 where = Spawns[Random.Range(0, Spawns.Count)];
            if (MonDict[chosen][chosenC].Cost > budget) continue;
            Spawns.Remove(where);
            EnemyController enemy = Instantiate(Prefab, where, Quaternion.identity);
            enemy.Setup(MonDict[chosen][chosenC]);
            budget -= MonDict[chosen][chosenC].Cost;
            yield return new WaitForSeconds(0.1f);
        }
        yield return new WaitForSeconds(0.5f);
        PC.Reset();
        AnnounceText.text = "";
        Paused = false;
    }

    public void GameOver()
    {
        StartCoroutine(gameOver());
    }

    public IEnumerator gameOver()
    {
        PC.Reset();
        AnnounceText.text = "GAME OVER\n\nLEVEL " + Level + "\n\n"+Creator+"\n\nHit 'x' To Continue";
        while(!Input.GetKeyDown(KeyCode.X))
            yield return null;
        SceneManager.LoadScene("Gameplay");
    }

}
