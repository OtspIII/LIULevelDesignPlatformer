using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmmanuelPower : GenericPower
{
    public GameObject DangerZones;
    private bool activated;
    public bool Activated
    {
        get
        {
            return activated;
        }
    }
    public override void Activate()
    {
        activated = true;
        AudioSource DZA = DangerZones.GetComponent<AudioSource>();

        int counter = 0;   
        foreach(Transform child in DangerZones.transform)
        {
            DangerController DC = child.GetComponent<DangerController>();
            if(DC != null)
            {
                counter++;
                if (DZA != null)
                    DC.audioSource = DZA;
                DC.dangerZoneEnabled = false;
            }
            SpriteRenderer SR = child.GetComponent<SpriteRenderer>();
            if(SR != null)
                SR.color = Color.yellow;
        }

        Player.SetWinCondition(counter);
        
    }



    void Update()
    {
        
    }
}
