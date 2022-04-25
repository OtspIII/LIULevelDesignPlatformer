using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Collections;
using Unity.Netcode;
using Unity.Netcode.Components;
using UnityEngine;
using Random = UnityEngine.Random;

public class FirstPersonController : NetworkBehaviour
{
    public Camera Eyes;
    public Rigidbody RB;
    public NetworkRigidbody NRB;
    public MeshRenderer MR;
    public Collider Coll;
    public TextMeshPro NameText;
    public float MouseSensitivity = 3;
    public float JumpPower = 7;
    public List<GameObject> Floors;
//    public JSONWeapon CurrentWeapon;
//    public JSONWeapon DefaultWeapon;
    public float ShotCooldown;
    public bool JustKnocked = false;
    public bool GhostMode;

    public NetworkVariable<FixedString64Bytes> Name = new NetworkVariable<FixedString64Bytes>();
    public NetworkVariable<FixedString64Bytes> Weapon = new NetworkVariable<FixedString64Bytes>();
//    public int ID;
    
    public NetworkVariable<Vector3> Position = new NetworkVariable<Vector3>();
    public NetworkVariable<Vector3> Fling = new NetworkVariable<Vector3>();
    public NetworkVariable<float> XRot = new NetworkVariable<float>();
    public NetworkVariable<float> YRot = new NetworkVariable<float>();
    public NetworkVariable<int> HP = new NetworkVariable<int>();
    public NetworkVariable<int> Ammo = new NetworkVariable<int>();

    
    void Start()
    {
//        ID = Random.Range(0, 100);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        God.Players.Add(this);
        if(IsOwner)
            SetName(God.NamePick != "" ? God.NamePick : "Player " + God.Players.Count);
        else
            SetName(Name.Value.ToString());
        God.PlayerDict.Add(Name.Value.ToString(),this);
    }
    
    public override void OnNetworkSpawn()
    {
        if (!IsOwner)
        {
            Eyes.enabled = false;
            
        }
        else
        {
            God.Camera = Eyes;
            NameText.gameObject.SetActive(false);
            if(!IsServer) Invoke("SpawnMap",0.01f);
        }

        if (IsServer)
        {
            Reset();
            if (IsOwner)
            {
                RoundManager rm = Instantiate(God.Library.RM);
                rm.NO.Spawn();
                rm.StartLevel();
            }
        }
    }

    public void SpawnMap()
    {
        God.RM.StartLevel();
    }
    
    
    public void RandomSpawn()
    {
        if (NetworkManager.Singleton.IsServer)
        {
            RandomSpawnServer();
        }
        else
        {
            RandomSpawnServerRPC();
        }
    }

    public void ImprintRules(JSONCreator ruleset)
    {
//        if (ruleset.Weapons.Count > 0)
//        {
//            SetWeapon(ruleset.Weapons[0]);
//        }

        if (IsServer)
            RandomSpawnServer();
    }

    public void SetWeapon(JSONWeapon wpn)
    {
        Weapon.Value = wpn.Text;
        Ammo.Value = wpn.Ammo;
//        if (def) DefaultWeapon = wpn;
//        else
//        {
//            CurrentWeapon = wpn;
//            Ammo = CurrentWeapon.Ammo;
//        }
    }

    public void SetName(string n)
    {
        if (IsServer) Name.Value = n;
        else if (IsOwner) SetNameServerRPC(n);
        NameText.text = n;
    }
    
    [ServerRpc]
    public void SetNameServerRPC(string n)
    {
        Name.Value = n;
        NameText.text = n;
    }
    
    [ServerRpc]
    void RandomSpawnServerRPC(ServerRpcParams rpcParams = default)
    {
        RandomSpawnServer();
    }

    void RandomSpawnServer()
    {
        if (God.LM == null) return;
        PlayerSpawnController psc = God.LM.GetPSpawn(this);
        Position.Value = psc != null ? psc.transform.position + new Vector3(0, 1, 0) : new Vector3(0,100,0);
        transform.position = Position.Value;
        SetPosClientRPC(Position.Value);
    }
    
    [ServerRpc]
    void UpdatePosServerRPC(Vector3 move,bool jump, float xRot,float yRot,bool sprint)
    {
        HandleMove(move,jump,xRot,yRot,sprint);
        Position.Value = transform.position;
        XRot.Value = transform.rotation.y;
        YRot.Value = Eyes.transform.rotation.x;
    }

    [ClientRpc]
    public void SetPosClientRPC(Vector3 pos)
    {
        transform.position = pos;
//        if(IsOwner)
//            Debug.Log("SETPOS");
    }

    public JSONWeapon GetWeapon()
    {
        return God.LM.GetWeapon(Weapon.Value.ToString());
//        if (CurrentWeapon != null) return CurrentWeapon;
//        if (DefaultWeapon != null) return DefaultWeapon;
        
    }
    
