using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;

public class WeaponController : NetworkBehaviour
{
    public WeaponSpawnController Spawner;
    public TextMeshPro Desc;
    public NetworkObject NO;
    public JSONWeapon Data;
    public MeshRenderer MR;
    public bool IsSetup = false;
    public NetworkVariable<FixedString64Bytes> Name = new NetworkVariable<FixedString64Bytes>();

    public void Setup(WeaponSpawnController s,JSONWeapon data)
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
        if (!IsSetup && Name.Value != "" && God.LM?.Ruleset != null && God.LM?.Ruleset.Author != "")
        {
            
            Data = God.LM.GetWeapon(Name.Value.ToString());
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
        pc.SetWeapon(Data);
        Spawner.TakenFrom(pc);
        Destroy(gameObject);
    }

    public virtual string GetName()
    {
        if (Data != null) return Data.Text;
        return "TEST ITEM";
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
