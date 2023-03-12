using UnityEngine;
using UniRx;

namespace Paddle
{
    public class PaddleAutoMovePresentation
    {
        public float ActiveAlfa { get; private set; } = 0.8f;
        public float DeactiveAlfa { get; private set; } = 0.4f;

        public PaddleAutoMovePresentation(
            PaddleAutoMoveInput paddleAutoMoveInput,
            CanvasGroup autoModeButtonCanvasGroup)
        {
            autoModeButtonCanvasGroup.alpha = DeactiveAlfa;
            paddleAutoMoveInput.AutoMove
                .Where(isAuto => isAuto)
                .Subscribe(_ =>
                {
                    autoModeButtonCanvasGroup.alpha = ActiveAlfa;
                });
            paddleAutoMoveInput.AutoMove
                .Where(isAuto => !isAuto)
                .Subscribe(_ =>
                {
                    autoModeButtonCanvasGroup.alpha = DeactiveAlfa;
                });
        }
    }
}