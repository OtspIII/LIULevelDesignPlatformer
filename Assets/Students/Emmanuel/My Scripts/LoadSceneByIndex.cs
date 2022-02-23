using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;


public class LoadSceneByIndex : MonoBehaviour
{
    public int sceneN = 27;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            StartCoroutine(LoadLevel(sceneN));
        }
    }
    IEnumerator LoadLevel(int n)
    {
            float timer = 0;
            while (timer < 1)
            {
                timer = Mathf.Lerp(timer, 1.05f, 0.1f);
                yield return null;
            }

        SceneManager.LoadScene(n);
    }
}
