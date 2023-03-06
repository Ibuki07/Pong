using UnityEngine;
using UnityEngine.UI;
using UniRx;

namespace Option
{
    public class OptionQuitGameLogic
    {
        public OptionQuitGameLogic(Button quitGameButton)
        {
            quitGameButton
                .OnClickAsObservable()
                .First()
                .Subscribe(_ =>
                {
#if UNITY_EDITOR
                    UnityEditor.EditorApplication.isPlaying = false;
#else
                    Application.Quit();
#endif
                });
        }

    }
}