using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using Unity.Netcode.Components;

public class ProjectileController : NetworkBehaviour
{
    public Rigidbody RB;
    public NetworkObject NO;
    public float Lifetime = 10;
    public FirstPersonController Shooter;
    public bool Hit = false;
    
    public void Setup(FirstPersonController pc)
    {
        Shooter = pc;
        NO.Spawn();
        RB.velocity = transform.forward * 50;
    }

    void Update()
    {
        Lifetime -= Time.deltaTime;
        if(Lifetime <= 0 && NetworkManager.Singleton.IsServer) 
            Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision other)
    {
        if (!NetworkManager.Singleton.IsServer || Hit) return;
        FirstPersonController pc = other.gameObject.GetComponent<FirstPersonController>();
        ParticleGnome partic = God.Library.Dust;
        if (pc != null && pc != Shooter)
        {
            pc.TakeDamage(10,Shooter);
            partic = God.Library.Blood;
            Hit = true;
        }
        
        ParticleGnome pg = Instantiate(partic, transform.position, Quaternion.identity);
        pg.Setup(10);
        Destroy(gameObject);
    }
}
