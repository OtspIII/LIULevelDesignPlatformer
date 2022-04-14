using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;

public class RoundManager : NetworkBehaviour
{
    public NetworkVariable<FixedString64Bytes> Level = new NetworkVariable<FixedString64Bytes>();
    
    public Dictionary<FirstPersonController, int> Scores = new Dictionary<FirstPersonController, int>();
    public NetworkObject NO;
    
    void Awake()
    {
        God.RM = this;
    }

    public void StartLevel()
    {
        God.LS.StartLevel();
    }

    public string GetLevel()
    {
        return Level.Value.ToString();
    }
}
