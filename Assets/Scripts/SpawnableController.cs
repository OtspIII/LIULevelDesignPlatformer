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
    public JSONItem Data;
    public MeshRenderer MR;

    public void Setup(ItemSpawnController s,JSONItem data)
    {
        Data = data;
        Spawner = s;
        Desc.text = GetName();
        NO.Spawn();
        if (data.Color != IColors.None)
            MR.material = God.Library.GetColor(data.Color);
    }

    void Update()
    {
        
    }

    public void GetTaken(FirstPersonController pc)
    {
        TakeEffects(pc);
        Spawner.TakenFrom(pc);
        Destroy(gameObject);
    }

    public virtual string GetName()
    {
        if (Data != null) return Data.Text;
        return "TEST ITEM";
    }

    public virtual void TakeEffects(FirstPersonController pc)
    {
        float amt = Data.Amount > 0 ? Data.Amount : 1;
        switch (Data.Type)
        {
            case ItemTypes.Healing:
            {
                pc.TakeDamage(-(int)amt);
                break;
            }
            case ItemTypes.Points:
            {
                pc.GetPoint((int)amt);
                break;
            }
            case ItemTypes.Jump:
            {
                Vector3 vel = pc.RB.velocity;
                vel.y = amt;
                pc.RB.velocity = vel;
                break;
            }
        }
//        Debug.Log("TOOK IT: " + gameObject.name);
        
    }

    void OnTriggerEnter(Collider other)
    {
        if (!NetworkManager.Singleton.IsServer) return;
        FirstPersonController pc = other.gameObject.GetComponent<FirstPersonController>();
//        Debug.Log("OCE: " + pc + " / " + other.gameObject);
        if(pc != null)
            GetTaken(pc);
    }
}
