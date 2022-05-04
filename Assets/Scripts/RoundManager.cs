using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;

public class RoundManager : NetworkBehaviour
{
    public NetworkVariable<FixedString64Bytes> Level = new NetworkVariable<FixedString64Bytes>();
    
    public Dictionary<FirstPersonController, int> Scores = new Dictionary<FirstPersonController, int>();
    public Dictionary<IColors, int> TeamScores = new Dictionary<IColors, int>();
    public NetworkObject NO;
    
    void Awake()
    {
        God.RM = this;
    }

    public void StartLevel()
    {
        
        God.LS.StartLevel();
//        Debug.Log("SL: " + Level.Value);
        if (IsServer)
        {
            StartLevelClientRPC();
        }
    }

    [ClientRpc]
    void StartLevelClientRPC()
    {
//        Debug.Log("SLCRPC: " + IsServer + " / " + Level.Value);
        if(!IsServer)
            God.LS.StartLevel(0.5f);
    }

    public string GetLevel()
    {
        return Level.Value.ToString();
    }

    [ClientRpc]
    public void AlertClientRPC(string txt,bool big=false)
    {
        if (IsServer) return;
        if(!big)
            God.LM?.StartCoroutine(God.LM?.Announce(txt));
        else
            God.LS.StartCoroutine(God.LM?.Winner(txt));
    }
}
