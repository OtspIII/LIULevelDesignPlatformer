using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContinuousSong : MonoBehaviour
{
    private AudioSource AS;
    public static float time = 0;

    // Start is called before the first frame update
    void Start()
    {
        AS = gameObject.GetComponent<AudioSource>();
        AS.time = time;
        AS.Play();
    }

    // Update is called once per frame
    void Update()
    {
        time = AS.time;
    }
}
