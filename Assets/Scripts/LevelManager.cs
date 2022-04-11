using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Services.Lobbies.Models;
using UnityEngine;
using Random = UnityEngine.Random;

public class LevelManager : MonoBehaviour
{
    public List<PlayerSpawnController> PSpawns;
    public List<ItemSpawnController> ISpawns;
    public PlayerSpawnController LastPS;
    public List<string> Announces;

    public Dictionary<FirstPersonController, int> Scores = new Dictionary<FirstPersonController, int>();
    public int ToWin = 5;

    void Awake()
    {
        God.LM = this;
    }

    void Update()
    {
        string txt = "";
        foreach(string a in Announces)
        {
            if (txt != "") txt += "\n";
            txt += a;
        }
        God.UpdateText.text = txt;
    }

    public PlayerSpawnController GetPSpawn(FirstPersonController pc)
    {
        PlayerSpawnController r = PSpawns[Random.Range(0, PSpawns.Count)];
        if (LastPS != null) PSpawns.Add(LastPS);
        if(PSpawns.Count > 1) PSpawns.Remove(r);
        LastPS = r;
        return r;
    }

    public void AwardPoint(FirstPersonController who, int amt = 1, string targ="")
    {
        if (!Scores.ContainsKey(who)) Scores.Add(who, amt);
        else Scores[who] += amt;
        if(Scores[who] >= ToWin) SetWinner(who);
        string txt = who.Name.Value.ToString();
        if (targ != "") txt += " > " + targ;
        txt += " ("+Scores[who]+")";
        StartCoroutine(Announce(txt));
    }

    public IEnumerator Announce(string txt)
    {
        Announces.Add(txt);
        yield return new WaitForSeconds(3);
        Announces.Remove(txt);
    }

    public void SetWinner(FirstPersonController who)
    {
        Debug.Log(who.Name.Value + " Wins!");
        God.LS.StartCoroutine(Winner(who));

    }
    
    public IEnumerator Winner(FirstPersonController who)
    {
        God.AnnounceText.text = who.Name.Value + " WINS!";
        yield return new WaitForSeconds(3);
        God.AnnounceText.text = "";
        God.LS.StartLevel();
    }
}
