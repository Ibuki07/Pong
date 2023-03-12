using DG.Tweening;
using UnityEngine;

namespace Option
{
    public class QuitGamePresentation
    {
        private CanvasGroup _settingPanel;
        private float _fadeOutValue = 0f;
        private float _fadeInValue = 1.0f;
        private float _fastDuration = 0.25f;

        public QuitGamePresentation(CanvasGroup settingPanel)
        {
            _settingPanel = settingPanel;
        }

        public void StartDisplayPanelAnimation()
        {
            // ÉpÉlÉãÇÃèâä˙ê›íË
            _settingPanel.transform.localScale = Vector3.zero;
            _settingPanel.alpha = _fadeOutValue;

            Sequence sequence = DOTween.Sequence();
            sequence
                .Append(_settingPanel.transform.DOScale(Vector3.one, _fastDuration))
                .Join(_settingPanel.DOFade(_fadeInValue, _fastDuration));
        }
    }
}