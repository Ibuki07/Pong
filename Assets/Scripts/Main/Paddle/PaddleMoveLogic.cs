using UniRx;
using UnityEngine;

namespace Paddle
{
    public class PaddleMoveLogic : System.IDisposable
    {
        private PaddleMoveInput _moveInput;
        private PaddleMoveSimulation _moveSimulation;
        private IPaddleLocalPositionAdapter _paddle;
        private System.IDisposable _disposable;
        private bool _isMove = true;

        public PaddleMoveLogic(
            PaddleMoveInput moveInput,
            PaddleMoveSimulation moveSimulation,
            IPaddleLocalPositionAdapter paddle) 
        {
            _moveInput = moveInput;
            _moveSimulation = moveSimulation;
            _paddle = paddle;

            // パドルが"下"方向に移動している時に、"上"方向に入力されたら移動を反転させる
            _moveInput.MoveDirection
                .Where(isMoveDirection => _moveSimulation.Velocity.y < 0 && isMoveDirection)
                .Subscribe(_ =>
                {
                    _isMove = true;
                    FlipVerticalVelocity();
                });
            // パドルが"上"方向に移動している時に、"下"方向に入力されたら移動を反転させる
            _moveInput.MoveDirection
                .Where(isMoveDirection => _moveSimulation.Velocity.y > 0 && !isMoveDirection)
                .Subscribe(_ =>
                {
                    _isMove = true;
                    FlipVerticalVelocity();
                });

            _disposable = Observable.EveryFixedUpdate()
                .Where(_ => _moveInput.IsPaddleMoveInput() && _isMove)
                .Subscribe(_ =>
                {
                    UpdateLocalPosition(Time.fixedDeltaTime);
                });
        }

        public void Dispose()
        {
            _disposable?.Dispose();
        }

        public Vector3 OnRandomizeVerticalVelocity(Vector3 velocity)
        {
            var rondomVerticalVelocity = Vector3.up * Random.Range(-velocity.y, velocity.y);
            return rondomVerticalVelocity;
        }
        
        public void OnStopPaddle()
        {
            _isMove = false;
        }

        private void UpdateLocalPosition(float fixedDeltaTime)
        {
            // 位置を更新する
            _paddle.LocalPosition = _moveSimulation.UpdatePosition(_paddle.LocalPosition, fixedDeltaTime);
        }


        private void FlipVerticalVelocity()
        {
            // 速度を取得します。
            var velocity = _moveSimulation.Velocity;
            // Y軸の速度を反転させます。
            _moveSimulation.Velocity = new Vector3(velocity.x, -velocity.y, velocity.z);
        }
    }
}