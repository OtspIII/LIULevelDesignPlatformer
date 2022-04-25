using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
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
    public TrailRenderer TR;
    public bool Doomed;
    public bool IsSetup = false;
    public NetworkVariable<FixedString64Bytes> Name = new NetworkVariable<FixedString64Bytes>();
    
    public void Setup(FirstPersonController pc,JSONWeapon data)
    {
//    public float ExplodeRadius;
//    public float ExplodeDamage;
//    public bool SelfDamage;
        Data = data;
        Shooter = pc;
        NO.Spawn();
        IsSetup = true;
        RB.velocity = transform.forward * Data.Speed;
        Lifetime = Data.Lifetime > 0 ? Data.Lifetime : 10;
        Name.Value = Data.Text;
        SetColor();

//        if (Shooter.IsOwner && Data.Type != WeaponTypes.Grenade) TR.enabled = false;

    }

    void Update()
    {
        if (!IsSetup && Name.Value != "")
        {
            
            Data = God.LM.GetWeapon(Name.Value.ToString());
            SetColor();
        }
    }

    public void SetColor()
    {
        IsSetup = true;
        if (Data.Color != IColors.None)
        {
            MR.material = God.Library.GetColor(Data.Color);
            TR.material = God.Library.GetColor(Data.Color);
        }
    }

    void FixedUpdate()
    {
        if(Data.Gravity != 0)
            RB.AddForce(new Vector3(0,-9.81f,0) * Data.Gravity);
        OldVel = RB.velocity;
        Lifetime -= Time.fixedDeltaTime;
        if(Lifetime <= 0 && NetworkManager.Singleton.IsServer) 
            Explode();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!NetworkManager.Singleton.IsServer || Hit) return;
        FirstPersonController pc = other.gameObject.GetComponent<FirstPersonController>();
        if (pc == Shooter) return;
        if (pc != null && pc != Shooter)
        {
            pc.TakeDamage(Data.Damage,Shooter);
            if(Data.Knockback >0 && Data.ExplodeRadius <= 0)
                pc.TakeKnockback(transform.forward * Data.Knockback);
            Hit = true;
        }
        
        
        if(Hit)
            Explode();
        
    }

    private void OnCollisionEnter(Collision other)
    {
        if (!NetworkManager.Singleton.IsServer || Hit) return;
        
        if(Data.Type != WeaponTypes.Grenade)
            Explode();
        else if (Data.Bounce == 0)
        {
            RB.velocity = Vector3.zero;
            transform.SetParent(other.transform);
        }
        else
        {
            RB.velocity -= (OldVel - RB.velocity) * Data.Bounce;
            RB.velocity += new Vector3(0,Data.Bounce*Data.Speed/3f,0);
            RB.velocity = RB.velocity.normalized * OldVel.magnitude;
        }
    }

    public void Explode()
    {
        if (Doomed) return;
        Doomed = true;
        if (Data.ExplodeRadius > 0)
        {
            ExplosionController exp = Instantiate(God.Library.Explosion, transform.position, Quaternion.Euler(0,0,0));
            exp.Setup(Shooter,Data);
            Destroy(gameObject);
            return;
        }
        ParticleGnome partic = Hit ? God.Library.Blood : God.Library.Dust;
        ParticleGnome pg = Instantiate(partic, transform.position, Quaternion.identity);
        pg.Setup(Data.Damage);
        Destroy(gameObject);
    }

}
