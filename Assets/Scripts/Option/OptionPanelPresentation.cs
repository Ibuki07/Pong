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
        private Text _infoMessageText;
        private float _duration = 0.5f;
        private float _fastDuration = 0.25f;
        private float _delayTime = 0.25f;
        
        private readonly string[] info_message_text =
        {
            "",
            "ゲーム設定画面を表示します。",
            "グラフィック設定画面を表示します。",
            "音量設定画面を表示します。",
            "タイトルへ戻ります。",
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
            _infoMessageText = infoMessageText;
            _infoMessageText.text = info_message_text[0];

            panelInput.InfoMessage
                .Subscribe(infoMessage =>
                {
                    switch(infoMessage)
                    {
                        case (int)OptionCore.ButtonType.GameSetting:
                            SetInfoMessageText(OptionCore.ButtonType.GameSetting);
                            break;
                        case (int)OptionCore.ButtonType.GraphicSetting:
                            SetInfoMessageText(OptionCore.ButtonType.GraphicSetting);
                            break;
                        case (int)OptionCore.ButtonType.AudioSetting:
                            SetInfoMessageText(OptionCore.ButtonType.AudioSetting);
                            break;
                        case (int)OptionCore.ButtonType.QuitTitle:
                            SetInfoMessageText(OptionCore.ButtonType.QuitTitle);
                            break;
                        case (int)OptionCore.ButtonType.QuitGame:
                            SetInfoMessageText(OptionCore.ButtonType.QuitGame);
                            break;
                        case (int)OptionCore.ButtonType.PanelClose:
                            SetInfoMessageText(OptionCore.ButtonType.PanelClose);
                            break;
                    }
                });

        }

        private void SetInfoMessageText(OptionCore.ButtonType buttonType)
        {
            _infoMessageText.text = info_message_text[(int)buttonType];
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