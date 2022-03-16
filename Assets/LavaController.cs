using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LavaController : MonoBehaviour
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
