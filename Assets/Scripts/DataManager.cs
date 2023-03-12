using System.IO;
using UnityEngine;

namespace Managers
{
    public class DataManager : MonoBehaviour
    {
        // SaveDataクラスのインスタンス
        public SaveData SaveData = new SaveData();

        // 保存先のパスを保持する変数
        private string _filePath;

        // 保存するファイル名
        private const string FileName = "SaveData.json";

        private void Awake()
        {
            // 保存先のパスを設定する
            _filePath = Path.Combine(Application.persistentDataPath, FileName);

            // ファイルが存在しない場合は保存する
            if (!File.Exists(_filePath))
            {
                Save();
            }

            // ファイルからデータを読み込む
            Load();
        }

        private void OnDestroy()
        {
            Save();
        }

        // SaveDataをJSONファイルに保存する
        private void Save()
        {
            // SaveDataをJSON形式の文字列に変換する
            string json = JsonUtility.ToJson(SaveData);

            // StreamWriterでファイルに書き込む
            using (StreamWriter streamWriter = new StreamWriter(_filePath))
            {
                streamWriter.Write(json);
            }
        }

        // JSONファイルからデータを読み込む
        public void Load()
        {
            if (File.Exists(_filePath))
            {
                // StreamReaderでファイルを読み込み、JSON形式の文字列を取得する
                using (StreamReader streamReader = new StreamReader(_filePath))
                {
                    string data = streamReader.ReadToEnd();

                    // JSON形式の文字列からSaveDataクラスのインスタンスを復元する
                    SaveData = JsonUtility.FromJson<SaveData>(data);
                }
            }
        }

        // 保存されたデータを削除する
        public void DeleteSaveData()
        {
            // SaveDataのデータを初期化する
            SaveData = new SaveData();

            // ファイルを削除する
            File.Delete(_filePath);

            // ファイルが存在しない場合は保存する
            if (!File.Exists(_filePath))
            {
                Save();
            }
        }
    }
}