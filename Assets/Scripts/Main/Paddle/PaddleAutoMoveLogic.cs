using Ball;
using UniRx;
using UnityEngine;

namespace Paddle
{
    public class PaddleAutoMoveLogic : System.IDisposable
    {
        // �p�h�����{�[�������m���鋗��
        public float DetectionRange { get; set; } = 0.2f;

        private PaddleAutoMoveInput _autoMoveInput;
        private PaddleMoveSimulation _moveSimulation;
        private IPaddleLocalPositionAdapter _paddle;
        private IBallLocalPositionAdapter _ball;
        private System.IDisposable _fixedUpdateDisposable;

        // �p�h���̒��S��x���W
        private float _centerPoint = 0;
        // �p�h���������Ă��邩�ǂ����̃t���O
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

            // �t���[���X�V���ƂɌĂ΂��T�u�X�N���v�V������ݒ肷��
            _fixedUpdateDisposable = Observable.EveryFixedUpdate()
                .Where(_ => _autoMoveInput.IsAutoMoveInput() && IsMoveTrackingBall() && _isMoving)
                .Subscribe(_ =>
                {
                    // �p�h���̈ʒu���X�V����
                    UpdateLocalPosition(Time.fixedDeltaTime);
                });

        }

        public void Dispose()
        {
            _fixedUpdateDisposable?.Dispose();
        }

        public void OnStopPaddle()
        {
            // �p�h���̓������~�߂�
            _isMoving = false;
        }

        private void UpdateLocalPosition(float fixedDeltaTime)
        {
            // �ʒu���X�V����
            _paddle.LocalPosition = _moveSimulation.UpdatePosition(_paddle.LocalPosition, fixedDeltaTime);
        }

        private bool IsMoveTrackingBall()
        {
            bool isMove = false;
            // �p�h����"��"�ɂ��鎞�ɁA�{�[�������Α���"�E"�ɂ���Ȃ瓮���Ȃ�
            if (_paddle.LocalPosition.x < _centerPoint && _ball.LocalPosition.x > _centerPoint)
            {
                return isMove;
            }
            // �p�h����"�E"�ɂ��鎞�ɁA�{�[�������Α���"��"�ɂ���Ȃ瓮���Ȃ�
            if (_paddle.LocalPosition.x > _centerPoint && _ball.LocalPosition.x < _centerPoint)
            {
                return isMove;
            }
            // �{�[�����p�h���̏�ɂ���ꍇ�́A�p�h������ɓ�����
            if (_ball.LocalPosition.y > _paddle.LocalPosition.y + DetectionRange)
            {
                isMove = true;
                if (_moveSimulation.Velocity.y < _centerPoint)
                {
                    // �p�h���̓������J�n���A�{�[���𔽓]������
                    _isMoving = true;
                    FlipVerticalVelocity();
                }
            }
            // �{�[�����p�h���̉��ɂ���ꍇ�́A�p�h�������ɓ�����
            if (_ball.LocalPosition.y < _paddle.LocalPosition.y - DetectionRange)
            {
                isMove = true;
                if (_moveSimulation.Velocity.y > _centerPoint)
                {
                    // �p�h���̓������J�n���A�{�[���𔽓]������
                    _isMoving = true;
                    FlipVerticalVelocity();
                }
            }
            return isMove;
        }

        private void FlipVerticalVelocity()
        {
            // ���x���擾���܂��B
            var velocity = _moveSimulation.Velocity;
            // Y���̑��x�𔽓]�����܂��B
            _moveSimulation.Velocity = new Vector3(velocity.x, -velocity.y, velocity.z);
        }
    }
}