using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class ParticleGnome : NetworkBehaviour
{
    public NetworkObject NO;
    public ParticleSystem PS;

    public void Setup(int amt)
    {
        PS.Emit(amt);
        NO.Spawn();
        Invoke("TimeUp",1);
    }

    public void TimeUp()
    {
        Destroy(gameObject);
    } 
}