    void Update()
    {
        JustKnocked = false;
        if (IsServer && transform.position.y < -100)
            Die();
        if (!IsOwner) return;
        God.HPText.text = HP.Value + "/" + GetMaxHP();
        God.StatusText.text = GetWeapon().Text + (Ammo.Value > 0 ? " - " + Ammo.Value : "");
        //Lobbyist.Text = transform.position.ToString();
        float xRot = Input.GetAxis("Mouse X") * MouseSensitivity;
        float yRot = -Input.GetAxis("Mouse Y") * MouseSensitivity;
        
        Vector3 move = Vector3.zero;
        
        if (GhostMode)
        {
            transform.Rotate(0,xRot,0);
            Vector3 eRot = Eyes.transform.localRotation.eulerAngles;
            eRot.x += yRot;
            if (eRot.x < -180) eRot.x += 360;
            if (eRot.x > 180) eRot.x -= 360;
            eRot = new Vector3(Mathf.Clamp(eRot.x, -90, 90),0,0);
            Eyes.transform.localRotation = Quaternion.Euler(eRot);
            if (Input.GetKey(KeyCode.W))
                move += Eyes.transform.forward;
            if (Input.GetKey(KeyCode.S))
                move -= Eyes.transform.forward;
            if (Input.GetKey(KeyCode.A))
                move -= Eyes.transform.right;
            if (Input.GetKey(KeyCode.D))
                move += Eyes.transform.right;
            if (Input.GetKey(KeyCode.Space))
                move += Eyes.transform.up;
            if (Input.GetKey(KeyCode.LeftControl))
                move -= Eyes.transform.up;
            transform.position += move.normalized * GetMoveSpeed() * Time.deltaTime;
            
            return;
        }
        
        bool jump = false;
        bool sprint = false;

        if (GetMoveSpeed() > 0)
        {
            
            if (Input.GetKey(KeyCode.W))
                move += transform.forward;
            if (Input.GetKey(KeyCode.S))
                move -= transform.forward;
            if (Input.GetKey(KeyCode.A))
                move -= transform.right;
            if (Input.GetKey(KeyCode.D))
                move += transform.right;
            if (Input.GetKey(KeyCode.LeftShift))
                sprint = true;
//            move = move.normalized * WalkSpeed;
            if (JumpPower > 0 && Input.GetKeyDown(KeyCode.Space))
                jump = true;
//                move.y = JumpPower;
//            else
//                move.y = RB.velocity.y;
//            RB.velocity = move;
        }
        HandleMove(move,jump,xRot,yRot,sprint);
        ShotCooldown -= Time.deltaTime;
        if (Input.GetMouseButton(0) && ShotCooldown <= 0)
        {
            JSONWeapon wpn = GetWeapon();
            ShotCooldown = wpn.RateOfFire;
            Shoot(Eyes.transform.position + Eyes.transform.forward,Eyes.transform.rotation);
//            if (Ammo.Value > 0)
//            {
//                Ammo.Value--;
//                if (Ammo.Value <= 0)
//                    Weapon.Value = "";
//            }
        }
        
        
    }

    public void HandleMove(Vector3 move,bool jump, float xRot,float yRot,bool sprint)
    {
        
        bool onGround = OnGround();
        if (!IsServer)
        {
            UpdatePosServerRPC(move,jump,xRot,yRot,sprint);
//            return;
        }
        move = move.normalized * (sprint ? GetSprintSpeed() : GetMoveSpeed());
        if (jump && onGround)
            move.y = JumpPower;
        else
            move.y = RB.velocity.y;
//        if (!onGround)
//        {
            if (Fling.Value.x != 0)
                move.x += Fling.Value.x;
            if (Fling.Value.z != 0)
                move.z += Fling.Value.z;
            if (Fling.Value != Vector3.zero && move.y == 0) move.y = 3;
//        }
        RB.velocity = move;
        transform.Rotate(0,xRot,0);
        Vector3 eRot = Eyes.transform.localRotation.eulerAngles;
        eRot.x += yRot;
        if (eRot.x < -180) eRot.x += 360;
        if (eRot.x > 180) eRot.x -= 360;
        eRot = new Vector3(Mathf.Clamp(eRot.x, -90, 90),0,0);
        Eyes.transform.localRotation = Quaternion.Euler(eRot);
        //Eyes.transform.Rotate(yRot,0,0);
        //Debug.Log("EYEROT: " + Eyes.transform.rotation.eulerAngles.x);
        // if(Eyes.transform.rotation.eulerAngles.x > 90 && Eyes.transform.rotation.eulerAngles.x < 270)
        //     Eyes.transform.rotation = Quaternion.Euler(eRot);
    }
    
    public void Shoot(Vector3 pos,Quaternion rot)
    {
        if (!IsServer)
        {
            ShootServerRPC(pos,rot);
            return;
        }
        ServerShoot(pos,rot);
    }
    
    [ServerRpc]
    void ShootServerRPC(Vector3 pos,Quaternion rot)
    {
        ServerShoot(pos,rot);
    }

