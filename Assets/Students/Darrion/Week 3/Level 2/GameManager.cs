using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public float coins, coinsDestroyed;
    public GameObject wall, wall2, wall3;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (coins == coinsDestroyed)
        {
            Destroy(wall);
            Destroy(wall2);
            Destroy(wall3);
        }
    }
}
