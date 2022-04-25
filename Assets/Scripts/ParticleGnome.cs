using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class ParticleGnome : NetworkBehaviour
{
    public NetworkObject NO;
    public ParticleSystem PS;
    public bool IsSetup = false;
    public NetworkVariable<int> Amount = new NetworkVariable<int>();

    void Update()
    {
        if (!IsSetup && Amount.Value > 0)
        {
            IsSetup = true;
            PS.Emit(Amount.Value);
        }
    }

    
    public void Setup(int amt)
    {
        PS.Emit(amt);
        NO.Spawn();
        Invoke("TimeUp",1);
        Amount.Value = amt;
    }

    public void TimeUp()
    {
        Destroy(gameObject);
    } 
}
