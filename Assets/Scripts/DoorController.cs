using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DoorController : MonoBehaviour
{
    public CameraController Camera;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            int sceneN = SceneManager.GetActiveScene().buildIndex;
            StartCoroutine(LoadLevel(sceneN + 1));
        }
            
    }
    
    private void OnCollisionEnter2D(Collision2D other)
    {
        PlayerController p = other.gameObject.GetComponent<PlayerController>();
        if (p != null)
        {
            int sceneN = SceneManager.GetActiveScene().buildIndex;
            StartCoroutine(LoadLevel(sceneN + 1));
        }
    }

    IEnumerator LoadLevel(int n)
    {
        if (Camera && Camera.Fader)
        {
            Camera.Fader.gameObject.SetActive(true);
            float timer = 0;
            while (timer < 1)
            {
                timer = Mathf.Lerp(timer, 1.05f, 0.1f);
                Camera.Fader.color = new Color(0, 0, 0, timer);
                yield return null;
            }
        }

        SceneManager.LoadScene(n);
    }
}
