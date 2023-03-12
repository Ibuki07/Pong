using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace Option
{
    public class GraphicSettingInput
    {
        // フルスクリーン状態を取得する読み取り専用のReactiveProperty
        public IReadOnlyReactiveProperty<bool> FullScreen => _fullScreen;

        private readonly ReactiveProperty<bool> _fullScreen = new ReactiveProperty<bool>(false);

        public GraphicSettingInput(Button fullScreenButton)
        {
            // フルスクリーンにする
            fullScreenButton.OnClickAsObservable()
                .Where(_ => !_fullScreen.Value && !Screen.fullScreen)
                .Subscribe(_ =>
                {
                    _fullScreen.Value = true;
                });

            // フルスクリーンを解除する
            fullScreenButton.OnClickAsObservable()
                .Where(_ => _fullScreen.Value && Screen.fullScreen)
                .Subscribe(_ =>
                {
                    _fullScreen.Value = false;
                });
        }
    }
}