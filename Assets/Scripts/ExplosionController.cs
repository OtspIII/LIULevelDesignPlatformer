using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionController : MonoBehaviour
{
    public BombController Parent;
    public int Damage = 5;
    public List<CharController> Immune;
    
    public void Activate()
    {
        gameObject.SetActive(true);
        Invoke("End",0.2f);
    }

    public void End()
    {
        if(Parent != null)
            Destroy(Parent.gameObject);
        else
            Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        CharController cc = other.GetComponent<CharController>();
        if (cc != null && !Immune.Contains(cc))
        {
            cc.TakeDamage(Damage);
            Immune.Add(cc);
        }
            
    }
}
