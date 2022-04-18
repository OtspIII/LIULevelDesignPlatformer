using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestController : MonoBehaviour
{
    public LevelManager CurrentTest;

    void Awake()
    {
        God.TestLevel = CurrentTest;
    }
}
