using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Roam : MonoBehaviour
{
    private Rigidbody2D RB;
    public float Speed;
    public bool MoveRight;
    public float Timer;
    public float MaxTimer = 2;

    private void Start()
    {
        RB = this.GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        Timer += Time.deltaTime;
        if (Timer >= MaxTimer)
        {
            Timer = 0;
            MoveRight = !MoveRight;
        }
        Vector3 vel = RB.velocity;
        if (MoveRight) vel.x = Speed;
        else vel.x = -Speed;
        RB.velocity = vel;
    }

}
