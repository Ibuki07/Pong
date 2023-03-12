using DG.Tweening;
using UnityEngine;

namespace Title
{
    public class TitlePresentation
    {
        private float _fadeInDuration = 1f;
        private float _moveDuration = 1.5f;
        private float _titleLogoVerticalMoveValue = 80f;
        private float _fadeOutAlpha = 0f;
        private float _fadeInAlpha = 1f;

        public TitlePresentation(CanvasGroup titleLogo, CanvasGroup startText)
        {
            // タイトルロゴとスタートテキストのアルファ値を初期化する
            titleLogo.alpha = _fadeOutAlpha;
            startText.alpha = _fadeOutAlpha;

            // アニメーションのシーケンスを作成する
            var sequence = DOTween.Sequence();
            sequence
                // タイトルロゴの移動とフェードインを設定する
                .Append(titleLogo.transform.DOMoveY(_titleLogoVerticalMoveValue, _moveDuration).From())
                .Join(titleLogo.DOFade(_fadeInAlpha, _fadeInDuration))
                // スタートテキストのフェードインを設定する
                .Append(startText.DOFade(_fadeInAlpha, _fadeInDuration))
                // スタートテキストのアニメーションを設定する
                .AppendCallback(() =>
                {
                    startText.alpha = _fadeOutAlpha;
                    startText
                        .DOFade(_fadeInAlpha, _fadeInDuration)
                        .SetEase(Ease.InOutQuad)
                        .SetLoops(-1, LoopType.Yoyo)
                        .SetLink(startText.gameObject);
                });

        }
    }
}