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
    public bool Victory = false;
    public bool GoalExists = false;

    public TextMeshPro HPText;
    public TextMeshPro CreditsText;
    public TextMeshPro LevelText;
    public TextMeshPro AnnounceText;
    public TextMeshPro NarrateText;

    public EnemyController Prefab;
    public BulletController BPrefab;
    public List<MonsterData> MonsterDatas = new List<MonsterData>();
    public static Dictionary<string, Dictionary<MColors, MonsterData>> MonDict = new Dictionary<string, Dictionary<MColors, MonsterData>>();
    public static List<string> Creators = new List<string>();
    public static int CurrentCreator = -1;
    public static bool Setup = false;
    public List<ThingController> Tiles = new List<ThingController>();
    public static Dictionary<string,Dictionary<string,Sprite>> ResourceSprites = new Dictionary<string, Dictionary<string, Sprite>>();
    public static Dictionary<string,Dictionary<string,AudioClip>> ResourceSounds = new Dictionary<string, Dictionary<string, AudioClip>>();
    
    
    public Dictionary<SpawnThings,ThingController> Prefabs = new Dictionary<SpawnThings, ThingController>();

    public Dictionary<string,Dictionary<char,JSONData>> Datas = new Dictionary<string, Dictionary<char, JSONData>>();
    public Dictionary<string,Dictionary<char,JSONData>> Bullets = new Dictionary<string, Dictionary<char, JSONData>>();
    
    public Dictionary<string,List<ThingController>> Tags = new Dictionary<string, List<ThingController>>();
    public AudioSource AS;
    
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
        if (CheckWin() && !Paused)
        {
            Paused = true;
            Level++;
            Victory = false;
            GoalExists = false;
            SpawnLevel(Level);
        }
    }

    public bool CheckWin()
    {
        if (GoalExists) return Victory;
        return Enemies.Count <= 0;
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
        Tags.Clear();
        foreach(BulletController bc in GameObject.FindObjectsOfType<BulletController>())
            Destroy(bc.gameObject);
        
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

    public static Sprite GetResourceSprite(string label, string author)
    {
        if(!ResourceSprites.ContainsKey(author)) ResourceSprites.Add(author,new Dictionary<string, Sprite>());
        if (!ResourceSprites[author].ContainsKey(label)) ResourceSprites[author].Add(label,Resources.Load<Sprite>("Assets/"+author+"/"+label));
        return ResourceSprites[author][label];
    }
    
    public static AudioClip GetResourceSound(string label, string author)
    {
        if(!ResourceSounds.ContainsKey(author)) ResourceSounds.Add(author,new Dictionary<string, AudioClip>());
        if (!ResourceSounds[author].ContainsKey(label)) ResourceSounds[author].Add(label,Resources.Load<AudioClip>("Assets/"+author+"/"+label));
        return ResourceSounds[author][label];
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

    public JSONData GetBullet(char symbol='.')
    {
        if (!Bullets.ContainsKey(Creator) || !Bullets[Creator].ContainsKey(symbol)) return null;
        return Bullets[Creator][symbol];
    }

    public ThingController SpawnThing(char symbol, string creator, Vector3 pos)
    {
        JSONData data = GetData(symbol, Creator);
        if (data?.Type == null)
        {
            return null;
        }
        switch (data.Type)
        {
            case SpawnThings.Player:
            {
                PC.transform.position = pos;
                PC.Setup(MonDict[creator][MColors.Player]);
                PC.ApplyJSON(data);
                return PC;
            }
            case SpawnThings.Enemy:
            {
                EnemyController enemy = (EnemyController)Instantiate(Prefabs[SpawnThings.Enemy], pos, Quaternion.identity);
                if(!MonDict[creator].ContainsKey(data.Color)) Debug.Log("MISSING COLOR: " + data.Color + " / " + creator);
                enemy.Setup(MonDict[creator][data.Color]);
                enemy.ApplyJSON(data);
                return enemy;
            }
            default:
            {
                pos.z = data.Type== SpawnThings.Floor ? 20 : 10;
                ThingController thing = Instantiate(Prefabs[data.Type], pos, Quaternion.identity);
                thing.ApplyJSON(data);
                Tiles.Add(thing);
                return thing;
            }
        }
    }

    public void AddTag(string tag, ThingController who)
    {
        if(!Tags.ContainsKey(tag)) Tags.Add(tag,new List<ThingController>());
        Tags[tag].Add(who);
    }

    public void Toggle(string tag)
    {
        if(Tags.ContainsKey(tag))
            foreach(ThingController t in Tags[tag])
                t.gameObject.SetActive(!t.gameObject.activeSelf);
    }

    public void PlaySound(AudioClip ac)
    {
        AS.PlayOneShot(ac);
    }
    

}
