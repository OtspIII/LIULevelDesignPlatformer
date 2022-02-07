using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIndicator : MonoBehaviour
{
    public static int BrickAmount;
    Vector3 mousePos;
    Vector2 mousePosition;
    public GameObject brick;

    // Start is called before the first frame update
    void Start()
    {
        BrickAmount = 50;
    }

    // Update is called once per frame
    void Update()
    {
        mousePos = Input.mousePosition;
        mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if (Input.GetMouseButtonDown(0) && BrickAmount > 0)
        {
            var brickspawn = Instantiate(brick, mousePosition, Quaternion.identity);
            brickspawn.GetComponent<BrickScript>().isOriginal = false;
            BrickAmount -= 1;
        }
    }
}
