using UnityEngine;

[ExecuteAlways]
public class AspectKeeper : MonoBehaviour
{
    [SerializeField] private Camera _targetCamera;
    // 目的解像度
    [SerializeField] private Vector2 _aspectVector; 

    private Rect _viewportRect;
    private float _screenAspect;
    private float _targetAspect;
    private float _magRate;

    private void Update()
    {
        if (_targetCamera == null)
        {
            Debug.LogWarning("Target Camera is not set in AspectKeeper script.");
            return;
        }

        // スクリーンのアスペクト比を取得する
        _screenAspect = Screen.width / (float)Screen.height;
        
        // 目的のアスペクト比を取得する
        _targetAspect = _aspectVector.x / _aspectVector.y;

        // 現在のアスペクト比と目的のアスペクト比が同じ場合は処理をスキップする
        if (_screenAspect == _targetAspect)
        {
            return;
        }

        // 目的アスペクト比にするための倍率を計算する
        _magRate = _targetAspect / _screenAspect;

        // カメラのクリッピング平面が設定されていることを確認する
        if (_targetCamera.nearClipPlane == 0f)
        {
            Debug.LogWarning("Near clip plane is not set in Target Camera.");
            return;
        }
        if (_targetCamera.farClipPlane == 0f)
        {
            Debug.LogWarning("Far clip plane is not set in Target Camera.");
            return;
        }

        // Viewport初期値でRectを作成
        _viewportRect = new Rect(0, 0, 1, 1);

        // 目的のアスペクト比に合わせてViewportのサイズを変更する
        if (_magRate < 1)
        {
            // 使用する横幅を変更
            _viewportRect.width = _magRate;

            // 中央寄せ
            _viewportRect.x = 0.5f - _viewportRect.width * 0.5f;
        }
        else
        {
            // 使用する縦幅を変更
            _viewportRect.height = 1 / _magRate;

            // 中央寄せ
            _viewportRect.y = 0.5f - _viewportRect.height * 0.5f;
        }

        // カメラのViewportに適用
        _targetCamera.rect = _viewportRect; 
    }
}
