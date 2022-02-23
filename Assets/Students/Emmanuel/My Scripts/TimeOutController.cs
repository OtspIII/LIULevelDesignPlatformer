using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class TimeOutController : MonoBehaviour
{
    public static float timer=0; 
    public float timeoutMax = 60*3;
    public PlayerController player;
    public TextMeshProUGUI timerText;
    public bool resetTimer;
    public bool freezeTime;
    public CameraController Camera;
    // Start is called before the first frame update
    void Start()
    {
        if (resetTimer)
        {
            timer = 0;
            resetTimer = false;
            ContinuousSong.time = 0;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!freezeTime)
        {
            timer += Time.deltaTime;

            timerText.text = "" + ((int)(timeoutMax - timer));

            if (timer > timeoutMax)
            {
                //player.Die(gameObject);
                int sceneN = 32;//SceneManager.GetActiveScene().buildIndex;
                StartCoroutine(LoadLevel(sceneN));
            }
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
