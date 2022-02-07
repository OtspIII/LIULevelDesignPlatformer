using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FiveMeterCollisionScript : MonoBehaviour
{
    bool touchy;

    private void Update()
    {
        //Debug.Log(touchy);
    }
 
    
    public void OnTriggerEnter2D(Collider2D collision)
    {
        touchy = true;
    }


    public void OnTriggerExit2D(Collider2D collision)
    {
        touchy = false;
    }

    public bool isTouchy()
    {
        return touchy;
    }
}
