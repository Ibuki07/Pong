using UnityEngine;
using UniRx;

namespace Option
{
    public class GraphicSettingLogic
    {
        public GraphicSettingLogic(GraphicSettingInput input)
        {
            // �t���X�N���[�����[�h���؂�ւ�邽�т� Screen.fullScreen �̒l��ύX����
            input.FullScreen
                .Subscribe(isFullScreen =>
                {
                    Screen.fullScreen = isFullScreen;
                });
        }
    }
}