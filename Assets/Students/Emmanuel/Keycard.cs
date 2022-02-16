using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Keycard : MonoBehaviour
{
    public Gate gate;
    public AudioSource audioSource; // optional to set, but don't
    public AudioClip audioClip; // do set
 
    void OnTriggerEnter2D(Collider2D other)
    {
        gate.Open();
        audioSource.PlayOneShot(audioClip);
        Destroy(gameObject, .50f);
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
