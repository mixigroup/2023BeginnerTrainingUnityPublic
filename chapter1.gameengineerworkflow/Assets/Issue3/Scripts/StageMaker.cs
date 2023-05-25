using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class StageMaker : MonoBehaviour
{
    [SerializeField] int teststage;
    [ContextMenu("StageSetup")]
    void TestStageSetup()
    {
        StageSetup(teststage);
    }

    public static StageMaker instance;
    private void Awake()
    {
        instance = this;
    }
#if ISSUE2 && ENABLE_WEBLOADSTAGE
    const string url = "https://tinyurl.com/20230508newemployee";
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    private IEnumerator Start()
    {
        var r = UnityEngine.Networking.UnityWebRequest.Get(url);
        yield return r.SendWebRequest();
        if (!string.IsNullOrEmpty(r.error))
        {
            Debug.LogWarning($"network error \n{r.error}");
            yield break;
        }
        else
        {
            Debug.Log($"{r.downloadHandler.text}");
            SetStageSettingFromString(r.downloadHandler.text);
        }
        yield break;
    }
#elif ISSUE2 && ENABLE_AUTOLOADSTAGE
    private void Start()
    {
        // @todo
        // status setup from jsonsetting
        ImportStageSettingFromFile();
    }
#else
    private void Start()
    {
        // @todo
        // status setup from jsonsetting
    }
#endif
    private void OnDestroy()
    {
        instance = null;
    }
    /// <summary>
    /// @todo
    /// </summary>
    /// <param name="str"></param>
    void SetStageSettingFromString(string str)
    {
        // @todo
        // convert from text
        //DataLoader.ConvertData<StageSettingList>()
        //変換したStageSettingListは StageMaker.ImportStageSettingFromFile()と同じ処理で
#if ISSUE2
        var r = DataLoader.ConvertData<StageSettingList>(str);
        enemystatus = r.enemystatus;
        fieldsetting = r.fieldsetting.Select(c => c.d.Select(cc => cc.d).ToArray()).ToArray();
        GameSystemController.instance?.SetTitleCharacter(r.titlesetting);
#endif
    }
    [ContextMenu("export current setting")]
    void ExportCurrentStageSettingToFile()
    {
        // @todo
        // status export to json
        //new StageSettingList { }
        //StageSettingList.titlesetting に GameSystemController.instance.GetTitleCharacter();
        //StageSettingList.fieldsetting に this.fieldsetting.Select(c => new IntArAr { d = c.Select(cc => new IntAr { d = cc }).ToArray() }).ToArray()
        //StageSettingList.enemystatus に this.enemystatus
        //DataLoader.WriteCurrentDir(settingfilename, data);
#if ISSUE2
        var titlesetting = GameSystemController.instance? GameSystemController.instance.GetTitleCharacter() : default;
        var d = new StageSettingList{
            fieldsetting = fieldsetting.Select(c => new IntArAr { d = c.Select(cc => new IntAr { d = cc}).ToArray() }).ToArray(),
            enemystatus = enemystatus,
            titlesetting = titlesetting,
        };
        Debug.Log($"string: {JsonUtility.ToJson(d)}");
        DataLoader.WriteCurrentDir(settingfilename, d);
#endif
    }
    [ContextMenu("import stage setting")]
    void ImportStageSettingFromFile()
    {
        // @todo
        //DataLoader.ReadCurrentDir<StageSettingList>(settingfilename)
        //StageSettingList.enemystatus　を this.enemystatus に
        //StageSettingList.fieldsetting　を this.fieldsetting に
        //int[][][]変換には　data.fieldsetting.Select(c => c.d.Select(cc => cc.d).ToArray()).ToArray()
        //StageSettingList.titlesetting　を　GameSystemController.SetTitleCharacter()
#if ISSUE2
        var r = DataLoader.ReadCurrentDir<StageSettingList>(settingfilename);
        Debug.Log("string: "+JsonUtility.ToJson(r));
        enemystatus = r.enemystatus;
        fieldsetting = r.fieldsetting.Select(c=>c.d.Select(cc=>cc.d).ToArray()).ToArray();
        GameSystemController.instance?.SetTitleCharacter(r.titlesetting);
#endif
    }
    readonly string settingfilename = "stagesetting.json";
    /// <summary>
    /// 
    /// </summary>
    /// <param name="stage"></param>
    public static void StageSetup(int stage = 0)
    {
        instance?._StageSetup(stage);
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="stage"></param>
    void _StageSetup(int stage)
    {
        var fd = fieldsetting[Mathf.Min(stage, fieldsetting.Length - 1)];
        instance?.StageSetupInner(new StageOrder { data = fd, status = enemystatus });
    }
    /// <summary>
    /// 
    /// </summary>
    public static void CreateObstacle() => instance?._CreateObstacle();
    /// <summary>
    /// 
    /// </summary>
    void _CreateObstacle()
    {
        //Debug.Log($"obstable create... {obstaclesetting.Length}");
        obstacles.Clear();
        Vector2 startpos = StageMaker.createobstacleposition(obstaclesetting.Length);
        var r = obstaclesetting.Select((c,i) =>
        {
            //Debug.Log($"obstable {i}, {c},{startpos} , {(obstaclestep * c)} {(startpos + (obstaclestep * c))}");
            if (c>0) GameSystemController.CharacterFactory(CharacterType.Obstacle, Color.white, startpos + (obstaclestep * i)).GetComponent<ObstacleCharacter>();
            return true;
        }).All(c=>true);
    }
    readonly static Vector2 createobstacleoffset = new Vector2(0, 94);
    readonly static Vector2 obstaclestep = new Vector2(40, 0);
    [NonSerialized] int[] obstaclesetting = new int[] { 0, 1, 1, 0, 1, 1, 0, 1, 1, 0, 1, 1 };
    [SerializeField] List<ObstacleCharacter> obstacles = new List<ObstacleCharacter>();
    public static Vector2 createobstacleposition(int WIDTHCHARA) => new Vector2(Mathf.CeilToInt(Screensize.width * 0.5f - setposmergin.x * (WIDTHCHARA * 0.5f)), 0f) + createobstacleoffset;

    static readonly Vector2 setposmergin = Vector2.one * 40;
    static readonly Vector2 createoffset = new Vector2(0, -64);
    public static Vector2 createposition(int WIDTHCHARA) => new Vector2(Mathf.CeilToInt(Screensize.width*0.5f - setposmergin.x * (WIDTHCHARA*0.5f)), Screensize.height) + createoffset;
    /// <summary>
    /// 
    /// </summary>
    /// <param name="setting"></param>
    public void StageSetupInner(StageOrder setting)
    {
        var f = setting.data;
        Vector2 _createpos = createposition(f[0].Length);
        int cnum = 0;
        
        for (var y = 0; y < f.Length; y++)
        {
            for (var x = 0; x < f[y].Length; x++)
            {
                if (f[y][x] != 0)
                {
                    var e = GameSystemController.CharacterFactory(
                        CharacterType.Enemy,
                        setting.status[f[y][x]].color,
                        _createpos + Vector2.Scale(setposmergin, new Vector2(x, -y))
                    ).GetComponent<EnemyCharacter>();
                    e.SetSetting(setting.status[f[y][x]]);
                    GameSystemController.AddFieldObj(cnum++, e);
                }
            }
        }
    }
#if ISSUE2 && ENABLE_AUTOLOADSTAGE
    [NonSerialized] EnemySetting[] enemystatus;
    [NonSerialized] int[][][] fieldsetting;

#else
    [NonSerialized] EnemySetting[] enemystatus = new EnemySetting[] {
            new EnemySetting{},
            new EnemySetting { bulletspeed = 100, hp = 1, shooter = false, shotinterval = 0.0f, color = Color.white,  }, //1
            new EnemySetting { bulletspeed = 100, hp = 1, shooter = false, shotinterval = 0.0f, color = Color.yellow, }, //2
            new EnemySetting { bulletspeed = 100, hp = 1, shooter = true , shotinterval = 3.5f, color = Color.blue,   }, //3
            new EnemySetting { bulletspeed = 100, hp = 1, shooter = true , shotinterval = 3.5f, color = Color.cyan,   }, //4
            new EnemySetting { bulletspeed = 100, hp = 2, shooter = true , shotinterval = 3.5f, color = Color.green,  }, //5
            new EnemySetting { bulletspeed = 100, hp = 2, shooter = true , shotinterval = 2.0f, color = Color.white,  }, //6
            new EnemySetting { bulletspeed = 100, hp = 3, shooter = true , shotinterval = 2.0f, color = Color.yellow, }, //7
            new EnemySetting { bulletspeed = 100, hp = 3, shooter = true , shotinterval = 1.0f, color = Color.blue,   }, //8
            new EnemySetting { bulletspeed = 100, hp = 4, shooter = true , shotinterval = 0.8f, color = Color.gray,   }, //9
        };

    [NonSerialized] int[][][] fieldsetting = new int[][][]
    {
            new int[][]
            {
                new int[]{ 1,1,1,1,1,5,1,1,1,1,1,1 },
            },
            new int[][]
            {
                new int[]{ 1,1,1,1,4,5,4,1,1,1,1,1 },
                new int[]{ 0,0,2,2,2,2,2,2,2,2,0,0 },
            },
            new int[][]
            {
                new int[]{ 1,1,1,1,4,5,4,1,1,1,1,1 },
                new int[]{ 0,0,0,2,2,2,2,2,2,0,0,0 },
                new int[]{ 0,0,0,0,3,3,3,3,0,0,0,0 },
            },
            new int[][]
            {
                new int[]{ 1,1,1,1,4,5,4,1,1,1,1,1 },
                new int[]{ 2,2,2,2,2,2,2,2,2,2,2,2 },
                new int[]{ 3,3,3,3,3,3,3,3,3,3,3,3 },
            },
            new int[][]
            {
                new int[]{ 1,1,1,1,8,9,8,1,1,1,1,1 },
                new int[]{ 2,2,2,2,2,2,2,2,2,2,2,2 },
                new int[]{ 3,3,3,3,3,3,3,3,3,3,3,3 },
                new int[]{ 4,4,4,4,4,4,4,4,4,4,4,4 },
                new int[]{ 5,5,5,5,5,5,5,5,5,5,5,5 },
            },
            new int[][]
            {
                new int[]{ 1,1,1,1,8,9,8,1,1,1,1,1 },
                new int[]{ 1,1,1,1,1,1,1,1,1,1,1,1 },
                new int[]{ 2,2,2,2,2,2,2,2,2,2,2,2 },
                new int[]{ 2,2,2,2,2,2,2,2,2,2,2,2 },
                new int[]{ 3,3,3,3,3,3,3,3,3,3,3,3 },
                new int[]{ 3,3,3,3,3,3,3,3,3,3,3,3 },
                new int[]{ 4,4,4,4,4,4,4,4,4,4,4,4 },
                new int[]{ 4,4,4,4,4,4,4,4,4,4,4,4 },
                new int[]{ 5,5,5,5,5,5,5,5,5,5,5,5 },
                new int[]{ 5,5,5,5,5,5,5,5,5,5,5,5 },
            },
            new int[][]
            {
                new int[]{ 3,3,3,3,8,9,8,3,3,3,3,3 },
                new int[]{ 3,3,3,3,3,3,3,3,3,3,3,3 },
                new int[]{ 4,4,4,4,4,4,4,4,4,4,4,4 },
                new int[]{ 4,4,4,4,4,4,4,4,4,4,4,4 },
                new int[]{ 5,5,5,5,5,5,5,5,5,5,5,5 },
                new int[]{ 5,5,5,5,5,5,5,5,5,5,5,5 },
                new int[]{ 0,6,6,6,6,6,6,6,6,6,6,0 },
                new int[]{ 0,0,0,7,7,7,7,7,7,0,0,0 },
            },
            new int[][]
            {
                new int[]{ 4,4,4,4,8,9,8,4,4,4,4,4 },
                new int[]{ 4,4,4,4,4,4,4,4,4,4,4,4 },
                new int[]{ 5,5,5,5,5,5,5,5,5,5,5,5 },
                new int[]{ 5,5,5,5,5,5,5,5,5,5,5,5 },
                new int[]{ 6,6,6,6,6,6,6,6,6,6,6,6 },
                new int[]{ 6,6,6,6,6,6,6,6,6,6,6,6 },
                new int[]{ 7,7,7,7,7,7,7,7,7,7,7,7 },
                new int[]{ 7,7,7,7,7,7,7,7,7,7,7,7 },
            },
        };
#endif
}
#if ISSUE2
[Serializable]
public struct StageSettingList
{
    public EnemySetting[] enemystatus;
    public IntArAr[] fieldsetting;
    public TitleSetting titlesetting;
}
[Serializable]
public struct TitleSetting
{
    public Target[] targets;
    public Target player;
    [Serializable]
    public struct Target
    {
        public Vector2 position;
        public Color color;
    }
}
[Serializable]
public struct IntArAr
{
    public IntAr[] d;
}
[Serializable]
public struct IntAr
{
    public int[] d;
}
#endif

[Serializable]
public struct StageOrder
{
    public int[][] data;
    public EnemySetting[] status;
}

