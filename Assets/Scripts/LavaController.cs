using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LavaController : ThingController
{
    public List<CharController> Touching;

    void Update()
    {
        foreach (CharController cc in Touching.ToArray())
        {
            cc.TakeDamage(1);
        }
    }
    
    private void OnCollisionEnter2D(Collision2D other)
    {
        CharController cc = other.gameObject.GetComponent<CharController>();
        if (cc != null && !Touching.Contains(cc))
        {
            Touching.Add(cc);
            if (JSON.Audio)GameManager.Me.PlaySound(JSON.Audio);
        }
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        CharController cc = other.gameObject.GetComponent<CharController>();
        if (cc != null)
        {
            Touching.Remove(cc);
        }
    }
}
