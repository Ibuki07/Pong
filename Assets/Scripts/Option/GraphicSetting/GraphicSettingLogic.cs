using UnityEngine;
using UniRx;

namespace Option
{
    public class GraphicSettingLogic
    {
        public GraphicSettingLogic(GraphicSettingInput input)
        {
            // フルスクリーンモードが切り替わるたびに Screen.fullScreen の値を変更する
            input.FullScreen
                .Subscribe(isFullScreen =>
                {
                    Screen.fullScreen = isFullScreen;
                });
        }
    }
}