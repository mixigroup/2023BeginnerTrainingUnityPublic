using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacter : MonoBehaviour
{
    RectTransform recttransform;
    public GameObject bullet;
    [Range(1,1000)]
    [SerializeField] float speed = 1;
    const float MOVECAPACITY = 40f;
    [SerializeField] Vector2 anchorpos;
    // Start is called before the first frame update
    void Awake()
    {
        recttransform = transform as RectTransform;
    }
    [SerializeField] Vector2 horizontal;
    // Update is called once per frame
    void Update()
    {
        //
        horizontal = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        MoveUpdate();
        //
        if(Input.GetButtonDown("Jump") || (Input.GetButton("Jump") && lastshottime + SHOTINTERVAL <= Time.realtimeSinceStartup))
        {
            lastshottime = Time.realtimeSinceStartup;
            BulletManager.Shot(bullet, transform, Quaternion.Euler(transform.up), bulletspeed);
        }
    }
    void MoveUpdate()
    {
        //Debug.Log($"movable? {(recttransform.position.x >= MOVECAPACITY && horizontal.x < 0)},{(recttransform.position.x <= (Screen.width - MOVECAPACITY) && horizontal.x > 0)}");
        if ((recttransform.anchoredPosition.x >= MOVECAPACITY && horizontal.x < 0)
            || (recttransform.anchoredPosition.x <= (Screensize.width - MOVECAPACITY) && horizontal.x > 0))
        {
            if (horizontal.x != 0f)
                recttransform.anchoredPosition += new Vector2Int(Mathf.RoundToInt((horizontal.x > 0 ? 1f : -1f) * speed * Time.deltaTime), 0);
            //
            //Debug.Log($"move pix ({Mathf.RoundToInt((horizontal.x > 0 ? 1f : -1f) * speed * Time.deltaTime)})");
        }
    }
    [Range(1f, 1000f)][SerializeField] float bulletspeed = 600;
    [SerializeField] float lastshottime;
    [Range(0.1f,3f)][SerializeField] float SHOTINTERVAL = 0.3f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Debug.Log("hit collision enter 2d (player)");
        GameSystemController.instance?.Death(this);
    }
    public void Dispose()
    {
        GameObject.Destroy(this.gameObject);
    }

}
