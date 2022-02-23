using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class VanishingText : MonoBehaviour
{
    private float timer = 0;
    public float timeoutMax = 4;
    private TextMeshProUGUI playerText;
    // Start is called before the first frame update
    void Start()
    {
        playerText = gameObject.GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        if (timer > timeoutMax)
        {
            this.gameObject.SetActive(false);
        }

    }
}
