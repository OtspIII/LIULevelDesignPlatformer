using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BuildAbility_Robert : GenericPower
{
    public static int BrickAmount;
    public static bool isActivated;
    public static bool hasBeenActivated;
    Vector3 mousePos;
    Vector2 mousePosition;
    public GameObject vertBuild;
    public GameObject player;
    public GameObject brick;

    private void Start()
    {
        hasBeenActivated = false;
        BrickAmount = 0;
        isActivated = false;
    }
    public override void Activate()
    {
        if (!hasBeenActivated)
        {
            if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("HW3Level2_Robert"))
            {
                BrickAmount = 100;
            }
            if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("FinalLevel1_Rob"))
            {
                BrickAmount = 155;
            }
            if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("FinalLevel2_Rob"))
            {
                BrickAmount = 70;
            }
            else
            {
                BrickAmount = 55;
            }
            isActivated = true;
            Player.SetInControl(true);
            hasBeenActivated = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        mousePos = Input.mousePosition;
        mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if (Input.GetMouseButtonDown(0) && BrickAmount > 0 && BuildCheck.canBuild)
        {
            var brickspawn = Instantiate(brick, mousePosition, Quaternion.identity);
            brickspawn.GetComponent<BrickScript>().isOriginal = false;
            vertBuild.transform.position = new Vector2(mousePosition.x, player.transform.position.y);
            VerticalBuildCheck.collidedWithPlatform = false;
            BrickAmount -= 1;
        }
        if (Input.GetKeyDown(KeyCode.A) && BrickAmount > 0 && VerticalBuildCheck.canBuild)
        {
            var brickspawn2 = Instantiate(brick, vertBuild.transform.position, Quaternion.identity);
            brickspawn2.GetComponent<BrickScript>().isOriginal = false;
            vertBuild.transform.position = new Vector2(transform.position.x, player.transform.position.y);
            VerticalBuildCheck.collidedWithPlatform = false;
            BrickAmount -= 1;
        }
    }
}
