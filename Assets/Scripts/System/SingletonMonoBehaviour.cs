using UnityEngine;
using System;

public class SingletonMonoBehaviour<T> : MonoBehaviour where T : MonoBehaviour
{
    // static��instance�ϐ���Singleton�̃C���X�^���X���i�[����
    private static T instance;

    // Singleton�̃C���X�^���X���擾����v���p�e�B
    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                // �V�[�����ɑ��݂��邷�ׂĂ�T�^�̃R���|�[�l���g���擾����
                T[] objects = FindObjectsOfType<T>();

                if (objects.Length == 0)
                {
                    // �V�[������T�^�̃R���|�[�l���g�����݂��Ȃ��ꍇ�̓G���[��\������
                    Debug.LogError(typeof(T) + "���V�[���ɑ��݂��܂���B");
                    return null;
                }

                // �V�[�����ɑ��݂���T�^�̃R���|�[�l���g��1�ȏ゠��ꍇ�́A�ŏ��Ɍ����������̂�instance�Ɋi�[����
                instance = objects[0];

                if (objects.Length > 1)
                {
                    // �V�[������T�^�̃R���|�[�l���g���������݂���ꍇ�́A�x����\������
                    Debug.LogWarning(typeof(T) + "���V�[�����ɕ������݂��܂��B�ŏ��Ɍ����������̂��g�p���܂��B");
                }
            }

            return instance;
        }
    }

    // Awake���\�b�h��virtual�ɂ��āA�h���N���X�ŃI�[�o�[���C�h�ł���悤�ɂ���
    protected virtual void Awake()
    {
        // instance��null�̏ꍇ�́A���g��instance�Ɋi�[����
        if (instance == null)
        {
            instance = this as T;
        }
        else
        {
            // instance�Ɏ��g�ȊO�̃I�u�W�F�N�g���i�[����Ă���ꍇ�́A���g��j������
            Destroy(gameObject);
        }

        // �V�[�����ׂ��ł��I�u�W�F�N�g��j�����Ȃ��悤�ɂ���
        DontDestroyOnLoad(gameObject);
    }
}