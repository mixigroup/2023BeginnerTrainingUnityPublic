using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
using UnityEngine;
using UnityEngine.UI;
public class GameSystemController : MonoBehaviour
{
    public static GameSystemController instance;
    public RectTransform inforoot;
    Text score;
    Text life;
    Text stage;
    public RectTransform teloproot;
    RectTransform telop_title;
    RectTransform telop_start;
    RectTransform telop_clear;
    RectTransform telop_gameover;
    RectTransform telop_newgame;
    //
    const string INFONAME_POINT     = "point";
    const string INFONAME_LIFE      = "life";
    const string INFONAME_STAGE     = "stage";
    //
    const string TELOPNAME_TITLE    = "title";
    const string TELOPNAME_START    = "start";
    const string TELOPNAME_CLEAR    = "clear";
    const string TELOPNAME_GAMEOVER = "gameover";
    const string TELOPNAME_NEWGAME  = "newgame";

    public RectTransform fieldroot;
    public PlayerCharacter playerprefab;
    public EnemyCharacter enemyprefab;
    public ObstacleCharacter obstacleprefab;
    public ScoreInfo scoreinfo;
    [SerializeField] GAMESTATE gamestate;
    [Serializable]
    public struct ScoreInfo
    {
        public const int DEFAULT_LIFE = 3;
        public int stagenum;
        public int scorepoint;
        public int lifecount;
    }
    private void Awake()
    {
        instance = this;
        _canvas = fieldroot.gameObject.GetComponentInParent<Canvas>(true);
        _scaler = fieldroot.gameObject.GetComponentInParent<CanvasScaler>(true);
        stageobj = new EnemyCharacter[INSTANCEMAX];
        SetupUITarget();
        SetGamestate(GAMESTATE.NONE);
    }
    private void OnDestroy()
    {
        instance = null;
    }
    [SerializeField] RectTransform testtarget;

    [ContextMenu("position test")]
    void positiontest()
    {
        Debug.Log($"_canvas... {_canvas} {_canvas.scaleFactor} {_canvas.pixelRect} {_canvas.renderingDisplaySize} {_scaler.referenceResolution}");
        Debug.Log($"fieldroot... {fieldroot.sizeDelta} {fieldroot.position} {fieldroot.anchoredPosition}");
        Debug.Log($"screen {Screensize.width}, {Screensize.height}");
        Debug.Log($"target... {testtarget.sizeDelta} {testtarget.position} {testtarget.anchoredPosition}");
    }
    // Start is called before the first frame update
    IEnumerator Start()
    {
        //positiontest();
        while (true)
        {
            switch(gamestate)
            {
                case GAMESTATE.TITLE:
                    if (Input.GetButton("Submit"))
                        SetGamestate(GAMESTATE.PLAYERINI);
                    break;
            }
            if (Input.GetButton("Cancel"))
                SkipNone();
            yield return null;
        }
    }
    public void SetGamestate(GAMESTATE s)
    {
        this.gamestate = s;
        switch(s)
        {
            case GAMESTATE.NONE:
                DeleteField();
                ScoreInit();
                ScoreUpdate();
                SetGamestate(GAMESTATE.TITLE);
                break;

            case GAMESTATE.TITLE:
                Telop(TELOPTARGET.TITLE);
                break;

            case GAMESTATE.PLAYERINI:
                TelopClear();
                CreatePlayer();
                StageSetupFromScore();
                break;

            case GAMESTATE.PLAY:
                Telop(TELOPTARGET.START);
                this.Invoke(nameof(TelopClear), SHOESTARTTIME);
                break;

            case GAMESTATE.STAGECLEAR:
                Telop(TELOPTARGET.CLEAR);
                this.Invoke(nameof(TelopClear), SHOEGAMECLEARTIME);
                Stagclear();
                this.Invoke(nameof(StageSetupFromScore), SHOEGAMECLEARTIME);
                break;

            case GAMESTATE.GAMEOVER:
                Telop(TELOPTARGET.GAMEOVER);
                Gameover();
                break;
        }
    }
    public void TelopClear() => Telop(TELOPTARGET.NONE);
    public void Telop(TELOPTARGET i)
    {
        telop_title.gameObject.SetActive(i == TELOPTARGET.TITLE);
        telop_start.gameObject.SetActive(i == TELOPTARGET.START);
        telop_clear.gameObject.SetActive(i == TELOPTARGET.CLEAR);
        telop_gameover.gameObject.SetActive(i == TELOPTARGET.GAMEOVER);
        telop_newgame.gameObject.SetActive(i == TELOPTARGET.NEWGAME);
    }

