using UnityEngine;
using System;

public class SingletonMonoBehaviour<T> : MonoBehaviour where T : MonoBehaviour
{
    // staticなinstance変数にSingletonのインスタンスを格納する
    private static T instance;

    // Singletonのインスタンスを取得するプロパティ
    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                // シーン内に存在するすべてのT型のコンポーネントを取得する
                T[] objects = FindObjectsOfType<T>();

                if (objects.Length == 0)
                {
                    // シーン内にT型のコンポーネントが存在しない場合はエラーを表示する
                    Debug.LogError(typeof(T) + "がシーンに存在しません。");
                    return null;
                }

                // シーン内に存在するT型のコンポーネントが1つ以上ある場合は、最初に見つかったものをinstanceに格納する
                instance = objects[0];

                if (objects.Length > 1)
                {
                    // シーン内にT型のコンポーネントが複数存在する場合は、警告を表示する
                    Debug.LogWarning(typeof(T) + "がシーン内に複数存在します。最初に見つかったものを使用します。");
                }
            }

            return instance;
        }
    }

    // Awakeメソッドをvirtualにして、派生クラスでオーバーライドできるようにする
    protected virtual void Awake()
    {
        // instanceがnullの場合は、自身をinstanceに格納する
        if (instance == null)
        {
            instance = this as T;
        }
        else
        {
            // instanceに自身以外のオブジェクトが格納されている場合は、自身を破棄する
            Destroy(gameObject);
        }

        // シーンを跨いでもオブジェクトを破棄しないようにする
        DontDestroyOnLoad(gameObject);
    }
}