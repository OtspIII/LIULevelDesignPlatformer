using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using Random = UnityEngine.Random;

public class TouchController : MonoBehaviour
{
    public TouchThings Type;
    public float Amount;
    public float KB = 10;

    private void OnCollisionEnter(Collision other)
    {
        if (!NetworkManager.Singleton.IsServer) return;
        FirstPersonController pc = other.gameObject.GetComponent<FirstPersonController>();
        if (pc == null) return;
        switch (Type)
        {
            case TouchThings.Lava:
            {
                pc.TakeDamage((int)Amount);
                Vector3 kb = (pc.transform.position - transform.position).normalized * KB;
                if (kb.y <= 5) kb.y = 5;
                pc.TakeKnockback(kb);
                break;
            }
        }
    }
}

public enum TouchThings
{
    None,
    Lava,
    
}