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
            // �{�[���̃Q�[���I�u�W�F�N�g���擾����
            _localPositionAdapter = localPositionAdapter;
            // �{�[���̃V�~�����[�V�����N���X���擾����
            _ballSimulation = ballSimulation;
        }

        public void UpdateLocalPosition(float fixedDeltaTime)
        {
            // �{�[���̈ʒu���X�V����
            _localPositionAdapter.LocalPosition = _ballSimulation.UpdatePosition(_localPositionAdapter.LocalPosition, fixedDeltaTime);
        }

        public void OnFlipVerticalVelocity(Vector3 additionVelocity)
        {
            // ���x��ǉ����܂��B
            var velocity = _ballSimulation.Velocity + additionVelocity;
            // Y���̑��x�𔽓]�����܂��B
            _ballSimulation.Velocity = new Vector3(velocity.x , -velocity.y, velocity.z);
        }

        public void OnFlipHorizontalVelocity(Vector3 additionVelocity)
        {
            // ���x��ǉ����܂��B
            var velocity = _ballSimulation.Velocity + additionVelocity;
            // X���̑��x�𔽓]�����܂��B
            _ballSimulation.Velocity = new Vector3(-velocity.x, velocity.y, velocity.z);
        }

        public async UniTask OnResetPosition(CancellationToken token)
        {
            await UniTask.Delay(System.TimeSpan.FromSeconds(1), cancellationToken: token);
            // �{�[���̑��x���擾���܂��B
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