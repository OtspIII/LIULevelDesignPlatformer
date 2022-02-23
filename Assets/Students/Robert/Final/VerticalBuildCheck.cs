using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VerticalBuildCheck : MonoBehaviour
{
    public SpriteRenderer render;
    public static bool canBuild;
    public GameObject player;
    public static bool collidedWithPlatform;
    // Start is called before the first frame update
    void Start()
    {
        canBuild = false;
        transform.position = new Vector2(player.transform.position.x, player.transform.position.y);
        collidedWithPlatform = false;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!collidedWithPlatform && BuildAbility_Robert.isActivated)
        {
            transform.position = new Vector2(player.transform.position.x, transform.position.y - .1f);
        }
        if (collidedWithPlatform && BuildAbility_Robert.isActivated)
        {
            transform.position = new Vector2(player.transform.position.x, transform.position.y);
        }
        if (canBuild && BuildAbility_Robert.isActivated && BuildAbility_Robert.BrickAmount > 0)
        {
            render.color = Color.cyan;
        }
        if (!canBuild || BuildAbility_Robert.BrickAmount <= 0)
        {
            render.color = Color.black;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Platforms" && BuildAbility_Robert.BrickAmount > 0)
        {
            canBuild = true;
            collidedWithPlatform = true;
        }
        if (collision.gameObject.tag == "Hazard" && BuildAbility_Robert.BrickAmount > 0)
        {
            canBuild = false;
            collidedWithPlatform = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Platforms" && BuildAbility_Robert.BrickAmount > 0)
        {
            collidedWithPlatform = false;
            canBuild = false;
        }
        if (collision.gameObject.tag == "Hazard" && BuildAbility_Robert.BrickAmount > 0)
        {
            collidedWithPlatform = false;
            canBuild = false;
        }
    }
}
