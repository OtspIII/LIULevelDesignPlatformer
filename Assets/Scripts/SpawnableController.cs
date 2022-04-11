using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Unity.Netcode;
using Unity.Netcode.Components;

public class SpawnableController : NetworkBehaviour
{
    public ItemSpawnController Spawner;
    public TextMeshPro Desc;
    public NetworkObject NO;
    
    public void Setup(ItemSpawnController s)
    {
        Spawner = s;
        Desc.text = GetName();
        NO.Spawn();
    }

    void Update()
    {
        if (God.Camera != null)
        {
            Desc.transform.LookAt(God.Camera.transform.position);
            Desc.transform.Rotate(0,180,0);
        }
    }

    public void GetTaken(FirstPersonController pc)
    {
        TakeEffects(pc);
        Spawner.TakenFrom(pc);
        Destroy(gameObject);
    }

    public virtual string GetName()
    {
        return "TEST ITEM";
    }

    public virtual void TakeEffects(FirstPersonController pc)
    {
        Debug.Log("TOOK IT: " + gameObject.name);
        pc.GetPoint(1);
    }

    void OnTriggerEnter(Collider other)
    {
        if (!NetworkManager.Singleton.IsServer) return;
        FirstPersonController pc = other.gameObject.GetComponent<FirstPersonController>();
        Debug.Log("OCE: " + pc + " / " + other.gameObject);
        if(pc != null)
            GetTaken(pc);
    }
}
