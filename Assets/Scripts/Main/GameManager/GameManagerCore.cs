using Ball;
using Cysharp.Threading.Tasks;
using Goal;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace Managers
{
    public class GameManagerCore : MonoBehaviour
    {
        [SerializeField] private GoalCore[] _goal = new GoalCore[2];
        [SerializeField] private BallCore _ball;
        [SerializeField] private Button _restartButton;
        [SerializeField] private Text _gameStartText;
        [SerializeField] private CanvasGroup _gameStartCanvasGroup;
        [SerializeField, Min(1)] private int _maxGoalCount;

        private GameManagerInput _gameManagerInput;
        private GameManagerLogic _gameManagerLogic;
        private GameManagerPresentation _gameManagerPresentation;

        private async void Start()
        {
            _gameManagerInput = new GameManagerInput(_restartButton);
            _gameManagerLogic = new GameManagerLogic();
            _gameManagerPresentation = new GameManagerPresentation(_gameStartText);


            // SceneStateManagerを使用してフェードインを開始する
            SceneStateManager.Instance.StartFadeIn();

            // GoalCoreの初期化を待つ
            foreach (var goal in _goal)
            {
                await goal.InitializedAsync;
                EndGame(goal);
            }

            // ボールが破壊されたら再開ボタンを表示する
            _ball.MoveLogic.Destroyed
                .Where(isDestroyed => isDestroyed)
                .Subscribe(_ =>
                {
                    _restartButton.gameObject.SetActive(true);
                });

            // 再開ボタンがクリックされたら、再開ボタンを非表示にしてゲームを再開する
            _gameManagerInput.Restart
                .Where(isRestart => isRestart)
                .Subscribe(_ =>
                {
                    _restartButton.gameObject.SetActive(false);
                    StartGame().Forget();
                });

            // ボールが破壊された状態でEnterキーが押されたら、再開ボタンを非表示にしてゲームを再開する
            Observable.EveryUpdate()
                .Where(_ => _ball.MoveLogic.Destroyed.Value)
                .Subscribe(_ =>
                {
                    _gameManagerInput.IsRestart();
                })
                .AddTo(this);

            // ボールとゲーム開始テキストを非表示にする
            _ball.gameObject.SetActive(false);
            _restartButton.gameObject.SetActive(false);
            _gameStartText.gameObject.SetActive(false);

            // フェードインが完了するまで待機する
            await UniTask.Delay(System.TimeSpan.FromSeconds(SceneStateManager.Instance.FadeDuration));

            // ゲームを開始する
            StartGame().Forget();

        }

        // ゲームを開始する
        private async UniTask StartGame()
        {
            // BGMをランダムに再生する
            SoundManager.Instance.PlayBGM(_gameManagerLogic.RandomizeBGM());

            // ゲーム開始テキストを表示する
            _gameStartText.gameObject.SetActive(true);
            
            // ゲーム開始アニメーションを再生する
            await _gameManagerPresentation.GameStartAnimation();
            
            // ゲーム開始テキストを非表示にする
            _gameStartText.gameObject.SetActive(false);

            // ゴールに関するスコアをリセットし、ボールを生成する
            foreach (var goal in _goal)
            {
                goal.ScoreLogic.ResetScoreCount();
            }
            _ball.MoveLogic.OnCreatedBall();
        }

        private void EndGame(GoalCore goal)
        {
            // ゴールが_maxGoalCount回以上達成された場合、ボールを破壊する
            goal.ScoreLogic.ScoreCount
                .Where(goalCount => goalCount >= _maxGoalCount)
                .Subscribe(_ =>
                {
                    _ball.MoveLogic.OnDestroyedBall();
                });
        }
    }
}