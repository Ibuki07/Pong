using UnityEngine;
using Cysharp.Threading.Tasks;
using UniRx;
using System.Threading;

namespace Ball
{
    public class BallMoveLogic : System.IDisposable
    {
        // ボールが破壊されたかどうかを表します。
        public IReadOnlyReactiveProperty<bool> Destroyed => _destroyed;

        private readonly ReactiveProperty<bool> _destroyed = new ReactiveProperty<bool>(true);
        private BallMoveSimulation _moveSimulation;

        // ボールの位置を取得するアダプターです。
        private IBallLocalPositionAdapter _ballPosition;

        // 毎フレーム実行する処理のディスポーザブルオブジェクトです。
        private System.IDisposable _fixedUpdateDisposable;

        // ボールの位置をリセットするための遅延時間です。
        private float _resetPositionDelayTime = 1f;

        public BallMoveLogic(
            BallMoveSimulation ballSimulation,
            IBallLocalPositionAdapter ball) 
        {
            _ballPosition = ball;
            _moveSimulation = ballSimulation;

            // 毎フレーム実行する処理を実装します。
            _fixedUpdateDisposable = Observable.EveryFixedUpdate().Subscribe(_ =>
            {
                // ボールの位置を更新する
                UpdateBallPosition(Time.fixedDeltaTime);
            });
        }

        public void Dispose()
        {
            _fixedUpdateDisposable.Dispose();
        }

        // ボールの位置を更新します。
        public void UpdateBallPosition(float fixedDeltaTime)
        {
            // ボールの位置を取得し、移動シミュレーションオブジェクトを使用して位置を更新します。
            var currentPosition = _ballPosition.LocalPosition;
            var newPosition = _moveSimulation.UpdatePosition(currentPosition, fixedDeltaTime);
            _ballPosition.LocalPosition = newPosition;
        }

        // ボールのY軸方向の速度を反転します。
        public void FlipVerticalVelocity()
        {
            var velocity = _moveSimulation.Velocity;
            _moveSimulation.Velocity = new Vector3(velocity.x, -velocity.y, velocity.z);
        }

        // ボールのX軸方向の速度を反転します。
        public void FlipHorizontalVelocity()
        {
            var velocity = _moveSimulation.Velocity;
            _moveSimulation.Velocity = new Vector3(-velocity.x, velocity.y, velocity.z);
        }

        // ボールに速度を加算します。
        public void AddVelocity(Vector3 velocity)
        {
            _moveSimulation.Velocity += velocity;
        }

        // ボールの位置をリセットします。
        public async UniTask ResetPosition(CancellationToken token)
        {
            // ボールの速度をX軸方向のみに変更します。
            var velocity = _moveSimulation.Velocity;
            _moveSimulation.Velocity = Vector3.right * velocity.x;

            // 一定時間待機してからボールの位置をリセットします。
            await UniTask.Delay(System.TimeSpan.FromSeconds(_resetPositionDelayTime), cancellationToken: token);
            _ballPosition.LocalPosition = Vector3.zero;
        }

        public void OnDestroyedBall()
        {
            _destroyed.Value = true;
        }

        public void OnCreatedBall()
        {
            _destroyed.Value = false;
        }
    }
}