using UniRx;
using UnityEngine;

namespace Paddle
{
    public class PaddleMoveInput
    {
        // �p�h���̈ړ������Ɋւ��郊�A�N�e�B�u�v���p�e�B
        public IReadOnlyReactiveProperty<bool> MoveDirection => _moveDirection;

        // �p�h�����ړ������ǂ����Ɋւ��郊�A�N�e�B�u�v���p�e�B
        public IReadOnlyReactiveProperty<bool> Move => _move;

        private readonly ReactiveProperty<bool> _moveDirection = new ReactiveProperty<bool>(true);
        private readonly ReactiveProperty<bool> _move = new ReactiveProperty<bool>(false);

        // �p�h���̈ʒu�����擾����A�_�v�^�[
        private IPaddleLocalPositionAdapter _paddlePosition;

        public PaddleMoveInput(IPaddleLocalPositionAdapter paddlePosition)
        {
            _paddlePosition = paddlePosition;
        }

        // �p�h���̈ړ������𔻒肷��
        public bool IsPaddleMoveInput()
        {
            // �p�h���̈ʒu�ɉ����āAInputArrow()��InputWASD()���Ăяo��
            if (_paddlePosition.LocalPosition.x < 0)
            {
                InputWASD();
            }
            if (_paddlePosition.LocalPosition.x > 0)
            {
                InputArrow();
            }
            // �ړ��̗L����Ԃ�
            return _move.Value;
        }

        // ���L�[�ňړ�������؂�ւ���
        public void InputArrow()
        {
            // �p�h�����ړ����ł��邱�Ƃ�������
            _move.Value = false;

            // ����L�[�Ńp�h������Ɉړ�������
            if (Input.GetKey(KeyCode.UpArrow))
            {
                _moveDirection.Value = true;
                _move.Value = true;
            }
            // �����L�[�Ńp�h�������Ɉړ�������
            if (Input.GetKey(KeyCode.DownArrow))
            {
                _moveDirection.Value = false;
                _move.Value = true;
            }
        }

        // WASD�L�[�ňړ�������؂�ւ���
        public void InputWASD()
        {
            // �p�h�����ړ����ł��邱�Ƃ�������
            _move.Value = false;

            // W�L�[�Ńp�h������Ɉړ�������
            if (Input.GetKey(KeyCode.W))
            {
                _moveDirection.Value = true;
                _move.Value = true;
            }
            // S�L�[�Ńp�h�������Ɉړ�������
            if (Input.GetKey(KeyCode.S))
            {
                _moveDirection.Value = false;
                _move.Value = true;
            }
        }
    }
}