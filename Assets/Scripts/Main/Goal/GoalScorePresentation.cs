using UnityEngine.UI;
using UniRx;

namespace Goal
{
    public class GoalScorePresentation
    {
        private Text _scoreCountText;

        public GoalScorePresentation(Text text, GoalScoreLogic scoreLogic)
        {
            // �X�R�A�\���p�e�L�X�g��ݒ�
            _scoreCountText = text;

            // �X�R�A���X�V���ꂽ�ۂɃX�R�A�\���p�e�L�X�g���X�V����
            scoreLogic.ScoreCount.Subscribe(scoreCount =>
            {
                UpdateScoreCount(scoreCount);
            });
        }

        // �X�R�A�\���p�e�L�X�g���X�V����
        private void UpdateScoreCount(int scoreCount)
        {
            _scoreCountText.text = scoreCount.ToString();
        }
    }
}