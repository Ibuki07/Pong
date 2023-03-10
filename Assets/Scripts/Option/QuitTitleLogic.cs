using UnityEngine;
using UnityEngine.UI;
using UniRx;

namespace Option
{
    public class QuitTitleLogic
    {
        public QuitTitleLogic(Button quitTitleButton) 
        {
            quitTitleButton
                .OnClickAsObservable()
                .First()
                .Subscribe(_ =>
                {
                    SceneStateManager.Instance.LoadSceneFadeOut(SceneStateManager.SceneType.Title);
                });


        }
    }
}