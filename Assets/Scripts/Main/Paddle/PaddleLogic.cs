using UniRx;
using UnityEngine;

namespace Paddle
{
    public class PaddleLogic
    {
        private IPaddleLocalPositionAdapter _localPositionAdapter;
        private PaddleSimulation _paddleSimulation;
        private PaddleMoveInput _paddleMoveInput;
        private bool _isMove = true;

        // --------------------------------------------------

        public PaddleLogic(IPaddleLocalPositionAdapter localPositionAdapter, PaddleMoveInput paddleMoveInput, PaddleSimulation paddleSimulation)
        {
            // �R�A�N���X���擾����
            _localPositionAdapter = localPositionAdapter;
            _paddleMoveInput = paddleMoveInput;
            // �V�~�����[�V�����N���X���擾����
            _paddleSimulation = paddleSimulation;
        }

        public void OnUpdateLocalPosition(float fixedDeltaTime)
        {
            if (_paddleMoveInput.IsPaddleMoveInput())
            {
                OnFlipVerticalDirection();

                if (_isMove)
                {
                    // �ʒu���X�V����
                    _localPositionAdapter.LocalPosition = _paddleSimulation.UpdatePosition(_localPositionAdapter.LocalPosition, fixedDeltaTime);
                }
            }
        }

        public void OnStopPaddle()
        {
            _isMove = false;
        }

        public Vector3 OnRandomizeVerticalVelocity(Vector3 velocity)
        {
            var rondomVerticalVelocity = Vector3.up * Random.Range(-velocity.y, velocity.y);
            return rondomVerticalVelocity;
        }

        // --------------------------------------------------

        private void OnFlipVerticalDirection()
        {
            // ���x���擾���܂��B
            var velocity = _paddleSimulation.Velocity;

            // ��
            _paddleMoveInput.MoveDirection
                .Where(isMoveDirection => velocity.y < 0 && isMoveDirection)
                .Subscribe(_ =>
                {
                    _isMove = true;
                    // Y���̑��x�𔽓]�����܂��B
                    _paddleSimulation.Velocity = new Vector3(velocity.x, -velocity.y, velocity.z);
                });
            // ��
            _paddleMoveInput.MoveDirection
                .Where(isMoveDirection => velocity.y > 0 && !isMoveDirection)
                .Subscribe(_ =>
                {
                    _isMove = true;
                    // Y���̑��x�𔽓]�����܂��B
                    _paddleSimulation.Velocity = new Vector3(velocity.x, -velocity.y, velocity.z);
                });
        }


    }
}