using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace Paddle
{
    public class PaddleAutoMoveInput
    {
        public IReadOnlyReactiveProperty<bool> AutoMove => _autoMove;

        private readonly ReactiveProperty<bool> _autoMove = new ReactiveProperty<bool>(true);
        private IPaddleLocalPositionAdapter _paddlePosition;

        public PaddleAutoMoveInput(
            IPaddleLocalPositionAdapter paddlePosition,
            Button autoMoveButton)
        {
            _paddlePosition = paddlePosition;

            autoMoveButton.OnClickAsObservable().Subscribe(_ =>
            {
                ToggleAutoMove();
            });
        }

        public bool IsAutoMoveInput()
        {
            // �p�h�������ɂ��鎞
            if (_paddlePosition.LocalPosition.x < 0 && Input.GetKeyDown(KeyCode.F))
            {
                ToggleAutoMove();
            }
            // �p�h�����E�ɂ��鎞
            if (_paddlePosition.LocalPosition.x > 0 && Input.GetKeyDown(KeyCode.J))
            {
                ToggleAutoMove();
            }
            return _autoMove.Value;
        }

        // --------------------------------------------------

        private void ToggleAutoMove()
        {
            if (!_autoMove.Value)
            {
                _autoMove.Value = true;
            }
            else if (_autoMove.Value)
            {
                _autoMove.Value = false;
            }
        }
    }
}