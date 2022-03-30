using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthController : ThingController
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        PlayerController pc = other.gameObject.GetComponent<PlayerController>();
        if (pc == null || pc.HP >= pc.MaxHP) return;
        if (JSON.Amount > 0)
            pc.HP = Mathf.Min(pc.MaxHP,pc.HP + Mathf.RoundToInt(JSON.Amount)) ;
        else
            pc.HP = pc.MaxHP;
        if (JSON.Audio)GameManager.Me.PlaySound(JSON.Audio);
        Destroy(gameObject);
    }
}
