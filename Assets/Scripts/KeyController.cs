using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyController : ThingController
{
    public int Number;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        PlayerController pc = other.gameObject.GetComponent<PlayerController>();
        if (pc != null)
        {
            pc.GetKey(this);
            Destroy(gameObject);
        }
    }
}
