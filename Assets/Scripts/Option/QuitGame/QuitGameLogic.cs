using UniRx;
using UnityEngine;

namespace Option
{
    public class QuitGameLogic
    {
        public QuitGameLogic(QuitGameInput input)
        {
            // QuitGameInputからQuitGameがtrueになった時、ゲームを終了する
            input.QuitGame
                .Where(isQuitGame => isQuitGame)
                .Subscribe(_ =>
                {
#if UNITY_EDITOR
                    // Unity Editor上で実行中の場合、再生を停止する
                    UnityEditor.EditorApplication.isPlaying = false;
#else
                    // 実行中のアプリケーションを終了する
                    Time.timeScale = 1;
                    Application.Quit();
#endif
                });
        }
    }
}