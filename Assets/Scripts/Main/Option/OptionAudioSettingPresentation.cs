using UnityEngine;
using UnityEngine.UI;
using UniRx;
using DG.Tweening;

public class OptionAudioSettingPresentation
{
    private CanvasGroup _audioSettingPanel;

    private float _fastDuration = 0.25f;

    public OptionAudioSettingPresentation(
        CanvasGroup audioSettingPanel
        ) 
    {
        _audioSettingPanel = audioSettingPanel;

    }

    public void OnDisplayPanelAnimation()
    {
        _audioSettingPanel.transform.localScale = Vector3.zero;
        _audioSettingPanel.alpha = 0f;
        Sequence sequence = DOTween.Sequence();
        sequence
        .Append(_audioSettingPanel.transform.DOScale(1f, _fastDuration))
        .Join(_audioSettingPanel.DOFade(1f, _fastDuration));
    }

}
