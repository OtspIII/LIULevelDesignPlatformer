using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using Unity.Netcode;

public class ItemSpawnController : MonoBehaviour
{
    public GameObject Holder;
    public SpawnableController Held;
    public float RespawnTime = 15;
    float Countdown = 0;

    void Start()
    {
        God.LM.ISpawns.Add(this);
        Spawn();
    }

    void Update()
    {
        Holder.transform.Rotate(0,5,0);
        if (!NetworkManager.Singleton.IsServer) return;
        if (Held != null) return;
        Countdown -= Time.deltaTime;
        if (Countdown <= 0)
        {
            Spawn();
        }
    }

    public void Spawn()
    {
        if (!NetworkManager.Singleton.IsServer) return;
        Countdown = RespawnTime;
        Held = Instantiate(GetPrefab(), Holder.transform.position, Quaternion.identity);
        Held.Setup(this);
    }
    
    public SpawnableController GetPrefab()
    {
        return God.Library.TestSpawn;
    }

    public void TakenFrom(FirstPersonController pc)
    {
        Held = null;
        Countdown = RespawnTime;
    }
}
