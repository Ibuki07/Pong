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
        public GoalLogic GoalLogic { get; private set; }
        public CancellationToken CancellationToken { get; private set; }
        public UniTask InitializedAsync => _uniTaskCompletionSource.Task;

        // --------------------------------------------------

        private readonly UniTaskCompletionSource _uniTaskCompletionSource = new UniTaskCompletionSource();
        private GoalPresentation _goalPresentation;
        [SerializeField] private Text _goalCountText;

        // --------------------------------------------------

        public void OnCollisionBall(BallCore ball)
        {
            ball.BallLogic.OnResetPosition(CancellationToken).Forget();
            GoalLogic.OnCollisionBall();
        }

        // --------------------------------------------------

        private void Start()
        {
            GoalLogic = new GoalLogic();
            _goalPresentation = new GoalPresentation(_goalCountText, GoalLogic);
            CancellationToken = this.GetCancellationTokenOnDestroy();

            _uniTaskCompletionSource.TrySetResult();
        }


        private void OnDestroy()
        {
            _uniTaskCompletionSource.TrySetCanceled();
        }
    }
}