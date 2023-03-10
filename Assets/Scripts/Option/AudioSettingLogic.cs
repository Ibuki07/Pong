using UniRx;
using UnityEngine.UI;
using Managers;

namespace Option
{
    public class AudioSettingLogic
    {

        private Slider _audioVolumeSlider;
        private SoundManager.SoundType _soundType;

        public AudioSettingLogic(
            Slider audioVolumeSlider, 
            SoundManager.SoundType soundType)
        {
            _audioVolumeSlider = audioVolumeSlider;
            _soundType = soundType;

            _audioVolumeSlider
                .OnValueChangedAsObservable()
                .Skip(1)
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
