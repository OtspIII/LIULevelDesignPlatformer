using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPlayer_Robert : MonoBehaviour
{
    private AudioSource source;
    public AudioClip music1;

    // Start is called before the first frame update
    void Start()
    {
        source = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (source.isPlaying == false)
        {
            source.PlayOneShot(music1);
        }
    }
}
