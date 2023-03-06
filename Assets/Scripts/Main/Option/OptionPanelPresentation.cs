using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

namespace Option
{
    public class OptionPanelPresentation
    {
        private CanvasGroup _optionPanel;
        private CanvasGroup _titleLogoBordor;
        private CanvasGroup _buttonSpace;
        private CanvasGroup _infoMessageBordor;

        private float _duration = 0.5f;
        private float _fastDuration = 0.25f;
        private float _delayTime = 0.25f;
        
        private readonly string[] _infoMessage =
        {
            "",
            "ゲーム設定画面を表示します。",
            "音量設定画面を表示します。",
            "ゲームを終了します。",
            "オプション画面を閉じます。",
        };

        public OptionPanelPresentation(
            OptionPanelInput panelInput,
            CanvasGroup optionPanel,
            CanvasGroup titleLogoBordor,
            CanvasGroup buttonSpace,
            CanvasGroup infoMessageBordor,
            Text infoMessageText
            )
        {
            _optionPanel = optionPanel;
            _titleLogoBordor = titleLogoBordor;
            _buttonSpace = buttonSpace;
            _infoMessageBordor = infoMessageBordor;
            infoMessageText.text = _infoMessage[0];

            panelInput.InfoMessage
                .Subscribe(infoMessage =>
                {
                    switch(infoMessage)
                    {
                        case (int)OptionPanelInput.InfoMessageType.GameSetting:
                            infoMessageText.text = _infoMessage[(int)OptionPanelInput.InfoMessageType.GameSetting];
                            break;
                        case (int)OptionPanelInput.InfoMessageType.AudioSetting:
                            infoMessageText.text = _infoMessage[(int)OptionPanelInput.InfoMessageType.AudioSetting];
                            break;
                        case (int)OptionPanelInput.InfoMessageType.QuitGame:
                            infoMessageText.text = _infoMessage[(int)OptionPanelInput.InfoMessageType.QuitGame];
                            break;
                        case (int)OptionPanelInput.InfoMessageType.PanelClose:
                            infoMessageText.text = _infoMessage[(int)OptionPanelInput.InfoMessageType.PanelClose];
                            break;
                    }
                });

        }

        public void OnDisplayPanelAnimation()
        {
            _optionPanel.alpha = 0f;
            _titleLogoBordor.alpha = 0f;
            _buttonSpace.alpha = 0f;
            _infoMessageBordor.alpha = 0f;

            Sequence sequence = DOTween.Sequence();
            sequence
            .Append(_optionPanel.DOFade(1f, _fastDuration))
            .Append(_titleLogoBordor.transform.DOMoveX(80f, _duration).From())
            .Join(_titleLogoBordor.DOFade(1f, _duration))
            .Join(_buttonSpace.transform.DOMoveY(80f, _fastDuration).From().SetDelay(_delayTime))
            .Join(_buttonSpace.DOFade(1f, _duration));

            sequence
            .Append(_infoMessageBordor.DOFade(1f, _duration));
        }

        public Sequence OnHiddenPanelAnimation()
        {
            Sequence sequence = DOTween.Sequence();
            sequence.Append(_optionPanel.DOFade(0f, _fastDuration));

            return sequence;
        }
    }
}