using UniRx;
using UnityEngine;

namespace Paddle
{
    public class PaddleLogic
    {
        private IPaddleLocalPositionAdapter _localPositionAdapter;
        private PaddleSimulation _paddleSimulation;
        private PaddleMoveInput _paddleMoveInput;
        private bool _isMove = true;

        // --------------------------------------------------

        public PaddleLogic(IPaddleLocalPositionAdapter localPositionAdapter, PaddleMoveInput paddleMoveInput, PaddleSimulation paddleSimulation)
        {
            // コアクラスを取得する
            _localPositionAdapter = localPositionAdapter;
            _paddleMoveInput = paddleMoveInput;
            // シミュレーションクラスを取得する
            _paddleSimulation = paddleSimulation;
        }

        public void OnUpdateLocalPosition(float fixedDeltaTime)
        {
            if (_paddleMoveInput.IsPaddleMoveInput())
            {
                OnFlipVerticalDirection();

                if (_isMove)
                {
                    // 位置を更新する
                    _localPositionAdapter.LocalPosition = _paddleSimulation.UpdatePosition(_localPositionAdapter.LocalPosition, fixedDeltaTime);
                }
            }
        }

        public void OnStopPaddle()
        {
            _isMove = false;
        }

        public Vector3 OnRandomizeVerticalVelocity(Vector3 velocity)
        {
            var rondomVerticalVelocity = Vector3.up * Random.Range(-velocity.y, velocity.y);
            return rondomVerticalVelocity;
        }

        // --------------------------------------------------

        private void OnFlipVerticalDirection()
        {
            // 速度を取得します。
            var velocity = _paddleSimulation.Velocity;

            // 上
            _paddleMoveInput.MoveDirection
                .Where(isMoveDirection => velocity.y < 0 && isMoveDirection)
                .Subscribe(_ =>
                {
                    _isMove = true;
                    // Y軸の速度を反転させます。
                    _paddleSimulation.Velocity = new Vector3(velocity.x, -velocity.y, velocity.z);
                });
            // 下
            _paddleMoveInput.MoveDirection
                .Where(isMoveDirection => velocity.y > 0 && !isMoveDirection)
                .Subscribe(_ =>
                {
                    _isMove = true;
                    // Y軸の速度を反転させます。
                    _paddleSimulation.Velocity = new Vector3(velocity.x, -velocity.y, velocity.z);
                });
        }


    }
}