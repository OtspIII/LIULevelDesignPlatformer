using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class OffScreen : MonoBehaviour
{
    public TextMeshProUGUI dialogueText;

    void OnTriggerEnter2D()
    {
        dialogueText.text = "WWhhoops! Looks like you fell. Your punishment? WAIT FOR THE TIMEOUT";
    }


    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }
}
