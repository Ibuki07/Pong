using UniRx;
using UnityEngine;

namespace Paddle
{
    public class PaddleMoveInput
    {
        // パドルの移動方向に関するリアクティブプロパティ
        public IReadOnlyReactiveProperty<bool> MoveDirection => _moveDirection;

        // パドルが移動中かどうかに関するリアクティブプロパティ
        public IReadOnlyReactiveProperty<bool> Move => _move;

        private readonly ReactiveProperty<bool> _moveDirection = new ReactiveProperty<bool>(true);
        private readonly ReactiveProperty<bool> _move = new ReactiveProperty<bool>(false);

        // パドルの位置情報を取得するアダプター
        private IPaddleLocalPositionAdapter _paddlePosition;

        public PaddleMoveInput(IPaddleLocalPositionAdapter paddlePosition)
        {
            _paddlePosition = paddlePosition;
        }

        // パドルの移動方向を判定する
        public bool IsPaddleMoveInput()
        {
            // パドルの位置に応じて、InputArrow()かInputWASD()を呼び出す
            if (_paddlePosition.LocalPosition.x < 0)
            {
                InputWASD();
            }
            if (_paddlePosition.LocalPosition.x > 0)
            {
                InputArrow();
            }
            // 移動の有無を返す
            return _move.Value;
        }

        // 矢印キーで移動方向を切り替える
        public void InputArrow()
        {
            // パドルが移動中であることを初期化
            _move.Value = false;

            // 上矢印キーでパドルを上に移動させる
            if (Input.GetKey(KeyCode.UpArrow))
            {
                _moveDirection.Value = true;
                _move.Value = true;
            }
            // 下矢印キーでパドルを下に移動させる
            if (Input.GetKey(KeyCode.DownArrow))
            {
                _moveDirection.Value = false;
                _move.Value = true;
            }
        }

        // WASDキーで移動方向を切り替える
        public void InputWASD()
        {
            // パドルが移動中であることを初期化
            _move.Value = false;

            // Wキーでパドルを上に移動させる
            if (Input.GetKey(KeyCode.W))
            {
                _moveDirection.Value = true;
                _move.Value = true;
            }
            // Sキーでパドルを下に移動させる
            if (Input.GetKey(KeyCode.S))
            {
                _moveDirection.Value = false;
                _move.Value = true;
            }
        }
    }
}