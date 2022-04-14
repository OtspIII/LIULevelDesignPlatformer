using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Services.Lobbies.Models;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class LevelManager : MonoBehaviour
{
    public string Creator;
    public string Name;
    public JSONCreator Ruleset;
    public List<PlayerSpawnController> PSpawns;
    public List<ItemSpawnController> ISpawns;
    public PlayerSpawnController LastPS;
    public List<string> Announces;
    public Dictionary<string, JSONItem> Items = new Dictionary<string, JSONItem>();


    void Awake()
    {
        God.LM = this;
    }

    void Start()
    {
        string cr = Creator != "" ? Creator : "Misha";
        if (!God.LS.Rulesets.ContainsKey(cr)) cr = "Misha";
        Ruleset = God.LS.Rulesets[cr];
        if (Ruleset.Gravity > 0)
        {
            Physics.gravity = new Vector3(0,-9.81f,0) * Ruleset.Gravity;
        }
        foreach(JSONItem i in Ruleset.Items)
            Items.Add(i.Text,i);
        foreach(FirstPersonController pc in God.Players)
            pc.ImprintRules(Ruleset);
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
        if (PSpawns.Count == 0) return null;
        PlayerSpawnController r = PSpawns[Random.Range(0, PSpawns.Count)];
        if (LastPS != null) PSpawns.Add(LastPS);
        if(PSpawns.Count > 1) PSpawns.Remove(r);
        LastPS = r;
        return r;
    }

    public void AwardPoint(FirstPersonController who, int amt = 1, string targ="")
    {
        if (!God.RM.Scores.ContainsKey(who)) God.RM.Scores.Add(who, amt);
        else God.RM.Scores[who] += amt;
        if(God.RM.Scores[who] >= Ruleset.PointsToWin) SetWinner(who);
        string txt = who.Name.Value.ToString();
        if (targ != "") txt += " > " + targ;
        txt += " ("+God.RM.Scores[who]+")";
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
//        Debug.Log(who.Name.Value + " Wins!");
        God.LS.StartCoroutine(Winner(who));
    }
    
    public IEnumerator Winner(FirstPersonController who)
    {
        God.AnnounceText.text = who.Name.Value + " WINS!";
        yield return new WaitForSeconds(3);
        God.AnnounceText.text = "";
        God.LS.StartLevel();
    }

    public JSONItem GetItem(string n)
    {
        if (Items.ContainsKey(n)) return Items[n];
        if (n == "" && Ruleset.Items.Count > 0) return Ruleset.Items[Random.Range(0, Ruleset.Items.Count)];
        JSONTempItem r = new JSONTempItem();
        r.Text = "Useless Item";
        return new JSONItem(r);
    }
}
