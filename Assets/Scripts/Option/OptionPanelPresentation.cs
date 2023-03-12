using DG.Tweening;
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

        // アニメーションパラメーター
        private float _verticalMoveValue = 80f;
        private float _horizontalMoveValue = 80f;
        private float _fadeOutValue = 0f;
        private float _fadeInValue = 1.0f;
        private float _duration = 0.5f;
        private float _fastDuration = 0.25f;
        private float _delayTime = 0.25f;

        // ボタンごとの説明文
        private readonly string[] info_message_text =
        {
            "",
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
            Text infoMessageText)
        {
            _optionPanel = optionPanel;
            _titleLogoBordor = titleLogoBordor;
            _buttonSpace = buttonSpace;
            _infoMessageBordor = infoMessageBordor;
            _infoMessageText = infoMessageText;

            // InfoMessageの変更を監視し、表示する説明文を切り替える
            panelInput.InfoMessage
                .Subscribe(infoMessage =>
                {
                    SetInfoMessageText((OPTION_TYPE)infoMessage);
                });

            // PanelOpenがtrueになった時、説明文を表示する
            panelInput.PanelOpen
                .Where(isPanelOpen => isPanelOpen)
                .Subscribe(_ =>
                {
                    SetInfoMessageText(OPTION_TYPE.PanelOpen);
                });
        }

        // ボタンごとの説明文を設定するメソッド
        private void SetInfoMessageText(OPTION_TYPE buttonType)
        {
            _infoMessageText.text = info_message_text[(int)buttonType];
        }

        // パネルの表示アニメーション
        public void StartDisplayPanelAnimation()
        {
            // UI要素の透明度を設定
            _optionPanel.alpha = _fadeOutValue;
            _titleLogoBordor.alpha = _fadeOutValue;
            _buttonSpace.alpha = _fadeOutValue;
            _infoMessageBordor.alpha = _fadeOutValue;

            Sequence sequence = DOTween.Sequence();
            sequence
                .Append(_optionPanel.DOFade(_fadeInValue, _fastDuration))
                .Append(_titleLogoBordor.transform.DOMoveX(_verticalMoveValue, _duration).From())
                .Join(_titleLogoBordor.DOFade(_fadeInValue, _duration))
                .Join(_buttonSpace.transform.DOMoveY(_horizontalMoveValue, _fastDuration).From().SetDelay(_delayTime))
                .Join(_buttonSpace.DOFade(_fadeInValue, _duration));

            sequence
                .Append(_infoMessageBordor.DOFade(_fadeInValue, _duration));
        }

        // パネルを非表示にするアニメーション
        public Sequence StartHiddenPanelAnimation()
        {
            Sequence sequence = DOTween.Sequence();
            sequence
                .Append(_optionPanel.DOFade(_fadeOutValue, _fastDuration));

            return sequence;
        }
    }
}