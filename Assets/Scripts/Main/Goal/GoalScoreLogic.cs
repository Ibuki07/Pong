using UniRx;

namespace Goal
{
    public class GoalScoreLogic
    {
        // �X�R�A�J�E���g
        public IReadOnlyReactiveProperty<int> ScoreCount => _scoreCount;

        private readonly ReactiveProperty<int> _scoreCount = new ReactiveProperty<int>(0);

        // �X�R�A�J�E���g���J�E���g�A�b�v����
        public void CountUpScore()
        {
            _scoreCount.Value++;
        }

        // �X�R�A�J�E���g�����Z�b�g����
        public void ResetScoreCount()
        {
            _scoreCount.Value = 0;
        }
    }
}