using UnityEngine;
using Cysharp.Threading.Tasks;
using UniRx;
using System.Threading;

namespace Ball
{
    public class BallLogic
    {
        public IReadOnlyReactiveProperty<bool> Destroyed => _destroyed;

        // --------------------------------------------------

        private readonly ReactiveProperty<bool> _destroyed = new ReactiveProperty<bool>(false);
        private BallSimulation _ballSimulation;
        private IBallLocalPositionAdapter _localPositionAdapter;

        // --------------------------------------------------

        public BallLogic(IBallLocalPositionAdapter localPositionAdapter, BallSimulation ballSimulation)
        {
            // ボールのゲームオブジェクトを取得する
            _localPositionAdapter = localPositionAdapter;
            // ボールのシミュレーションクラスを取得する
            _ballSimulation = ballSimulation;
        }

        public void UpdateLocalPosition(float fixedDeltaTime)
        {
            // ボールの位置を更新する
            _localPositionAdapter.LocalPosition = _ballSimulation.UpdatePosition(_localPositionAdapter.LocalPosition, fixedDeltaTime);
        }

        public void OnFlipVerticalVelocity(Vector3 additionVelocity)
        {
            // 速度を追加します。
            var velocity = _ballSimulation.Velocity + additionVelocity;
            // Y軸の速度を反転させます。
            _ballSimulation.Velocity = new Vector3(velocity.x , -velocity.y, velocity.z);
        }

        public void OnFlipHorizontalVelocity(Vector3 additionVelocity)
        {
            // 速度を追加します。
            var velocity = _ballSimulation.Velocity + additionVelocity;
            // X軸の速度を反転させます。
            _ballSimulation.Velocity = new Vector3(-velocity.x, velocity.y, velocity.z);
        }

        public async UniTask OnResetPosition(CancellationToken token)
        {
            await UniTask.Delay(System.TimeSpan.FromSeconds(1), cancellationToken: token);
            // ボールの速度を取得します。
            var velocity = _ballSimulation.Velocity;
            _ballSimulation.Velocity = new Vector3(velocity.x, 0, velocity.z);
            _localPositionAdapter.LocalPosition = Vector2.zero;
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