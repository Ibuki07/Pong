using DG.Tweening;
using UnityEngine;

public class TitlePresentation
{
    private float _fadeInDuration = 1f;
    private float _moveDuration = 1.5f;
    private float _titleLogoVerticalMoveValue = 80f;
    private float _fadeInValue = 1f;

    public TitlePresentation(CanvasGroup titleLogo, CanvasGroup startText)
    {
        titleLogo.alpha = 0.0f;
        startText.alpha = 0.0f;

        var sequence = DOTween.Sequence();
        sequence
            .Append(titleLogo.transform.DOMoveY(_titleLogoVerticalMoveValue, _moveDuration).From())
            .Join(titleLogo.DOFade(_fadeInValue, _fadeInDuration))
            .Append(startText.DOFade(_fadeInValue, _fadeInDuration))
            .AppendCallback(() => 
            { 
                startText.alpha = 0.0f; 
                startText
                    .DOFade(_fadeInValue, _fadeInDuration)
                    .SetEase(Ease.InOutQuad)
                    .SetLoops(-1, LoopType.Yoyo)
                    .SetLink(startText.gameObject);
            });

    }









}
