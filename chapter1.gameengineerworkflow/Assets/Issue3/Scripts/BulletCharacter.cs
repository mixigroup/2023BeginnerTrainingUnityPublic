using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletCharacter : MonoBehaviour
{
    public int hp;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        hp -= 1;
        if (hp <= 0) GameSystemController.instance?.Death(this);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        hp -= 1;
        if (hp <= 0) GameSystemController.instance?.Death(this);
    }

}
