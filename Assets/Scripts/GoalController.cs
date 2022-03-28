using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalController : ThingController
{
    public override void OnAwake()
    {
        base.OnAwake();
        GameManager.Me.GoalExists = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        PlayerController pc = other.gameObject.GetComponent<PlayerController>();
        if (pc == null) return;

        GameManager.Me.Victory = true;
    }
}
