using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BadBall : MonoBehaviour
{
    public float bspeed;
    public float bkill;
    public float killstart;


    void Update()
    {
        transform.position = transform.position + new Vector3(bspeed * -Time.deltaTime,0,0);
        bkill = bkill - Time.deltaTime;

        if (bkill <= 0)
        {
            Destroy(gameObject,.5f);
            bkill = killstart;
        }
        
    }
    
}
