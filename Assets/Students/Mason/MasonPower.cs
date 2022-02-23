using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MasonPower : GenericPower
{
    public GameObject att;
    private float timeCounter;
    public float time;
    public bool canAtt;
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
        canAtt = true;
    }

    void Update()
    {
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
