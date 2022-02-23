using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public float timer, timerReset;
    public GameObject enemy;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (timer < 0)
        {
            Instantiate(enemy, transform.position, Quaternion.identity);
            timer = timerReset;
        }

        timer -= Time.deltaTime;
    }
}
