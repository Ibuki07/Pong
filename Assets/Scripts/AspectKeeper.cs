using UniRx;
using UnityEngine;

[ExecuteAlways]
public class AspectKeeper : MonoBehaviour
{
    [SerializeField] private Camera _targetCamera; //対象とするカメラ
    [SerializeField] private Vector2 _aspectVec; //目的解像度

    private float _screenAspect;
    private float _targetAspect;
    private float _magRate;
    private Rect _viewportRect;

    private void Update()
    {
        _screenAspect = Screen.width / (float)Screen.height; //画面のアスペクト比
        _targetAspect = _aspectVec.x / _aspectVec.y; //目的のアスペクト比
        _magRate = _targetAspect / _screenAspect; //目的アスペクト比にするための倍率
        _viewportRect = new Rect(0, 0, 1, 1); //Viewport初期値でRectを作成

        if (_magRate < 1)
        {
            _viewportRect.width = _magRate; //使用する横幅を変更
            _viewportRect.x = 0.5f - _viewportRect.width * 0.5f;//中央寄せ
        }
        else
        {
            _viewportRect.height = 1 / _magRate; //使用する縦幅を変更
            _viewportRect.y = 0.5f - _viewportRect.height * 0.5f;//中央余生
        }

        _targetCamera.rect = _viewportRect; //カメラのViewportに適用
    }
}
