using UnityEngine;
using System;

public class SingletonMonoBehaviour<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T instance;

    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                Type t = typeof(T);

                instance = (T)FindObjectOfType(t);

                if (instance == null)
                {
                    Debug.LogError(typeof(T) + "���V�[���ɑ��݂��܂���B");
                }
            }

            return instance;
        }
    }
    virtual protected void Awake()
    {
        // ����GameObject�ɃA�^�b�`����Ă��邩���ׂ�.
        // �A�^�b�`����Ă���ꍇ�͔j������.
        if (this != Instance)
        {
            Destroy(this);
            //Destroy(this.gameObject);
            Debug.LogError(
                typeof(T) +
                " �͊��ɑ���GameObject�ɃA�^�b�`����Ă��邽�߁A�R���|�[�l���g��j�����܂���." +
                " �A�^�b�`����Ă���GameObject�� " + Instance.gameObject.name + " �ł�.");
            return;
        }

        DontDestroyOnLoad(this.gameObject);
    }
}