using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointController : MonoBehaviour
{
    public Transform Spawn;
    public SpriteRenderer SR;
    public Color StartColor;
    public Color ActiveColor;

    private void Start()
    {
        SR.color = StartColor;
    }

    public void GetChecked()
    {
        SR.color = ActiveColor;
    }
}
