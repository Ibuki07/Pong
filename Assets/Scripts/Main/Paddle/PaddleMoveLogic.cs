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

            // �p�h����"��"�����Ɉړ����Ă��鎞�ɁA"��"�����ɓ��͂��ꂽ��ړ��𔽓]������
            moveInput.MoveDirection
                .Where(isMoveDirection => _moveSimulation.Velocity.y < 0 && isMoveDirection)
                .Subscribe(_ =>
                {
                    _isMoving = true;
                    FlipVerticalVelocity();
                });

            // �p�h����"��"�����Ɉړ����Ă��鎞�ɁA"��"�����ɓ��͂��ꂽ��ړ��𔽓]������
            moveInput.MoveDirection
                .Where(isMoveDirection => _moveSimulation.Velocity.y > 0 && !isMoveDirection)
                .Subscribe(_ =>
                {
                    _isMoving = true;
                    FlipVerticalVelocity();
                });

            // �p�h�����ړ��\�ł���΁A����I�Ɉʒu���X�V����
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

        // �p�h����Y�����x�������_��������
        public Vector3 RandomizeVerticalVelocity(Vector3 velocity)
        {
            var randomVerticalVelocity = Vector3.up * Random.Range(-velocity.y, velocity.y);
            return randomVerticalVelocity;
        }

        // �p�h���̈ړ����~����
        public void StopPaddle()
        {
            _isMoving = false;
        }

        // �p�h���̈ʒu���X�V����
        private void UpdatePaddlePosition(float fixedDeltaTime)
        {
            // �ʒu���X�V����
            _paddle.LocalPosition = _moveSimulation.UpdatePosition(_paddle.LocalPosition, fixedDeltaTime);
        }

        // �p�h����Y�����x�𔽓]������
        private void FlipVerticalVelocity()
        {
            // ���x���擾���܂��B
            var velocity = _moveSimulation.Velocity;
            // Y���̑��x�𔽓]�����܂��B
            _moveSimulation.Velocity = new Vector3(velocity.x, -velocity.y, velocity.z);
        }
    }
}