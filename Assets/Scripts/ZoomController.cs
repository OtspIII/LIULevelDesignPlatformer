using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoomController : ThingController
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.gameObject.GetComponent<PlayerController>()) return;
        if (JSON.Amount > 0)
            CameraController.Me.SetZoom(JSON.Amount);
        if(JSON.Color != MColors.None)
            CameraController.Me.SetBG(JSON.Color);
        if (JSON.Audio)GameManager.Me.PlaySound(JSON.Audio);
    }
}
