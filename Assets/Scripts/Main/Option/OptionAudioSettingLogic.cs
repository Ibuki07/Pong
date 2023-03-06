using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Managers;
using DG.Tweening;

namespace Option
{
    public class OptionAudioSettingLogic
    {

        private Slider _audioVolumeSlider;
        private SoundManager.SoundType _soundType;

        public OptionAudioSettingLogic(
            Slider audioVolumeSlider, 
            SoundManager.SoundType soundType)
        {
            _audioVolumeSlider = audioVolumeSlider;
            _soundType = soundType;

            _audioVolumeSlider
                .OnValueChangedAsObservable()
                .Subscribe(volumeValue =>
                {
                    SoundManager.Instance.SetVolumeValue(soundType, volumeValue);
                });
        }

        public void UpdateSliderValue()
        {
            _audioVolumeSlider.value = SoundManager.Instance.GetVolumeValue(_soundType);
        }

    }


}