    void SetupUITarget()
    {
        // setup info
        var comps = inforoot.gameObject.GetComponentsInChildren<Text>(true);
        for (int i = 0; i < comps.Length; i++)
        {
            switch (comps[i].name)
            {
                case INFONAME_LIFE: life = comps[i]; break;
                case INFONAME_POINT: score = comps[i]; break;
                case INFONAME_STAGE: stage = comps[i]; break;
            }
        }
        // setup telop
        var telops = teloproot.gameObject.GetComponentsInChildren<RectTransform>(true);
        for (int i = 0; i < telops.Length; i++)
        {
            switch (telops[i].name)
            {
                case TELOPNAME_TITLE: telop_title = telops[i]; break;
                case TELOPNAME_START: telop_start = telops[i]; break;
                case TELOPNAME_CLEAR: telop_clear = telops[i]; break;
                case TELOPNAME_GAMEOVER: telop_gameover = telops[i]; break;
                case TELOPNAME_NEWGAME: telop_newgame = telops[i]; break;
            }
        }
    }

    public void DeleteField()
    {
        var r = new int[fieldroot.childCount].Select((c,i)=> fieldroot.GetChild(i));
        //Debug.Log($"delete {r.Count()}");
        r.ToList().ForEach(c => { if (c != fieldroot) GameObject.Destroy(c.gameObject); });
    }

    public void ScoreInit()
    {
        scoreinfo = new ScoreInfo() { stagenum = 0, lifecount = ScoreInfo.DEFAULT_LIFE };
    }
    public void ScoreUpdate()
    {
        score.text = $"SCORE: {scoreinfo.scorepoint:0000000}";
        life.text = $"LIFE x {scoreinfo.lifecount:00}";
        stage.text = $"STAGE {scoreinfo.stagenum+1:00}";
    }
    public static GameObject CharacterFactory(CharacterType tp, Color cl, Vector2 pos)
    {
        if (!instance) return default;
        //Debug.Log($"create factory {tp} {cl} {pos}");
        GameObject prefab = null;
        switch(tp)
        {
            case CharacterType.Player: prefab = instance.playerprefab.gameObject; break;
            case CharacterType.Enemy: prefab = instance.enemyprefab.gameObject; break;
            case CharacterType.Obstacle: prefab = instance.obstacleprefab.gameObject; break;
        }
        if (prefab)
        {
            var ins = GameObject.Instantiate(prefab, instance.fieldroot, false);
            ins.gameObject.GetComponent<Image>().color = cl;
            (ins.transform as RectTransform).anchoredPosition = pos;
            return ins;
        }
        else
        {
            return default;
        }
    }
    public void Death(ObstacleCharacter e)
    {
        GameObject.Destroy(e.gameObject);
    }
    public void Death(BulletCharacter e)
    {
        GameObject.Destroy(e.gameObject);
    }
    public void Death(PlayerCharacter e)
    {
        if (scoreinfo.lifecount > 1)
        {
            scoreinfo.lifecount -= 1;
            ScoreUpdate();
            e.Dispose();

            this.Invoke(nameof(CreatePlayer),RESPAWNTIME);
        }
        else
        {
            SetGamestate(GAMESTATE.GAMEOVER);
            e.Dispose();
        }
    }
    public void Death(EnemyCharacter e)
    {
        if (e)
        {
            for (int i = 0; i < stageobj.Length;i++)
            {
                if (stageobj[i] == e)
                {
                    stageobj[i] = null;
                    GameObject.Destroy(e.gameObject);
                }
            }
            scoreinfo.scorepoint += 100;
            ScoreUpdate();
        }
        if (stageobj.All(c => c == null))
        {
            SetGamestate(GAMESTATE.STAGECLEAR);
        }
    }
    public static void AddFieldObj(int i, EnemyCharacter e)
    {
        if (!instance) return;
        instance.stageobj[i] = e;
    }
    /// <summary>
    /// 
    /// </summary>
    void Gameover()
    {
        Debug.Log("game over");
        this.Invoke(nameof(SkipNone),SHOWGAMEOVERTIME);
    }
    void SkipNone() => SetGamestate(GAMESTATE.NONE);

