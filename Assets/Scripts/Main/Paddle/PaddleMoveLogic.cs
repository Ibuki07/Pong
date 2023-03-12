using UniRx;
using UnityEngine;

namespace Paddle
{
    public class PaddleMoveLogic : System.IDisposable
    {
        private PaddleMoveSimulation _moveSimulation;
        private IPaddleLocalPositionAdapter _paddle;
        private System.IDisposable _disposable;
        private bool _isMoving = true;

        public PaddleMoveLogic(
            PaddleMoveInput moveInput,
            PaddleMoveSimulation moveSimulation,
            IPaddleLocalPositionAdapter paddle) 
        {
            _moveSimulation = moveSimulation;
            _paddle = paddle;

            // パドルが"下"方向に移動している時に、"上"方向に入力されたら移動を反転させる
            moveInput.MoveDirection
                .Where(isMoveDirection => _moveSimulation.Velocity.y < 0 && isMoveDirection)
                .Subscribe(_ =>
                {
                    _isMoving = true;
                    FlipVerticalVelocity();
                });

            // パドルが"上"方向に移動している時に、"下"方向に入力されたら移動を反転させる
            moveInput.MoveDirection
                .Where(isMoveDirection => _moveSimulation.Velocity.y > 0 && !isMoveDirection)
                .Subscribe(_ =>
                {
                    _isMoving = true;
                    FlipVerticalVelocity();
                });

            // パドルが移動可能であれば、定期的に位置を更新する
            _disposable = Observable.EveryFixedUpdate()
                .Where(_ => moveInput.IsPaddleMoveInput() && _isMoving)
                .Subscribe(_ =>
                {
                    UpdatePaddlePosition(Time.fixedDeltaTime);
                });
        }

        public void Dispose()
        {
            _disposable?.Dispose();
        }

        // パドルのY軸速度をランダム化する
        public Vector3 RandomizeVerticalVelocity(Vector3 velocity)
        {
            var randomVerticalVelocity = Vector3.up * Random.Range(-velocity.y, velocity.y);
            return randomVerticalVelocity;
        }

        // パドルの移動を停止する
        public void StopPaddle()
        {
            _isMoving = false;
        }

        // パドルの位置を更新する
        private void UpdatePaddlePosition(float fixedDeltaTime)
        {
            // 位置を更新する
            _paddle.LocalPosition = _moveSimulation.UpdatePosition(_paddle.LocalPosition, fixedDeltaTime);
        }

        // パドルのY軸速度を反転させる
        private void FlipVerticalVelocity()
        {
            // 速度を取得します。
            var velocity = _moveSimulation.Velocity;
            // Y軸の速度を反転させます。
            _moveSimulation.Velocity = new Vector3(velocity.x, -velocity.y, velocity.z);
        }
    }
}