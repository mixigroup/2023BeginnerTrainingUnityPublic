using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;

public class DataLoader : MonoBehaviour
{
	public string filename1 = "setting1.json";
    public string filename2 = "setting2.json";
    public SettingData setting;
	public Text console;
	public Button loadbutton1;
    public Button loadbutton2;

    private void Awake()
    {
        console.text = "ready..";

        loadbutton1.onClick.AddListener(()=>GetSetting(filename1));
        loadbutton2.onClick.AddListener(()=>GetSetting(filename2));
    }

    public void GetSetting(string _name)
	{
		setting = Read<SettingData>(_name);
		console.text = setting.ToString();

    }

	public static T Read<T>(string filename)
	{
		var path = $"{Application.streamingAssetsPath}/{filename}";
        Debug.Log($"Read request {path} ... {File.Exists(path)}");
		return File.Exists(path)? JsonUtility.FromJson<T>(File.ReadAllText(path)): default;
	}
#if ISSUE2 && true
    /// <summary>
    /// ファイル読み込み
    /// @todo
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="filename"></param>
    /// <returns></returns>
    public static T ReadCurrentDir<T>(string filename)
    {
        var path = $"{Directory.GetCurrentDirectory()}/{filename}";
        Debug.Log($"Read request {path} ... {File.Exists(path)}");
        return File.Exists(path) ? JsonUtility.FromJson<T>(File.ReadAllText(path)) : default;
    }
    /// <summary>
    /// ファイル保存
    /// @todo
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="filename"></param>
    /// <param name="data"></param>
    public static void WriteCurrentDir<T>(string filename, T data)
    {
        var path = $"{Directory.GetCurrentDirectory()}/{filename}";
        Debug.Log($"Write request {path} ... {File.Exists(path)}");
        File.WriteAllText(path, JsonUtility.ToJson(data));
    }
    /// <summary>
    /// stringデータのT型変換
    /// @todo
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="data"></param>
    /// <returns></returns>
    public static T ConvertData<T>(string data)
    {
        return JsonUtility.FromJson<T>(data);
    }
#else
    /// <summary>
    /// ファイル読み込み
    /// @todo
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="filename"></param>
    /// <returns></returns>
    public static T ReadCurrentDir<T>(string filename)
    {
        // ＜実装解説＞
        // 処理APIは以下を利用して組み立てる
        //Directory.GetCurrentDirectory()
        //File.Exists(path)
        //File.ReadAllText(path)
        //JsonUtility.FromJson<T>(str)
        // Tは型情報でReadCurrentDir()呼び出し時に与えTで受け取り
        // FromJsom<T>()で使用する
        // 返却時に型を合わせるcastが必要なら data as T、もしくは (T) data とcastする
        return default;
    }
    /// <summary>
    /// ファイル保存
    /// @todo
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="filename"></param>
    /// <param name="data"></param>
    public static void WriteCurrentDir<T>(string filename, T data)
    {
        // ＜実装解説＞
        // 処理APIは以下を利用して組み立てる
        //Directory.GetCurrentDirectory()
        //JsonUtility.ToJson(data)
        //File.WriteAllText(path, str);
    }
    /// <summary>
    /// stringデータのT型変換
    /// @todo
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="data"></param>
    /// <returns></returns>
    public static T ConvertData<T>(string data)
    {
        // T型のへの変換をReadCurrentDirを参考に実装する
        return default;
    }
#endif
}

[Serializable]
public struct SettingData
{
	public string name;
	public int value;
    public override string ToString()
    {
        return $"({name}:{value})";
    }
}