    void Stagclear()
    {
        Debug.Log("stage clear");
        scoreinfo.stagenum += 1;
        StageClean();
    }
    void CreatePlayer()
    {
        //Debug.Log($"player : {Screensize.width}{new Vector2(Screensize.width / 2, 48)} {Screen.width} {canvas.pixelRect} {instance.fieldroot.rect}");
        player = CharacterFactory(CharacterType.Player, Color.red, new Vector2(Screensize.width / 2, 48))?.GetComponent<PlayerCharacter>();
    }
    [ContextMenu("StageClean")]
    public void StageClean()
    {
        stageobj.ToList().ForEach(c=>GameObject.Destroy(c));
        BulletManager.Dispose();
    }
    public void StageSetupFromScore()
    {
        StageMaker.StageSetup(scoreinfo.stagenum);
        StageMaker.CreateObstacle();
        SetGamestate(GAMESTATE.PLAY);
    }
    public TitleSetting GetTitleCharacter()
    {
        var result = new TitleSetting { };
        var targets = new List<TitleSetting.Target>();
        var r = telop_title.GetComponentsInChildren<RectTransform>(true);
        for (int i = 0; i < r.Length; i++)
        {
            if (r[i].name.StartsWith("player"))
            {
                result.player = new TitleSetting.Target { position = r[i].anchoredPosition, color = r[i].GetComponent<Image>().color };
            }
            if (r[i].name.StartsWith("enemy"))
            {
                targets.Add(new TitleSetting.Target { position = r[i].anchoredPosition, color = r[i].GetComponent<Image>().color });
                result.targets = targets.ToArray();
            }
        }
        return result;
    }
    public void SetTitleCharacter(TitleSetting setting)
    {
        RectTransform _p = null;
        RectTransform _e = null;
        var r = telop_title.GetComponentsInChildren<RectTransform>(true);
        for(int i =0;i< r.Length;i++)
        {
            if (r[i].name.StartsWith("player")) _p = r[i];
            if (r[i].name.StartsWith("enemy")) _e = r[i];
        }
        Action<TitleSetting.Target, GameObject> setaction = (c, g) =>
        {
            g.gameObject.SetActive(true);
            g.GetComponent<Image>().color = c.color;
            (g.transform as RectTransform).anchoredPosition = c.position;
        };
        if (_e)
        {
            _e.gameObject.SetActive(false);
            setting.targets?.ToList().ForEach(c =>
            {
                var g = GameObject.Instantiate(_e.gameObject, _e.transform.parent);
                setaction(c, g);
            });
        }
        if (_p)
        {
            _p.gameObject.SetActive(false);
            setaction(setting.player, _p.gameObject);
        }
    }
    public PlayerCharacter player;
    const int INSTANCEMAX = 256;
    const float RESPAWNTIME = 1.4f;
    const float SHOWTITLETIME = 1.5f;
    const float SHOWGAMEOVERTIME = 1.5f;
    const float SHOEGAMECLEARTIME = 2.0f;
    const float SHOESTARTTIME = 0.5f;
    public EnemyCharacter[] stageobj;
    [SerializeField] Canvas _canvas;
    [SerializeField] CanvasScaler _scaler;
    public static Canvas canvas => instance?._canvas;
    public static CanvasScaler scaler => instance?._scaler;
}
public static class Screensize
{
    //public static int width  => GameSystemController.scaler? (int) GameSystemController.scaler.referenceResolution.x:0;
    //public static int height => GameSystemController.scaler? (int) GameSystemController.scaler.referenceResolution.y:0;
    public static int width  => GameSystemController.instance? (int)GameSystemController.instance.fieldroot.rect.width :800;
    public static int height => GameSystemController.instance? (int)GameSystemController.instance.fieldroot.rect.height:600;
}
public enum CharacterType
{
    None = 0,
    Player = 1,
    Enemy = 10,
    Obstacle = 100,
}
public enum GAMESTATE
{
    NONE = 0,
    TITLE,
    PLAYERINI,
    PLAY,
    STAGECLEAR,
    GAMEOVER,
    NEWGAME,
}
public enum TELOPTARGET
{
    NONE = 0,
    TITLE,
    START,
    CLEAR,
    GAMEOVER,
    NEWGAME,
}
