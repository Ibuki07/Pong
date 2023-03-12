using UniRx;
using Managers;

namespace Option
{
    public class QuitTitleLogic
    {
        public QuitTitleLogic(QuitTitleInput input) 
        {
            // QuitTitle‚Ì’l‚ªTrue‚É•Ï‰»‚µ‚½Žž‚ÉTitleƒV[ƒ“‚É‘JˆÚ‚·‚é
            input.QuitTitle
                .Where(isQuitTitle => isQuitTitle)
                .Subscribe(_ =>
                {
                    SceneStateManager.Instance.LoadSceneFadeOut(SCENE_TYPE.Title);
                });
        }
    }
}