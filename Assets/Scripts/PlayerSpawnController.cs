using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawnController : MonoBehaviour
{
    void Start()
    {
        God.LM.PSpawns.Add(this);
    }
}
