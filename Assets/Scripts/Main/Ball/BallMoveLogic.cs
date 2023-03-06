using UnityEngine;
using Cysharp.Threading.Tasks;
using UniRx;
using System.Threading;

namespace Ball
{
    public class BallMoveLogic : System.IDisposable
    {
        public IReadOnlyReactiveProperty<bool> Destroyed => _destroyed;

        // --------------------------------------------------

        private readonly ReactiveProperty<bool> _destroyed = new ReactiveProperty<bool>(false);
        private BallMoveSimulation _moveSimulation;
        private IBallLocalPositionAdapter _ball;
        private System.IDisposable _disposable;
        private float _resetPositionDelayTime = 1f;

        // --------------------------------------------------

        public BallMoveLogic(
            BallMoveSimulation ballSimulation,
            IBallLocalPositionAdapter ball) 
        {
            _ball = ball;
            _moveSimulation = ballSimulation;

            _disposable = Observable.EveryFixedUpdate().Subscribe(_ =>
            {
                UpdateLocalPosition(Time.fixedDeltaTime);
            });
        }

        public void Dispose()
        {
            _disposable.Dispose();
        }

        public void UpdateLocalPosition(float fixedDeltaTime)
        {
            // ボールの位置を更新する
            _ball.LocalPosition = _moveSimulation.UpdatePosition(_ball.LocalPosition, fixedDeltaTime);
        }

        public void FlipVerticalVelocity()
        {
            var velocity = _moveSimulation.Velocity;
            // Y軸の速度を反転させます。
            _moveSimulation.Velocity = new Vector3(velocity.x , -velocity.y, velocity.z);
        }

        public void OnAdditionVelocity(Vector3 additionVelocity)
        {
            _moveSimulation.Velocity += additionVelocity;
        }

        public void FlipHorizontalVelocity()
        {
            var velocity = _moveSimulation.Velocity;
            // X軸の速度を反転させます。
            _moveSimulation.Velocity = new Vector3(-velocity.x, velocity.y, velocity.z);
        }

        public async UniTask ResetPosition(CancellationToken token)
        {
            // ボールの速度を取得します。
            var velocity = _moveSimulation.Velocity;
            _moveSimulation.Velocity = Vector3.right * velocity.x;

            await UniTask.Delay(System.TimeSpan.FromSeconds(_resetPositionDelayTime), cancellationToken: token);

            _ball.LocalPosition = Vector3.zero; 
        }

        public void OnDestroyedBall()
        {
            _destroyed.Value = true;
        }

        public void OnCreatedBall()
        {
            Debug.Log("CreateBall");
            _destroyed.Value = false;
        }

    }
}