using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CameraController : MonoBehaviour
{
    public GameObject Target;
    private Rect Bounds;
    private Camera Cam;
    public SpriteRenderer Fader;

    void Start()
    {
        Cam = GetComponent<Camera>();
        Rect bounds = new Rect();
        Collider2D[] plats = BoxCollider2D.FindObjectsOfType<Collider2D>();
        foreach(Collider2D coll in plats)
        {
            GameObject p = coll.gameObject;
            bounds.x = Mathf.Min(bounds.x,p.transform.position.x - coll.bounds.extents.x);
            bounds.width = Mathf.Max(bounds.width,p.transform.position.x + coll.bounds.extents.x);
            bounds.y = Mathf.Min(bounds.y,p.transform.position.y - coll.bounds.extents.y);
            bounds.height = Mathf.Max(bounds.height,p.transform.position.y + coll.bounds.extents.y);
        }

        bounds.x += 9;
        bounds.width -= 9;
        bounds.y += 5;
        bounds.height -= 5;
        Bounds = bounds;
        StartCoroutine(ShowName());
    }
    
    void FixedUpdate()
    {
        Vector3 desired = Target.transform.position;
        desired.z = transform.position.z;
        desired.x = Mathf.Clamp(desired.x,Bounds.x, Bounds.width);
        desired.y = Mathf.Clamp(desired.y,Bounds.y, Bounds.height);
        transform.position = Vector3.Lerp(transform.position,desired,0.3f);
        transform.position = Vector3.MoveTowards(transform.position,desired,0.01f);
    }

    IEnumerator ShowName()
    {
        GameObject name = new GameObject("Name");
        TextMeshPro tmp = name.AddComponent<TextMeshPro>();
        name.transform.SetParent(transform);
        name.transform.localPosition = new Vector3(0,0,1.1f);
        tmp.text = SceneManager.GetActiveScene().name.ToUpper();
        tmp.alignment = TextAlignmentOptions.Center;
        yield return new WaitForSeconds(1);
        Destroy(name);

    }
}
