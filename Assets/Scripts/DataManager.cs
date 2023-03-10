using Cysharp.Threading.Tasks;
using System.IO;
using UnityEngine;

public class DataManager : MonoBehaviour
{


    public SaveData SaveData;     // json�ϊ�����f�[�^�̃N���X
    private string _filePath;                            // json�t�@�C���̃p�X
    private string _fileName = "SaveData.json";              // json�t�@�C����

    private void Awake()
    {
        // �p�X���擾
        _filePath = Application.dataPath + "/" + _fileName;

        // �t�@�C�����Ȃ��Ƃ��A�t�@�C���쐬
        if (!File.Exists(_filePath))
        {
            Save();
        }

        // �t�@�C����ǂݍ����data�Ɋi�[
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

    // �Q�[���I�����ɕۑ�
    private void OnDestroy()
    {
        Save();
    }

    // �ۑ������f�[�^���폜
    public void DeleteSaveData()
    {
        // �f�[�^������������
        SaveData = new SaveData();
        // �t�@�C�����폜����
        File.Delete(_filePath);
        if (!File.Exists(_filePath))
        {
            Save();
        }
    }
}