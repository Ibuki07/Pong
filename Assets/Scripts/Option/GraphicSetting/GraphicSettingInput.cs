using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace Option
{
    public class GraphicSettingInput
    {
        // �t���X�N���[����Ԃ��擾����ǂݎ���p��ReactiveProperty
        public IReadOnlyReactiveProperty<bool> FullScreen => _fullScreen;

        private readonly ReactiveProperty<bool> _fullScreen = new ReactiveProperty<bool>(false);

        public GraphicSettingInput(Button fullScreenButton)
        {
            // �t���X�N���[���ɂ���
            fullScreenButton.OnClickAsObservable()
                .Where(_ => !_fullScreen.Value && !Screen.fullScreen)
                .Subscribe(_ =>
                {
                    _fullScreen.Value = true;
                });

            // �t���X�N���[������������
            fullScreenButton.OnClickAsObservable()
                .Where(_ => _fullScreen.Value && Screen.fullScreen)
                .Subscribe(_ =>
                {
                    _fullScreen.Value = false;
                });
        }
    }
}