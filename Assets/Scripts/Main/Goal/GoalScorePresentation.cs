using UnityEngine.UI;
using UniRx;

namespace Goal
{
    public class GoalScorePresentation
    {
        private Text _scoreCountText;

        public GoalScorePresentation(Text text, GoalScoreLogic scoreLogic)
        {
            _scoreCountText = text;

            scoreLogic.ScoreCount.Subscribe(scoreCount =>
            {
                UpdateScoreCount(scoreCount);
            });
        }

        private void UpdateScoreCount(int scoreCount)
        {
            _scoreCountText.text = scoreCount.ToString();
        }
    }
}