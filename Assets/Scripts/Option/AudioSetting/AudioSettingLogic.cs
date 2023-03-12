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
            // 音量調節に変更があった場合の処理
            input.AudioVolume
                .Skip(1)
                .Subscribe(audioVolume =>
                {
                    // SoundManagerのインスタンスに変更を反映
                    SoundManager.Instance.SetVolumeValue(soundType, audioVolume);
                });
        }
    }
}
