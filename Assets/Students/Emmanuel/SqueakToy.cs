using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SqueakToy : MonoBehaviour
{
    private AudioSource audioSource; // optional to set, but don't
    public AudioClip audioClip; // do set
    private bool play;
    public bool PlayOnce = false;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!play)
        {
            if(PlayOnce)
                play = true;
            audioSource.PlayOneShot(audioClip);
        }
        
    }
    // Start is called before the first frame update
    void Start()
    {
        audioSource = gameObject.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
