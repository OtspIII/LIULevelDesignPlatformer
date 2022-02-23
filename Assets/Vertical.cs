using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vertical : MonoBehaviour
{
    private Rigidbody2D RB;
    public float Speed;
    public bool MoveUp;
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
            MoveUp = !MoveUp;
        }
        Vector3 vel = RB.velocity;
        if (MoveUp) vel.y = Speed;
        else vel.y = -Speed;
        RB.velocity = vel;
    }
}
