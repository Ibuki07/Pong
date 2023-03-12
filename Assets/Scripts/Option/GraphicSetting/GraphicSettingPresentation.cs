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
            // �R���X�g���N�^�����Ŏ󂯎�����l���v���C�x�[�g�ϐ��ɑ��
            _settingPanel = settingPanel;
            var fullScreenButtonImage = fullScreenButton.GetComponent<Image>();

            // �t���X�N���[���ݒ肪ON/OFF�ɐ؂�ւ�����Ƃ��̏���
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

        // �p�l���\���̃A�j���[�V��������
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