using UnityEngine;
using UniRx;

namespace Paddle
{
    public class PaddleAutoMovePresentation
    {
        // ボタンがアクティブな場合のアルファ値
        public float ActiveAlpha { get; private set; } = 0.8f;
        // ボタンが非アクティブな場合のアルファ値
        public float DeactiveAlpha { get; private set; } = 0.4f;

        public PaddleAutoMovePresentation(
            PaddleAutoMoveInput paddleAutoMoveInput,
            CanvasGroup autoModeButtonCanvasGroup)
        {
            // 自動移動が有効になった場合、ボタンのアルファ値を ActiveAlpha に変更
            paddleAutoMoveInput.AutoMove
                .Where(isAuto => isAuto)
                .Subscribe(_ =>
                {
                    autoModeButtonCanvasGroup.alpha = ActiveAlpha;
                });

            // 自動移動が無効になった場合、ボタンのアルファ値を DeactiveAlpha に変更
            paddleAutoMoveInput.AutoMove
                .Where(isAuto => !isAuto)
                .Subscribe(_ =>
                {
                    autoModeButtonCanvasGroup.alpha = DeactiveAlpha;
                });

            // ボタンのアルファ値を初期化
            autoModeButtonCanvasGroup.alpha = DeactiveAlpha;
        }
    }
}