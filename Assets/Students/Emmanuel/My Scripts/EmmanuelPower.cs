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
    public GameObject Door;
    public GameObject CollectibleDoor;
    private int WinCondition;
    private int blocksCollected = 0;

    public override void Activate()
    {
        activated = true;
        AudioSource DZA = DangerZones.GetComponent<AudioSource>();

        WinCondition = 0;   
        foreach(Transform child in DangerZones.transform)
        {
            DangerControllerEmmanuel DC = child.GetComponent<DangerControllerEmmanuel>();
            if(DC != null)
            {
                WinCondition++;
                if (DZA != null)
                    DC.audioSource = DZA;
                DC.dangerZoneEnabled = false;
            }
            SpriteRenderer SR = child.GetComponent<SpriteRenderer>();
            if(SR != null)
                SR.color = Color.yellow;
        }
        if (Input.GetKeyDown(KeyCode.X))
        {
            Door.SetActive(false);
            CollectibleDoor.SetActive(true);
        }

        Debug.Log(WinCondition);
    }

    public void AddToBlocksCollected()
    {
        blocksCollected++;
        Debug.Log(blocksCollected);
    }

    public int GetWinCondition() {
        return WinCondition;
    }
    public int GetBlocksCollected()
    {
        return blocksCollected;
    }


    void Update()
    {
        
    }
}
