using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionController : ThingController
{
    public BombController Parent;
    public int Damage = 5;
    public List<CharController> Immune;
    
    public override void ApplyJSON(JSONData data)
    {
        base.ApplyJSON(data);
        if (data.Amount > 0) Damage = Mathf.CeilToInt(data.Amount);
        gameObject.SetActive(true);
        if (data.Audio)GameManager.Me.PlaySound(data.Audio);
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
        if (cc != null && !Immune.Contains(cc) && (cc != Source || cc.Player))
        {
            cc.TakeDamage(Damage);
            Immune.Add(cc);
        }
            
    }
}
