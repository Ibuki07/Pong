using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GraphicSettingPresentation
{
    private CanvasGroup _settingPanel;

    private float _fastDuration = 0.25f;

    public GraphicSettingPresentation(
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
