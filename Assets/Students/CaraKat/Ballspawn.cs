using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ballspawn : MonoBehaviour
{
    public GameObject ball;
    public float count;
    public float count2;
    public Vector3 bdrop;
    //public int crange;

    //GOAL: get spawner to summon bad ball gameobject at position (bonus: spawn timer is in a range to make it sometimes shorter or longer in between spawns)

    void Start()
    {
        count = count2;
        //count2 = crange;
        //[Range(1, 6)];
    }

    void FixedUpdate()
    {
        count = count - 1;
        if (count <= 0)
        {
            count = count2;
            Instantiate(ball, bdrop, Quaternion.identity);
        }
    }
}
