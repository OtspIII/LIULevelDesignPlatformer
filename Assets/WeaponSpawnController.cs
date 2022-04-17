using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class WeaponSpawnController : MonoBehaviour
{
    public GameObject Holder;
    public WeaponController Held;
    public float RespawnTime = 15;
    float Countdown = 0;
    public string WeaponToSpawn;

    void Start()
    {
        God.LM.WSpawns.Add(this);
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
        Held.Setup(this,God.LM.GetWeapon(WeaponToSpawn));
    }
    
    public WeaponController GetPrefab()
    {
        return God.Library.WeaponSpawn;
    }

    public void TakenFrom(FirstPersonController pc)
    {
        Held = null;
        Countdown = RespawnTime;
    }
}
