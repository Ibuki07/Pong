using Ball;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Goal;
using System.Threading;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace Managers
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private GoalCore[] _goal = new GoalCore[2];
        [SerializeField] private BallCore _ball;
        [SerializeField] private Button _restartButton;
        [SerializeField] private Text _gameStartText;
        [SerializeField] private CanvasGroup _gameStartCanvasGroup;
        [SerializeField, Min(1)] private int _maxGoalCount;

        private float _duration = 1.0f;
        private int _gameStartCountTime = 3;

        private async void Start()
        {
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
            _restartButton.OnClickAsObservable()
                .Subscribe(_ =>
                {
                    _restartButton.gameObject.SetActive(false);
                    StartGame().Forget();
                });

            // ボールが破壊された状態でEnterキーが押されたら、再開ボタンを非表示にしてゲームを再開する
            Observable.EveryUpdate()
                .Where(_ => _ball.MoveLogic.Destroyed.Value && Input.GetKeyDown(KeyCode.Return))
                .Subscribe(_ =>
                {
                    _restartButton.gameObject.SetActive(false);
                    StartGame().Forget();
                })
                .AddTo(this);

            // ボールとゲーム開始テキストを非表示にする
            _ball.gameObject.SetActive(false);
            _restartButton.gameObject.SetActive(false);
            _gameStartText.text = "";
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
            var bgm = Random.Range((int)BGM_TYPE.Main_1, (int)BGM_TYPE.Main_3);
            SoundManager.Instance.PlayBGM((BGM_TYPE)bgm);

            // ゲーム開始アニメーションを再生する
            await GameStartAnimation();

            // ゴールに関するスコアをリセットし、ボールを生成する
            foreach (var goal in _goal)
            {
                goal.ScoreLogic.ResetScoreCount();
            }
            _ball.MoveLogic.OnCreatedBall();
        }

        // ゲーム開始アニメーションを再生する
        private async UniTask GameStartAnimation()
        {
            // ゲーム開始テキストを表示する
            _gameStartText.gameObject.SetActive(true);
            for(int i = _gameStartCountTime; i > 0; i--)
            {
                // カウントダウンのアニメーションを再生する
                _gameStartText.transform.localScale = Vector3.zero;
                _gameStartText.text = i.ToString();
                await _gameStartText.transform.DOScale(Vector3.one, _duration);
            }
            _gameStartText.transform.localScale = Vector3.zero;
            _gameStartText.text = "Game Start !";
            await _gameStartText.transform.DOScale(Vector3.one, _duration);
            
            // ゲーム開始テキストを非表示にする
            _gameStartText.gameObject.SetActive(false);
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