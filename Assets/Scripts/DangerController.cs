using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DangerController : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D other)
    {
        PlayerController p = other.gameObject.GetComponent<PlayerController>();
        if (p != null)
        {
            p.Die(gameObject);
        }
    }
}
