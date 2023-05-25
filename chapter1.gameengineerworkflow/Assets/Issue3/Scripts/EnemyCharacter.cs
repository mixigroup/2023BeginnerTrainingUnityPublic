using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
public class EnemyCharacter : MonoBehaviour
{
    [SerializeField] EnemySetting setting;
    public int hp              { get { return setting.hp; } private set { setting.hp = value; }}
    public bool shooter        { get { return setting.shooter; }}
    public float bulletspeed   { get { return setting.bulletspeed; }}
    public float shotinterval  { get { return setting.shotinterval; }}

    /// <summary>
    /// 
    /// </summary>
    /// <param name="setting"></param>
    public void SetSetting(EnemySetting setting)
    {
        this.setting = setting;
        this.gameObject.GetComponent<Image>().color = setting.color;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Debug.Log("hit collision enter 2d (enemy)");
        this.hp -= 1;
        if (this.hp <= 0) GameSystemController.instance?.Death(this);
    }
    private void Awake()
    {
        lastchecktime = 0f;
        movecounter = (int)MOVERANGE/2; // center start
    }
    private void Update()
    {
        if (lastchecktime + MOVEINTERVAL <= Time.realtimeSinceStartup)
        {
            var rt = transform as RectTransform;
            lastchecktime = Time.realtimeSinceStartup;
            var p = rt.anchoredPosition;
            if (test) Debug.Log($"Update {movecounter},{Mathf.CeilToInt(movecounter / MOVERANGE)},{(Mathf.CeilToInt(movecounter / MOVERANGE) % 2)},{((Mathf.CeilToInt(movecounter / MOVERANGE) % 2) == 0 ? 1 : -1)}");
            p.x += movesize * ((Mathf.CeilToInt(movecounter++ / MOVERANGE)%2)==0 ? 1 : -1);
            if (movecounter%(MOVERANGE*2) == 0 && movecounter / MOVERANGE < MOVEHEIGHT) p.y += movesize * -2;
            rt.anchoredPosition = p;
        }

        if (shooter && lastshottime + shotinterval <= Time.realtimeSinceStartup)
        {
            lastshottime = Time.realtimeSinceStartup;
            BulletManager.Shot(bullet, transform, Quaternion.Euler(0f,0f,180f), bulletspeed);
        }
    }
    [SerializeField] float lastshottime;
    [SerializeField] bool test;
    float lastchecktime;
    int movecounter;
    public const int movesize = 20;
    public const int MOVERANGE = (1920/2) / 20;
    public const int MOVEHEIGHT = 12;
    public const float MOVEINTERVAL = 0.3f;
    public GameObject bullet;
}
[Serializable]
public struct EnemySetting
{
    public bool shooter;
    public float bulletspeed;
    public int hp;
    public float shotinterval;
    public Color color;
    
}