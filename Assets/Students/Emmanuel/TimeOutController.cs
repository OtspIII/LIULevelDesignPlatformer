using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TimeOutController : MonoBehaviour
{
    private float timer=0; 
    public float timeoutMax = 60;
    public PlayerController player;
    public TextMeshProUGUI timerText;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        timerText.text = "" + ((int)(timeoutMax - timer));

        if (timer > timeoutMax)
        {
            player.Die(gameObject);
        }
    }
}
