using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject Target;
    public static CameraController Me;
    public Camera Cam;
    public GameObject UI;

    private void Awake()
    {
        Me = this;
    }

    void FixedUpdate()
    {
        Vector3 pos = Vector3.Lerp(transform.position, Target.transform.position,0.2f);
        pos.z = transform.position.z;
        transform.position = pos;
    }

    public void SetZoom(float amt)
    {
        float size = amt * 5;
        Cam.orthographicSize = size;
        UI.transform.localScale = new Vector3(amt,amt,1);
    }

    public void SetBG(MColors color)
    {
        Cam.backgroundColor = God.GetColor(color);
    }
}
