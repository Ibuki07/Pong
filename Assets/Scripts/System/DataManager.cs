using System.IO;
using UnityEngine;

namespace Managers
{
    public class DataManager : MonoBehaviour
    {
        // SaveData�N���X�̃C���X�^���X
        public SaveData SaveData = new SaveData();

        // �ۑ���̃p�X��ێ�����ϐ�
        private string _filePath;

        // �ۑ�����t�@�C����
        private const string FileName = "SaveData.json";

        private void Awake()
        {
            // �ۑ���̃p�X��ݒ肷��
            _filePath = Path.Combine(Application.persistentDataPath, FileName);

            // �t�@�C�������݂��Ȃ��ꍇ�͕ۑ�����
            if (!File.Exists(_filePath))
            {
                Save();
            }

            // �t�@�C������f�[�^��ǂݍ���
            Load();
        }

        private void OnDestroy()
        {
            Save();
        }

        // SaveData��JSON�t�@�C���ɕۑ�����
        private void Save()
        {
            // SaveData��JSON�`���̕�����ɕϊ�����
            string json = JsonUtility.ToJson(SaveData);

            // StreamWriter�Ńt�@�C���ɏ�������
            using (StreamWriter streamWriter = new StreamWriter(_filePath))
            {
                streamWriter.Write(json);
            }
        }

        // JSON�t�@�C������f�[�^��ǂݍ���
        public void Load()
        {
            if (File.Exists(_filePath))
            {
                // StreamReader�Ńt�@�C����ǂݍ��݁AJSON�`���̕�������擾����
                using (StreamReader streamReader = new StreamReader(_filePath))
                {
                    string data = streamReader.ReadToEnd();

                    // JSON�`���̕����񂩂�SaveData�N���X�̃C���X�^���X�𕜌�����
                    SaveData = JsonUtility.FromJson<SaveData>(data);
                }
            }
        }

        // �ۑ����ꂽ�f�[�^���폜����
        public void DeleteSaveData()
        {
            // SaveData�̃f�[�^������������
            SaveData = new SaveData();

            // �t�@�C�����폜����
            File.Delete(_filePath);

            // �t�@�C�������݂��Ȃ��ꍇ�͕ۑ�����
            if (!File.Exists(_filePath))
            {
                Save();
            }
        }
    }
}