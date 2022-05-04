using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;

public class ExplosionController : NetworkBehaviour
{
    public JSONWeapon Data;
    public FirstPersonController Shooter;
    public ParticleSystem PS;
    public NetworkObject NO;
    public SphereCollider Coll;
    public bool IsSetup = false;
    public NetworkVariable<FixedString64Bytes> Name = new NetworkVariable<FixedString64Bytes>();
    
    public void Setup(FirstPersonController pc,JSONWeapon data)
    {
        Data = data;
        Shooter = pc;
        NO.Spawn();
        
        Name.Value = Data.Text;
        SetColor();
    }
    
    public void SetColor()
    {
        IsSetup = true;
        transform.localScale = Vector3.one * Data.ExplodeRadius;
        StartCoroutine(Explode());
    }
    
    void Update()
    {
        if (!IsSetup && Name.Value != "")
        {
            
            Data = God.LM.GetWeapon(Name.Value.ToString());
            SetColor();
        }
    }

    public IEnumerator Explode()
    {
        Coll.enabled = true;
        PS.Emit(Data.ExplodeDamage);
        yield return null;
        Coll.enabled = false;
        yield return new WaitForSeconds(2);
        if(IsServer) Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
//        if (!NetworkManager.Singleton.IsServer) return;
        FirstPersonController pc = other.GetComponent<FirstPersonController>();
        if (pc && pc.IsOwner)
        {
            if(Data.Knockback >0)
                pc.TakeKnockback((pc.transform.position - transform.position).normalized * Data.Knockback);
            if (pc == Shooter && !Data.SelfDamage) return;
            pc.TakeDamage(Data.ExplodeDamage,Shooter);
        }
    }
}
