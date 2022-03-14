using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject Target;

    void Update()
    {
        Vector3 pos = Vector3.Lerp(transform.position, Target.transform.position,0.2f);
        pos.z = transform.position.z;
        transform.position = pos;
    }
}
