using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ObstacleCharacter : MonoBehaviour
{
    public int hp = 3;
    [SerializeField] int starthp;
    private void Start()
    {
        starthp = hp;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        hp -= 1;
        this.gameObject.GetComponent<Image>().color = new Color(1f, (float) hp / starthp, (float)hp / starthp);
        if (hp <= 0) GameSystemController.instance.Death(this);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        hp -= 1;
        if (hp <= 0) GameSystemController.instance?.Death(this);
    }

}
