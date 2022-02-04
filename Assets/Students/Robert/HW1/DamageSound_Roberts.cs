using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageSounds_Roberts : MonoBehaviour
{
    private AudioSource source;
    public AudioClip punch;
    public AudioClip death;

    void Start()
    {
        source = GetComponent<AudioSource>();
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        PlayerController p = other.gameObject.GetComponent<PlayerController>();
        if (p != null)
        {
            source.PlayOneShot(punch);
            source.PlayOneShot(death);
        }
    }
}
