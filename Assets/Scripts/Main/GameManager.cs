using Ball;
using Goal;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace Manager
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private GoalCore[] _goal = new GoalCore[2];
        [SerializeField] private BallCore _ball;
        [SerializeField, Min(1)] private int _maxGoalCount;
        [SerializeField] private Button _restartButton;

        // --------------------------------------------------

        private async void Start()
        {
            foreach (var goal in _goal)
            {
                await goal.InitializedAsync;
                EndGame(goal);
            }

            _restartButton.gameObject.SetActive(false);
            _ball.BallLogic.Destroyed.Where(isDestroyed => isDestroyed)
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
        }

        private void StartGame()
        {
            foreach (var goal in _goal)
            {
                goal.GoalLogic.ResetGoalCount();
            }
            _ball.BallLogic.OnCreatedBall();
        }


        private void EndGame(GoalCore goal)
        {
            goal.GoalLogic.GoalCount
                .Where(goalCount => goalCount >= _maxGoalCount)
                .Subscribe(_ =>
                {
                    _ball.BallLogic.OnDestroyedBall();
                });
        }


    }
}