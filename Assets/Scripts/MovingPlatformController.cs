using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatformController : MonoBehaviour
{
    public List<Vector3> Destinations;
    private int CurrentDest;
    public float Speed = 0.1f;
    private List<Transform> Riders = new List<Transform>();
    
    void FixedUpdate()
    {
        if (Destinations.Count == 0) return;
        Vector3 dest = Destinations[CurrentDest];
        Vector3 old = transform.position;
        transform.position = Vector3.MoveTowards(transform.position, dest, Speed);
        Vector3 movement = transform.position - old;
        foreach (Transform tra in Riders)
        {
            tra.position += movement;
        }
        if (Vector3.Distance(transform.position, dest) < 0.01f)
        {
            CurrentDest++;
            if (CurrentDest >= Destinations.Count)
                CurrentDest = 0;
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (!Riders.Contains(other.transform))
            Riders.Add(other.transform);
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        Riders.Remove(other.transform);
    }
}
