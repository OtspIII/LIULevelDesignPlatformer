using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIScript : MonoBehaviour
{
    public GameObject UItext;
    UnityEngine.UI.Text myText;
    // Start is called before the first frame update
    void Start()
    {
        UItext.GetComponent<UnityEngine.UI.Text>().text = "text";
        myText = GameObject.Find("Text").GetComponent<UnityEngine.UI.Text>();
    }

    // Update is called once per frame
    void Update()
    {
        myText.text = BuildAbility_Robert.BrickAmount + " Bricks Left";
    }
}
