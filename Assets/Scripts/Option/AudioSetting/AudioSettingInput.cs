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
            // �X���C�_�[�̒l�ύX�����m���A_audioVolume �ɒl�𔽉f����
            audioVolumeSlider.OnValueChangedAsObservable()
                .Subscribe(audioVolume =>
                {
                    _audioVolume.Value = audioVolume;
                });
        }
    }
}