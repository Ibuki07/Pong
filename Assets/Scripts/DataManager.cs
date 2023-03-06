using System.IO;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    [HideInInspector] public SaveData saveData;     // json変換するデータのクラス
    private string _filePath;                            // jsonファイルのパス
    private string _fileName = "SaveData.json";              // jsonファイル名

    private const string Key = "myencryptionkey1234"; // 16 bytes
    private const string Iv = "myencryptioniv567890"; // 16 bytes
    private const int KeySize = 128;
    private const int BlockSize = 128;

    private void Awake()
    {
        // パス名取得
        _filePath = Application.dataPath + "/" + _fileName;

        // ファイルがないとき、ファイル作成
        if (!File.Exists(_filePath))
        {
            Save(saveData, _filePath);
        }

        // ファイルを読み込んでdataに格納
        saveData = Load<SaveData>(_filePath);
    }

    private void Save<SaveData>(SaveData data, string filePath)
    {
        string json = JsonUtility.ToJson(data);
        byte[] encryptedData = Encrypt(json, Key, Iv, KeySize, BlockSize);
        File.WriteAllBytes(filePath, encryptedData);
    }

    private SaveData Load<SaveData>(string filePath)
    {
        byte[] encryptedData = File.ReadAllBytes(filePath);
        string decryptedData = Decrypt(encryptedData, Key, Iv, KeySize, BlockSize);
        return JsonUtility.FromJson<SaveData>(decryptedData);
    }

    private byte[] Encrypt(string plainText, string key, string iv, int keySize, int blockSize)
    {
        byte[] encrypted;

        using (Aes aes = Aes.Create())
        {
            aes.KeySize = keySize;
            aes.BlockSize = blockSize;
            aes.Key = Encoding.UTF8.GetBytes(key.PadRight(16, '0').Substring(0, 16));
            aes.IV = Encoding.UTF8.GetBytes(iv.PadRight(16, '0').Substring(0, 16));
            aes.Mode = CipherMode.CBC;
            aes.Padding = PaddingMode.PKCS7;

            ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

            using (MemoryStream ms = new MemoryStream())
            {
                using (CryptoStream cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                {
                    using (StreamWriter sw = new StreamWriter(cs))
                    {
                        sw.Write(plainText);
                    }

                    encrypted = ms.ToArray();
                }
            }
        }

        return encrypted;
    }

    private string Decrypt(byte[] cipherText, string key, string iv, int keySize, int blockSize)
    {
        string decrypted;

        using (Aes aes = Aes.Create())
        {
            aes.KeySize = keySize;
            aes.BlockSize = blockSize;
            aes.Key = Encoding.UTF8.GetBytes(key.PadRight(16, '0').Substring(0, 16));
            aes.IV = Encoding.UTF8.GetBytes(iv.PadRight(16, '0').Substring(0, 16));
            aes.Mode = CipherMode.CBC;
            aes.Padding = PaddingMode.PKCS7;

            ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

            using (MemoryStream ms = new MemoryStream(cipherText))
            {
                using (CryptoStream cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
                {
                    using (StreamReader sr = new StreamReader(cs))
                    {
                        decrypted = sr.ReadToEnd();
                    }
                }
            }
        }
        return decrypted;
    }

    // ゲーム終了時に保存
    private void OnDestroy()
    {
        Save(saveData, _filePath);
    }

    // 保存したデータを削除
    public void DeleteSaveData()
    {
        // データを初期化する
        saveData = new SaveData();
        // ファイルを削除する
        File.Delete(_filePath);
        if (!File.Exists(_filePath))
        {
            Save(saveData, _filePath);
        }
    }
}