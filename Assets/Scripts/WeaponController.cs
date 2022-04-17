using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;

public class WeaponController : NetworkBehaviour
{
    public WeaponSpawnController Spawner;
    public TextMeshPro Desc;
    public NetworkObject NO;
    public JSONWeapon Data;
    public MeshRenderer MR;

    public void Setup(WeaponSpawnController s,JSONWeapon data)
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
}
