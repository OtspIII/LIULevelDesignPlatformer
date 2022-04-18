using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateText : MonoBehaviour
{
    
    void Update()
    {
        if (God.Camera != null)
        {
            transform.LookAt(God.Camera.transform.position);
            transform.Rotate(0,180,0);
        }
    }
}
