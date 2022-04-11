using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class LevelSpawner : MonoBehaviour
{
    public Dictionary<string, LevelManager> Levels = new Dictionary<string, LevelManager>();
    public LevelManager Current;
    
    void Awake()
    {
        foreach (LevelManager s in Resources.LoadAll<LevelManager>("Levels"))
            Levels.Add(s.name,s);
        God.LS = this;
    }

    void Start()
    {
        StartLevel();
    }

    public void StartLevel()
    {
        StartCoroutine(LoadScene(GetRandomLevel()));
    }

    public LevelManager GetRandomLevel()
    {
        LevelManager[] all = Levels.Values.ToArray();
        return all[Random.Range(0, all.Length)];
    }

    public IEnumerator LoadScene(LevelManager who)
    {
        if(Current != null) Destroy(Current.gameObject);
        Current = Instantiate(who);
        yield return null;
        foreach(FirstPersonController pc in God.Players)
            pc.Reset();
    }
}
