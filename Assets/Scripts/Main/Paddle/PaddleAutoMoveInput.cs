using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace Paddle
{
    public class PaddleAutoMoveInput
    {
        // �����ړ��̏�Ԃ�`����ReactiveProperty
        public IReadOnlyReactiveProperty<bool> AutoMove => _autoMove;
        private readonly ReactiveProperty<bool> _autoMove = new ReactiveProperty<bool>(true);

        // �p�h���̍��W���擾���邽�߂̃A�_�v�^
        private readonly IPaddleLocalPositionAdapter _paddlePosition;

        public PaddleAutoMoveInput(IPaddleLocalPositionAdapter paddlePosition, Button autoMoveButton)
        {
            _paddlePosition = paddlePosition;

            // �{�^�����N���b�N�������ɁA�����ړ��̏�Ԃ�؂�ւ���
            autoMoveButton.OnClickAsObservable().Subscribe(_ =>
            {
                ToggleAutoMove();
            });
        }

        // �����ړ���Ԃ��ǂ�����Ԃ����\�b�h
        public bool IsAutoMoveInput()
        {
            // �p�h�������ɂ��鎞��F�L�[�A�E�ɂ��鎞��J�L�[�������Ǝ����ړ��̏�Ԃ�؂�ւ���
            if (_paddlePosition.LocalPosition.x < 0 && Input.GetKeyDown(KeyCode.F))
            {
                ToggleAutoMove();
            }
            if (_paddlePosition.LocalPosition.x > 0 && Input.GetKeyDown(KeyCode.J))
            {
                ToggleAutoMove();
            }
            // ���݂̎����ړ���Ԃ�Ԃ�
            return _autoMove.Value;
        }

        // �����ړ��̏�Ԃ�؂�ւ��郁�\�b�h
        private void ToggleAutoMove()
        {
            _autoMove.Value = !_autoMove.Value;
        }
    }
}