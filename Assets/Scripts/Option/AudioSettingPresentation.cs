using UnityEngine;
using DG.Tweening;
using Managers;
using UnityEngine.UI;

namespace Option
{
    public class AudioSettingPresentation
    {
        private Slider _bgmAudioVolumeSlider;
        private Slider _seAudioVolumeSlider;
        private CanvasGroup _settingPanel;

        private float _fadeOutValue = 0f;
        private float _fadeInValue = 1.0f;
        private float _fastDuration = 0.25f;

        public AudioSettingPresentation(
            Slider bgmAudioVolumeSlider,
            Slider seAudioVolumeSlider,
            CanvasGroup settingPanel)
        {
            _bgmAudioVolumeSlider = bgmAudioVolumeSlider;
            _seAudioVolumeSlider = seAudioVolumeSlider;
            _settingPanel = settingPanel;
        }

        public void StartDisplayPanelAnimation()
        {
            // �ݒ��ʂ̃p�l�����\���ɂ���
            _settingPanel.transform.localScale = Vector3.zero;
            _settingPanel.alpha = _fadeOutValue;

            // �p�l�����t�F�[�h�C��������A�j���[�V����
            Sequence sequence = DOTween.Sequence();
            sequence
                .Append(_settingPanel.transform.DOScale(_fadeInValue, _fastDuration))
                .Join(_settingPanel.DOFade(_fadeInValue, _fastDuration));
        }

        public void UpdateSliderValue()
        {
            // BGM���ʂ�SE���ʂ̃X���C�_�[�l���X�V����
            _bgmAudioVolumeSlider.value = SoundManager.Instance.GetVolumeValue(SOUND_TYPE.BGM);
            _seAudioVolumeSlider.value = SoundManager.Instance.GetVolumeValue(SOUND_TYPE.SE);
        }
    }
}