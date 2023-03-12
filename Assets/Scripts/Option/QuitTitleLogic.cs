using UniRx;
using Managers;

namespace Option
{
    public class QuitTitleLogic
    {
        public QuitTitleLogic(QuitTitleInput input) 
        {
            // QuitTitle�̒l��True�ɕω���������Title�V�[���ɑJ�ڂ���
            input.QuitTitle
                .Where(isQuitTitle => isQuitTitle)
                .Subscribe(_ =>
                {
                    SceneStateManager.Instance.LoadSceneFadeOut(SCENE_TYPE.Title);
                });
        }
    }
}