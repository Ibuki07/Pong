using Cysharp.Threading.Tasks;
using System.IO;
using UnityEngine;

public class DataManager : MonoBehaviour
{


    public SaveData SaveData;     // json変換するデータのクラス
    private string _filePath;                            // jsonファイルのパス
    private string _fileName = "SaveData.json";              // jsonファイル名

    private void Awake()
    {
        // パス名取得
        _filePath = Application.dataPath + "/" + _fileName;

        // ファイルがないとき、ファイル作成
        if (!File.Exists(_filePath))
        {
            Save();
        }

        // ファイルを読み込んでdataに格納
        Load();
    }

    private void Save()
    {
        Debug.Log("SaveData:" + SaveData.VolumeValues[0]);
        string json = JsonUtility.ToJson(SaveData);
        StreamWriter streamWriter = new StreamWriter(_filePath);
        streamWriter.Write(json); 
        streamWriter.Flush();
        streamWriter.Close();
        Debug.Log("JsonNData:" + json);
    }

    private void Load()
    {
        if (File.Exists(_filePath))
        {
            StreamReader streamReader;
            streamReader = new StreamReader(_filePath);
            string data = streamReader.ReadToEnd();
            streamReader.Close();
            SaveData = JsonUtility.FromJson<SaveData>(data);
            Debug.Log("LoadData:" + SaveData.VolumeValues[0]);
        }
    }

    // ゲーム終了時に保存
    private void OnDestroy()
    {
        Save();
    }

    // 保存したデータを削除
    public void DeleteSaveData()
    {
        // データを初期化する
        SaveData = new SaveData();
        // ファイルを削除する
        File.Delete(_filePath);
        if (!File.Exists(_filePath))
        {
            Save();
        }
    }
}