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
    //GOAL: if 2 ball hits object with Danger tag= it destorys itself
    // void OnCollisionEnter2D()
    // {
    //     if (gameObject.CompareTag("Hazard"))
    //     {
    //         Destroy(gameObject,.5f);
    //         Debug.Log("Danger hit Danger");
    //     }
    // }
    
}
