using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DangerController : MonoBehaviour
{
    public bool dangerZoneEnabled = true;
    public AudioSource audioSource; // optional to set, but don't
    public AudioClip audioClip; // do set

    void Start()
    {
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        PlayerController p = other.gameObject.GetComponent<PlayerController>();
        if (p != null)
        {
            if (!dangerZoneEnabled)
            {
                p.AddToBlocksCollected();
                if (audioSource != null && audioClip != null)
                {
                    audioSource.PlayOneShot(audioClip);
                }
                this.gameObject.SetActive(false);
            }
            else
                p.Die(gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        PlayerController p = other.gameObject.GetComponent<PlayerController>();
        if (p != null)
        {
            if (!dangerZoneEnabled)
            {
                p.AddToBlocksCollected();
                if (audioSource != null && audioClip != null)
                {
                    audioSource.PlayOneShot(audioClip);
                }
                this.gameObject.SetActive(false);
            }
            else
                p.Die(gameObject);
        }
    }
}
