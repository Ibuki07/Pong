using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace Paddle
{
    public class PaddleAutoMoveInput
    {
        // 自動移動の状態を伝えるReactiveProperty
        public IReadOnlyReactiveProperty<bool> AutoMove => _autoMove;
        private readonly ReactiveProperty<bool> _autoMove = new ReactiveProperty<bool>(true);

        // パドルの座標を取得するためのアダプタ
        private readonly IPaddleLocalPositionAdapter _paddlePosition;

        public PaddleAutoMoveInput(IPaddleLocalPositionAdapter paddlePosition, Button autoMoveButton)
        {
            _paddlePosition = paddlePosition;

            // ボタンをクリックした時に、自動移動の状態を切り替える
            autoMoveButton.OnClickAsObservable().Subscribe(_ =>
            {
                ToggleAutoMove();
            });
        }

        // 自動移動状態かどうかを返すメソッド
        public bool IsAutoMoveInput()
        {
            // パドルが左にある時はFキー、右にある時はJキーを押すと自動移動の状態を切り替える
            if (_paddlePosition.LocalPosition.x < 0 && Input.GetKeyDown(KeyCode.F))
            {
                ToggleAutoMove();
            }
            if (_paddlePosition.LocalPosition.x > 0 && Input.GetKeyDown(KeyCode.J))
            {
                ToggleAutoMove();
            }
            // 現在の自動移動状態を返す
            return _autoMove.Value;
        }

        // 自動移動の状態を切り替えるメソッド
        private void ToggleAutoMove()
        {
            _autoMove.Value = !_autoMove.Value;
        }
    }
}