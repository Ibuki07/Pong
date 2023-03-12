using Managers;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace Title
{
    public class TitleInput
    {
        // ゲーム開始フラグの読み取り専用ReactiveProperty
        public IReadOnlyReactiveProperty<bool> IsGameStart => _isGameStart;

        private readonly ReactiveProperty<bool> _isGameStart = new ReactiveProperty<bool>(false);
        private Button _gameStartButton;

        public TitleInput(Button gameStartButton)
        {
            // ゲームスタートボタンがクリックされたときに呼ばれるイベント
            gameStartButton.OnClickAsObservable().Subscribe(_ =>
            {
                SoundManager.Instance.PlaySE(SE_TYPE.Start);
                // ゲーム開始フラグを立てる
                _isGameStart.Value = true;
            });
        }

        public void OnGameStartInput()
        {
            // エンターキーが押されたとき
            if (Input.GetKeyDown(KeyCode.Return))
            {
                SoundManager.Instance.PlaySE(SE_TYPE.Start);
                // ゲーム開始フラグを立てる
                _isGameStart.Value = true;
            }
        }



    }
}