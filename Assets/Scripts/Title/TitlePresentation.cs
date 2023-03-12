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
            // �^�C�g�����S�ƃX�^�[�g�e�L�X�g�̃A���t�@�l������������
            titleLogo.alpha = _fadeOutAlpha;
            startText.alpha = _fadeOutAlpha;

            // �A�j���[�V�����̃V�[�P���X���쐬����
            var sequence = DOTween.Sequence();
            sequence
                // �^�C�g�����S�̈ړ��ƃt�F�[�h�C����ݒ肷��
                .Append(titleLogo.transform.DOMoveY(_titleLogoVerticalMoveValue, _moveDuration).From())
                .Join(titleLogo.DOFade(_fadeInAlpha, _fadeInDuration))
                // �X�^�[�g�e�L�X�g�̃t�F�[�h�C����ݒ肷��
                .Append(startText.DOFade(_fadeInAlpha, _fadeInDuration))
                // �X�^�[�g�e�L�X�g�̃A�j���[�V������ݒ肷��
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