#define ISSUE1_ENABLE
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

#if ISSUE1 && ISSUE1_ENABLE
    /// <summary>
    /// @todo 1
    /// </summary>
    [ContextMenu("test print")]
    void TestPrint()
    {
        Debug.Log($"bool   : {sizeof(bool)  } byte");
        Debug.Log($"byte   : {sizeof(byte)  } byte," +$" size ({checkbit(byte.MaxValue- byte.MinValue)})"                             + $",  {byte.MinValue}~{byte.MaxValue}"    );
        Debug.Log($"short  : {sizeof(short) } byte," +$" size ({checkbit((short.MaxValue- short.MinValue) + 1)})"                     + $",  {short.MinValue}~{short.MaxValue}"  );
        Debug.Log($"ushort : {sizeof(ushort)} byte," +$" size ({checkbit(ushort.MaxValue- ushort.MinValue)})"                         + $",  {ushort.MinValue}~{ushort.MaxValue}");
        Debug.Log($"Int16  : {sizeof(Int16) } byte," +$" size ({checkbit((ulong)((long)Int16.MaxValue- (long)Int16.MinValue) + 1)})"  + $",  {Int16.MinValue}~{Int16.MaxValue}"  );
        Debug.Log($"UInt16 : {sizeof(UInt16)} byte," +$" size ({checkbit((ulong)(UInt16.MaxValue- UInt16.MinValue))})"                + $",  {UInt16.MinValue}~{UInt16.MaxValue}");
        Debug.Log($"int    : {sizeof(int)   } byte," +$" size ({checkbit((ulong)((long)int.MaxValue-(long)int.MinValue) + 1)})"       + $",  {int.MinValue}~{int.MaxValue}"      );
        Debug.Log($"uint   : {sizeof(uint)  } byte," +$" size ({checkbit(uint.MaxValue- uint.MinValue)})"                             + $",  {uint.MinValue}~{uint.MaxValue}"    );
        Debug.Log($"Int32  : {sizeof(Int32) } byte," +$" size ({checkbit((ulong)((long)Int32.MaxValue - (long)Int32.MinValue) + 1)})" + $",  {Int32.MinValue}~{Int32.MaxValue}"  );
        Debug.Log($"UInt32 : {sizeof(UInt32)} byte," +$" size ({checkbit( (ulong)((ulong)UInt32.MaxValue-(ulong)UInt32.MinValue) )})" + $",  {UInt32.MinValue}~{UInt32.MaxValue}");
        Debug.Log($"Int64  : {sizeof(Int64) } byte," +$" size ({checkbit( (ulong)((ulong)UInt64.MaxValue-(ulong)UInt64.MinValue) )})" + $",  {Int64.MinValue}~{Int64.MaxValue}"  );
        Debug.Log($"UInt64 : {sizeof(UInt64)} byte," +$" size ({checkbit( (ulong)((ulong)UInt64.MaxValue-(ulong)UInt64.MinValue) )})" + $",  {UInt64.MinValue}~{UInt64.MaxValue}");
    }
    void _test()
    {
        // @todo 1
        // 以下を入力する
        Debug.Log($"bool   : {sizeof(bool)  } byte");
        Debug.Log($"byte   : {sizeof(byte)  } byte"); 
        Debug.Log($"short  : {sizeof(short) } byte"); 
        Debug.Log($"ushort : {sizeof(ushort)} byte"); 
        Debug.Log($"Int16  : {sizeof(Int16) } byte"); 
        Debug.Log($"UInt16 : {sizeof(UInt16)} byte"); 
        Debug.Log($"int    : {sizeof(int)   } byte"); 
        Debug.Log($"uint   : {sizeof(uint)  } byte"); 
        Debug.Log($"Int32  : {sizeof(Int32) } byte"); 
        Debug.Log($"UInt32 : {sizeof(UInt32)} byte"); 
        Debug.Log($"Int64  : {sizeof(Int64) } byte"); 
        Debug.Log($"UInt64 : {sizeof(UInt64)} byte");

        // 関数定義の上に次のattributeを設定する
        // [ContextMenu("test print")]
    }
#endif
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

#if ISSUE1 && ISSUE1_ENABLE
    void SetUIInfos()
    {
        clearcount = 0;
        toggles = new Toggle[BITSIZE];
        Enumerable.Range(0, BITSIZE).ToList().ForEach(i =>
        {
            GameObject t;
            GameObject b;
            if (i == 0)
            {
                t = bitlabel.gameObject;
                b = bittoggle.gameObject;
            }
            else
            {
                t = GameObject.Instantiate(bitlabel.gameObject, bitlabel.parent);
                b = GameObject.Instantiate(bittoggle.gameObject, bittoggle.parent);
            }
            t.GetComponent<Text>().text = (i + 1).ToString();
            toggles[i] = b.GetComponent<Toggle>();
            //toggles[i].name = (i + 1).ToString();
            toggles[i].onValueChanged.AddListener(onToggleValueChange);
        });
        gotright?.GetComponentInChildren<Button>(true).onClick.AddListener(NextQuestion);
        gotright?.SetActive(false);
        gameclear?.GetComponentInChildren<Button>(true).onClick.AddListener(NewGame);
        gameclear?.SetActive(false);
    }
#else
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

#endif

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


#if ISSUE1 && ISSUE1_ENABLE
    /// <summary>
    /// 選択しているBITの実数を得る
    /// @todo
    /// </summary>
    int CheckValue => toggles.Select((c, i) => c.isOn ? (int)Mathf.Pow(2, i) : 0).Sum();
    /// <summary>
    /// 新しい課題を出力する
    /// @todo
    /// </summary>
    public void NextQuestion()
    {
        ClearToggles();
        gotright.SetActive(false);
        while (true)
        {
            var i = UnityEngine.Random.Range(0, (int)Mathf.Pow(2, BITSIZE)); // maxは含まない値
            if (currectanswer != i)
            {
                currectanswer = i;
                break;
            }
        }
        question.text = $"(question) Enter the number {currectanswer} in bits";
        ShowScore(true);
    }
#else
    /// <summary>
    /// 選択しているBITの実数を得る
    /// @todo
    /// </summary>
    int CheckValue { get { return 0;}}
    /// <summary>
    /// 新しい課題を出力する
    /// @todo
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
#endif
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
