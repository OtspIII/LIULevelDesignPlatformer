using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Collections;
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
    
    public bool IsSetup = false;
    public NetworkVariable<FixedString64Bytes> Name = new NetworkVariable<FixedString64Bytes>();

    public void Setup(ItemSpawnController s,JSONItem data)
    {
        Data = data;
        Spawner = s;
        NO.Spawn();
        Name.Value = Data.Text;
        SetColor();
        God.LM.Spawned.Add(gameObject);
    }

    void Update()
    {
        if (!IsSetup && Name.Value != "" && God.LM?.Ruleset != null)
        {
            Data = God.LM.GetItem(Name.Value.ToString());
            SetColor();
        }
    }
    
    public void SetColor()
    {   
        IsSetup = true;
        Desc.text = GetName();
        if (Data.Color != IColors.None)
        {
            MR.material = God.Library.GetColor(Data.Color);
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
                pc.TakeHeal((int)amt);
                break;
            }
            case ItemTypes.Points:
            {
                pc.GetPoint((int)amt);
                if (Spawner.Destination != Vector3.zero)
                {
                    pc.transform.position = God.LM.transform.position + Spawner.Destination;
                    pc.SetPosClientRPC(God.LM.transform.position + Spawner.Destination);
                }
                break;
            }
            case ItemTypes.Jump:
            {
                Vector3 vel = pc.RB.velocity;
                if (Spawner.Destination == Vector3.zero)
                {
                    vel.y = amt;
                    pc.RB.velocity = vel;
                }
                else
                    pc.TakeKnockback(Spawner.Destination);
                
                
                break;
            }
            case ItemTypes.Teleport:
            {
                pc.transform.position = God.LM.transform.position + Spawner.Destination;
                pc.SetPosClientRPC(God.LM.transform.position + Spawner.Destination);
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

    public override void OnDestroy()
    {
        base.OnDestroy();
        God.LM?.Spawned.Remove(gameObject);
    }
}
