using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Diagnostics;

public class DoorController : ThingController
{
    public int Number;
    
    private void OnCollisionEnter2D(Collision2D other)
    {
        PlayerController pc = other.gameObject.GetComponent<PlayerController>();
        if (pc != null && pc.Keys.Contains(Number))
        {
            pc.Keys.Remove(Number);
            if (JSON.Audio)GameManager.Me.PlaySound(JSON.Audio);
            Destroy(gameObject);
        }
    }
}
