using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletManager : MonoBehaviour
{
    public static BulletManager instance;
    public BulletEntry[] entries;
    [SerializeField] int bulletindex = 0;
    private void Awake()
    {
        instance = this;
        entries = new BulletEntry[256];
    }
    private void OnDestroy()
    {
        instance = null;
    }
    private void FixedUpdate()
    {
        BulletUpdate();
    }
    /// <summary>
    /// 
    /// </summary>
    public static void Shot(GameObject bullet, Transform transform, Quaternion rotation, float bulletspeed)
    {
        instance?._Shot(bullet, transform, rotation, bulletspeed);
    }
    void _Shot(GameObject bullet, Transform transform, Quaternion rotate, float bulletspeed)
    {
        var ins = GameObject.Instantiate(bullet, transform.position,rotate, transform.parent);
        if (entries[bulletindex].target) GameObject.Destroy(entries[bulletindex].target.gameObject);
        entries[bulletindex++] = new BulletEntry { target = ins.GetComponent<Rigidbody2D>(), speed = bulletspeed };
        if (bulletindex >= entries.Length) bulletindex = 0;
    }
    public static void Dispose() => instance._Dispose();
    void _Dispose()
    {
        for (int i = 0; i < entries.Length; i++)
        {
            if (entries[i].target) GameObject.Destroy(entries[i].target.gameObject);
        }
    }
    void BulletUpdate()
    {
        if (screen.width!= Screensize.width ||screen.height != Screensize.height) screen = getscreenrect;
        for (int i = 0; i < entries.Length; i++)
        {
            if (entries[i].target)
            {
                var p = entries[i].target.position;
                var pp = entries[i].target.transform.up * entries[i].speed * GameSystemController.canvas.scaleFactor * Time.deltaTime;
                entries[i].target.MovePosition(p + new Vector2Int(Mathf.RoundToInt(pp.x), Mathf.RoundToInt(pp.y)));
                //
                if (!screen.Contains( entries[i].target.position))
                {
                    GameObject.Destroy(entries[i].target.gameObject);
                }
            }
        }
    }
    Rect screen;
    Rect getscreenrect => new Rect(0, 0, Screen.width, Screen.height);
}
[Serializable]
public struct BulletEntry
{
    public Rigidbody2D target;
    public float speed;
}


