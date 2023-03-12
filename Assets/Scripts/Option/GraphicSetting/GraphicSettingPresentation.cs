using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

namespace Option
{
    public class GraphicSettingPresentation
    {
        private CanvasGroup _settingPanel;
        private float _fadeOutValue = 0f;
        private float _fadeInValue = 1.0f;
        private float _fastDuration = 0.25f;

        public GraphicSettingPresentation(
            GraphicSettingInput input,
            CanvasGroup settingPanel,
            Button fullScreenButton)
        {
            // コンストラクタ引数で受け取った値をプライベート変数に代入
            _settingPanel = settingPanel;
            var fullScreenButtonImage = fullScreenButton.GetComponent<Image>();

            // フルスクリーン設定がON/OFFに切り替わったときの処理
            input.FullScreen
                .Where(isFullScreen => isFullScreen)
                .Subscribe(_ =>
                {
                    fullScreenButtonImage.color = Color.red;
                });
            input.FullScreen
                .Where(isFullScreen => !isFullScreen)
                .Subscribe(_ =>
                {
                    fullScreenButtonImage.color = Color.white;
                });
        }

        // パネル表示のアニメーション処理
        public void StartDisplayPanelAnimation()
        {
            _settingPanel.transform.localScale = Vector3.zero;
            _settingPanel.alpha = _fadeOutValue;

            Sequence sequence = DOTween.Sequence();
            sequence
                .Append(_settingPanel.transform.DOScale(Vector3.one, _fastDuration))
                .Join(_settingPanel.DOFade(_fadeInValue, _fastDuration));
        }
    }
}