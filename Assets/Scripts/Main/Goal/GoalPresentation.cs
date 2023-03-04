using UnityEngine.UI;
using UniRx;

namespace Goal
{
    public class GoalPresentation
    {
        private Text _goalCountText;

        // --------------------------------------------------

        public GoalPresentation(Text text, GoalLogic goalLogic)
        {
            _goalCountText = text;

            goalLogic.GoalCount.Subscribe(goalCount =>
            {
                UpdateGoalCount(goalCount);
            });
        }

        public void UpdateGoalCount(int goalCount)
        {
            _goalCountText.text = goalCount.ToString();
        }
    }
}