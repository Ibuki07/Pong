using Cysharp.Threading.Tasks;
using System.Text;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class TextRainbowAnimation : MonoBehaviour
{
    [SerializeField] private bool _isTokeiMawari = true;
    [SerializeField] private float _changeDelayTime = 0.5f;

    private static int _rainbowStartId = 0;
    private static string hedder_tag = "<color=#Value>";
    private static string footer_tag = "</color>";

    // 虹色の中身　※ 2桁づつで区切って[00 00 00]＝[ R, G, B ]の値になっている
    // 　　　　　　※ この配列の要素を追加・編集したらオリジナルの動く色が作れる
    private static string[] color_tag = new string[]
    {
        "ff0000",
        "ffff00",
        "00ff00",
        "00ffff",
        "0000ff",
        "ff00ff",
    };

    private void Start()
    {
        var token = this.GetCancellationTokenOnDestroy();
        UpdateAsync(token).Forget();
    }

    private async UniTask UpdateAsync(CancellationToken token)
    {
        while (true)
        {
            await UniTask.Delay(System.TimeSpan.FromSeconds(_changeDelayTime), cancellationToken: token);
            SetTextColorChange(_isTokeiMawari, this.GetComponent<Text>());
        }
    }

    /// テキスト色を変える処理（毎割り込み）
    public static void SetTextColorChange(bool IsVecLR, Text TxtSet)
    {
        // テキストの文字を取得※Tag文字を取り除く
        string textNoTag = TxtSet.text;
        textNoTag = textNoTag.Replace(footer_tag, "");
        for (int i_ColorId = 0; i_ColorId < color_tag.Length; i_ColorId++)
        {
            textNoTag = textNoTag.Replace(hedder_tag.Replace("Value", color_tag[i_ColorId]), "");
        }
        // 一文字ずつ色を設定
        int setColorId = _rainbowStartId;
        StringBuilder textSet = new StringBuilder();
        for (int i_Word = 0; i_Word < textNoTag.Length; i_Word++)
        {
            textSet.Append(hedder_tag.Replace("Value", color_tag[setColorId]));
            textSet.Append(textNoTag.Substring(i_Word, 1));
            textSet.Append(footer_tag);
            if (IsVecLR)
            {
                setColorId--;
                if (setColorId < 0)
                {
                    setColorId = color_tag.Length - 1;
                }
            }
            else
            {
                setColorId++;
                if (setColorId >= color_tag.Length)
                {
                    setColorId = 0;
                }
            }
        }
        // 次回の開始色を更新
        _rainbowStartId++;
        if (_rainbowStartId >= color_tag.Length)
        {
            _rainbowStartId = 0;
        }
        // テキスト文字を変更
        TxtSet.text = textSet.ToString();
    }
}