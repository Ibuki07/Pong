using Managers;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace Title
{
    public class TitleCore : MonoBehaviour
    {

        [SerializeField] private Button _gameStartButton;
        [SerializeField] private CanvasGroup titleLogo;
        [SerializeField] private CanvasGroup startText;

        private TitleInput _titleInput;
        private TitlePresentation _titlePresentation;

        private void Start()
        {
            // シーン内のフェードイン処理
            SceneStateManager.Instance.StartFadeIn();

            // タイトルBGM再生
            SoundManager.Instance.PlayBGM(BGM_TYPE.Title);

            _titleInput = new TitleInput(_gameStartButton);
            _titlePresentation = new TitlePresentation(titleLogo, startText);

            // ゲームスタートの入力があった場合の処理
            _titleInput.IsGameStart
                .Where(isGameStart => isGameStart)
                // 一回だけ実行
                .Take(1) 
                .Subscribe(_ =>
                {
                    // ゲームメインシーンに遷移する
                    SceneStateManager.Instance.LoadSceneFadeOut(SCENE_TYPE.Main); 
                });

            // 毎フレーム、ゲームスタートの入力を取得する
            Observable.EveryUpdate()
                .Subscribe(_ =>
                {
                    _titleInput.OnGameStartInput();
                })
                .AddTo(this);
        }
    }
}