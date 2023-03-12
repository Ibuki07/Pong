using Ball;
using UniRx;
using UnityEngine;

namespace Paddle
{
    public class PaddleAutoMoveLogic : System.IDisposable
    {
        // パドルがボールを感知する距離
        public float DetectionRange { get; set; } = 0.2f;

        private PaddleAutoMoveInput _autoMoveInput;
        private PaddleMoveSimulation _moveSimulation;
        private IPaddleLocalPositionAdapter _paddle;
        private IBallLocalPositionAdapter _ball;
        private System.IDisposable _fixedUpdateDisposable;

        // パドルの中心のx座標
        private float _centerPoint = 0;
        // パドルが動いているかどうかのフラグ
        private bool _isMoving = true;

        public PaddleAutoMoveLogic(
            PaddleAutoMoveInput autoMoveInput,
            PaddleMoveSimulation moveSimulation,
            IPaddleLocalPositionAdapter paddle,
            IBallLocalPositionAdapter ball)
        {
            _autoMoveInput = autoMoveInput;
            _moveSimulation = moveSimulation;
            _paddle = paddle;
            _ball = ball;

            // フレーム更新ごとに呼ばれるサブスクリプションを設定する
            _fixedUpdateDisposable = Observable.EveryFixedUpdate()
                .Where(_ => _autoMoveInput.IsAutoMoveInput() && IsMoveTrackingBall() && _isMoving)
                .Subscribe(_ =>
                {
                    // パドルの位置を更新する
                    UpdateLocalPosition(Time.fixedDeltaTime);
                });

        }

        public void Dispose()
        {
            _fixedUpdateDisposable?.Dispose();
        }

        public void OnStopPaddle()
        {
            // パドルの動きを止める
            _isMoving = false;
        }

        private void UpdateLocalPosition(float fixedDeltaTime)
        {
            // 位置を更新する
            _paddle.LocalPosition = _moveSimulation.UpdatePosition(_paddle.LocalPosition, fixedDeltaTime);
        }

        private bool IsMoveTrackingBall()
        {
            bool isMove = false;
            // パドルが"左"にある時に、ボールが反対側の"右"にあるなら動かない
            if (_paddle.LocalPosition.x < _centerPoint && _ball.LocalPosition.x > _centerPoint)
            {
                return isMove;
            }
            // パドルが"右"にある時に、ボールが反対側の"左"にあるなら動かない
            if (_paddle.LocalPosition.x > _centerPoint && _ball.LocalPosition.x < _centerPoint)
            {
                return isMove;
            }
            // ボールがパドルの上にある場合は、パドルを上に動かす
            if (_ball.LocalPosition.y > _paddle.LocalPosition.y + DetectionRange)
            {
                isMove = true;
                if (_moveSimulation.Velocity.y < _centerPoint)
                {
                    // パドルの動きを開始し、ボールを反転させる
                    _isMoving = true;
                    FlipVerticalVelocity();
                }
            }
            // ボールがパドルの下にある場合は、パドルを下に動かす
            if (_ball.LocalPosition.y < _paddle.LocalPosition.y - DetectionRange)
            {
                isMove = true;
                if (_moveSimulation.Velocity.y > _centerPoint)
                {
                    // パドルの動きを開始し、ボールを反転させる
                    _isMoving = true;
                    FlipVerticalVelocity();
                }
            }
            return isMove;
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