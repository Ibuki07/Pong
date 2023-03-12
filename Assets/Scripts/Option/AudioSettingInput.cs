using UniRx;
using UnityEngine.UI;

namespace Option
{
    public class AudioSettingInput
    {
        public IReadOnlyReactiveProperty<float> AudioVolume => _audioVolume;

        private readonly ReactiveProperty<float> _audioVolume = new ReactiveProperty<float>();

        public AudioSettingInput(Slider audioVolumeSlider)
        {
            // スライダーの値変更を検知し、_audioVolume に値を反映する
            audioVolumeSlider.OnValueChangedAsObservable()
                .Subscribe(audioVolume =>
                {
                    _audioVolume.Value = audioVolume;
                });
        }
    }
}