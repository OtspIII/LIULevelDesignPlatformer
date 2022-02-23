using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DarrionPower2 : GenericPower
{
    public bool faceRight;
    public GameObject rightBullet, leftBullet;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            faceRight = false;
        }       
        
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            faceRight = true;
        }
    }

    public override void Activate()
    {
        if (faceRight == true)
        {
            Instantiate(rightBullet, transform.position + new Vector3(0+1, 0, 0), Quaternion.identity);
            Debug.Log("Spawned Right Bullet");
        }      
        
        if (faceRight == false)
        {
            Instantiate(leftBullet, transform.position + new Vector3(0 - 1, 0, 0), Quaternion.identity);
            Debug.Log("Spawned Left Bullet");
        }

        //Debug.Log("Power 2 Works!");
    }
}
