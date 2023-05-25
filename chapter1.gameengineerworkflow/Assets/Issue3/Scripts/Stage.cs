using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Stage : MonoBehaviour
{
    public GameObject player;
    public GameObject enemy;
    /// <summary>
    /// 
    /// </summary>
    private void Start()
    {
        var rect = (transform as RectTransform).rect;
        var p = GameObject.Instantiate(player, transform).transform as RectTransform;
        p.anchoredPosition = new Vector2(rect.width/2, 50);

        int colum = 12;
        Vector2 size = new Vector2(40,40);
        Vector2 createpos = new Vector2(rect.width/2, rect.height-40);
        Enumerable.Range(0, 60).ToList().ForEach(i =>
        {
            var e = GameObject.Instantiate(enemy, transform).transform as RectTransform;
            e.anchoredPosition = createpos - new Vector2(size.x * ((i % colum) - colum/2), size.y * Mathf.FloorToInt(i/colum));
        });
        // 解説を消す
        transform.Find("infomation").gameObject.SetActive(false);
    }
}
