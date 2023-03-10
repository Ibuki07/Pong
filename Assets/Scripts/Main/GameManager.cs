using Ball;
using Goal;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace Managers
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private GoalCore[] _goal = new GoalCore[2];
        [SerializeField] private BallCore _ball;
        [SerializeField, Min(1)] private int _maxGoalCount;
        [SerializeField] private Button _restartButton;

        private System.IDisposable _disposable;

        // --------------------------------------------------

        private async void Start()
        {
            SceneStateManager.Instance.OnFadeIn();

            var bgm = Random.Range(1, 3);
            SoundManager.Instance.PlayBGM((SoundManager.BGMType)bgm);

            foreach (var goal in _goal)
            {
                await goal.InitializedAsync;
                EndGame(goal);
            }

            _restartButton.gameObject.SetActive(false);
            _ball.MoveLogic.Destroyed.Where(isDestroyed => isDestroyed)
                .Subscribe(_ =>
                {
                    _restartButton.gameObject.SetActive(true);
                });

            _restartButton.OnClickAsObservable()
                .Subscribe(_ =>
                {
                    _restartButton.gameObject.SetActive(false);
                    StartGame();
                });

            _disposable = Observable.EveryUpdate()
            .Where(_ => _ball.MoveLogic.Destroyed.Value && Input.GetKeyDown(KeyCode.Return))
            .Subscribe(_ =>
            {
                _restartButton.gameObject.SetActive(false);
                StartGame();
            });
        }

        private void StartGame()
        {
            var bgm = Random.Range(1, 3);
            SoundManager.Instance.PlayBGM((SoundManager.BGMType)bgm);
            foreach (var goal in _goal)
            {
                goal.ScoreLogic.ResetScoreCount();
            }
            _ball.MoveLogic.OnCreatedBall();
        }


        private void EndGame(GoalCore goal)
        {
            goal.ScoreLogic.ScoreCount
                .Where(goalCount => goalCount >= _maxGoalCount)
                .Subscribe(_ =>
                {
                    _ball.MoveLogic.OnDestroyedBall();
                });
        }


    }
}