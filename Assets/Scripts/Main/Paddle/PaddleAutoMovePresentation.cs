using UnityEngine;
using UniRx;

namespace Paddle
{
    public class PaddleAutoMovePresentation
    {
        // �{�^�����A�N�e�B�u�ȏꍇ�̃A���t�@�l
        public float ActiveAlpha { get; private set; } = 0.8f;
        // �{�^������A�N�e�B�u�ȏꍇ�̃A���t�@�l
        public float DeactiveAlpha { get; private set; } = 0.4f;

        public PaddleAutoMovePresentation(
            PaddleAutoMoveInput paddleAutoMoveInput,
            CanvasGroup autoModeButtonCanvasGroup)
        {
            // �����ړ����L���ɂȂ����ꍇ�A�{�^���̃A���t�@�l�� ActiveAlpha �ɕύX
            paddleAutoMoveInput.AutoMove
                .Where(isAuto => isAuto)
                .Subscribe(_ =>
                {
                    autoModeButtonCanvasGroup.alpha = ActiveAlpha;
                });

            // �����ړ��������ɂȂ����ꍇ�A�{�^���̃A���t�@�l�� DeactiveAlpha �ɕύX
            paddleAutoMoveInput.AutoMove
                .Where(isAuto => !isAuto)
                .Subscribe(_ =>
                {
                    autoModeButtonCanvasGroup.alpha = DeactiveAlpha;
                });

            // �{�^���̃A���t�@�l��������
            autoModeButtonCanvasGroup.alpha = DeactiveAlpha;
        }
    }
}