using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SecurityGuard : MonoBehaviour //Gate
{
    private SpriteRenderer sr;
    public Sprite sprite;
    public TextMeshProUGUI dialogueText;

    public /*override*/ void Open()
    {
        sr.sprite = sprite;
        dialogueText.text = "Thank you. Come again!";
    }


    // Start is called before the first frame update
    void Start()
    {
        sr = gameObject.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
