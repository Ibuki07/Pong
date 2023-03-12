using UnityEngine;
using Cysharp.Threading.Tasks;
using UniRx;
using System.Threading;

namespace Ball
{
    public class BallMoveLogic : System.IDisposable
    {
        // �{�[�����j�󂳂ꂽ���ǂ�����\���܂��B
        public IReadOnlyReactiveProperty<bool> Destroyed => _destroyed;

        private readonly ReactiveProperty<bool> _destroyed = new ReactiveProperty<bool>(true);
        private BallMoveSimulation _moveSimulation;

        // �{�[���̈ʒu���擾����A�_�v�^�[�ł��B
        private IBallLocalPositionAdapter _ballPosition;

        // ���t���[�����s���鏈���̃f�B�X�|�[�U�u���I�u�W�F�N�g�ł��B
        private System.IDisposable _fixedUpdateDisposable;

        // �{�[���̈ʒu�����Z�b�g���邽�߂̒x�����Ԃł��B
        private float _resetPositionDelayTime = 1f;

        public BallMoveLogic(
            BallMoveSimulation ballSimulation,
            IBallLocalPositionAdapter ball) 
        {
            _ballPosition = ball;
            _moveSimulation = ballSimulation;

            // ���t���[�����s���鏈�����������܂��B
            _fixedUpdateDisposable = Observable.EveryFixedUpdate().Subscribe(_ =>
            {
                // �{�[���̈ʒu���X�V����
                UpdateBallPosition(Time.fixedDeltaTime);
            });
        }

        public void Dispose()
        {
            _fixedUpdateDisposable.Dispose();
        }

        // �{�[���̈ʒu���X�V���܂��B
        public void UpdateBallPosition(float fixedDeltaTime)
        {
            // �{�[���̈ʒu���擾���A�ړ��V�~�����[�V�����I�u�W�F�N�g���g�p���Ĉʒu���X�V���܂��B
            var currentPosition = _ballPosition.LocalPosition;
            var newPosition = _moveSimulation.UpdatePosition(currentPosition, fixedDeltaTime);
            _ballPosition.LocalPosition = newPosition;
        }

        // �{�[����Y�������̑��x�𔽓]���܂��B
        public void FlipVerticalVelocity()
        {
            var velocity = _moveSimulation.Velocity;
            _moveSimulation.Velocity = new Vector3(velocity.x, -velocity.y, velocity.z);
        }

        // �{�[����X�������̑��x�𔽓]���܂��B
        public void FlipHorizontalVelocity()
        {
            var velocity = _moveSimulation.Velocity;
            _moveSimulation.Velocity = new Vector3(-velocity.x, velocity.y, velocity.z);
        }

        // �{�[���ɑ��x�����Z���܂��B
        public void AddVelocity(Vector3 velocity)
        {
            _moveSimulation.Velocity += velocity;
        }

        // �{�[���̈ʒu�����Z�b�g���܂��B
        public async UniTask ResetPosition(CancellationToken token)
        {
            // �{�[���̑��x��X�������݂̂ɕύX���܂��B
            var velocity = _moveSimulation.Velocity;
            _moveSimulation.Velocity = Vector3.right * velocity.x;

            // ��莞�ԑҋ@���Ă���{�[���̈ʒu�����Z�b�g���܂��B
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