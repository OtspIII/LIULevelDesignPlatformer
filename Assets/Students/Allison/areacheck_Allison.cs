using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class areacheck_Allison : MonoBehaviour
{
    public CircleCollider2D CC;
    public float timetowait = 0;
    public float MaxTimer = 1;
    public bool SetActive;


    void Start()
    {
        CC = GetComponent<CircleCollider2D>();

    } 
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.X))
        {
		CC.enabled = true;  
        timetowait = 0;
		}
        
        timetowait += Time.deltaTime;
        if (timetowait >= .5f)
        {
		CC.enabled = false;  
        }
    }
	void OnCollisionEnter2D(Collision2D other){

		if (other.gameObject.CompareTag ("block")){
		Destroy(other.gameObject);	
	}
	}

}
