using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConveyorController : ThingController
{
    public List<CharController> Touching;
    public Vector3 Movement;
    public float Speed = 0.5f;

    public override void ApplyJSON(JSONData data)
    {
        base.ApplyJSON(data);
        if (data.Amount > 0)
            Speed = data.Amount;
    }

    void LateUpdate()
    {
        Vector2 move = (Touching.Count > 0
            ? Movement * (Speed * GameSettings.CurrentPlayerSpeed)
            : Vector3.zero);
        foreach (CharController cc in Touching.ToArray())
        {
            if (cc.Belted) continue;
            cc.Belted = true;
            //Debug.Log("MOVE: " + move + " / " + Speed + " / " + GameSettings.CurrentPlayerSpeed );
//            cc.RB.MovePosition(cc.transform.position + move);
            cc.RB.velocity += move;
        }
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        CharController cc = other.gameObject.GetComponent<CharController>();
        if (cc != null && !Touching.Contains(cc))
        {
            Touching.Add(cc);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        CharController cc = other.gameObject.GetComponent<CharController>();
        if (cc != null)
        {
            Touching.Remove(cc);
        }
    }
}
