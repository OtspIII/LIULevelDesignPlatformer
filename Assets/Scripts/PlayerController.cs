using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class PlayerController : MonoBehaviour
{
    public static PlayerController PC;
    public CheckpointController LastCheckpoint;
    public static int DeathCount = 0;
    public float Speed = 10;
    public float JumpPower = 10;
    public float JumpTime = 0.5f;
    public float Gravity = 2;
    public int MaxAirJumps = 0;
    public SpriteRenderer Body;
    public BoxCollider2D Foot;
    
    public Rigidbody2D RB;
    private bool FaceLeft = false;
    private float JumpTimer = 0;
    public List<GameObject> Floors = new List<GameObject>();
    private GenericPower Power;
    private bool InControl = true;
    private int AirJumps;
    public float FallPlatTime = 0;

    private void Awake()
    {
        PlayerController.PC = this;
    }

    void Start()
    {
        RB = GetComponent<Rigidbody2D>();
        RB.gravityScale = Gravity;
        Power = GetComponent<GenericPower>();
    }

    void Update()
    {
        FallPlatTime -= Time.deltaTime;
        if (!InControl) return;      
        
        Vector2 vel = RB.velocity;
        
        float xDesire = 0;
        if (Input.GetKey(KeyCode.RightArrow))
            xDesire = Speed;
        else if (Input.GetKey(KeyCode.LeftArrow))
            xDesire = -Speed;
        if (Mathf.Sign(xDesire) != Mathf.Sign(vel.x))
            vel.x = 0;
        vel.x = Mathf.Lerp(vel.x, xDesire, 0.25f);

        if (Input.GetKey(KeyCode.Z))
        {
            if (OnGround())
            {
                if (Input.GetKey(KeyCode.DownArrow))
                {
                    FallPlatTime = 0.5f;
                    JumpTimer = 999;
                    vel.y = -3f;
                }
                else
                {
                    vel.y = JumpPower;
                    JumpTimer = 0;
                    AirJumps = MaxAirJumps;
                }
            }
            else if (JumpTimer < JumpTime)
            {
                JumpTimer += Time.deltaTime;
                vel.y = JumpPower;
            }
            else if (Input.GetKeyDown(KeyCode.Z) && AirJumps > 0)
            {
                AirJumps--;
                JumpTimer = 0;
                vel.y = JumpPower;
            }
        }
        else
            JumpTimer = 999;
        
        RB.velocity = vel; 
        if (xDesire != 0)
            SetFlip(vel.x < 0);
        
        if (Input.GetKeyDown(KeyCode.X) && Power != null)
            Power.Activate();
        if (Input.GetKeyDown(KeyCode.R))
            Die(gameObject);

        if (vel.y > 0 || FallPlatTime > 0)
        {
            gameObject.layer = LayerMask.NameToLayer("RisingPlayer");
            Foot.gameObject.layer = LayerMask.NameToLayer("RisingPlayerFoot");
        }
        else
        {
            gameObject.layer = LayerMask.NameToLayer("Player");
            Foot.gameObject.layer = LayerMask.NameToLayer("Foot");
        }
            
    }

    public void SetFlip(bool faceLeft)
    {
        if (faceLeft == FaceLeft) return;
        FaceLeft = faceLeft;
        transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x) * (FaceLeft ? -1 : 1),
            transform.localScale.y,1);
    }

    public bool OnGround()
    {
        return Floors.Count > 0;
    }

    public void SetInControl(bool inControl)
    {
        InControl = inControl;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.otherCollider.gameObject == Foot.gameObject && !Floors.Contains(other.gameObject))
        {
            Floors.Add(other.gameObject);
        }
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.otherCollider.gameObject == Foot.gameObject)
            Floors.Remove(other.gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        CheckpointController cc = other.gameObject.GetComponent<CheckpointController>();
        if (cc != null)
        {
            SetCheckpoint(cc);
            cc.GetChecked();
        }
    }

    private IEnumerator DeathAnimation(GameObject source)
    {
        SetInControl(false);
        Vector2 toss = new Vector2(Random.Range(-10,10),Random.Range(-10,10));
        if (source != null)
        {
            Vector2 dist = source.transform.position - transform.position;
            if (Mathf.Abs(dist.x) >= Mathf.Abs(dist.y))
            {
                toss.x = 10 * (dist.x > 0 ? -1 : 1);
                toss.y = Mathf.Abs(toss.y) * (RB.velocity.y >= 0 ? 1 : -1);
            }
            else
            {
                toss.y = 10 * (dist.y > 0 ? -1 : 1);
                toss.x = Mathf.Abs(toss.x) * (RB.velocity.x >= 0 ? 1 : -1);
            }
        }
        RB.velocity = toss;
        CameraController Camera = FindObjectOfType<CameraController>();
        if (Camera != null && Camera.Fader != null)
        {
            Camera.Fader.gameObject.SetActive(true);
            float spin = 0;
            float timer = 0;
            while (timer < 1)
            {
                spin += Time.deltaTime * 1000 * (RB.velocity.x >= 0 ? -1 : 1);
                Body.transform.rotation = Quaternion.Euler(0,0,spin);
                timer = Mathf.Lerp(timer, 1.01f, 0.07f);
                Camera.Fader.color = new Color(0, 0, 0, timer);
                yield return null;
            }
        }

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void Die(GameObject source)
    {
        DeathCount++;
        Debug.Log("YOU DIED: " + DeathCount + " / " + SceneManager.GetActiveScene().name.ToUpper());
        if (LastCheckpoint == null)
            StartCoroutine(DeathAnimation(source));
        else
            transform.position = LastCheckpoint.Spawn.position;
    }
    
    public void SetCheckpoint(CheckpointController cc){
        LastCheckpoint = cc;
    }
}
