using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
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
    public JSONWeapon Data;
    public MeshRenderer MR;
    public Vector3 OldVel;
    
    public void Setup(FirstPersonController pc,JSONWeapon data)
    {
//    public float ExplodeRadius;
//    public float ExplodeDamage;
//    public bool SelfDamage;
        Data = data;
        Shooter = pc;
        NO.Spawn();
        RB.velocity = transform.forward * Data.Speed;
        Lifetime = Data.Lifetime > 0 ? Data.Lifetime : 10;
        if (data.Color != IColors.None)
            MR.material = God.Library.GetColor(data.Color);
    }

    void FixedUpdate()
    {
        if(Data.Gravity != 0)
            RB.AddForce(new Vector3(0,-9.81f,0) * Data.Gravity);
        OldVel = RB.velocity;
        Lifetime -= Time.fixedDeltaTime;
        if(Lifetime <= 0 && NetworkManager.Singleton.IsServer) 
            Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision other)
    {
        if (!NetworkManager.Singleton.IsServer || Hit) return;
        FirstPersonController pc = other.gameObject.GetComponent<FirstPersonController>();
        
        if (pc != null && pc != Shooter)
        {
            pc.TakeDamage(Data.Damage,Shooter);
            if(Data.Knockback >0 && Data.ExplodeRadius <= 0)
                pc.RB.AddForce(transform.forward * Data.Knockback,ForceMode.Impulse);
            Hit = true;
        }
        
        
        if(Hit || Data.Type != WeaponTypes.Grenade)
            Destroy(gameObject);
        else if (Data.Bounce == 0)
        {
            RB.velocity = Vector3.zero;
            transform.SetParent(other.transform);
        }
        else
        {
            RB.velocity -= (OldVel - RB.velocity) * Data.Bounce;
        }
    }

    public override void OnDestroy()
    {
        base.OnDestroy();
        if (Data.ExplodeRadius > 0)
        {
            ExplosionController exp = Instantiate(God.Library.Explosion, transform.position, Quaternion.Euler(0,0,0));
            exp.Setup(Shooter,Data);
            return;
        }
        ParticleGnome partic = Hit ? God.Library.Blood : God.Library.Dust;
        ParticleGnome pg = Instantiate(partic, transform.position, Quaternion.identity);
        pg.Setup(Data.Damage);
    }
}
