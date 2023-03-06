using Cysharp.Threading.Tasks;
using UniRx;

namespace Goal
{
    public class GoalScoreLogic
    {
        public IReadOnlyReactiveProperty<int> ScoreCount => _scoreCount;

        // --------------------------------------------------

        private readonly ReactiveProperty<int> _scoreCount = new ReactiveProperty<int>(0);

        // --------------------------------------------------

        public void CountUpScore()
        {
            _scoreCount.Value++;
        }

        public void ResetScoreCount()
        {
            _scoreCount.Value = 0;
        }
    }
}