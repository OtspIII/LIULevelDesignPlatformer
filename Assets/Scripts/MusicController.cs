using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicController : ThingController
{

    public override void ApplyJSON(JSONData data)
    {
        base.ApplyJSON(data);
        if (data.Audio != null)
        {
            AS.clip = data.Audio;
            AS.Play();
        }
        if(JSON.Color != MColors.None)
            CameraController.Me.SetBG(JSON.Color);
    }
}
