using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MasonPower : GenericPower
{
    public GameObject att;
    private float timeCounter;
    public float time;
    public bool canAtt;
    private float cdownCounter;
    public float cooldown;
    public override void Activate()
    {
        if (canAtt == true)
        {
            canAtt = false;
            att.SetActive(true);
        }
    }

    public void Start()
    {
        timeCounter = time;
        cdownCounter = cooldown;
        canAtt = true;
    }

    void Update()
    {
        print(cdownCounter);
        if (canAtt == false)
        {
            timeCounter -= Time.deltaTime;
        }
        if (timeCounter <= 0)
        {
            timeCounter = time;
            att.SetActive(false);
            canAtt = true;
        }
    }
}