    void ServerShoot(Vector3 pos, Quaternion rot)
    {
//    public WeaponTypes Type;

        JSONWeapon wpn = GetWeapon();
        if (Ammo.Value > 0)
        {
            Ammo.Value--;
            if (Ammo.Value <= 0)
                Weapon.Value = "";
        }
        for (int n = 0; n < Math.Max(1, wpn.Shots); n++)
        {
            Vector3 r = rot.eulerAngles;
            r.y += Random.Range(-wpn.Accuracy, wpn.Accuracy);
            r.x += Random.Range(-wpn.Accuracy, wpn.Accuracy);
            ProjectileController p = Instantiate(God.Library.Projectile, pos,Quaternion.Euler(r));
            p.Setup(this,wpn);
        }
        
    }
    
    public void GetPoint(int amt=1,string targ="")
    {
        if (!IsServer)
        {
            GetPointServerRPC(amt);
            return;
        }
        ServerGetPoint(amt);
    }
    
    [ServerRpc]
    void GetPointServerRPC(int amt=1,string targ="")
    {
        ServerGetPoint(amt);
    }
    
    [ClientRpc]
    void GetPointClientRPC(int amt=1,string targ="")
    {
        if (IsServer) return;
        getPoint(amt);
//        Debug.Log("GOT POINT");
    }
    
    void ServerGetPoint(int amt=1,string targ="")
    {
        GetPointClientRPC();
        getPoint(amt);
    }

    void getPoint(int amt = 1,string targ="")
    {
//        Debug.Log("GET POINT: " + Name + " / " + amt + " / " + targ);
        God.LM.AwardPoint(this,amt,targ);
    }

    public bool OnGround()
    {
        return Floors.Count > 0;// && Physics.Raycast(transform.position,transform.position + new Vector3(0,-5,0),1.5f);
    }

    private void OnCollisionEnter(Collision other)
    {
        if (!Floors.Contains(other.gameObject))
            Floors.Add(other.gameObject);
        if (IsServer && Fling.Value != Vector3.zero && !JustKnocked)
        {
//            Debug.Log("ENDFLING");
            Fling.Value = Vector3.zero;
        }
    }

    private void OnCollisionExit(Collision other)
    {
        Floors.Remove(other.gameObject);
    }

    public int GetMaxHP()
    {
        return God.LM != null && God.LM.Ruleset != null && God.LM.Ruleset.PlayerHP > 0 ? God.LM.Ruleset.PlayerHP : 100;
    }
    
    public float GetMoveSpeed()
    {
        return God.LM != null && God.LM.Ruleset != null && God.LM.Ruleset.MoveSpeed > 0 ? God.LM.Ruleset.MoveSpeed : 10;
    }
    
    public float GetSprintSpeed()
    {
        float move = GetMoveSpeed();
        return God.LM != null && God.LM.Ruleset != null && God.LM.Ruleset.SprintSpeed > 0 ? God.LM.Ruleset.SprintSpeed * move : move * 1.5f;
    }
    
    public void Reset()
    {
        if (!IsServer) return;
        HP.Value = GetMaxHP();
        RandomSpawnServer();
        RB.velocity = Vector3.zero;
        Fling.Value = Vector3.zero;
        SetGhostMode(false);
    }

    public void TakeDamage(int amt, FirstPersonController source = null)
    {
        string src = source != null ? source.Name.Value.ToString() : "";
        if(IsServer) TakeDamageS(amt,source);
        else TakeDamageServerRpc(amt,src);
    }
    
    [ServerRpc]
    void TakeDamageServerRpc(int amt,string source="",Vector3 kb=new Vector3())
    {
        if (amt > 0)
        {
            FirstPersonController who = God.GetPlayer(source);
            TakeDamageS(amt, who);
        }

        if (kb != Vector3.zero)
        {
            TakeKnockbackS(kb);
        }
    }
    
    public void TakeDamageS(int amt,FirstPersonController source=null)
    {
        HP.Value -= amt;
        if (HP.Value <= 0)
        {
            Die(source);
        }
    }
    
    public void TakeHeal(int amt)
    {
        HP.Value += amt;
        if (HP.Value > GetMaxHP())
        {
            HP.Value = GetMaxHP();
        }
    }

    public void SetGhostMode(bool set)
    {
        if (set)
        {
            RB.velocity = Vector3.zero;
            MR.enabled = false;
            Coll.enabled = false;
            RB.isKinematic = true;
            GhostMode = true;
            transform.position = new Vector3(0,20,0);
        }
        else
        {
            MR.enabled = true;
            Coll.enabled = true;
            RB.isKinematic = false;
            GhostMode = false;
        }
    }

    public void Die(FirstPersonController source=null)
    {
        Debug.Log("KILLED BY " + source);
        
        if(God.LM.Respawn(this))
            Reset();
        else
            SetGhostMode(true);
        God.LM.NoticeDeath(this,source);
    }

    public override void OnDestroy()
    {
        base.OnDestroy();
        God.Players.Remove(this);
    }

    public void TakeKnockback(Vector3 kb)
    {
        if(IsServer) TakeDamageServerRpc(0,"",kb);
        TakeKnockbackS(kb);
        RB.velocity = kb;
        JustKnocked = true;
    }
    public void TakeKnockbackS(Vector3 kb)
    {
        RB.velocity = kb;
        Fling.Value = new Vector3(kb.x,0,kb.z);
//        Debug.Log("KB: " + kb);
        JustKnocked = true;
    }
}


