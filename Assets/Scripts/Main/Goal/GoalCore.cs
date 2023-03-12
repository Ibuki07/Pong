using Ball;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using System.Threading;

namespace Goal
{
    public class GoalCore : MonoBehaviour, IBallCollisionHandler
    {
        public GoalScoreLogic ScoreLogic { get; private set; }
        // UniTask処理のキャンセルトークン
        public CancellationToken CancellationToken { get; private set; }
        // 初期化処理が完了したかどうかを返すUniTask
        public UniTask InitializedAsync => _uniTaskCompletionSource.Task;

        [SerializeField] private Text _scoreCountText;

        private readonly UniTaskCompletionSource _uniTaskCompletionSource = new UniTaskCompletionSource();

        private GoalScorePresentation _scorePresentation;

        private void Start()
        {
            ScoreLogic = new GoalScoreLogic();
            // 得点を表示するUI要素と得点カウントを管理するロジックを渡して、得点表示を管理するプレゼンテーションを作成
            _scorePresentation = new GoalScorePresentation(_scoreCountText, ScoreLogic);
            // キャンセルトークンを設定
            CancellationToken = this.GetCancellationTokenOnDestroy();

            // 初期化処理が完了したことをUniTaskCompletionSourceに通知
            _uniTaskCompletionSource.TrySetResult();
        }

        private void OnDestroy()
        {
            _uniTaskCompletionSource.TrySetCanceled();
        }

        // ボールがゴールに衝突した時の処理
        public void OnCollisionBall(BallCore ball)
        {
            // ボールの移動をリセット
            ball.MoveLogic.ResetPosition(CancellationToken).Forget();
            // 得点を加算
            ScoreLogic.CountUpScore();
        }
    }
}