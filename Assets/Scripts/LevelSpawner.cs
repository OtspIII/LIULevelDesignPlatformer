using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class LevelSpawner : MonoBehaviour
{
    public Dictionary<string,JSONCreator> Rulesets = new Dictionary<string, JSONCreator>();
    public Dictionary<string, LevelManager> Levels = new Dictionary<string, LevelManager>();
    public LevelManager Current;
    
    void Awake()
    {
        
        foreach (LevelManager s in Resources.LoadAll<LevelManager>("Levels"))
            Levels.Add(s.name,s);
        God.LS = this;
        TextAsset[] jsons = Resources.LoadAll<TextAsset>("JSON");
        foreach (TextAsset json in jsons)
        {
            try
            {
                JSONTempCreator cr = JsonUtility.FromJson<JSONTempCreator>(json.text);
                JSONCreator c = new JSONCreator(cr,json.name,json);
                Rulesets.Add(json.name,c);
            }
            catch (Exception e)
            {
                Debug.Log(e);
                Debug.Log("INVALID JSON: " + json.name);
                throw;
            }
            
        }
    }

//    void Start()
//    {
//        StartLevel();
//    }

    public void StartLevel(float delay=0)
    {
        StartCoroutine(LoadScene(GetLevel(),delay));
    }

    public LevelManager GetLevel()
    {
        if (God.TestLevel != null && NetworkManager.Singleton.IsServer) return God.TestLevel;
        if (God.RM != null)
        {
            string lvl = God.RM.GetLevel();
            if (Levels.ContainsKey(lvl)) return Levels[lvl];
        }
        if(!NetworkManager.Singleton.IsServer)
            Debug.Log("INVALID LEVEL SPAWN");
        LevelManager[] all = Levels.Values.ToArray();
        return all[Random.Range(0, all.Length)];
    }

    public IEnumerator LoadScene(LevelManager who,float delay)
    {
        if(delay > 0) yield return new WaitForSeconds(delay);
        //Debug.Log("FOR THREAD");
        if(Current != null) Destroy(Current.gameObject);
        Current = Instantiate(who);
        Current.Name = who.name;
        if(NetworkManager.Singleton.IsServer)
            God.RM.Level.Value = who.name;
        yield return null;
        foreach(FirstPersonController pc in God.Players)
            pc.Reset();
    }

    public void PickNextLevel()
    {
        LevelManager[] all = Levels.Values.ToArray();
        LevelManager chosen=  all[Random.Range(0, all.Length)];
        God.RM.Level.Value = chosen.name;
    }
}
