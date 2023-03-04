using Cysharp.Threading.Tasks;
using UniRx;

namespace Goal
{
    public class GoalLogic
    {
        public IReadOnlyReactiveProperty<int> GoalCount => _goalCount;

        // --------------------------------------------------

        private readonly ReactiveProperty<int> _goalCount = new ReactiveProperty<int>(0);

        // --------------------------------------------------

        public void OnCollisionBall()
        {
            _goalCount.Value++;
        }

        public void ResetGoalCount()
        {
            _goalCount.Value = 0;
        }
    }
}