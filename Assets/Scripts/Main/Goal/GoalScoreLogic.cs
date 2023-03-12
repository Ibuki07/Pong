using UniRx;

namespace Goal
{
    public class GoalScoreLogic
    {
        // スコアカウント
        public IReadOnlyReactiveProperty<int> ScoreCount => _scoreCount;

        private readonly ReactiveProperty<int> _scoreCount = new ReactiveProperty<int>(0);

        // スコアカウントをカウントアップする
        public void CountUpScore()
        {
            _scoreCount.Value++;
        }

        // スコアカウントをリセットする
        public void ResetScoreCount()
        {
            _scoreCount.Value = 0;
        }
    }
}