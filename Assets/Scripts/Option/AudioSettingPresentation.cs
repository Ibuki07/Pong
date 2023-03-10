using UnityEngine;
using DG.Tweening;

namespace Option
{
    public class AudioSettingPresentation
    {
        private CanvasGroup _settingPanel;

        private float _fastDuration = 0.25f;

        public AudioSettingPresentation(
            CanvasGroup settingPanel
            )
        {
            _settingPanel = settingPanel;

        }

        public void OnDisplayPanelAnimation()
        {
            _settingPanel.transform.localScale = Vector3.zero;
            _settingPanel.alpha = 0f;
            Sequence sequence = DOTween.Sequence();
            sequence
            .Append(_settingPanel.transform.DOScale(1f, _fastDuration))
            .Join(_settingPanel.DOFade(1f, _fastDuration));
        }

    }
}