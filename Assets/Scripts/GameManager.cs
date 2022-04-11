using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public NetworkManager NM;
    public TextMeshProUGUI StatusText;
    public NetStatus NStatus;
    public string PlayerDebug;

    void Awake()
    {
        God.GM = this;
        if (NM != null) God.NM = NM;
    }
    
    void Update()
    { 
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            NM.StartClient();
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            NM.StartHost();
        }

        NStatus = God.GetStatus();
        StatusText.text = NStatus.ToString() + " / " + PlayerDebug;
    }
    
    
}
