using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhasingPlatformController : MonoBehaviour
{
    public float Timer;
    public float PhaseInTime = 3;
    public float PhaseOutTime = 1;
    public bool PhasedIn = true;

    public Collider2D Collider;
    public SpriteRenderer Body;
    
    void Update()
    {
        Timer -= Time.deltaTime;
        if (Timer <= 0)
        {
            PhasedIn = !PhasedIn;
            if (PhasedIn)
            {
                Collider.enabled = true;
                Body.color = new Color(1,1,1,1);
                Timer = PhaseInTime;
            }
            else
            {
                Collider.enabled = false;
                Body.color = new Color(1,1,1,0.1f);
                Timer = PhaseOutTime;
            }
        }
    }
}
