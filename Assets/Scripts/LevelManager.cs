using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Unity.Netcode;
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
    public List<WeaponSpawnController> WSpawns;
    public PlayerSpawnController LastPS;
    public List<string> Announces;
    public Dictionary<string, JSONItem> Items = new Dictionary<string, JSONItem>();
    public Dictionary<string, JSONWeapon> Weapons = new Dictionary<string, JSONWeapon>();
    public List<FirstPersonController> AlivePlayers = new List<FirstPersonController>();
    public List<GameObject> Spawned;
    public bool RoundComplete;
    public Dictionary<IColors,List<FirstPersonController>> Teams = new Dictionary<IColors, List<FirstPersonController>>();


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
        foreach(JSONWeapon i in Ruleset.Weapons)
            Weapons.Add(i.Text,i);
        foreach (FirstPersonController pc in God.Players)
        {
            pc.ImprintRules(Ruleset);
            AlivePlayers.Add(pc);
        }
            
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
        if (who.Team.Value != IColors.None)
        {
            IColors team = who.Team.Value;
            if (!God.RM.TeamScores.ContainsKey(team)) God.RM.TeamScores.Add(team, amt);
            else God.RM.TeamScores[team] += amt;
            if(God.RM.TeamScores[team] >= Ruleset.PointsToWin) SetWinner(team);
            string teamtxt = who.Name.Value.ToString()  + " <"+team.ToString()+"> ";
            if (targ != "") teamtxt += " > " + targ;
            teamtxt += " ("+God.RM.TeamScores[team]+")";
            StartCoroutine(Announce(teamtxt));
            return;
        }
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
        God.LS.StartCoroutine(Winner(who.Name.Value.ToString()));
        RoundComplete = true;
    }
    
    public void SetWinner(IColors team)
    {
//        Debug.Log(who.Name.Value + " Wins!");
        God.LS.StartCoroutine(Winner(team.ToString()));
        RoundComplete = true;
    }
    
    public IEnumerator Winner(string who)
    {
        God.AnnounceText.text = who + " WINS!";
        if (NetworkManager.Singleton.IsServer)
            God.LS.PickNextLevel();
        yield return new WaitForSeconds(3);
        God.RM.Scores.Clear();
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

    public JSONWeapon GetWeapon(string n)
    {
        if (Weapons.ContainsKey(n)) return Weapons[n];
        if (Ruleset.Weapons.Count > 0) return Ruleset.Weapons[0];
        JSONTempWeapon wpn = new JSONTempWeapon();
        wpn.Damage = 10;
        wpn.Text = "GENERIC WEAPON";
        return new JSONWeapon(wpn);
    }

    public bool Respawn(FirstPersonController pc)
    {
        if (Ruleset.Mode == GameModes.Elim) return false;
        return true;
    }

    private void OnDestroy()
    {
        if(NetworkManager.Singleton.IsServer)
            foreach(GameObject obj in Spawned)
                Destroy(obj);
    }

    public void NoticeDeath(FirstPersonController pc,FirstPersonController source=null)
    {
        if(source != null && Ruleset.Mode == GameModes.Deathmatch)AwardPoint(source,1);
        AlivePlayers.Remove(pc);
        if (Ruleset.Mode == GameModes.Elim)
        {
            if (AlivePlayers.Count == 1)
            {
                FirstPersonController winner = AlivePlayers[0];
                AwardPoint(winner);
                if (RoundComplete) return;
            }
            if (AlivePlayers.Count <= 1)
            {
                AlivePlayers.Clear();
                foreach (FirstPersonController dead in God.Players)
                {
                    dead.Reset();
                    AlivePlayers.Add(dead);
                }
            }
        }
    }

    public IColors PickTeam(FirstPersonController pc)
    {
        if (Ruleset.Teams.Count <= 1) return IColors.None;
        int amt = 999;
        IColors best = IColors.None;
        foreach (IColors c in Ruleset.Teams)
        {
            if(!Teams.ContainsKey(c)) Teams.Add(c,new List<FirstPersonController>());
            if (Teams[c].Contains(pc)) return c;
            int mem = Teams[c].Count;
            if (mem < amt)
            {
                best = c;
                amt = mem;
            }
        }

        if (Teams.ContainsKey(best))
            Teams[best].Add(pc);
        return best;
    }

    public void RemovePlayer(FirstPersonController pc)
    {
        AlivePlayers.Remove(pc);
        foreach (IColors c in Teams.Keys)
            Teams[c].Remove(pc);
    }
}
