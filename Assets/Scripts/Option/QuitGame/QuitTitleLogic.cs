using UniRx;
using Managers;

namespace Option
{
    public class QuitTitleLogic
    {
        public QuitTitleLogic(QuitTitleInput input) 
        {
            // QuitTitleの値がTrueに変化した時にTitleシーンに遷移する
            input.QuitTitle
                .Where(isQuitTitle => isQuitTitle)
                .Subscribe(_ =>
                {
                    SceneStateManager.Instance.LoadSceneFadeOut(SCENE_TYPE.Title);
                });
        }
    }
}