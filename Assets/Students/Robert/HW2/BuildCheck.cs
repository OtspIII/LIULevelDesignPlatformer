using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildCheck : MonoBehaviour
{
    public SpriteRenderer render;
    Vector2 mousePosition;
    public static bool canBuild;
    // Start is called before the first frame update
    void Start()
    {
        canBuild = false;
    }

    // Update is called once per frame
    void Update()
    {
        mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = new Vector2(mousePosition.x, mousePosition.y);
        if (canBuild && BuildAbility_Robert.isActivated && BuildAbility_Robert.BrickAmount > 0)
        {
            render.color = Color.green;
        }
        if (!canBuild || BuildAbility_Robert.BrickAmount <= 0)
        {
            render.color = Color.grey;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Platforms" && BuildAbility_Robert.BrickAmount > 0)
        {
            canBuild = true;
            Debug.Log("canBuild");
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Platforms")
        {
            canBuild = false;
            Debug.Log("cannotBuild");
        }
    }
}
