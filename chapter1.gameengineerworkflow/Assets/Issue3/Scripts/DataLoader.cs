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
	}
    /// <summary>
    /// ファイル読み込み
    /// @todo1
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
    /// @todo2
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
    /// @todo3
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="data"></param>
    /// <returns></returns>
    public static T ConvertData<T>(string data)
    {
        // T型のへの変換をReadCurrentDirを参考に実装する
        return default;
    }
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

