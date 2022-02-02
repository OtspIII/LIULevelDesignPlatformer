using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobertSounds_Robert : MonoBehaviour
{
    private AudioSource source;
    public AudioClip jump;
    public AudioClip finished;
    private bool isOnGround;

    void Start()
    {
        source = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (isOnGround && Input.GetKeyDown(KeyCode.Z))
        {
            source.PlayOneShot(jump);
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Platforms")
        {
            isOnGround = true;
        }
        if (other.gameObject.name == "Door")
        {
            source.PlayOneShot(finished);
        }
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.tag == "Platforms")
        {
            isOnGround = false;
        }
    }
}
