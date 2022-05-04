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
    public string ItemToSpawn;
    public Vector3 Destination;
    public Transform DestObj;

    void Start()
    {
        if (DestObj != null) Destination = DestObj.position - God.LM.transform.position;
        God.LM.ISpawns.Add(this);
        Spawn();
    }

    void Update()
    {
        if(Holder != null)
            Holder?.transform.Rotate(0,5,0);
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
        Vector3 where = Holder != null ? Holder.transform.position : transform.position;
        Held = Instantiate(GetPrefab(), where, Quaternion.identity);
        Held.Setup(this,God.LM.GetItem(ItemToSpawn));
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
