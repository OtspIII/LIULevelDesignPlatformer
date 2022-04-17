using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class ExplosionController : NetworkBehaviour
{
    public JSONWeapon Data;
    public FirstPersonController Shooter;
    public ParticleSystem PS;
    public NetworkObject NO;
    public SphereCollider Coll;
    
    public void Setup(FirstPersonController pc,JSONWeapon data)
    {
        Data = data;
        Shooter = pc;
        NO.Spawn();
        transform.localScale = Vector3.one * Data.ExplodeRadius;
        StartCoroutine(Explode());
    }

    public IEnumerator Explode()
    {
        Coll.enabled = true;
        PS.Emit(Data.ExplodeDamage);
        yield return null;
        Coll.enabled = false;
        yield return new WaitForSeconds(2);
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!NetworkManager.Singleton.IsServer) return;
        FirstPersonController pc = other.GetComponent<FirstPersonController>();
        if (pc)
        {
           
            if(Data.Knockback >0)
                pc.TakeKnockback((pc.transform.position - transform.position).normalized * Data.Knockback);
            if (pc == Shooter && !Data.SelfDamage) return;
            pc.TakeDamage(Data.ExplodeDamage,Shooter);
        }
    }
}
