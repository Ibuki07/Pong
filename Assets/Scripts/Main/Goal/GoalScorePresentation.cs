using UnityEngine.UI;
using UniRx;

namespace Goal
{
    public class GoalScorePresentation
    {
        private Text _scoreCountText;

        public GoalScorePresentation(Text text, GoalScoreLogic scoreLogic)
        {
            // スコア表示用テキストを設定
            _scoreCountText = text;

            // スコアが更新された際にスコア表示用テキストを更新する
            scoreLogic.ScoreCount.Subscribe(scoreCount =>
            {
                UpdateScoreCount(scoreCount);
            });
        }

        // スコア表示用テキストを更新する
        private void UpdateScoreCount(int scoreCount)
        {
            _scoreCountText.text = scoreCount.ToString();
        }
    }
}