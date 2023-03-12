using UniRx;
using UnityEngine.UI;

namespace Option
{
    public class QuitGameInput
    {
        // ゲーム終了要求の状態を表す ReactiveProperty
        public IReadOnlyReactiveProperty<bool> QuitGame => _quitGame;
        // ゲーム終了のキャンセル要求の状態を表す ReactiveProperty
        public IReadOnlyReactiveProperty<bool> CancelQuitGame => _cancelQuitGame;

        // ゲーム終了要求の状態を格納する ReactiveProperty
        private readonly ReactiveProperty<bool> _quitGame = new ReactiveProperty<bool>(false);
        // ゲーム終了のキャンセル要求の状態を格納する ReactiveProperty
        private readonly ReactiveProperty<bool> _cancelQuitGame = new ReactiveProperty<bool>(false);

        public QuitGameInput(
            Button quitGameButton,
            Button decisionButton,
            Button cancelButton)
        {
            // ゲーム終了確定ボタンが押された時の処理
            decisionButton.OnClickAsObservable()
                // 最初の1回だけ反応するようにする
                .First()
                .Subscribe(_ =>
                {
                    // ゲーム終了要求を発行する
                    _quitGame.Value = true;
                });

            // ゲーム終了キャンセルボタンが押された時の処理
            cancelButton.OnClickAsObservable()
                .Subscribe(_ =>
                {
                    // ゲーム終了キャンセル要求を発行する
                    _cancelQuitGame.Value = true;
                });

            // ゲーム終了ボタンが押された時の処理
            quitGameButton.OnClickAsObservable()
                .Subscribe(_ =>
                {
                    // ゲーム終了キャンセル要求をクリアする
                    _cancelQuitGame.Value = false;
                });
        }
    }
}