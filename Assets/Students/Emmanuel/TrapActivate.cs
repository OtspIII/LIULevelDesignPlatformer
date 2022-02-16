using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapActivate : MonoBehaviour
{
    public Trap trap;
    private void OnTrigger2D(Collider2D other)

    {
        if(other.gameObject.tag == "Player")
        trap.TrapActivate();
    }
    private void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.tag == "Player")
            trap.TrapActivate();
    }



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
