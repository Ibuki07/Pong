using UniRx;
using Managers;

namespace Option
{
    public class AudioSettingLogic
    {
        public AudioSettingLogic(
            AudioSettingInput input,
            SOUND_TYPE soundType)
        {
            // ���ʒ��߂ɕύX���������ꍇ�̏���
            input.AudioVolume
                .Skip(1)
                .Subscribe(audioVolume =>
                {
                    // SoundManager�̃C���X�^���X�ɕύX�𔽉f
                    SoundManager.Instance.SetVolumeValue(soundType, audioVolume);
                });
        }
    }
}
