using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EventController : ThingController
{
    public AudioSource AS;
    public TextMeshPro Text;
    
    public override void ApplyJSON(JSONData data)
    {
        base.ApplyJSON(data);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.gameObject.GetComponent<PlayerController>()) return;
        if(JSON.Audio != null) AS.PlayOneShot(JSON.Audio);
        if (JSON.Text != "") GameManager.Me.NarrateText.text = JSON.Text;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.gameObject.GetComponent<PlayerController>()) return;
        if (JSON.Text != "") GameManager.Me.NarrateText.text = "";
    }
}
