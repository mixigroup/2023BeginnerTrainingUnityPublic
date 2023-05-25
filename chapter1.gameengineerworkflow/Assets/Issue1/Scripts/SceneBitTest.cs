using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class SceneBitTest : MonoBehaviour
{
    public RectTransform bitlabel;
    public RectTransform bittoggle;
    public Text question;
    public Text result;
    public const int BITSIZE = 4;
    public Toggle[] toggles;

    /// <summary>
    /// @todo 1
    /// </summary>
    void TestPrint()
    {
        // @todo 1
        // 以下を入力する
        Debug.Log($"bool   : {sizeof(bool)  } byte");
        Debug.Log($"byte   : ???? byte"); 
        Debug.Log($"short  : ???? byte"); 
        Debug.Log($"ushort : ???? byte"); 
        Debug.Log($"Int16  : ???? byte"); 
        Debug.Log($"UInt16 : ???? byte"); 
        Debug.Log($"int    : ???? byte"); 
        Debug.Log($"uint   : ???? byte"); 
        Debug.Log($"Int32  : ???? byte"); 
        Debug.Log($"UInt32 : ???? byte"); 
        Debug.Log($"Int64  : ???? byte"); 
        Debug.Log($"UInt64 : ???? byte");

        // 関数定義の上に次のattributeを設定する
        // [ContextMenu("test print")]
    }

    //int checkbit(long i){ var r = i >> 1;
    int checkbit(ulong i, int o=0) => ((Mathf.Pow(2, o))<i) ? checkbit(i,++o):o;

    private void Awake()
    {
        // set menus
        ClearUIInfos();
        SetUIInfos();
        // show question
        NewGame();
    }

    private void Update()
    {
        gametimetxt.text =$"TIME : {gametime:f2} s";
    }

    void ClearUIInfos()
    {
        Enumerable.Range(0, bitlabel.parent.childCount)
            .Select(i=> bitlabel.parent.GetChild(i)).ToList()
            .ForEach(t =>
            {
                if (t != bitlabel) GameObject.Destroy(t.gameObject);
            });
        Enumerable.Range(0, bittoggle.parent.childCount)
            .Select(i => bittoggle.parent.GetChild(i)).ToList()
            .ForEach(t =>
            {
                if (t != bittoggle) GameObject.Destroy(t.gameObject);
            });
        ShowScore(false);
    }

    void SetUIInfos()
    {
        // @todo2
        // ------------------
        // ＜指示＞
        // (RectTransform)bitlabelと(RectTransform)bittoggleを BITSIZE だけ複製して
        // bitlabelのそれぞれのテキストには 1 から連番を振る
        // Toggleのvaluechangeイベントに、onToggleValueChangeをつける
        // ------------------
        // ＜実装解説＞
        // 順番ラベル
        //bitlabel.gameObject;
        // ビット
        //bittoggle.gameObject;
        // 複製には GameObject.Instantiate() を用いる
        // 
        // 同階層Component取得には obj.GetComponent<Text>() を用いる
        // テキストラベルには text.text = "ラベル" を用いる
        // ------------------

        // @todo4
        // ------------------
        // ＜指示＞
        // 連続出題の進行に正解と全クリアのテロップ（GameObject）を表示するため
        // 含まれているButtonのonClickイベントに以下のアクションをつけ
        // 現在は閉じておく
        // gotright には NextQuestion
        // gameclear には NewGame
        // ------------------
        // ＜実装解説＞
        // 課題正解
        //gotright;
        // ゲームクリア
        //gameclear;
        //
        // 下階層Component取得には obj.GetComponentInChildren<Button>(true) を用いる
        // onClickイベントには、btn.onClick.AddListener() を用いる
        // ------------------


        // 課題制回数
        clearcount = 0;
    }

    void onToggleValueChange(bool value)
    {
        Debug.Log($"{value} ({CheckValue})");
        // on value change action
        SetGameStep(CheckFlag());
    }
    void ClearToggles()
    {
        toggles.ToList().ForEach(c => c.SetIsOnWithoutNotify(false));
        result.text = $"result : {0}";
    }
    /// <summary>
    /// 出題に正解しているか検査してゲームを進める
    /// </summary>
    int CheckFlag()
    {
        var r = CheckValue;
        result.text = $"result : {r}";
        return r;
    }
    void SetGameStep(int r)
    {
        if (currectanswer == r)
        {
            ++clearcount;
            clearcounttxt.text = clearcount_label;
            if (clearcount >= GAMEQUESTSIZE)
            {
                this.enabled = false;
                gameclear.SetActive(true);
                gamecleartime.text = $"CLEAR TIME : {gametime:f2} s";
            }
            else
            {
                gotright.SetActive(true);
            }
        }
    }

    /// <summary>
    /// 選択しているBITの実数を得る
    /// @todo3
    /// </summary>
    int CheckValue { get { return 0;}}
    /// <summary>
    /// 新しい課題を出力する
    /// @todo4
    /// </summary>
    public void NextQuestion()
    {
        ClearToggles();
        gotright.SetActive(false);
        // @todo4
        // ------------------
        // ＜指示＞
        // 課題を選択する
        // BITSIZE の個数あるBビットフラグの実数をランダム選出して
        // question.textに表記して、currectanswer に登録する
        // ゲームスコア表示 ShowScore() を呼ぶ 
        // ------------------
        // ＜実装解説＞
        // 選択には UnityEngine.Random.Range(0, 上限) (maxは含まない値)　を用いる
        // ------------------
        currectanswer = 0;
        question.text = ""; // 課題テキスト
    }

    public void ShowScore(bool flg = true)
    {
        clearcounttxt.gameObject.SetActive(flg);
        gametimetxt.gameObject.SetActive(flg);
    }
    /// <summary>
    /// 
    /// </summary>
    public void NewGame()
    {
        clearcount = 0;
        clearcounttxt.text = clearcount_label;
        starttime = Time.realtimeSinceStartup;
        gameclear.SetActive(false);
        this.enabled = true;
        NextQuestion();
    }
    [SerializeField] float starttime;
    float gametime => Time.realtimeSinceStartup - starttime;
    string clearcount_label => $"COUNT : {clearcount} / {GAMEQUESTSIZE}";
    public int currectanswer;
    public int clearcount;
    public const int GAMEQUESTSIZE = 2;
    public GameObject gotright;
    public GameObject gameclear;
    public Text gamecleartime;
    public Text gametimetxt;
    public Text clearcounttxt;
}
