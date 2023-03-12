using Ball;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace Paddle
{
    public class PaddleAutoMoveLogic : System.IDisposable
    {
        public float DetectionRange { get; set; } = 0.2f;

        private PaddleAutoMoveInput _autoMoveInput;
        private PaddleMoveSimulation _moveSimulation;
        private IPaddleLocalPositionAdapter _paddle;
        private IBallLocalPositionAdapter _ball;
        private System.IDisposable _disposable;
        private float _centerPoint = 0;
        private bool _isMove = true;

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
            _disposable = Observable.EveryFixedUpdate()
                .Where(_ => _autoMoveInput.IsAutoMoveInput() && IsMoveTrackingBall() && _isMove)
                .Subscribe(_ =>
                {
                    UpdateLocalPosition(Time.fixedDeltaTime);
                });

        }

        public void Dispose()
        {
            _disposable?.Dispose();
        }

        public void OnStopPaddle()
        {
            _isMove = false;
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
            if (_ball.LocalPosition.y > _paddle.LocalPosition.y + DetectionRange)
            {
                isMove = true;
                if (_moveSimulation.Velocity.y < _centerPoint)
                {
                    _isMove = true;
                    FlipVerticalVelocity();
                }
            }
            if (_ball.LocalPosition.y < _paddle.LocalPosition.y - DetectionRange)
            {
                isMove = true;
                if (_moveSimulation.Velocity.y > _centerPoint)
                {
                    _isMove = true;
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