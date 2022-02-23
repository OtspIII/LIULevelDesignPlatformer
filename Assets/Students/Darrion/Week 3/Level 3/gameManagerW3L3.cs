using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gameManagerW3L3 : MonoBehaviour
{
    public float timer;
    public GameObject floor, floor2;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (timer < 0)
        {
            Destroy(floor);
            Destroy(floor2);
        }

        timer -= Time.deltaTime;
    }
}
