using Ball;
using Cysharp.Threading.Tasks;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using System.Threading;

namespace Goal
{
    public class GoalCore : MonoBehaviour, IBallCollisionHandler
    {
        public GoalScoreLogic ScoreLogic { get; private set; }
        public CancellationToken CancellationToken { get; private set; }
        public UniTask InitializedAsync => _uniTaskCompletionSource.Task;

        [SerializeField] private Text _scoreCountText;
        private readonly UniTaskCompletionSource _uniTaskCompletionSource = new UniTaskCompletionSource();
        private GoalScorePresentation _scorePresentation;


        public void OnCollisionBall(BallCore ball)
        {
            ball.MoveLogic.ResetPosition(CancellationToken).Forget();
            ScoreLogic.CountUpScore();
        }

        private void Start()
        {
            ScoreLogic = new GoalScoreLogic();
            _scorePresentation = new GoalScorePresentation(_scoreCountText, ScoreLogic);
            CancellationToken = this.GetCancellationTokenOnDestroy();

            _uniTaskCompletionSource.TrySetResult();
        }

        private void OnDestroy()
        {
            _uniTaskCompletionSource.TrySetCanceled();
        }
    }
}