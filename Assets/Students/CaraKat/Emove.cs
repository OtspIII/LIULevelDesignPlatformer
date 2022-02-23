using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Emove : MonoBehaviour
{
    private Vector3 enemove;
    public float mospe;
    public int maxmo;
    public int minmo;

    void Start()
    {
    
    }

    void Update()
    {
        transform.position = transform.position + new Vector3(0,mospe * Time.deltaTime,0);

        if (transform.position.y >= maxmo)
        {
            transform.position = transform.position + new Vector3(0,minmo,0);
        } 
        // else if (transform.position.y <= minmo)
        // {
        //     transform.position = transform.position + new Vector3(0,mospe * Time.deltaTime,0);
        // }
    }
}
