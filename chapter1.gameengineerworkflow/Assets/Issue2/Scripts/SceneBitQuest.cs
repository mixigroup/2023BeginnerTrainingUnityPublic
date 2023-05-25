using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class SceneBitQuest : MonoBehaviour
{
    public RectTransform bitlabel;
    public RectTransform bittoggle;
    public Text question;
    public Text result;
    public const int BITSIZE = 16;
    public Toggle[] toggles;

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
        if (timelimit <= 0)
        {
            gametimetxt.text = $"TIME : {0:f2} s";
            SetGamestop(GAMESTEP.GAMEOVER);
        }
        else
        {
            gametimetxt.text = $"TIME : {timelimit:f2} s";
        }
    }

    void ClearUIInfos()
    {
        Enumerable.Range(0, bittoggle.parent.childCount)
            .Select(i => bittoggle.parent.GetChild(i)).ToList()
            .ForEach(t =>
            {
                if (t != bittoggle) GameObject.Destroy(t.gameObject);
            });
    }

    void SetUIInfos()
    {
        clearcount = 0;
        errorcount = 0;
        toggles = new Toggle[BITSIZE];
        Enumerable.Range(0, BITSIZE).ToList().ForEach(i =>
        {
            GameObject b;
            if (i == 0)
            {
                b = bittoggle.gameObject;
            }
            else
            {
                b = GameObject.Instantiate(bittoggle.gameObject, bittoggle.parent);
            }
            toggles[i] = b.GetComponent<Toggle>();
            var label = b.GetComponentInChildren<Text>(true);
            label.text = (i+1).ToString();
            //toggles[i].name = (i + 1).ToString();
            toggles[i].onValueChanged.AddListener((f)=>onToggleValueChange(i,f));
        });
        gotright?.GetComponentInChildren<Button>(true).onClick.AddListener(NextQuestion);
        gotright?.SetActive(false);
        gameclear?.GetComponentInChildren<Button>(true).onClick.AddListener(ClickNextNewgame);
        gameclear?.SetActive(false);
        gameover?.GetComponentInChildren<Button>(true).onClick.AddListener(NewGame);
        gameover?.SetActive(false);
    }
    void ClickNextNewgame()
    {
        if (clicked) NewGame();
        clicked = true;
    }
    bool clicked;

    /// <summary>
    /// 指示されたモードのテロップを表示する
    ///
    /// ...表示/非表示は以下のAPIでboolを指定する
    /// gotright.SetActive(true)
    /// gameclear.SetActive(true)
    /// gameover.SetActive(true)
    /// @todo
    /// </summary>
    /// <param name="flag"></param>
    void SetGamestop(GAMESTEP flag)
    {
        this.enabled = false;
        // telop enable here
        gameclear.SetActive(false);
        gameover.SetActive(false);
        gotright.SetActive(false);
    }
    /// <summary>
    /// 与えたのBITFLAGにて正解しているか
    /// @todo
    /// </summary>
    /// <param name="flags"></param>
    /// <returns></returns>
    bool CheckValue(DOOR flags) {
        return false;
    }
    /// <summary>
    /// 選択のBITがSAFETYか検査する
    /// @todo
    /// </summary>
    /// <param name="i"></param>
    /// <returns></returns>
    bool SelectValue(UInt16 i)
    {
        return false;
    }
    /// <summary>
    /// 現在のBITFLAGを取得する
    /// @todo
    /// </summary>
    UInt16 currentselect
    {
        get { return 0; }
    }

    void onToggleValueChange(int i, bool value)
    {
        Debug.Log($"{i} -> {value} (select:{SelectValue((UInt16)i)}) " +
            $"(hold: {currentselect}) " +
            $"result:{CheckValue((DOOR)currentselect)} " +
            $"...{Mathf.Pow(2, i)}:{quest.ElementAt(clearcount).key}");
        // on value change action
        if (value) CheckFlag((UInt16)i);
    }

    void CheckFlag(UInt16 i)
    {
        var safe = SelectValue(i);
        var r = CheckValue((DOOR)currentselect);
        //
        if (safe)
        {
            if (r) result.text = $"Quest Success! ({currentselect})";
            else if (safe) result.text = $"Touch: {i}... {(safe ? "Safety!" : "failure...")} ({currentselect})";
            if (r)
            {
                ++clearcount;
                clearcounttxt.text = clearcount_label;
                if (clearcount >= GAMEQUESTSIZE)
                {
                    SetGamestop(GAMESTEP.GAMECLEAR);
                    gamecleartime.text = $"CLEAR TIME : {gametime:f2} s";
                }
                else
                {
                    SetGamestop(GAMESTEP.QUESTCLEAR);
                }
            }
        }
        else
        {
            result.text = $"Touch: {i}... {(safe ? "Safety!" : "failure...")} ({currentselect})";
            ++errorcount;
            clearcounttxt.text = clearcount_label;
            if (ERRORMAX <= errorcount) gameover.SetActive(true);
        }
    }

    void ClearToggles(string value = "")
    {
        toggles.ToList().ForEach(c => c.SetIsOnWithoutNotify(false));
        result.text = $"{value}";
        clearcounttxt.text = clearcount_label;
    }

    void onGameStart()
    {
        this.enabled = true;
        gameclear.SetActive(false);
        gameover.SetActive(false);
        gotright.SetActive(false);
    }

    public void NextQuestion()
    {
        ClearToggles();
        onGameStart();
        var data = quest.ElementAt(clearcount);
        question.text = $"(quest {clearcount+1}) {data.message}...";
        starttime = Time.realtimeSinceStartup;
    }
    public void NewGame()
    {
        //clearcount = 0;
        clicked = false;
        clearcount = 0;
        errorcount = 0;
        quest = defaultquestsetting;
        Debug.Log($"quest lange : {quest?.Count()}");
        quest = quest.OrderBy(p => Guid.NewGuid()).ToList();
        NextQuestion();
    }

    public enum GAMESTEP : byte
    {
        None = 0,
        QUESTCLEAR = 1 << 0,
        GAMECLEAR  = 1 << 1,
        GAMEOVER   = 1 << 2,
    }

    [SerializeField] float starttime;
    float gametime => Time.realtimeSinceStartup - starttime;
    float timelimit => GAMEOVERTIME - gametime;
    string clearcount_label => $"COUNT : <b>{clearcount}</b> / {GAMEQUESTSIZE} | LIFE : <b>{ERRORMAX-errorcount}</b> / {ERRORMAX}";
    public int currectanswer;
    public int clearcount;
    public int errorcount;
    public const int GAMEQUESTSIZE = 5;
    public const int ERRORMAX = 2;
    public const float GAMEOVERTIME = 10f;
    public GameObject gotright;
    public GameObject gameclear;
    public GameObject gameover;
    public Text gamecleartime;
    public Text gametimetxt;
    public Text clearcounttxt;
    [SerializeField] List<GAMECHAPTER> quest;
    readonly static List<GAMECHAPTER> defaultquestsetting = new List<GAMECHAPTER>
    {
        new GAMECHAPTER{key = DOOR.A | DOOR.B, message = $"DOOR.A と B を開け..."},
        // @todo
        // $"DOOR.B と H を開け..."
        // $"DOOR.C と D を開け..."
        // $"DOOR.D と A を開け..."
        // $"DOOR.E と G を開け..."
        // $"DOOR.F と D を開け..."
        // $"DOOR.G と E を開け..."
        // $"DOOR.H と G を開け..."
    };                                                                                                               
    [Serializable]
    public struct GAMECHAPTER
    {
        public DOOR key;
        public string message;
    }
    [Flags]
    public enum DOOR : UInt16
    {
        None=0,
        A = 1 << 0,
        B = 1 << 1,
        C = 1 << 2,
        D = 1 << 3,
        E = 1 << 4,
        F = 1 << 5,
        G = 1 << 6,
        H = 1 << 7,
        I = 1 << 8,
        J = 1 << 9,
        K = 1 << 10,
        L = 1 << 11,
        M = 1 << 12,
        N = 1 << 13,
        O = 1 << 14,
        P = 1 << 15,
    }


}
