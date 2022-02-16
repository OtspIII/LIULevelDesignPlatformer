using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : MonoBehaviour
{
    public string TrapType;
    private bool trapActivated = false;

    public List<Vector3> Destinations;
    private int CurrentDest;
    public float Speed = 0.1f;
    private List<Transform> Riders = new List<Transform>();

    // Start is called before the first frame update
    void Start()
    {

    }

    public void TrapActivate()
    {
        trapActivated = true;
        Debug.Log("Trap Activated");
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (trapActivated)
            switch (TrapType)
            {
                case "MovingPlatform":
                    {
                        Debug.Log("ShouldBeMoving");
                        if (Destinations.Count == 0) return;
                        Debug.Log("Moving");
                        Vector3 dest = Destinations[CurrentDest];
                        Vector3 old = transform.position;
                        transform.position = Vector3.MoveTowards(transform.position, dest, Speed);
                        Vector3 movement = transform.position - old;
                        /*foreach (Transform tra in Riders)
                        {
                            tra.position += movement;
                        }
                        if (Vector3.Distance(transform.position, dest) < 0.01f)
                        {
                            CurrentDest++;
                            if (CurrentDest >= Destinations.Count)
                                CurrentDest = 0;
                        }*/
                        break;
                    }
                default:
                    Debug.Log("Default");
                    break;
            }
    }


    private void OnCollisionEnter2D(Collision2D other)
    {
        //if (!Riders.Contains(other.transform))
        //Riders.Add(other.transform);
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        //Riders.Remove(other.transform);
    }
}
